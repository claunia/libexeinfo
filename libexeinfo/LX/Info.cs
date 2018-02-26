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

using System.Text;

namespace libexeinfo
{
    public partial class LX
    {
        public string Information => GetInfo(header, baseExecutable);

        static string GetInfo(LXHeader header, IExecutable baseExecutable)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(baseExecutable.Information);
            sb.AppendLine(header.signature == SIGNATURE16 ? "Linear Executable (LE):" : "Linear eXecutable (LX):");

            switch(header.os_type)
            {
                case TargetOS.OS2:
                    sb.AppendLine("\tOS/2 application");
                    if(header.module_flags.HasFlag(ModuleFlags.PMIncompatible) &&
                       !header.module_flags.HasFlag(ModuleFlags.PMCompatible))
                        sb.AppendLine("\tApplication is full screen, unaware of Presentation Manager");
                    else if(!header.module_flags.HasFlag(ModuleFlags.PMIncompatible) &&
                            header.module_flags.HasFlag(ModuleFlags.PMCompatible))
                        sb.AppendLine("\tApplication is aware of Presentation Manager, but doesn't use it");
                    else if(header.module_flags.HasFlag(ModuleFlags.PMIncompatible) &&
                            header.module_flags.HasFlag(ModuleFlags.PMCompatible))
                        sb.AppendLine("\tApplication uses Presentation Manager");
                    break;
                case TargetOS.Windows:
                case TargetOS.Win32:
                case TargetOS.Unknown:
                    switch(header.os_type)
                    {
                        case TargetOS.Windows:
                        case TargetOS.Unknown:
                            sb.AppendLine("\t16-bit Windows application");
                            break;
                        case TargetOS.Win32:
                            sb.AppendLine("\t32-bit Windows application");
                            break;
                    }

                    if(header.module_flags.HasFlag(ModuleFlags.PMIncompatible) &&
                       !header.module_flags.HasFlag(ModuleFlags.PMCompatible))
                        sb.AppendLine("\tApplication is full screen, unaware of Windows");
                    else if(!header.module_flags.HasFlag(ModuleFlags.PMIncompatible) &&
                            header.module_flags.HasFlag(ModuleFlags.PMCompatible))
                        sb.AppendLine("\tApplication is aware of Windows, but doesn't use it");
                    else if(header.module_flags.HasFlag(ModuleFlags.PMIncompatible) &&
                            header.module_flags.HasFlag(ModuleFlags.PMCompatible))
                        sb.AppendLine("\tApplication uses Windows");
                    break;
                case TargetOS.DOS:
                    sb.AppendLine("\tDOS application");
                    if(header.module_flags.HasFlag(ModuleFlags.PMIncompatible) &&
                       !header.module_flags.HasFlag(ModuleFlags.PMCompatible))
                        sb.AppendLine("\tApplication is full screen, unaware of Windows");
                    else if(!header.module_flags.HasFlag(ModuleFlags.PMIncompatible) &&
                            header.module_flags.HasFlag(ModuleFlags.PMCompatible))
                        sb.AppendLine("\tApplication is aware of Windows, but doesn't use it");
                    else if(header.module_flags.HasFlag(ModuleFlags.PMIncompatible) &&
                            header.module_flags.HasFlag(ModuleFlags.PMCompatible))
                        sb.AppendLine("\tApplication uses Windows");
                    break;
                default:
                    sb.AppendFormat("\tApplication for unknown OS {0}", (ushort)header.os_type).AppendLine();
                    break;
            }

            sb.AppendFormat("\tByte ordering: {0}", header.byte_order == 1 ? "Big-endian" : "Little-Endian")
              .AppendLine();
            sb.AppendFormat("\tWord ordering: {0}", header.word_order == 1 ? "Big-endian" : "Little-Endian")
              .AppendLine();
            sb.AppendFormat("\tFormat level: {0}",       header.format_level).AppendLine();
            sb.AppendFormat("\tExecutable version: {0}", header.module_version).AppendLine();

            switch(header.cpu_type)
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
                                    (ushort)header.cpu_type).AppendLine();
                    break;
            }

            if(header.module_flags.HasFlag(ModuleFlags.PerProcessLibrary))
                sb.AppendLine("Library should be initialized per-process.");
            if(header.module_flags.HasFlag(ModuleFlags.PerProcessTermination))
                sb.AppendLine("Library should be terminated per-process.");
            if(header.module_flags.HasFlag(ModuleFlags.InternalFixups))
                sb.AppendLine("Internal fixups have been applied.");
            if(header.module_flags.HasFlag(ModuleFlags.ExternalFixups))
                sb.AppendLine("External fixups have been applied.");
            if(header.module_flags.HasFlag(ModuleFlags.NotLoadable))
                sb.AppendLine("Executable is not loadable, it contains errors or is still being linked.");

            if(header.module_flags.HasFlag(ModuleFlags.VirtualDeviceDriver))
                sb.AppendLine("Executable is a driver for a virtual device.");
            else if(header.module_flags.HasFlag(ModuleFlags.PhysicalDeviceDriver))
                sb.AppendLine("Executable is a driver for a physical device.");
            else if(header.module_flags.HasFlag(ModuleFlags.ProtectedMemoryLibrary))
                sb.AppendLine("Executable is a protected mode library.");
            else if(header.module_flags.HasFlag(ModuleFlags.Library))
                sb.AppendLine("Executable is a library.");

            sb.AppendFormat("\tThis executable contains {0} pages", header.module_pages_no)
              .AppendLine();
            sb.AppendFormat("\tObject number to which the Entry Address is relative: {0}", header.eip_object)
              .AppendLine();
            sb.AppendFormat("\tEntry address of module: {0:X8}h",                header.eip).AppendLine();
            sb.AppendFormat("\tObject number to which the ESP is relative: {0}", header.esp_object).AppendLine();
            sb.AppendFormat("\tStarting stack address of module: {0:X8}h",       header.esp).AppendLine();
            sb.AppendFormat("\tOne page is {0} bytes",                           header.page_size).AppendLine();
            sb.AppendFormat("\tShift left bits for page offsets: {0}",           header.page_off_shift).AppendLine();
            sb.AppendFormat("\tTotal size of the fixup information: {0}",        header.fixup_size).AppendLine();
            sb.AppendFormat("\tChecksum for fixup information: 0x{0:X8}",        header.fixup_checksum).AppendLine();
            sb.AppendFormat("\tMemory resident tables are {0} bytes long",       header.loader_size).AppendLine();
            sb.AppendFormat("\tChecksum for loader section: 0x{0:X8}",           header.loader_checksum).AppendLine();
            sb.AppendFormat("\tObject table starts at {0} and contains {1} objects", header.obj_table_off,
                            header.obj_no).AppendLine();
            sb.AppendFormat("\tObject page table starts at {0}",     header.obj_page_table_off).AppendLine();
            sb.AppendFormat("\tObject iterated pages starts at {0}", header.obj_iter_pages_off).AppendLine();
            sb.AppendFormat("\tResources table starts at {0} and contains {1} entries", header.resource_table_off,
                            header.resource_entries).AppendLine();
            sb.AppendFormat("\tResident name table starts at {0}", header.resident_names_off).AppendLine();
            sb.AppendFormat("\tEntry table starts at {0}",         header.entry_table_off).AppendLine();
            sb.AppendFormat("\tModule format directives table starts at {0} and contains {1} entries",
                            header.directives_off, header.directives_no).AppendLine();
            sb.AppendFormat("\tFixup page table starts at {0}",   header.fixup_page_table_off).AppendLine();
            sb.AppendFormat("\tFixup record table starts at {0}", header.fixup_record_table_off).AppendLine();
            sb.AppendFormat("\tImport module name table starts at {0} and contains {1} entries",
                            header.import_module_table_off, header.import_module_entries).AppendLine();
            sb.AppendFormat("\tImport procedure name table starts at {0}", header.import_proc_table_off).AppendLine();
            sb.AppendFormat("\tPer-page checksum table starts at {0}",     header.perpage_checksum_off).AppendLine();
            sb.AppendFormat("\tData pages start at {0}",                   header.data_pages_off).AppendLine();
            sb.AppendFormat("\t{0} pages to preload in this executable",   header.preload_pages_no).AppendLine();
            sb.AppendFormat("\tNon-resident names table starts at {0} and runs for {1} bytes",
                            header.nonresident_name_table_off, header.nonresident_name_table_len).AppendLine();
            sb.AppendFormat("\tNon-resident name table checksum: 0x{0:X8}", header.nonresident_name_table_checksum)
              .AppendLine();
            sb.AppendFormat("\tThe auto data segment object number: {0}", header.auto_ds_obj_no).AppendLine();
            sb.AppendFormat("\tDebug information starts at {0} and is {1} bytes", header.debug_info_off,
                            header.debug_info_len).AppendLine();
            sb.AppendFormat("\tInstance pages in preload section: {0}",     header.instance_preload_no).AppendLine();
            sb.AppendFormat("\tInstance pages in demand section: {0}",      header.instance_demand_no).AppendLine();
            sb.AppendFormat("\tHeap size added to the auto ds object: {0}", header.heap_size).AppendLine();
            return sb.ToString();
        }
    }
}