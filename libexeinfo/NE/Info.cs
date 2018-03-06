//
// Info.cs
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
using System.Text;
using libexeinfo.Os2;

namespace libexeinfo
{
    public partial class NE
    {
        public string Information
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(BaseExecutable.Information);
                sb.AppendLine("New Executable (NE):");
                if(!string.IsNullOrEmpty(ModuleName)) sb.AppendFormat("\tModule name: {0}", ModuleName).AppendLine();
                if(!string.IsNullOrEmpty(ModuleDescription))
                    sb.AppendFormat("\tModule description: {0}", ModuleDescription).AppendLine();
                sb.AppendFormat("\tFile's CRC: 0x{0:X8}",        Header.crc).AppendLine();
                sb.AppendFormat("\tLinker version: {0}.{1}",     Header.linker_major, Header.linker_minor).AppendLine();
                if(Header.program_flags.HasFlag(ProgramFlags.SingleDGroup) &&
                   !Header.program_flags.HasFlag(ProgramFlags.MultipleDGroup))
                    sb.AppendLine("\tApplication uses a single shared DGroup");
                else if(!Header.program_flags.HasFlag(ProgramFlags.SingleDGroup) &&
                        Header.program_flags.HasFlag(ProgramFlags.MultipleDGroup))
                    sb.AppendLine("\tApplication uses a multiple DGroup");
                else if(Header.program_flags.HasFlag(ProgramFlags.SingleDGroup) &&
                        Header.program_flags.HasFlag(ProgramFlags.MultipleDGroup))
                    sb.AppendLine("\tApplication indicates an incorrect DGroup value");
                else if(!Header.program_flags.HasFlag(ProgramFlags.SingleDGroup) &&
                        !Header.program_flags.HasFlag(ProgramFlags.MultipleDGroup))
                    sb.AppendLine("\tApplication does not use DGroup");
                if(Header.program_flags.HasFlag(ProgramFlags.GlobalInit))
                    sb.AppendLine("\tApplication uses global initialization");
                if(Header.program_flags.HasFlag(ProgramFlags.ProtectedMode))
                    sb.AppendLine("\tApplication uses protected mode");
                if(Header.program_flags.HasFlag(ProgramFlags.i86))
                    sb.AppendLine("\tApplication uses 8086 instructions");
                if(Header.program_flags.HasFlag(ProgramFlags.i286))
                    sb.AppendLine("\tApplication uses 80286 instructions");
                if(Header.program_flags.HasFlag(ProgramFlags.i386))
                    sb.AppendLine("\tApplication uses 80386 instructions");
                if(Header.program_flags.HasFlag(ProgramFlags.i87))
                    sb.AppendLine("\tApplication uses floating point instructions");

                switch(Header.target_os)
                {
                    case TargetOS.OS2:
                        sb.AppendLine("\tOS/2 application");
                        if(Header.os_major > 0)
                            sb.AppendFormat("\tApplication requires OS/2 {0}.{1} to run", Header.os_major,
                                            Header.os_minor).AppendLine();
                        else sb.AppendLine("\tApplication requires OS/2 1.0 to run");
                        if(Header.application_flags.HasFlag(ApplicationFlags.FullScreen) &&
                           !Header.application_flags.HasFlag(ApplicationFlags.GUICompatible))
                            sb.AppendLine("\tApplication is full screen, unaware of Presentation Manager");
                        else if(!Header.application_flags.HasFlag(ApplicationFlags.FullScreen) &&
                                Header.application_flags.HasFlag(ApplicationFlags.GUICompatible))
                            sb.AppendLine("\tApplication is aware of Presentation Manager, but doesn't use it");
                        else if(Header.application_flags.HasFlag(ApplicationFlags.FullScreen) &&
                                Header.application_flags.HasFlag(ApplicationFlags.GUICompatible))
                            sb.AppendLine("\tApplication uses Presentation Manager");
                        if(Header.ExecutableFlags.HasFlag(ExecutableFlags.LongFilename))
                            sb.AppendLine("\tApplication supports long filenames");
                        if(Header.ExecutableFlags.HasFlag(ExecutableFlags.ProtectedMode2))
                            sb.AppendLine("\tApplication uses OS/2 2.x protected mode");
                        if(Header.ExecutableFlags.HasFlag(ExecutableFlags.ProportionalFonts))
                            sb.AppendLine("\tApplication uses OS/2 2.x proportional fonts");
                        if(Header.ExecutableFlags.HasFlag(ExecutableFlags.GangloadArea))
                            sb.AppendFormat("\tGangload area starts at {0} an runs for {1} bytes",
                                            Header.return_thunks_offset, Header.segment_reference_thunks).AppendLine();
                        else
                        {
                            sb.AppendFormat("\tReturn thunks are at: {0}", Header.return_thunks_offset)
                              .AppendLine();
                            sb.AppendFormat("\tSegment reference thunks are at: {0}", Header.segment_reference_thunks)
                              .AppendLine();
                        }

                        break;
                    case TargetOS.Windows:
                    case TargetOS.Win32:
                    case TargetOS.Unknown:
                        switch(Header.target_os)
                        {
                            case TargetOS.Windows:
                            case TargetOS.Unknown:
                                sb.AppendLine("\t16-bit Windows application");
                                break;
                            case TargetOS.Win32:
                                sb.AppendLine("\t32-bit Windows application");
                                break;
                        }

                        if(Header.os_major > 0)
                            sb.AppendFormat("\tApplication requires Windows {0}.{1} to run", Header.os_major,
                                            Header.os_minor).AppendLine();
                        else
                            switch(Header.target_os)
                            {
                                case TargetOS.Windows:
                                    sb.AppendLine("\tApplication requires Windows 2.0 to run");
                                    break;
                                case TargetOS.Unknown:
                                    sb.AppendLine("\tApplication requires Windows 1.0 to run");
                                    break;
                            }
                        if(Header.application_flags.HasFlag(ApplicationFlags.FullScreen) &&
                           !Header.application_flags.HasFlag(ApplicationFlags.GUICompatible))
                            sb.AppendLine("\tApplication is full screen, unaware of Windows");
                        else if(!Header.application_flags.HasFlag(ApplicationFlags.FullScreen) &&
                                Header.application_flags.HasFlag(ApplicationFlags.GUICompatible))
                            sb.AppendLine("\tApplication is aware of Windows, but doesn't use it");
                        else if(Header.application_flags.HasFlag(ApplicationFlags.FullScreen) &&
                                Header.application_flags.HasFlag(ApplicationFlags.GUICompatible))
                            sb.AppendLine("\tApplication uses Windows");
                        sb.AppendFormat("\tReturn thunks are at: {0}", Header.return_thunks_offset)
                          .AppendLine();
                        sb.AppendFormat("\tSegment reference thunks are at: {0}", Header.segment_reference_thunks)
                          .AppendLine();
                        break;
                    case TargetOS.DOS:
                        sb.AppendLine("\tDOS application");
                        sb.AppendFormat("\tApplication requires DOS {0}.{1} to run", Header.os_major, Header.os_minor)
                          .AppendLine();
                        if(Header.application_flags.HasFlag(ApplicationFlags.FullScreen) &&
                           !Header.application_flags.HasFlag(ApplicationFlags.GUICompatible))
                            sb.AppendLine("\tApplication is full screen, unaware of Windows");
                        else if(!Header.application_flags.HasFlag(ApplicationFlags.FullScreen) &&
                                Header.application_flags.HasFlag(ApplicationFlags.GUICompatible))
                            sb.AppendLine("\tApplication is aware of Windows, but doesn't use it");
                        else if(Header.application_flags.HasFlag(ApplicationFlags.FullScreen) &&
                                Header.application_flags.HasFlag(ApplicationFlags.GUICompatible))
                            sb.AppendLine("\tApplication uses Windows");
                        sb.AppendFormat("\tReturn thunks are at: {0}", Header.return_thunks_offset)
                          .AppendLine();
                        sb.AppendFormat("\tSegment reference thunks are at: {0}", Header.segment_reference_thunks)
                          .AppendLine();
                        break;
                    case TargetOS.Borland:
                        sb.AppendLine("\tBorland Operating System Services application");
                        sb.AppendFormat("\tApplication requires DOS {0}.{1} to run", Header.os_major, Header.os_minor)
                          .AppendLine();
                        if(Header.application_flags.HasFlag(ApplicationFlags.FullScreen) &&
                           !Header.application_flags.HasFlag(ApplicationFlags.GUICompatible))
                            sb.AppendLine("\tApplication is full screen, unaware of Windows");
                        else if(!Header.application_flags.HasFlag(ApplicationFlags.FullScreen) &&
                                Header.application_flags.HasFlag(ApplicationFlags.GUICompatible))
                            sb.AppendLine("\tApplication is aware of Windows, but doesn't use it");
                        else if(Header.application_flags.HasFlag(ApplicationFlags.FullScreen) &&
                                Header.application_flags.HasFlag(ApplicationFlags.GUICompatible))
                            sb.AppendLine("\tApplication uses Windows");
                        sb.AppendFormat("\tReturn thunks are at: {0}", Header.return_thunks_offset)
                          .AppendLine();
                        sb.AppendFormat("\tSegment reference thunks are at: {0}", Header.segment_reference_thunks)
                          .AppendLine();
                        break;
                    default:
                        sb.AppendFormat("\tApplication for unknown OS {0}", (byte)Header.target_os)
                          .AppendLine();
                        sb.AppendFormat("\tApplication requires OS {0}.{1} to run", Header.os_major, Header.os_minor)
                          .AppendLine();
                        sb.AppendFormat("\tReturn thunks are at: {0}", Header.return_thunks_offset)
                          .AppendLine();
                        sb.AppendFormat("\tSegment reference thunks are at: {0}", Header.segment_reference_thunks)
                          .AppendLine();
                        break;
                }

                if(Header.application_flags.HasFlag(ApplicationFlags.Errors)) sb.AppendLine("\tExecutable has errors");
                if(Header.application_flags.HasFlag(ApplicationFlags.NonConforming))
                    sb.AppendLine("\tExecutable is non conforming");
                if(Header.application_flags.HasFlag(ApplicationFlags.DLL))
                    sb.AppendLine("\tExecutable is a dynamic library or a driver");

                sb.AppendFormat("\tMinimum code swap area: {0} bytes",      Header.minimum_swap_area).AppendLine();
                sb.AppendFormat("\tFile alignment shift: {0}",              1 << Header.alignment_shift).AppendLine();
                sb.AppendFormat("\tInitial local heap should be {0} bytes", Header.initial_heap).AppendLine();
                sb.AppendFormat("\tInitial stack size should be {0} bytes", Header.initial_stack).AppendLine();
                sb.AppendFormat("\tCS:IP entry point: {0:X4}:{1:X4}", (Header.entry_point & 0xFFFF0000) >> 16,
                                Header.entry_point                                        & 0xFFFF).AppendLine();
                if(!Header.application_flags.HasFlag(ApplicationFlags.DLL))
                    sb.AppendFormat("\tSS:SP initial stack pointer: {0:X4}:{1:X4}",
                                    (Header.stack_pointer & 0xFFFF0000) >> 16, Header.stack_pointer & 0xFFFF)
                      .AppendLine();
                sb.AppendFormat("\tEntry table starts at {0} and runs for {1} bytes", Header.entry_table_offset,
                                Header.entry_table_length).AppendLine();
                sb.AppendFormat("\tSegment table starts at {0} and contain {1} segments", Header.segment_table_offset,
                                Header.segment_count).AppendLine();
                sb.AppendFormat("\tModule reference table starts at {0} and contain {1} references",
                                Header.module_reference_offset, Header.reference_count).AppendLine();
                sb.AppendFormat("\tNon-resident names table starts at {0} and runs for {1} bytes",
                                Header.nonresident_names_offset, Header.nonresident_table_size).AppendLine();
                sb.AppendFormat("\tResources table starts at {0} and contains {1} entries",
                                Header.resource_table_offset, Header.resource_entries).AppendLine();
                sb.AppendFormat("\tResident names table starts at {0}", Header.resident_names_offset).AppendLine();
                sb.AppendFormat("\tImported names table starts at {0}", Header.imported_names_offset).AppendLine();
                if(segments != null && segments.Length > 0)
                {
                    sb.AppendLine("\tSegments:");
                    for(int i = 0; i < segments.Length; i++)
                    {
                        sb.AppendFormat("\t\tSegment {0}:", i + 1).AppendLine();
                        sb.AppendFormat("\t\t\tStarts at {0} and is {1} bytes long",
                                        segments[i].dwLogicalSectorOffset * 16, segments[i].dwSegmentLength)
                          .AppendLine();
                        sb.AppendFormat("\t\t\tNeeds at least {0} bytes allocated", segments[i].dwMinimumAllocation)
                          .AppendLine();
                        sb.AppendFormat("\t\t\tType: {0}", (SegmentType)(segments[i].dwFlags & SEGMENT_TYPE_MASK))
                          .AppendLine();
                        sb.AppendFormat("\t\t\tFlags: {0}", (SegmentFlags)(segments[i].dwFlags & SEGMENT_FLAGS_MASK))
                          .AppendLine();
                        sb.AppendFormat("\t\t\tI/O privilege: {0}", (segments[i].dwFlags & SEGMENT_IOPRVL_MASK) >> 10)
                          .AppendLine();
                        sb.AppendFormat("\t\t\tDiscard priority: {0}",
                                        (segments[i].dwFlags & SEGMENT_DISCARD_MASK) >> 12).AppendLine();
                    }
                }

                if(ImportedNames != null)
                {
                    sb.AppendLine("\tImported names:");
                    foreach(string name in ImportedNames) sb.AppendFormat("\t\t{0}", name).AppendLine();
                }

                if(ResidentNames != null)
                {
                    sb.AppendLine("\tResident names:");
                    foreach(ResidentName name in ResidentNames)
                        sb.AppendFormat("\t\t{0} at index {1}", name.name, name.entryTableIndex).AppendLine();
                }

                if(NonResidentNames != null)
                {
                    sb.AppendLine("\tNon-resident names:");
                    foreach(ResidentName name in NonResidentNames)
                        sb.AppendFormat("\t\t{0} at index {1}", name.name, name.entryTableIndex).AppendLine();
                }

                if(Resources.types != null)
                {
                    sb.AppendLine("\tResources:");
                    for(int i = 0; i < Resources.types.Length; i++)
                    {
                        sb.AppendFormat("\t\tType {0} has {1} items:", Resources.types[i].name,
                                        Resources.types[i].resources.Length).AppendLine();
                        for(int j = 0; j < Resources.types[i].resources.Length; j++)
                        {
                            bool intId = int.TryParse(Resources.types[i].resources[j].name, out _);
                            sb.AppendFormat("\t\t\t{0}: {1}, starts at {2}, {3} bytes, flags: {4}",
                                            intId ? "ID" : "Name", Resources.types[i].resources[j].name,
                                            Resources.types[i].resources[j].dataOffset,
                                            Resources.types[i].resources[j].data.Length,
                                            (ResourceFlags)((ushort)Resources.types[i].resources[j].flags &
                                                            KNOWN_RSRC_FLAGS)).AppendLine();
                        }
                    }
                }

                return sb.ToString();
            }
        }

        static ResidentName[] GetResidentStrings(Stream stream, uint neStart, ushort tableOff, ushort upperLimit)
        {
            if(tableOff >= upperLimit) return null;

            List<ResidentName> names = new List<ResidentName>();
            byte               stringSize;
            byte[]             nameString;
            byte[]             DW = new byte[2];

            long oldPosition = stream.Position;

            stream.Position = neStart          + tableOff;
            while(stream.Position < upperLimit + neStart)
            {
                stringSize = (byte)stream.ReadByte();

                if(stringSize == 0) break;

                nameString = new byte[stringSize];
                stream.Read(nameString, 0, stringSize);
                stream.Read(DW,         0, 2);

                names.Add(new ResidentName
                {
                    name            = Encoding.ASCII.GetString(nameString),
                    entryTableIndex = BitConverter.ToUInt16(DW, 0)
                });
            }

            stream.Position = oldPosition;

            return names.Count > 0 ? names.ToArray() : null;
        }

        public static ResourceTable GetResources(Stream stream, uint neStart, ushort tableOff, ushort upperLimit)
        {
            long   oldPosition = stream.Position;
            byte[] DW          = new byte[2];
            byte[] DD          = new byte[4];

            stream.Position     = neStart + tableOff;
            ResourceTable table = new ResourceTable();
            stream.Read(DW, 0, 2);
            table.alignment_shift = BitConverter.ToUInt16(DW, 0);

            List<ResourceType> types = new List<ResourceType>();

            while(stream.Position < upperLimit + neStart)
            {
                ResourceType type = new ResourceType();
                stream.Read(DW, 0, 2);
                type.id = BitConverter.ToUInt16(DW, 0);
                if(type.id == 0) break;

                stream.Read(DW, 0, 2);
                type.count = BitConverter.ToUInt16(DW, 0);
                stream.Read(DD, 0, 4);
                type.reserved = BitConverter.ToUInt32(DD, 0);

                type.resources = new Resource[type.count];
                for(int i = 0; i < type.count; i++)
                {
                    type.resources[i] = new Resource();
                    stream.Read(DW, 0, 2);
                    type.resources[i].dataOffset = BitConverter.ToUInt16(DW, 0);
                    stream.Read(DW, 0, 2);
                    type.resources[i].length = BitConverter.ToUInt16(DW, 0);
                    stream.Read(DW, 0, 2);
                    type.resources[i].flags = (ResourceFlags)BitConverter.ToUInt16(DW, 0);
                    stream.Read(DW, 0, 2);
                    type.resources[i].id = BitConverter.ToUInt16(DW, 0);
                    stream.Read(DD, 0, 4);
                    type.resources[i].reserved = BitConverter.ToUInt32(DD, 0);
                }

                types.Add(type);
            }

            table.types = types.ToArray();

            for(int t = 0; t < table.types.Length; t++)
            {
                if((table.types[t].id & 0x8000) == 0)
                {
                    stream.Position = neStart + tableOff + table.types[t].id;
                    byte   len      = (byte)stream.ReadByte();
                    byte[] str      = new byte[len];
                    stream.Read(str, 0, len);
                    table.types[t].name = Encoding.ASCII.GetString(str);
                }
                else table.types[t].name = ResourceIdToName(table.types[t].id);

                for(int r = 0; r < table.types[t].resources.Length; r++)
                {
                    if((table.types[t].resources[r].id & 0x8000) == 0)
                    {
                        stream.Position = neStart + tableOff + table.types[t].resources[r].id;
                        byte   len      = (byte)stream.ReadByte();
                        byte[] str      = new byte[len];
                        stream.Read(str, 0, len);
                        table.types[t].resources[r].name = Encoding.ASCII.GetString(str);
                    }
                    else table.types[t].resources[r].name = $"{table.types[t].resources[r].id & 0x7FFF}";

                    table.types[t].resources[r].data =
                        new byte[table.types[t].resources[r].length          * (1 << table.alignment_shift)];
                    stream.Position = table.types[t].resources[r].dataOffset * (1 << table.alignment_shift);
                    stream.Read(table.types[t].resources[r].data, 0, table.types[t].resources[r].data.Length);
                }
            }

            stream.Position = oldPosition;

            return table;
        }
    }
}