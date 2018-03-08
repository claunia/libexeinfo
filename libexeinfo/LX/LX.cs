//
// LX.cs
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
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using libexeinfo.Os2;

namespace libexeinfo
{
    /// <summary>
    ///     Represents a Microsoft/IBM Linear EXecutable
    /// </summary>
    public partial class LX : IExecutable
    {
        MZ BaseExecutable;
        /// <summary>
        ///     Header for this executable
        /// </summary>
        LXHeader Header;
        string[]                 ImportedNames;
        public NE.ResidentName[] ImportNames;
        string                   ModuleDescription;
        string                   ModuleName;
        public NE.ResourceTable         neFormatResourceTable;
        public NE.ResidentName[] NonResidentNames;
        ObjectPageTableEntry[]   objectPageTableEntries;
        ObjectTableEntry[]       objectTableEntries;
        public NE.ResidentName[] ResidentNames;
        ResourceTableEntry[]     resources;
        public NE.Version        WinVersion;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:libexeinfo.NE" /> class.
        /// </summary>
        /// <param name="path">Executable path.</param>
        public LX(string path)
        {
            BaseStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            Initialize();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:libexeinfo.NE" /> class.
        /// </summary>
        /// <param name="stream">Stream containing the executable.</param>
        public LX(Stream stream)
        {
            BaseStream = stream;
            Initialize();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:libexeinfo.NE" /> class.
        /// </summary>
        /// <param name="data">Byte array containing the executable.</param>
        public LX(byte[] data)
        {
            BaseStream = new MemoryStream(data);
            Initialize();
        }

        /// <summary>
        ///     The <see cref="FileStream" /> that contains the executable represented by this instance
        /// </summary>
        public Stream BaseStream { get; }
        public bool                      IsBigEndian             { get; private set; }
        public bool                      Recognized              { get; private set; }
        public string                    Type                    { get; private set; }
        public IEnumerable<Architecture> Architectures           => new[] {CpuToArchitecture(Header.cpu_type)};
        public OperatingSystem           RequiredOperatingSystem { get; private set; }
        public IEnumerable<string>       Strings                 { get; private set; }
        public IEnumerable<Segment>      Segments                { get; private set; }

        // TODO: How to know VxD structure offset
        void Initialize()
        {
            Recognized = false;
            if(BaseStream == null) return;

            BaseExecutable = new MZ(BaseStream);
            if(!BaseExecutable.Recognized) return;

            if(BaseExecutable.Header.new_offset >= BaseStream.Length) return;

            BaseStream.Seek(BaseExecutable.Header.new_offset, SeekOrigin.Begin);
            byte[] buffer = new byte[Marshal.SizeOf(typeof(LXHeader))];
            BaseStream.Read(buffer, 0, buffer.Length);
            IntPtr hdrPtr = Marshal.AllocHGlobal(buffer.Length);
            Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
            Header = (LXHeader)Marshal.PtrToStructure(hdrPtr, typeof(LXHeader));
            Marshal.FreeHGlobal(hdrPtr);
            Recognized = Header.signature == SIGNATURE || Header.signature == SIGNATURE16;

            if(!Recognized) return;

            Type        = Header.signature  == SIGNATURE16 ? "Linear Executable (LE)" : "Linear eXecutable (LX)";
            IsBigEndian = Header.byte_order == 1 || Header.word_order == 1;
            List<string> strings = new List<string>();

            OperatingSystem reqOs = new OperatingSystem();
            switch(Header.os_type)
            {
                case TargetOS.OS2:
                    reqOs.Name = "OS/2";
                    if(Header.module_flags.HasFlag(ModuleFlags.PMIncompatible) &&
                       !Header.module_flags.HasFlag(ModuleFlags.PMCompatible) ||
                       !Header.module_flags.HasFlag(ModuleFlags.PMIncompatible) &&
                       Header.module_flags.HasFlag(ModuleFlags.PMCompatible)) reqOs.Subsystem = "Console";
                    else if(Header.module_flags.HasFlag(ModuleFlags.PMIncompatible) &&
                            Header.module_flags.HasFlag(ModuleFlags.PMCompatible))
                        reqOs.Subsystem = "Presentation Manager";
                    break;
                case TargetOS.Windows:
                case TargetOS.Win32:
                case TargetOS.Unknown:
                    reqOs.Name = "Windows";
                    break;
                case TargetOS.DOS:
                    reqOs.Name = "Windows";
                    break;
                default:
                    reqOs.Name = $"Unknown code {(ushort)Header.os_type}";
                    break;
            }

            RequiredOperatingSystem = reqOs;

            if(Header.resident_names_off != 0)
            {
                ResidentNames =
                    GetResidentStrings(BaseStream, BaseExecutable.Header.new_offset, Header.resident_names_off);

                if(ResidentNames.Length >= 1)
                {
                    ModuleName = ResidentNames[0].name;

                    if(ResidentNames.Length > 1)
                    {
                        NE.ResidentName[] newResidentNames = new NE.ResidentName[ResidentNames.Length - 1];
                        Array.Copy(ResidentNames, 1, newResidentNames, 0, ResidentNames.Length - 1);
                        ResidentNames = newResidentNames;
                    }
                    else ResidentNames = null;
                }
            }

            if(Header.nonresident_name_table_len > 0)
            {
                NonResidentNames = GetResidentStrings(BaseStream, 0, Header.nonresident_name_table_off);

                if(NonResidentNames.Length >= 1)
                {
                    ModuleDescription = NonResidentNames[0].name;

                    if(NonResidentNames.Length > 1)
                    {
                        NE.ResidentName[] newNonResidentNames = new NE.ResidentName[NonResidentNames.Length - 1];
                        Array.Copy(NonResidentNames, 1, newNonResidentNames, 0, NonResidentNames.Length - 1);
                        NonResidentNames = newNonResidentNames;
                    }
                    else NonResidentNames = null;
                }
            }

            if(Header.import_module_table_off != 0 && Header.import_module_entries > 0)
            {
                BaseStream.Position = Header.import_module_table_off + BaseExecutable.Header.new_offset;
                ImportedNames       = new string[Header.import_module_entries];
                for(int i = 0; i < Header.import_module_entries; i++)
                {
                    int len = BaseStream.ReadByte();
                    buffer = new byte[len];
                    BaseStream.Read(buffer, 0, len);
                    ImportedNames[i] = Encoding.ASCII.GetString(buffer);
                }
            }

            if(!string.IsNullOrEmpty(ModuleName)) strings.Add(ModuleName);
            if(!string.IsNullOrEmpty(ModuleDescription)) strings.Add(ModuleDescription);

            objectTableEntries     = new ObjectTableEntry[Header.obj_no];
            objectPageTableEntries = new ObjectPageTableEntry[Header.module_pages_no];

            BaseStream.Position = Header.obj_table_off + BaseExecutable.Header.new_offset;
            buffer              = new byte[Marshal.SizeOf(typeof(ObjectTableEntry))];
            for(int i = 0; i < Header.obj_no; i++)
            {
                BaseStream.Read(buffer, 0, buffer.Length);
                objectTableEntries[i] = BigEndianMarshal.ByteArrayToStructureLittleEndian<ObjectTableEntry>(buffer);
            }

            BaseStream.Position = Header.obj_page_table_off + BaseExecutable.Header.new_offset;

            if(Header.signature == SIGNATURE16)
            {
                buffer = new byte[Marshal.SizeOf(typeof(ObjectPageTableEntry16))];
                for(int i = 0; i < Header.module_pages_no; i++)
                {
                    BaseStream.Read(buffer, 0, buffer.Length);
                    ObjectPageTableEntry16 page16 =
                        BigEndianMarshal.ByteArrayToStructureLittleEndian<ObjectPageTableEntry16>(buffer);

                    int pageNo = (page16.High << 8) + page16.Low;

                    objectPageTableEntries[i] = new ObjectPageTableEntry
                    {
                        DataSize       = (ushort)Header.page_size,
                        Flags          = (PageTableAttributes)page16.Flags,
                        PageDataOffset = (uint)((pageNo - 1) * Header.page_size)
                    };
                }
            }
            else
            {
                buffer = new byte[Marshal.SizeOf(typeof(ObjectPageTableEntry))];
                for(int i = 0; i < Header.module_pages_no; i++)
                {
                    BaseStream.Read(buffer, 0, buffer.Length);
                    objectPageTableEntries[i] =
                        BigEndianMarshal.ByteArrayToStructureLittleEndian<ObjectPageTableEntry>(buffer);
                }
            }

            int debugSections   = 0;
            int winrsrcSections = 0;

            if(Header.debug_info_len > 0) debugSections   = 1;
            if(Header.win_res_len    > 0) winrsrcSections = 1;

            Segment[] sections = new Segment[objectTableEntries.Length + debugSections + winrsrcSections];
            for(int i = 0; i < objectTableEntries.Length; i++)
            {
                sections[i] = new Segment {Flags = $"{objectTableEntries[i].ObjectFlags}"};
                if(objectTableEntries[i].ObjectFlags.HasFlag(ObjectFlags.Resource)) sections[i].Name        = ".rsrc";
                else if(objectTableEntries[i].ObjectFlags.HasFlag(ObjectFlags.Executable)) sections[i].Name = ".text";
                else if(!objectTableEntries[i].ObjectFlags.HasFlag(ObjectFlags.Writable)) sections[i].Name  = ".rodata";
                else if(StringHandlers.CToString(objectTableEntries[i].Name).ToLower() == "bss")
                    sections[i].Name = ".bss";
                else if(!string.IsNullOrWhiteSpace(StringHandlers.CToString(objectTableEntries[i].Name).Trim()))
                    sections[i].Name  = StringHandlers.CToString(objectTableEntries[i].Name).Trim();
                else sections[i].Name = ".data";

                if(objectTableEntries[i].PageTableEntries == 0 ||
                   objectTableEntries[i].PageTableIndex   > objectPageTableEntries.Length)
                {
                    sections[i].Size = objectTableEntries[i].VirtualSize;
                    continue;
                }

                int shift = (int)(Header.signature == SIGNATURE16 ? 0 : Header.page_off_shift);

                if(objectPageTableEntries[objectTableEntries[i].PageTableIndex - 1]
                  .Flags.HasFlag(PageTableAttributes.IteratedDataPage))
                    sections[i].Offset =
                        (objectPageTableEntries[objectTableEntries[i].PageTableIndex - 1].PageDataOffset << shift) +
                        Header.obj_iter_pages_off;
                else if(objectPageTableEntries[objectTableEntries[i].PageTableIndex - 1]
                       .Flags.HasFlag(PageTableAttributes.LegalPhysicalPage))
                    sections[i].Offset =
                        (objectPageTableEntries[objectTableEntries[i].PageTableIndex - 1].PageDataOffset << shift) +
                        Header.data_pages_off;
                else sections[i].Offset = 0;

                sections[i].Size = 0;
                for(int j = 0; j < objectTableEntries[i].PageTableEntries; j++)
                    sections[i].Size += objectPageTableEntries[j + objectTableEntries[i].PageTableIndex - 1].DataSize;

                if(sections[i].Offset + sections[i].Size > BaseStream.Length)
                    sections[i].Size = BaseStream.Length - sections[i].Offset;
            }

            if(winrsrcSections > 0)
                sections[sections.Length - debugSections - winrsrcSections] = new Segment
                {
                    Name   = ".rsrc",
                    Size   = Header.win_res_len,
                    Offset = Header.win_res_off
                };

            if(debugSections > 0)
                sections[sections.Length - debugSections] = new Segment
                {
                    Name   = ".debug",
                    Size   = Header.debug_info_len,
                    Offset = Header.debug_info_off
                };

            // It only contains a RT_VERSION resource prefixed by some 12-byte header I can't find information about, so let's just skip it.
            if(winrsrcSections > 0)
            {
                buffer              = new byte[Header.win_res_len];
                BaseStream.Position = Header.win_res_off + 12;
                BaseStream.Read(buffer, 0, buffer.Length);
                WinVersion = new NE.Version(buffer);
                strings.AddRange(from s in WinVersion.StringsByLanguage from k in s.Value select k.Value);
            }

            resources           = new ResourceTableEntry[Header.resource_entries];
            BaseStream.Position = Header.resource_table_off + BaseExecutable.Header.new_offset;
            buffer              = new byte[Marshal.SizeOf(typeof(ResourceTableEntry))];
            for(int i = 0; i < resources.Length; i++)
            {
                BaseStream.Read(buffer, 0, buffer.Length);
                resources[i] = BigEndianMarshal.ByteArrayToStructureLittleEndian<ResourceTableEntry>(buffer);
            }

            SortedDictionary<ResourceTypes, List<NE.Resource>> os2Resources =
                new SortedDictionary<ResourceTypes, List<NE.Resource>>();

            for(int i = 0; i < resources.Length; i++)
            {
                os2Resources.TryGetValue(resources[i].type, out List<NE.Resource> thisResourceType);

                if(thisResourceType == null) thisResourceType = new List<NE.Resource>();

                NE.Resource thisResource = new NE.Resource
                {
                    id         = resources[i].id,
                    name       = $"{resources[i].id}",
                    flags      = 0,
                    dataOffset = (uint)(sections[resources[i].obj_no - 1].Offset + resources[i].offset),
                    length     = resources[i].size
                };

                thisResource.data   = new byte[thisResource.length];
                BaseStream.Position = thisResource.dataOffset;
                BaseStream.Read(thisResource.data, 0, thisResource.data.Length);

                thisResourceType.Add(thisResource);
                os2Resources.Remove(resources[i].type);
                os2Resources.Add(resources[i].type, thisResourceType);
            }

            if(os2Resources.Count > 0)
            {
                neFormatResourceTable = new NE.ResourceTable();
                int counter = 0;
                neFormatResourceTable.types = new NE.ResourceType[os2Resources.Count];
                foreach(KeyValuePair<ResourceTypes, List<NE.Resource>> kvp in os2Resources)
                {
                    neFormatResourceTable.types[counter].count     = (ushort)kvp.Value.Count;
                    neFormatResourceTable.types[counter].id        = (ushort)kvp.Key;
                    neFormatResourceTable.types[counter].name      = NE.ResourceIdToNameOs2((ushort)kvp.Key);
                    neFormatResourceTable.types[counter].resources = kvp.Value.OrderBy(r => r.id).ToArray();
                    counter++;
                }

                foreach(NE.ResourceType rtype in neFormatResourceTable.types)
                {
                    if(rtype.name != "RT_STRING") continue;

                    strings.AddRange(NE.GetOs2Strings(rtype));
                }
            }

            Segments = sections;
            Strings  = strings;
        }

        /// <summary>
        ///     Identifies if the specified executable is a Microsoft/IBM Linear EXecutable
        /// </summary>
        /// <returns><c>true</c> if the specified executable is a Microsoft/IBM Linear EXecutable, <c>false</c> otherwise.</returns>
        /// <param name="path">Executable path.</param>
        public static bool Identify(string path)
        {
            FileStream baseStream     = File.Open(path, FileMode.Open, FileAccess.Read);
            MZ         baseExecutable = new MZ(baseStream);
            if(!baseExecutable.Recognized) return false;

            if(baseExecutable.Header.new_offset >= baseStream.Length) return false;

            baseStream.Seek(baseExecutable.Header.new_offset, SeekOrigin.Begin);
            byte[] buffer = new byte[Marshal.SizeOf(typeof(LXHeader))];
            baseStream.Read(buffer, 0, buffer.Length);
            IntPtr hdrPtr = Marshal.AllocHGlobal(buffer.Length);
            Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
            LXHeader header = (LXHeader)Marshal.PtrToStructure(hdrPtr, typeof(LXHeader));
            Marshal.FreeHGlobal(hdrPtr);
            return header.signature == SIGNATURE || header.signature == SIGNATURE16;
        }

        /// <summary>
        ///     Identifies if the specified executable is a Microsoft/IBM Linear EXecutable
        /// </summary>
        /// <returns><c>true</c> if the specified executable is a Microsoft/IBM Linear EXecutable, <c>false</c> otherwise.</returns>
        /// <param name="stream">Stream containing the executable.</param>
        public static bool Identify(FileStream stream)
        {
            FileStream baseStream     = stream;
            MZ         baseExecutable = new MZ(baseStream);
            if(!baseExecutable.Recognized) return false;

            if(baseExecutable.Header.new_offset >= baseStream.Length) return false;

            baseStream.Seek(baseExecutable.Header.new_offset, SeekOrigin.Begin);
            byte[] buffer = new byte[Marshal.SizeOf(typeof(LXHeader))];
            baseStream.Read(buffer, 0, buffer.Length);
            IntPtr hdrPtr = Marshal.AllocHGlobal(buffer.Length);
            Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
            LXHeader header = (LXHeader)Marshal.PtrToStructure(hdrPtr, typeof(LXHeader));
            Marshal.FreeHGlobal(hdrPtr);
            return header.signature == SIGNATURE || header.signature == SIGNATURE16;
        }
    }
}