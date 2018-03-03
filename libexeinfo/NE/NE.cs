//
// NE.cs
//
// Author:
//       Natalia Portillo <claunia@claunia.com>
//
// Copyright (c) 2017 Copyright © Claunia.com
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

namespace libexeinfo
{
    /// <summary>
    ///     Represents a Microsoft New Executable
    /// </summary>
    public partial class NE : IExecutable
    {
        MZ BaseExecutable;
        /// <summary>
        ///     Header for this executable
        /// </summary>
        public NEHeader       Header;
        string[]              ImportedNames;
        string                ModuleDescription;
        string                ModuleName;
        public ResidentName[] NonResidentNames;
        public ResidentName[] ResidentNames;
        public ResourceTable  Resources;
        SegmentEntry[]        segments;
        public Version[]      Versions;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:libexeinfo.NE" /> class.
        /// </summary>
        /// <param name="path">Executable path.</param>
        public NE(string path)
        {
            BaseStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            Initialize();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:libexeinfo.NE" /> class.
        /// </summary>
        /// <param name="stream">Stream containing the executable.</param>
        public NE(Stream stream)
        {
            BaseStream = stream;
            Initialize();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:libexeinfo.NE" /> class.
        /// </summary>
        /// <param name="data">Stream containing the executable.</param>
        public NE(byte[] data)
        {
            BaseStream = new MemoryStream(data);
            Initialize();
        }

        public Stream                    BaseStream    { get; }
        public bool                      IsBigEndian   => false;
        public bool                      Recognized    { get; private set; }
        public string                    Type          { get; private set; }
        public IEnumerable<Architecture> Architectures =>
            new[]
            {
                Header.target_os == TargetOS.Win32 || Header.program_flags.HasFlag(ProgramFlags.i386)
                    ? Architecture.I386
                    : Header.target_os == TargetOS.OS2 || Header.program_flags.HasFlag(ProgramFlags.i286)
                        ? Architecture.I286
                        : Architecture.I86
            };
        public OperatingSystem     RequiredOperatingSystem { get; private set; }
        public IEnumerable<string> Strings                 { get; }
        public IEnumerable<Segment> Segments { get; private set; }

        void Initialize()
        {
            Recognized = false;

            if(BaseStream == null) return;

            BaseExecutable = new MZ(BaseStream);
            if(!BaseExecutable.Recognized) return;

            if(BaseExecutable.Header.new_offset >= BaseStream.Length) return;

            BaseStream.Seek(BaseExecutable.Header.new_offset, SeekOrigin.Begin);
            byte[] buffer = new byte[Marshal.SizeOf(typeof(NEHeader))];
            BaseStream.Read(buffer, 0, buffer.Length);
            IntPtr hdrPtr = Marshal.AllocHGlobal(buffer.Length);
            Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
            Header = (NEHeader)Marshal.PtrToStructure(hdrPtr, typeof(NEHeader));
            Marshal.FreeHGlobal(hdrPtr);
            if(Header.signature != SIGNATURE) return;

            Recognized = true;
            Type       = "New Executable (NE)";

            OperatingSystem reqOs = new OperatingSystem();

            switch(Header.target_os)
            {
                case TargetOS.OS2:
                    reqOs.Name = "OS/2";
                    if(Header.os_major > 0)
                    {
                        reqOs.MajorVersion = Header.os_major;
                        reqOs.MinorVersion = Header.os_minor;
                    }
                    else
                    {
                        reqOs.MajorVersion = 1;
                        reqOs.MinorVersion = 0;
                    }

                    if(Header.application_flags.HasFlag(ApplicationFlags.FullScreen) &&
                       !Header.application_flags.HasFlag(ApplicationFlags.GUICompatible) ||
                       !Header.application_flags.HasFlag(ApplicationFlags.FullScreen) &&
                       Header.application_flags.HasFlag(ApplicationFlags.GUICompatible)) reqOs.Subsystem = "Console";
                    else if(Header.application_flags.HasFlag(ApplicationFlags.FullScreen) &&
                            Header.application_flags.HasFlag(ApplicationFlags.GUICompatible))
                        reqOs.Subsystem = "Presentation Manager";
                    break;
                case TargetOS.Windows:
                case TargetOS.Win32:
                case TargetOS.Unknown:
                    reqOs.Name = "Windows";
                    if(Header.os_major > 0)
                    {
                        reqOs.MajorVersion = Header.os_major;
                        reqOs.MinorVersion = Header.os_minor;
                    }
                    else
                        switch(Header.target_os)
                        {
                            case TargetOS.Windows:
                                reqOs.MajorVersion = 2;
                                reqOs.MinorVersion = 0;
                                break;
                            case TargetOS.Unknown:
                                reqOs.MajorVersion = 1;
                                reqOs.MinorVersion = 0;
                                break;
                        }

                    break;
                case TargetOS.DOS:
                case TargetOS.Borland:
                    reqOs.Name                                               = "DOS";
                    reqOs.MajorVersion                                       = Header.os_major;
                    reqOs.MinorVersion                                       = Header.os_minor;
                    if(Header.target_os == TargetOS.Borland) reqOs.Subsystem = "Borland Operating System Services";
                    break;
                default:
                    reqOs.Name         = $"Unknown code {(byte)Header.target_os}";
                    reqOs.MajorVersion = Header.os_major;
                    reqOs.MinorVersion = Header.os_minor;
                    break;
            }

            RequiredOperatingSystem = reqOs;

            if(Header.segment_count                                           > 0 && Header.segment_table_offset > 0 &&
               Header.segment_table_offset + BaseExecutable.Header.new_offset < BaseStream.Length)
            {
                BaseStream.Position = Header.segment_table_offset + BaseExecutable.Header.new_offset;
                segments            = new SegmentEntry[Header.segment_count];
                for(int i = 0; i < segments.Length; i++)
                {
                    buffer = new byte[Marshal.SizeOf(typeof(SegmentEntry))];
                    BaseStream.Read(buffer, 0, buffer.Length);
                    segments[i] = BigEndianMarshal.ByteArrayToStructureLittleEndian<SegmentEntry>(buffer);
                }
            }

            // Some executable indicates 0 entries, some indicate a table start and no limit, will need to explore till next item
            ushort resourceUpperLimit = ushort.MaxValue;

            if(Header.entry_table_offset      >= Header.resource_table_offset &&
               Header.entry_table_offset      <= resourceUpperLimit) resourceUpperLimit = Header.entry_table_offset;
            if(Header.segment_table_offset    >= Header.resource_table_offset &&
               Header.segment_table_offset    <= resourceUpperLimit) resourceUpperLimit = Header.segment_table_offset;
            if(Header.module_reference_offset >= Header.resource_table_offset &&
               Header.module_reference_offset <= resourceUpperLimit)
                resourceUpperLimit = Header.module_reference_offset;
            if(Header.nonresident_names_offset >= Header.resource_table_offset &&
               Header.nonresident_names_offset <= resourceUpperLimit)
                resourceUpperLimit = (ushort)Header.nonresident_names_offset;
            if(Header.resident_names_offset >= Header.resource_table_offset &&
               Header.resident_names_offset <= resourceUpperLimit) resourceUpperLimit = Header.resident_names_offset;
            if(Header.imported_names_offset >= Header.resource_table_offset &&
               Header.imported_names_offset <= resourceUpperLimit) resourceUpperLimit = Header.imported_names_offset;

            if(Header.resource_table_offset < resourceUpperLimit && Header.resource_table_offset != 0)
                if(Header.target_os         == TargetOS.Windows || Header.target_os              == TargetOS.Win32)
                {
                    Resources = GetResources(BaseStream, BaseExecutable.Header.new_offset, Header.resource_table_offset,
                                             resourceUpperLimit);

                    for(int t = 0; t < Resources.types.Length; t++)
                        Resources.types[t].resources = Resources.types[t].resources.OrderBy(r => r.name).ToArray();

                    Resources.types = Resources.types.OrderBy(t => t.name).ToArray();

                    Versions = GetVersions().ToArray();
                }
                else if(Header.target_os == TargetOS.OS2 && segments != null && Header.resource_entries > 0)
                {
                    BaseStream.Position = BaseExecutable.Header.new_offset + Header.resource_table_offset;
                    buffer              = new byte[Header.resource_entries * 4];
                    BaseStream.Read(buffer, 0, buffer.Length);
                    Os2ResourceTableEntry[] entries = new Os2ResourceTableEntry[Header.resource_entries];
                    for(int i = 0; i < entries.Length; i++)
                    {
                        entries[i].etype = BitConverter.ToUInt16(buffer, i * 4 + 0);
                        entries[i].ename = BitConverter.ToUInt16(buffer, i * 4 + 2);
                    }

                    SegmentEntry[] resourceSegments = new SegmentEntry[Header.resource_entries];
                    Array.Copy(segments, Header.segment_count - Header.resource_entries, resourceSegments, 0,
                               Header.resource_entries);
                    SegmentEntry[] realSegments = new SegmentEntry[Header.segment_count - Header.resource_entries];
                    Array.Copy(segments, 0, realSegments, 0, realSegments.Length);
                    segments = realSegments;

                    SortedDictionary<ushort, List<Resource>> os2resources =
                        new SortedDictionary<ushort, List<Resource>>();

                    for(int i = 0; i < entries.Length; i++)
                    {
                        os2resources.TryGetValue(entries[i].etype, out List<Resource> thisResourceType);

                        if(thisResourceType == null) thisResourceType = new List<Resource>();

                        Resource thisResource = new Resource
                        {
                            id         = entries[i].ename,
                            name       = $"{entries[i].ename}",
                            flags      = (ResourceFlags)resourceSegments[i].dwFlags,
                            dataOffset = (uint)(resourceSegments[i].dwLogicalSectorOffset * 16),
                            length     = resourceSegments[i].dwSegmentLength
                        };

                        if(thisResource.length == 0)
                            thisResource.length = 65536;
                        if(thisResource.dataOffset == 0)
                            thisResource.dataOffset = 65536;
                        if((resourceSegments[i].dwFlags & (ushort)SegmentFlags.Huge) == (ushort)SegmentFlags.Huge)
                            thisResource.length *= 16;
                        thisResource.data       =  new byte[thisResource.length];
                        BaseStream.Position     =  thisResource.dataOffset;
                        BaseStream.Read(thisResource.data, 0, thisResource.data.Length);

                        thisResourceType.Add(thisResource);
                        os2resources.Remove(entries[i].etype);
                        os2resources.Add(entries[i].etype, thisResourceType);
                    }

                    if(os2resources.Count > 0)
                    {
                        Resources       = new ResourceTable();
                        int counter     = 0;
                        Resources.types = new ResourceType[os2resources.Count];
                        foreach(KeyValuePair<ushort, List<Resource>> kvp in os2resources)
                        {
                            Resources.types[counter].count     = (ushort)kvp.Value.Count;
                            Resources.types[counter].id        = kvp.Key;
                            Resources.types[counter].name      = ResourceIdToNameOs2(kvp.Key);
                            Resources.types[counter].resources = kvp.Value.OrderBy(r => r.id).ToArray();
                            counter++;
                        }
                    }
                }

            resourceUpperLimit = ushort.MaxValue;

            if(Header.entry_table_offset       >= Header.module_reference_offset &&
               Header.entry_table_offset       <= resourceUpperLimit) resourceUpperLimit = Header.entry_table_offset;
            if(Header.segment_table_offset     >= Header.module_reference_offset &&
               Header.segment_table_offset     <= resourceUpperLimit) resourceUpperLimit = Header.segment_table_offset;
            if(Header.resource_table_offset    >= Header.module_reference_offset &&
               Header.resource_table_offset    <= resourceUpperLimit) resourceUpperLimit = Header.resource_table_offset;
            if(Header.nonresident_names_offset >= Header.module_reference_offset &&
               Header.nonresident_names_offset <= resourceUpperLimit)
                resourceUpperLimit = (ushort)Header.nonresident_names_offset;
            if(Header.imported_names_offset >= Header.module_reference_offset &&
               Header.imported_names_offset <= resourceUpperLimit) resourceUpperLimit = Header.imported_names_offset;

            if(Header.module_reference_offset < resourceUpperLimit && Header.module_reference_offset != 0 &&
               Header.reference_count         > 0)
            {
                short[] referenceOffsets = new short[Header.reference_count];
                buffer                   = new byte[2];
                BaseStream.Position      = Header.module_reference_offset + BaseExecutable.Header.new_offset;
                for(int i = 0; i < Header.reference_count; i++)
                {
                    BaseStream.Read(buffer, 0, 2);
                    referenceOffsets[i] = BitConverter.ToInt16(buffer, 0);
                }

                ImportedNames = new string[Header.reference_count];
                for(int i = 0; i < Header.reference_count; i++)
                {
                    BaseStream.Position = Header.imported_names_offset + BaseExecutable.Header.new_offset +
                                          referenceOffsets[i];
                    int len = BaseStream.ReadByte();
                    buffer  = new byte[len];
                    BaseStream.Read(buffer, 0, len);
                    ImportedNames[i] = Encoding.ASCII.GetString(buffer);
                }
            }

            resourceUpperLimit = ushort.MaxValue;

            if(Header.entry_table_offset      >= Header.resident_names_offset &&
               Header.entry_table_offset      <= resourceUpperLimit) resourceUpperLimit = Header.entry_table_offset;
            if(Header.segment_table_offset    >= Header.resident_names_offset &&
               Header.segment_table_offset    <= resourceUpperLimit) resourceUpperLimit = Header.segment_table_offset;
            if(Header.module_reference_offset >= Header.resident_names_offset &&
               Header.module_reference_offset <= resourceUpperLimit)
                resourceUpperLimit = Header.module_reference_offset;
            if(Header.nonresident_names_offset >= Header.resident_names_offset &&
               Header.nonresident_names_offset <= resourceUpperLimit)
                resourceUpperLimit = (ushort)Header.nonresident_names_offset;
            if(Header.imported_names_offset >= Header.resident_names_offset &&
               Header.imported_names_offset <= resourceUpperLimit) resourceUpperLimit = Header.imported_names_offset;

            if(Header.resident_names_offset < resourceUpperLimit && Header.resident_names_offset != 0)
            {
                ResidentNames = GetResidentStrings(BaseStream,                   BaseExecutable.Header.new_offset,
                                                   Header.resident_names_offset, resourceUpperLimit);

                if(ResidentNames.Length >= 1)
                {
                    ModuleName = ResidentNames[0].name;

                    if(ResidentNames.Length > 1)
                    {
                        ResidentName[] newResidentNames = new ResidentName[ResidentNames.Length - 1];
                        Array.Copy(ResidentNames, 1, newResidentNames, 0, ResidentNames.Length  - 1);
                        ResidentNames = newResidentNames;
                    }
                    else ResidentNames = null;
                }
            }

            if(Header.nonresident_table_size > 0)
            {
                NonResidentNames = GetResidentStrings(BaseStream, Header.nonresident_names_offset, 0,
                                                      (ushort)(Header.nonresident_names_offset +
                                                               Header.nonresident_table_size));

                if(NonResidentNames.Length >= 1)
                {
                    ModuleDescription = NonResidentNames[0].name;

                    if(NonResidentNames.Length > 1)
                    {
                        ResidentName[] newNonResidentNames = new ResidentName[NonResidentNames.Length   - 1];
                        Array.Copy(NonResidentNames, 1, newNonResidentNames, 0, NonResidentNames.Length - 1);
                        NonResidentNames = newNonResidentNames;
                    }
                    else NonResidentNames = null;
                }
            }

            if(segments == null) return;

            List<Segment> libsegs = new List<Segment>();
            foreach(SegmentEntry seg in segments)
            {
                Segment libseg = new Segment
                {
                    Flags = $"{(SegmentFlags)(seg.dwFlags & SEGMENT_FLAGS_MASK)}",
                    Name  =
                        ((SegmentType)(seg.dwFlags & SEGMENT_TYPE_MASK)) == SegmentType.Code ? ".text" : ".data",
                    Offset = seg.dwLogicalSectorOffset * 16,
                    Size   = seg.dwSegmentLength
                };

                if(Header.target_os == TargetOS.OS2 && (seg.dwFlags & (int)SegmentFlags.Huge) == (int)SegmentFlags.Huge)
                    libseg.Size *= 16;
                
                libsegs.Add(libseg);
            }

            Segments = libsegs.OrderBy(s => s.Offset).ToArray();
        }

        /// <summary>
        ///     Identifies if the specified executable is a Microsoft New Executable
        /// </summary>
        /// <returns><c>true</c> if the specified executable is a Microsoft New Executable, <c>false</c> otherwise.</returns>
        /// <param name="path">Executable path.</param>
        public static bool Identify(string path)
        {
            FileStream BaseStream     = File.Open(path, FileMode.Open, FileAccess.Read);
            MZ         BaseExecutable = new MZ(BaseStream);
            if(!BaseExecutable.Recognized) return false;

            if(BaseExecutable.Header.new_offset >= BaseStream.Length) return false;

            BaseStream.Seek(BaseExecutable.Header.new_offset, SeekOrigin.Begin);
            byte[] buffer = new byte[Marshal.SizeOf(typeof(NEHeader))];
            BaseStream.Read(buffer, 0, buffer.Length);
            IntPtr hdrPtr = Marshal.AllocHGlobal(buffer.Length);
            Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
            NEHeader Header = (NEHeader)Marshal.PtrToStructure(hdrPtr, typeof(NEHeader));
            Marshal.FreeHGlobal(hdrPtr);
            return Header.signature == SIGNATURE;
        }

        /// <summary>
        ///     Identifies if the specified executable is a Microsoft New Executable
        /// </summary>
        /// <returns><c>true</c> if the specified executable is a Microsoft New Executable, <c>false</c> otherwise.</returns>
        /// <param name="stream">Stream containing the executable.</param>
        public static bool Identify(FileStream stream)
        {
            FileStream BaseStream     = stream;
            MZ         BaseExecutable = new MZ(BaseStream);
            if(!BaseExecutable.Recognized) return false;

            if(BaseExecutable.Header.new_offset >= BaseStream.Length) return false;

            BaseStream.Seek(BaseExecutable.Header.new_offset, SeekOrigin.Begin);
            byte[] buffer = new byte[Marshal.SizeOf(typeof(NEHeader))];
            BaseStream.Read(buffer, 0, buffer.Length);
            IntPtr hdrPtr = Marshal.AllocHGlobal(buffer.Length);
            Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
            NEHeader Header = (NEHeader)Marshal.PtrToStructure(hdrPtr, typeof(NEHeader));
            Marshal.FreeHGlobal(hdrPtr);
            return Header.signature == SIGNATURE;
        }
    }
}