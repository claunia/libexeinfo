//
// PE.cs
//
// Author:
//       Natalia Portillo <claunia@claunia.com>
//
// Copyright (c) 2017-2018 Copyright © Claunia.com
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITPESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONPECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using libexeinfo.BeOS;

namespace libexeinfo
{
    /// <summary>
    ///     Represents a Microsoft Portable Executable
    /// </summary>
    public partial class PE : IExecutable
    {
        MZ                         baseExecutable;
        public ResourceTypeBlock[] BeosResources;
        DebugDirectory             debugDirectory;
        ImageDataDirectory[]       directoryEntries;
        string[]                   exportedNames;
        /// <summary>
        ///     Header for this executable
        /// </summary>
        PEHeader header;
        string[]             importedNames;
        string               moduleName;
        COFF.SectionHeader[] sectionHeaders;
        public Version[]     Versions;
        public ResourceNode  WindowsResourcesRoot;
        WindowsHeader64      winHeader;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:libexeinfo.PE" /> class.
        /// </summary>
        /// <param name="path">Executable path.</param>
        public PE(string path)
        {
            BaseStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            Initialize();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:libexeinfo.PE" /> class.
        /// </summary>
        /// <param name="stream">Stream containing the executable.</param>
        public PE(Stream stream)
        {
            BaseStream = stream;
            Initialize();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:libexeinfo.PE" /> class.
        /// </summary>
        /// <param name="data">Byte array containing the executable.</param>
        public PE(byte[] data)
        {
            BaseStream = new MemoryStream(data);
            Initialize();
        }

        public Stream BaseStream  { get; }
        public bool   IsBigEndian => false;
        public bool   Recognized  { get; private set; }
        public string Type        { get; private set; }
        public IEnumerable<Architecture> Architectures =>
            new[] {COFF.MachineTypeToArchitecture(header.coff.machine)};
        public OperatingSystem      RequiredOperatingSystem { get; private set; }
        public IEnumerable<string>  Strings                 { get; private set; }
        public IEnumerable<Segment> Segments                { get; private set; }

        void Initialize()
        {
            Recognized = false;
            if(BaseStream == null) return;

            baseExecutable = new MZ(BaseStream);
            if(!baseExecutable.Recognized) return;

            if(baseExecutable.Header.new_offset >= BaseStream.Length) return;

            BaseStream.Seek(baseExecutable.Header.new_offset, SeekOrigin.Begin);
            byte[] buffer = new byte[Marshal.SizeOf(typeof(PEHeader))];
            BaseStream.Read(buffer, 0, buffer.Length);
            IntPtr hdrPtr = Marshal.AllocHGlobal(buffer.Length);
            Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
            header = (PEHeader)Marshal.PtrToStructure(hdrPtr, typeof(PEHeader));
            Marshal.FreeHGlobal(hdrPtr);
            Recognized = header.signature == SIGNATURE;

            if(!Recognized) return;

            Type = "Portable Executable (PE)";

            if(header.coff.optionalHeader.magic == PE32Plus)
            {
                BaseStream.Position -= 4;
                buffer              =  new byte[Marshal.SizeOf(typeof(WindowsHeader64))];
                BaseStream.Read(buffer, 0, buffer.Length);
                hdrPtr = Marshal.AllocHGlobal(buffer.Length);
                Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
                winHeader = (WindowsHeader64)Marshal.PtrToStructure(hdrPtr, typeof(WindowsHeader64));
                Marshal.FreeHGlobal(hdrPtr);
            }
            else
            {
                buffer = new byte[Marshal.SizeOf(typeof(WindowsHeader))];
                BaseStream.Read(buffer, 0, buffer.Length);
                hdrPtr = Marshal.AllocHGlobal(buffer.Length);
                Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
                WindowsHeader hdr32 = (WindowsHeader)Marshal.PtrToStructure(hdrPtr, typeof(WindowsHeader));
                Marshal.FreeHGlobal(hdrPtr);
                winHeader = ToPlus(hdr32);
            }

            OperatingSystem reqOs = new OperatingSystem();

            switch(winHeader.subsystem)
            {
                case Subsystems.IMAGE_SUBSYSTEM_UNKNOWN:
                    reqOs.Name = "Unknown";
                    break;
                case Subsystems.IMAGE_SUBSYSTEM_NATIVE:
                    reqOs.Name      = "Windows NT";
                    reqOs.Subsystem = "Native";
                    break;
                case Subsystems.IMAGE_SUBSYSTEM_WINDOWS_GUI:
                    reqOs.Name      = winHeader.majorSubsystemVersion <= 3 ? "Windows NT" : "Windows";
                    reqOs.Subsystem = "GUI";
                    break;
                case Subsystems.IMAGE_SUBSYSTEM_WINDOWS_CUI:
                    reqOs.Name      = winHeader.majorSubsystemVersion <= 3 ? "Windows NT" : "Windows";
                    reqOs.Subsystem = "Console";
                    break;
                case Subsystems.IMAGE_SUBSYSTEM_OS2_CUI:
                    reqOs.Name      = "Windows NT";
                    reqOs.Subsystem = "OS/2";
                    break;
                case Subsystems.IMAGE_SUBSYSTEM_POSIX_CUI:
                    reqOs.Name      = "Windows NT";
                    reqOs.Subsystem = "POSIX";
                    break;
                case Subsystems.IMAGE_SUBSYSTEM_NATIVE_WINDOWS:
                    reqOs.Name      = "Windows";
                    reqOs.Subsystem = "Native";
                    break;
                case Subsystems.IMAGE_SUBSYSTEM_WINDOWS_CE_GUI:
                    reqOs.Name = "Windows CE";
                    break;
                case Subsystems.IMAGE_SUBSYSTEM_EFI_APPLICATION:
                case Subsystems.IMAGE_SUBSYSTEM_EFI_BOOT_SERVICE_DRIVER:
                case Subsystems.IMAGE_SUBSYSTEM_EFI_RUNTIME_DRIVER:
                case Subsystems.IMAGE_SUBSYSTEM_EFI_ROM:
                    reqOs.Name = "EFI";
                    break;
                case Subsystems.IMAGE_SUBSYSTEM_XBOX:
                    reqOs.Name = "Xbox OS";
                    break;
                case Subsystems.IMAGE_SUBSYSTEM_WINDOWS_BOOT_APPLICATION:
                    reqOs.Name      = "Windows NT";
                    reqOs.Subsystem = "Boot environment";
                    break;
                default:
                    reqOs.Name = $"Unknown code ${(ushort)winHeader.subsystem}";
                    break;
            }

            reqOs.MajorVersion      = winHeader.majorSubsystemVersion;
            reqOs.MinorVersion      = winHeader.minorSubsystemVersion;
            RequiredOperatingSystem = reqOs;

            buffer           = new byte[Marshal.SizeOf(typeof(ImageDataDirectory))];
            directoryEntries = new ImageDataDirectory[winHeader.numberOfRvaAndSizes];
            for(int i = 0; i < directoryEntries.Length; i++)
            {
                BaseStream.Read(buffer, 0, buffer.Length);
                directoryEntries[i] = BigEndianMarshal.ByteArrayToStructureLittleEndian<ImageDataDirectory>(buffer);
            }

            buffer         = new byte[Marshal.SizeOf(typeof(COFF.SectionHeader))];
            sectionHeaders = new COFF.SectionHeader[header.coff.numberOfSections];
            for(int i = 0; i < sectionHeaders.Length; i++)
            {
                BaseStream.Read(buffer, 0, buffer.Length);
                sectionHeaders[i] = BigEndianMarshal.ByteArrayToStructureLittleEndian<COFF.SectionHeader>(buffer);
            }

            Dictionary<string, COFF.SectionHeader> newSectionHeaders =
                sectionHeaders.ToDictionary(section => section.name);

            for(int i = 0; i < directoryEntries.Length; i++)
            {
                string tableName;
                switch(i)
                {
                    case 0:
                        tableName = ".edata";
                        break;
                    case 1:
                        tableName = ".idata";
                        break;
                    case 2:
                        tableName = ".rsrc";
                        break;
                    case 3:
                        tableName = ".pdata";
                        break;
                    case 5:
                        tableName = ".reloc";
                        break;
                    case 6:
                        tableName = ".debug";
                        break;
                    case 9:
                        tableName = ".tls";
                        break;
                    case 14:
                        tableName = ".cormeta";
                        break;
                    default: continue;
                }

                if(newSectionHeaders.ContainsKey(tableName)) continue;
                if(directoryEntries[i].rva == 0) continue;

                newSectionHeaders.Add(tableName,
                                      new COFF.SectionHeader
                                      {
                                          characteristics =
                                              COFF.SectionFlags.IMAGE_SCN_CNT_INITIALIZED_DATA |
                                              COFF.SectionFlags.IMAGE_SCN_MEM_READ,
                                          name             = tableName,
                                          pointerToRawData = RvaToReal(directoryEntries[i].rva, sectionHeaders),
                                          virtualAddress   = directoryEntries[i].rva,
                                          sizeOfRawData    = directoryEntries[i].size,
                                          virtualSize      = directoryEntries[i].size
                                      });
            }

            List<byte>   chars;
            List<string> strings = new List<string>();

            if(newSectionHeaders.TryGetValue(".edata", out COFF.SectionHeader edata))
            {
                buffer              = new byte[Marshal.SizeOf(typeof(ExportDirectoryTable))];
                BaseStream.Position = edata.pointerToRawData;
                BaseStream.Read(buffer, 0, buffer.Length);
                ExportDirectoryTable edataTable =
                    BigEndianMarshal.ByteArrayToStructureLittleEndian<ExportDirectoryTable>(buffer);

                BaseStream.Position = RvaToReal(edataTable.nameRva, sectionHeaders);
                chars               = new List<byte>();
                while(true)
                {
                    int ch = BaseStream.ReadByte();
                    if(ch <= 0) break;

                    chars.Add((byte)ch);
                }

                moduleName = Encoding.ASCII.GetString(chars.ToArray());

                uint[] namePointers = new uint[edataTable.numberOfNamePointers];
                exportedNames       = new string[edataTable.numberOfNamePointers];
                buffer              = new byte[Marshal.SizeOf(typeof(uint)) * edataTable.numberOfNamePointers];
                BaseStream.Position = RvaToReal(edataTable.namePointerRva, sectionHeaders);
                BaseStream.Read(buffer, 0, buffer.Length);
                for(int i = 0; i < edataTable.numberOfNamePointers; i++)
                {
                    namePointers[i]     = BitConverter.ToUInt32(buffer, i * 4);
                    BaseStream.Position = RvaToReal(namePointers[i], sectionHeaders);

                    chars = new List<byte>();
                    while(true)
                    {
                        int ch = BaseStream.ReadByte();
                        if(ch <= 0) break;

                        chars.Add((byte)ch);
                    }

                    exportedNames[i] = Encoding.ASCII.GetString(chars.ToArray());
                }
            }

            if(newSectionHeaders.TryGetValue(".idata", out COFF.SectionHeader idata))
            {
                buffer              = new byte[Marshal.SizeOf(typeof(ImportDirectoryTable))];
                BaseStream.Position = idata.pointerToRawData;
                List<ImportDirectoryTable> importDirectoryEntries = new List<ImportDirectoryTable>();

                while(true)
                {
                    BaseStream.Read(buffer, 0, buffer.Length);
                    if(buffer.All(b => b == 0)) break;

                    importDirectoryEntries.Add(BigEndianMarshal
                                                  .ByteArrayToStructureLittleEndian<ImportDirectoryTable>(buffer));
                }

                importedNames = new string[importDirectoryEntries.Count];
                for(int i = 0; i < importDirectoryEntries.Count; i++)
                {
                    BaseStream.Position = RvaToReal(importDirectoryEntries[i].nameRva, sectionHeaders);

                    chars = new List<byte>();
                    while(true)
                    {
                        int ch = BaseStream.ReadByte();
                        if(ch <= 0) break;

                        chars.Add((byte)ch);
                    }

                    importedNames[i] = Encoding.ASCII.GetString(chars.ToArray());

                    // BeOS R3 uses PE with no subsystem
                    if(importedNames[i].ToLower() == "libbe.so")
                    {
                        reqOs.MajorVersion      = 3;
                        reqOs.MinorVersion      = 0;
                        reqOs.Subsystem         = null;
                        reqOs.Name              = "BeOS";
                        RequiredOperatingSystem = reqOs;
                    }
                    // Singularity appears as a native NT executable
                    else if(importedNames[i].ToLower() == "singularity.v1.dll")
                    {
                        reqOs.MajorVersion      = 1;
                        reqOs.MinorVersion      = 0;
                        reqOs.Subsystem         = null;
                        reqOs.Name              = "Singularity";
                        RequiredOperatingSystem = reqOs;
                    }
                }
            }

            if(newSectionHeaders.TryGetValue(".debug", out COFF.SectionHeader debug) && debug.virtualAddress > 0)
            {
                buffer              = new byte[Marshal.SizeOf(typeof(DebugDirectory))];
                BaseStream.Position = debug.pointerToRawData;
                BaseStream.Read(buffer, 0, buffer.Length);
                debugDirectory = BigEndianMarshal.ByteArrayToStructureLittleEndian<DebugDirectory>(buffer);
            }

            if(newSectionHeaders.TryGetValue(".rsrc", out COFF.SectionHeader rsrc))
                if(reqOs.Name == "BeOS")
                {
                    newSectionHeaders.Remove(".rsrc");
                    rsrc.pointerToRawData = rsrc.virtualAddress;

                    long maxPosition = BaseStream.Length;
                    foreach(KeyValuePair<string, COFF.SectionHeader> kvp in newSectionHeaders)
                        if(kvp.Value.pointerToRawData <= maxPosition &&
                           kvp.Value.pointerToRawData > rsrc.pointerToRawData)
                            maxPosition = kvp.Value.pointerToRawData;

                    rsrc.sizeOfRawData = (uint)(maxPosition - rsrc.pointerToRawData);
                    rsrc.virtualSize   = rsrc.sizeOfRawData;
                    newSectionHeaders.Add(".rsrc", rsrc);

                    buffer              = new byte[rsrc.sizeOfRawData];
                    BaseStream.Position = rsrc.pointerToRawData;
                    BaseStream.Read(buffer, 0, buffer.Length);
                    BeosResources = Resources.Decode(buffer);

                    strings.AddRange(from type in BeosResources
                                     where type.type == Consts.B_VERSION_INFO_TYPE
                                     from resource in type.resources
                                     select BigEndianMarshal
                                        .ByteArrayToStructureLittleEndian<VersionInfo>(resource.data)
                                     into versionInfo
                                     select StringHandlers.CToString(versionInfo.long_info));
                }
                else
                {
                    WindowsResourcesRoot = GetResourceNode(BaseStream, rsrc.pointerToRawData,
                                                           rsrc.virtualAddress,
                                                           rsrc.pointerToRawData, 0, null, 0);
                    Versions = GetVersions().ToArray();

                    strings.AddRange(from v in Versions from s in v.StringsByLanguage from k in s.Value select k.Value);

                    foreach(ResourceNode rtype in WindowsResourcesRoot.children.Where(r => r.name == "RT_STRING"))
                        strings.AddRange(GetStrings(rtype));
                }

            sectionHeaders = newSectionHeaders.Values.OrderBy(s => s.pointerToRawData).ToArray();
            Segment[] segments = new Segment[sectionHeaders.Length];
            for(int i = 0; i < segments.Length; i++)
                segments[i] = new Segment
                {
                    Flags  = $"{sectionHeaders[i].characteristics}",
                    Name   = sectionHeaders[i].name,
                    Offset = sectionHeaders[i].pointerToRawData,
                    Size   = sectionHeaders[i].sizeOfRawData
                };

            Segments = segments;
            strings.Sort();
            Strings = strings;
        }

        static ResourceNode GetResourceNode(Stream stream, long position, long rsrcVa, long rsrcStart, uint id,
                                            string name,   int  level)
        {
            long         oldPosition = stream.Position;
            ResourceNode thisNode    = new ResourceNode {name = name, id = id, level = level};

            if(thisNode.name == null)
                thisNode.name = level == 1 ? Windows.Resources.IdToName((ushort)thisNode.id) : $"{thisNode.id}";

            stream.Position = position;
            byte[] buffer = new byte[Marshal.SizeOf(typeof(ResourceDirectoryTable))];
            stream.Read(buffer, 0, buffer.Length);
            ResourceDirectoryTable rsrcTable =
                BigEndianMarshal.ByteArrayToStructureLittleEndian<ResourceDirectoryTable>(buffer);

            buffer = new byte[Marshal.SizeOf(typeof(ResourceDirectoryEntries))];
            ResourceDirectoryEntries[] entries =
                new ResourceDirectoryEntries[rsrcTable.nameEntries + rsrcTable.idEntries];

            for(int i = 0; i < rsrcTable.nameEntries; i++)
            {
                stream.Read(buffer, 0, buffer.Length);
                entries[i] = BigEndianMarshal.ByteArrayToStructureLittleEndian<ResourceDirectoryEntries>(buffer);
            }

            for(int i = 0; i < rsrcTable.idEntries; i++)
            {
                stream.Read(buffer, 0, buffer.Length);
                entries[rsrcTable.nameEntries + i] =
                    BigEndianMarshal.ByteArrayToStructureLittleEndian<ResourceDirectoryEntries>(buffer);
            }

            thisNode.children = new ResourceNode[entries.Length];

            for(int i = 0; i < rsrcTable.nameEntries; i++)
            {
                byte[] len = new byte[2];

                stream.Position = rsrcStart + (entries[i].nameOrID & 0x7FFFFFFF);
                stream.Read(len, 0, 2);
                buffer = new byte[BitConverter.ToUInt16(len, 0) * 2];
                stream.Read(buffer, 0, buffer.Length);
                string childName = Encoding.Unicode.GetString(buffer);

                if((entries[i].rva & 0x80000000) == 0x80000000)
                    thisNode.children[i] = GetResourceNode(stream,    rsrcStart + (entries[i].rva & 0x7FFFFFFF), rsrcVa,
                                                           rsrcStart, 0,
                                                           childName, level + 1);
                else
                {
                    buffer          = new byte[Marshal.SizeOf(typeof(ResourceDataEntry))];
                    stream.Position = rsrcStart + (entries[i].rva & 0x7FFFFFFF);
                    stream.Read(buffer, 0, buffer.Length);
                    ResourceDataEntry dataEntry =
                        BigEndianMarshal.ByteArrayToStructureLittleEndian<ResourceDataEntry>(buffer);
                    thisNode.children[i] = new ResourceNode
                    {
                        data  = new byte[dataEntry.size],
                        id    = 0,
                        name  = childName,
                        level = level + 1
                    };
                    stream.Position = dataEntry.rva - (rsrcVa - rsrcStart);
                    stream.Read(thisNode.children[i].data, 0, (int)dataEntry.size);
                }
            }

            for(int i = rsrcTable.nameEntries; i < rsrcTable.nameEntries + rsrcTable.idEntries; i++)
                if((entries[i].rva & 0x80000000) == 0x80000000)
                    thisNode.children[i] = GetResourceNode(stream,    rsrcStart + (entries[i].rva & 0x7FFFFFFF), rsrcVa,
                                                           rsrcStart, entries[i].nameOrID & 0x7FFFFFFF,          null,
                                                           level + 1);
                else
                {
                    buffer          = new byte[Marshal.SizeOf(typeof(ResourceDataEntry))];
                    stream.Position = rsrcStart + (entries[i].rva & 0x7FFFFFFF);
                    stream.Read(buffer, 0, buffer.Length);
                    ResourceDataEntry dataEntry =
                        BigEndianMarshal.ByteArrayToStructureLittleEndian<ResourceDataEntry>(buffer);
                    thisNode.children[i] = new ResourceNode
                    {
                        data  = new byte[dataEntry.size],
                        id    = entries[i].nameOrID & 0x7FFFFFFF,
                        name  = $"{entries[i].nameOrID & 0x7FFFFFFF}",
                        level = level + 1
                    };

                    if(level == 2)
                        if(thisNode.children[i].id == 0)
                            thisNode.children[i].name = "Neutral";
                        else
                            try
                            {
                                thisNode.children[i].name = new CultureInfo((int)thisNode.children[i].id).DisplayName;
                            }
                            catch { thisNode.children[i].name = $"Language ID {thisNode.children[i].id}"; }

                    stream.Position = dataEntry.rva - (rsrcVa - rsrcStart);
                    stream.Read(thisNode.children[i].data, 0, (int)dataEntry.size);
                }

            stream.Position = oldPosition;
            return thisNode;
        }

        /// <summary>
        ///     Identifies if the specified executable is a Microsoft Portable Executable
        /// </summary>
        /// <returns><c>true</c> if the specified executable is a Microsoft Portable Executable, <c>false</c> otherwise.</returns>
        /// <param name="path">Executable path.</param>
        public static bool Identify(string path)
        {
            FileStream baseStream     = File.Open(path, FileMode.Open, FileAccess.Read);
            MZ         baseExecutable = new MZ(baseStream);
            if(!baseExecutable.Recognized) return false;

            if(baseExecutable.Header.new_offset >= baseStream.Length) return false;

            baseStream.Seek(baseExecutable.Header.new_offset, SeekOrigin.Begin);
            byte[] buffer = new byte[Marshal.SizeOf(typeof(PEHeader))];
            baseStream.Read(buffer, 0, buffer.Length);
            IntPtr hdrPtr = Marshal.AllocHGlobal(buffer.Length);
            Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
            PEHeader header = (PEHeader)Marshal.PtrToStructure(hdrPtr, typeof(PEHeader));
            Marshal.FreeHGlobal(hdrPtr);
            return header.signature == SIGNATURE;
        }

        /// <summary>
        ///     Identifies if the specified executable is a Microsoft Portable Executable
        /// </summary>
        /// <returns><c>true</c> if the specified executable is a Microsoft Portable Executable, <c>false</c> otherwise.</returns>
        /// <param name="stream">Stream containing the executable.</param>
        public static bool Identify(FileStream stream)
        {
            FileStream baseStream     = stream;
            MZ         baseExecutable = new MZ(baseStream);
            if(!baseExecutable.Recognized) return false;

            if(baseExecutable.Header.new_offset >= baseStream.Length) return false;

            baseStream.Seek(baseExecutable.Header.new_offset, SeekOrigin.Begin);
            byte[] buffer = new byte[Marshal.SizeOf(typeof(PEHeader))];
            baseStream.Read(buffer, 0, buffer.Length);
            IntPtr hdrPtr = Marshal.AllocHGlobal(buffer.Length);
            Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
            PEHeader header = (PEHeader)Marshal.PtrToStructure(hdrPtr, typeof(PEHeader));
            Marshal.FreeHGlobal(hdrPtr);
            return header.signature == SIGNATURE;
        }

        static WindowsHeader64 ToPlus(WindowsHeader header)
        {
            return new WindowsHeader64
            {
                imageBase                   = header.imageBase,
                sectionAlignment            = header.sectionAlignment,
                fileAlignment               = header.fileAlignment,
                majorOperatingSystemVersion = header.majorOperatingSystemVersion,
                minorOperatingSystemVersion = header.minorOperatingSystemVersion,
                majorImageVersion           = header.majorImageVersion,
                minorImageVersion           = header.minorImageVersion,
                majorSubsystemVersion       = header.majorSubsystemVersion,
                minorSubsystemVersion       = header.minorSubsystemVersion,
                win32VersionValue           = header.win32VersionValue,
                sizeOfImage                 = header.sizeOfImage,
                sizeOfHeaders               = header.sizeOfHeaders,
                checksum                    = header.checksum,
                subsystem                   = header.subsystem,
                dllCharacteristics          = header.dllCharacteristics,
                sizeOfStackReserve          = header.sizeOfStackReserve,
                sizeOfStackCommit           = header.sizeOfStackCommit,
                sizeOfHeapReserve           = header.sizeOfHeapReserve,
                sizeOfHeapCommit            = header.sizeOfHeapCommit,
                loaderFlags                 = header.loaderFlags,
                numberOfRvaAndSizes         = header.numberOfRvaAndSizes
            };
        }

        static uint RvaToReal(uint rva, COFF.SectionHeader[] sections)
        {
            for(int i = 0; i < sections.Length; i++)
                if(rva >= sections[i].virtualAddress && rva <= sections[i].virtualAddress + sections[i].sizeOfRawData)
                    return sections[i].pointerToRawData + (rva - sections[i].virtualAddress);

            return 0;
        }
    }
}