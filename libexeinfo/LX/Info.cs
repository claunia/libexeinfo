//
// Info.cs
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
using System.Text;

namespace libexeinfo
{
    public partial class LX
    {
        public string Information
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(BaseExecutable.Information);
                sb.AppendLine(Header.signature == SIGNATURE16 ? "Linear Executable (LE):" : "Linear eXecutable (LX):");
                if(!string.IsNullOrEmpty(ModuleName)) sb.AppendFormat("\tModule name: {0}", ModuleName).AppendLine();
                if(!string.IsNullOrEmpty(ModuleDescription))
                    sb.AppendFormat("\tModule description: {0}", ModuleDescription).AppendLine();

                switch(Header.os_type)
                {
                    case TargetOS.OS2:
                        sb.AppendLine("\tOS/2 application");
                        if(Header.module_flags.HasFlag(ModuleFlags.PMIncompatible) &&
                           !Header.module_flags.HasFlag(ModuleFlags.PMCompatible))
                            sb.AppendLine("\tApplication is full screen, unaware of Presentation Manager");
                        else if(!Header.module_flags.HasFlag(ModuleFlags.PMIncompatible) &&
                                Header.module_flags.HasFlag(ModuleFlags.PMCompatible))
                            sb.AppendLine("\tApplication is aware of Presentation Manager, but doesn't use it");
                        else if(Header.module_flags.HasFlag(ModuleFlags.PMIncompatible) &&
                                Header.module_flags.HasFlag(ModuleFlags.PMCompatible))
                            sb.AppendLine("\tApplication uses Presentation Manager");
                        break;
                    case TargetOS.Windows:
                    case TargetOS.Win32:
                    case TargetOS.Unknown:
                        switch(Header.os_type)
                        {
                            case TargetOS.Windows:
                            case TargetOS.Unknown:
                                sb.AppendLine("\t16-bit Windows application");
                                break;
                            case TargetOS.Win32:
                                sb.AppendLine("\t32-bit Windows application");
                                break;
                        }

                        if(Header.module_flags.HasFlag(ModuleFlags.PMIncompatible) &&
                           !Header.module_flags.HasFlag(ModuleFlags.PMCompatible))
                            sb.AppendLine("\tApplication is full screen, unaware of Windows");
                        else if(!Header.module_flags.HasFlag(ModuleFlags.PMIncompatible) &&
                                Header.module_flags.HasFlag(ModuleFlags.PMCompatible))
                            sb.AppendLine("\tApplication is aware of Windows, but doesn't use it");
                        else if(Header.module_flags.HasFlag(ModuleFlags.PMIncompatible) &&
                                Header.module_flags.HasFlag(ModuleFlags.PMCompatible))
                            sb.AppendLine("\tApplication uses Windows");
                        break;
                    case TargetOS.DOS:
                        sb.AppendLine("\tDOS application");
                        if(Header.module_flags.HasFlag(ModuleFlags.PMIncompatible) &&
                           !Header.module_flags.HasFlag(ModuleFlags.PMCompatible))
                            sb.AppendLine("\tApplication is full screen, unaware of Windows");
                        else if(!Header.module_flags.HasFlag(ModuleFlags.PMIncompatible) &&
                                Header.module_flags.HasFlag(ModuleFlags.PMCompatible))
                            sb.AppendLine("\tApplication is aware of Windows, but doesn't use it");
                        else if(Header.module_flags.HasFlag(ModuleFlags.PMIncompatible) &&
                                Header.module_flags.HasFlag(ModuleFlags.PMCompatible))
                            sb.AppendLine("\tApplication uses Windows");
                        break;
                    default:
                        sb.AppendFormat("\tApplication for unknown OS {0}", (ushort)Header.os_type).AppendLine();
                        break;
                }

                sb.AppendFormat("\tByte ordering: {0}", Header.byte_order == 1 ? "Big-endian" : "Little-Endian")
                  .AppendLine();
                sb.AppendFormat("\tWord ordering: {0}", Header.word_order == 1 ? "Big-endian" : "Little-Endian")
                  .AppendLine();
                sb.AppendFormat("\tFormat level: {0}.{1}", Header.format_major, Header.format_minor).AppendLine();
                sb.AppendFormat("\tExecutable version: {0}.{1}", Header.module_major, Header.module_minor).AppendLine();

                switch(Header.cpu_type)
                {
                    case TargetCpu.i286:
                        sb.AppendLine("\tExecutable requires at least an 80286 processor to run.");
                        break;
                    case TargetCpu.i386:
                        sb.AppendLine("\tExecutable requires at least an 80386 processor to run.");
                        break;
                    case TargetCpu.i486:
                        sb.AppendLine("\tExecutable requires at least an 80486 processor to run.");
                        break;
                    case TargetCpu.Pentium:
                        sb.AppendLine("\tExecutable requires at least a Pentium processor to run.");
                        break;
                    case TargetCpu.i860:
                        sb.AppendLine("\tExecutable requires at least an Intel 860  processor to run.");
                        break;
                    case TargetCpu.N11:
                        sb.AppendLine("\tExecutable requires at least an Intel N11 processor to run.");
                        break;
                    case TargetCpu.MIPS1:
                        sb.AppendLine("\tExecutable requires at least a MIPS I processor to run.");
                        break;
                    case TargetCpu.MIPS2:
                        sb.AppendLine("\tExecutable requires at least a MIPS II processor to run.");
                        break;
                    case TargetCpu.MIPS3:
                        sb.AppendLine("\tExecutable requires at least a MIPS III processor to run.");
                        break;
                    default:
                        sb.AppendFormat("\tExecutable requires unknown cpu with type code {0} to run.",
                                        (ushort)Header.cpu_type).AppendLine();
                        break;
                }

                if(Header.module_flags.HasFlag(ModuleFlags.PerProcessLibrary))
                    sb.AppendLine("\tLibrary should be initialized per-process.");
                if(Header.module_flags.HasFlag(ModuleFlags.PerProcessTermination))
                    sb.AppendLine("\tLibrary should be terminated per-process.");
                if(Header.module_flags.HasFlag(ModuleFlags.InternalFixups))
                    sb.AppendLine("\tInternal fixups have been applied.");
                if(Header.module_flags.HasFlag(ModuleFlags.ExternalFixups))
                    sb.AppendLine("\tExternal fixups have been applied.");
                if(Header.module_flags.HasFlag(ModuleFlags.NotLoadable))
                    sb.AppendLine("\tExecutable is not loadable, it contains errors or is still being linked.");

                if(Header.module_flags.HasFlag(ModuleFlags.VirtualDeviceDriver))
                    sb.AppendLine("\tExecutable is a driver for a virtual device.");
                else if(Header.module_flags.HasFlag(ModuleFlags.PhysicalDeviceDriver))
                    sb.AppendLine("\tExecutable is a driver for a physical device.");
                else if(Header.module_flags.HasFlag(ModuleFlags.ProtectedMemoryLibrary))
                    sb.AppendLine("\tExecutable is a protected mode library.");
                else if(Header.module_flags.HasFlag(ModuleFlags.Library)) sb.AppendLine("\tExecutable is a library.");

                sb.AppendFormat("\tThis executable contains {0} pages", Header.module_pages_no).AppendLine();
                sb.AppendFormat("\tObject number to which the Entry Address is relative: {0}", Header.eip_object)
                  .AppendLine();
                sb.AppendFormat("\tEntry address of module: {0:X8}h", Header.eip).AppendLine();
                sb.AppendFormat("\tObject number to which the ESP is relative: {0}", Header.esp_object).AppendLine();
                sb.AppendFormat("\tStarting stack address of module: {0:X8}h", Header.esp).AppendLine();
                sb.AppendFormat("\tOne page is {0} bytes", Header.page_size).AppendLine();
                sb
                   .AppendFormat(Header.signature == SIGNATURE16 ? "\tLast page size: {0} bytes" : "\tShift left bits for page offsets: {0}",
                                 Header.page_off_shift).AppendLine();
                sb.AppendFormat("\tTotal size of the fixup information: {0}", Header.fixup_size).AppendLine();
                sb.AppendFormat("\tChecksum for fixup information: 0x{0:X8}", Header.fixup_checksum).AppendLine();
                sb.AppendFormat("\tMemory resident tables are {0} bytes long", Header.loader_size).AppendLine();
                sb.AppendFormat("\tChecksum for loader section: 0x{0:X8}", Header.loader_checksum).AppendLine();
                sb.AppendFormat("\tObject table starts at {0} and contains {1} objects", Header.obj_table_off,
                                Header.obj_no).AppendLine();
                sb.AppendFormat("\tObject page table starts at {0}", Header.obj_page_table_off).AppendLine();
                sb.AppendFormat("\tObject iterated pages starts at {0}", Header.obj_iter_pages_off).AppendLine();
                sb.AppendFormat("\tResources table starts at {0} and contains {1} entries", Header.resource_table_off,
                                Header.resource_entries).AppendLine();
                sb.AppendFormat("\tResident name table starts at {0}", Header.resident_names_off).AppendLine();
                sb.AppendFormat("\tEntry table starts at {0}", Header.entry_table_off).AppendLine();
                sb.AppendFormat("\tModule format directives table starts at {0} and contains {1} entries",
                                Header.directives_off, Header.directives_no).AppendLine();
                sb.AppendFormat("\tFixup page table starts at {0}", Header.fixup_page_table_off).AppendLine();
                sb.AppendFormat("\tFixup record table starts at {0}", Header.fixup_record_table_off).AppendLine();
                sb.AppendFormat("\tImport module name table starts at {0} and contains {1} entries",
                                Header.import_module_table_off, Header.import_module_entries).AppendLine();
                sb.AppendFormat("\tImport procedure name table starts at {0}", Header.import_proc_table_off)
                  .AppendLine();
                sb.AppendFormat("\tPer-page checksum table starts at {0}", Header.perpage_checksum_off).AppendLine();
                sb.AppendFormat("\tData pages start at {0}", Header.data_pages_off).AppendLine();
                sb.AppendFormat("\t{0} pages to preload in this executable", Header.preload_pages_no).AppendLine();
                sb.AppendFormat("\tNon-resident names table starts at {0} and runs for {1} bytes",
                                Header.nonresident_name_table_off, Header.nonresident_name_table_len).AppendLine();
                sb.AppendFormat("\tNon-resident name table checksum: 0x{0:X8}", Header.nonresident_name_table_checksum)
                  .AppendLine();
                sb.AppendFormat("\tThe auto data segment object number: {0}", Header.auto_ds_obj_no).AppendLine();
                sb.AppendFormat("\tDebug information starts at {0} and is {1} bytes", Header.debug_info_off,
                                Header.debug_info_len).AppendLine();
                sb.AppendFormat("\tInstance pages in preload section: {0}", Header.instance_preload_no).AppendLine();
                sb.AppendFormat("\tInstance pages in demand section: {0}", Header.instance_demand_no).AppendLine();
                sb.AppendFormat("\tHeap size added to the auto ds object: {0}", Header.heap_size).AppendLine();
                if(Header.signature == SIGNATURE16 && Header.win_res_len > 0)
                {
                    sb.AppendFormat("\tWindows resource starts at {0} and runs for {1} bytes", Header.win_res_off,
                                    Header.win_res_len).AppendLine();
                    sb.AppendFormat("\tDevice ID: {0}", Header.device_id).AppendLine();
                    sb.AppendFormat("\tDDK version {0}.{1}", Header.ddk_major, Header.ddk_minor).AppendLine();
                }

                if(ImportedNames != null)
                {
                    sb.AppendLine("\tImported names:");
                    foreach(string name in ImportedNames) sb.AppendFormat("\t\t{0}", name).AppendLine();
                }

                if(ResidentNames != null)
                {
                    sb.AppendLine("\tResident names:");
                    foreach(NE.ResidentName name in ResidentNames)
                        sb.AppendFormat("\t\t{0} at index {1}", name.name, name.entryTableIndex).AppendLine();
                }

                if(NonResidentNames != null)
                {
                    sb.AppendLine("\tNon-resident names:");
                    foreach(NE.ResidentName name in NonResidentNames)
                        sb.AppendFormat("\t\t{0} at index {1}", name.name, name.entryTableIndex).AppendLine();
                }

                sb.AppendLine("\tObjects:");
                for(int i = 0; i < objectTableEntries.Length; i++)
                {
                    sb.AppendFormat("\t\tObject {0}:", i + 1).AppendLine();
                    sb.AppendFormat("\t\t\tVirtual size: {0}", objectTableEntries[i].VirtualSize).AppendLine();
                    sb.AppendFormat("\t\t\tRelocation base address: {0:X8}h",
                                    objectTableEntries[i].RelocationBaseAddress).AppendLine();
                    sb.AppendFormat("\t\t\tFlags: {0}", objectTableEntries[i].ObjectFlags).AppendLine();
                    sb.AppendFormat("\t\t\tFirst page table index: {0}", objectTableEntries[i].PageTableIndex)
                      .AppendLine();
                    sb.AppendFormat("\t\t\tHas {0} pages", objectTableEntries[i].PageTableEntries).AppendLine();
                    if(Header.signature == SIGNATURE16)
                        sb.AppendFormat("\t\t\tName: \"{0}\"",
                                        StringHandlers.CToString(objectTableEntries[i].Name).Trim()).AppendLine();
                }

                sb.AppendLine("\tPages:");
                for(int i = 0; i < objectPageTableEntries.Length; i++)
                {
                    sb.AppendFormat("\t\tPage {0}:", i + 1).AppendLine();
                    sb.AppendFormat("\t\t\tFlags: {0}", objectPageTableEntries[i].Flags).AppendLine();
                    sb.AppendFormat("\t\t\tSize: {0} bytes", objectPageTableEntries[i].DataSize).AppendLine();
                    sb.AppendFormat("\t\t\tRelative offset: {0}", objectPageTableEntries[i].PageDataOffset)
                      .AppendLine();
                }

                sb.AppendLine("\tSections:");
                int count = 0;
                foreach(Segment section in Segments)
                {
                    sb.AppendFormat("\t\tSection {0}:", count).AppendLine();
                    sb.AppendFormat("\t\t\tName: {0}", section.Name).AppendLine();
                    sb.AppendFormat("\t\t\tFlags: {0}", section.Flags).AppendLine();
                    sb.AppendFormat("\t\t\tOffset: {0}", section.Offset).AppendLine();
                    sb.AppendFormat("\t\t\tSize: {0} bytes", section.Size).AppendLine();
                    count++;
                }

                if(resources.Length <= 0) return sb.ToString();

                {
                    sb.AppendLine("\tResources:");
                    for(int i = 0; i < resources.Length; i++)
                        sb.AppendFormat("\t\tType {0}, id {1}, {2} bytes, object {3} at offset {4}", resources[i].type,
                                        resources[i].id, resources[i].size, resources[i].obj_no, resources[i].offset)
                          .AppendLine();
                }

                return sb.ToString();
            }
        }

        static NE.ResidentName[] GetResidentStrings(Stream stream, uint neStart, uint tableOff)
        {
            List<NE.ResidentName> names = new List<NE.ResidentName>();
            byte                  stringSize;
            byte[]                nameString;
            byte[]                DW = new byte[2];

            long oldPosition = stream.Position;

            stream.Position = neStart + tableOff;
            while(true)
            {
                stringSize = (byte)stream.ReadByte();

                if(stringSize == 0) break;

                nameString = new byte[stringSize];
                stream.Read(nameString, 0, stringSize);
                stream.Read(DW,         0, 2);

                names.Add(new NE.ResidentName
                {
                    name            = Encoding.ASCII.GetString(nameString),
                    entryTableIndex = BitConverter.ToUInt16(DW, 0)
                });
            }

            stream.Position = oldPosition;

            return names.Count > 0 ? names.ToArray() : null;
        }
    }
}