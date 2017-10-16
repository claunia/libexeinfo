//
// Structs.cs
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
using System.Runtime.InteropServices;

namespace libexeinfo
{
    public partial class LX
    {
        /// <summary>
        /// Header for a Microsoft Linear Executable and IBM Linear eXecutable
        /// </summary>
        [StructLayout(LayoutKind.Sequential/*, Pack = 2*/)]
        public struct LXHeader
        {
            /// <summary>
            /// Executable signature
            /// </summary>
            public ushort signature;
            /// <summary>
            /// Byte ordering
            /// </summary>
            public byte byte_order;
            /// <summary>
            /// Word ordering
            /// </summary>
            public byte word_order;
            /// <summary>
            /// Format level, should be 0
            /// </summary>
            public uint format_level;
            /// <summary>
            /// Type of CPU required by this executable to run
            /// </summary>
            public TargetCpu cpu_type;
            /// <summary>
            /// Type of operating system requires by this executable to run
            /// </summary>
            public TargetOS os_type;
            /// <summary>
            /// Executable version
            /// </summary>
            public uint module_version;
            /// <summary>
            /// Executable flags
            /// </summary>
            public ModuleFlags module_flags;
            /// <summary>
            /// Pages contained in this module
            /// </summary>
            public uint module_pages_no;
            /// <summary>
            /// Object number to which the Entry Address is relative
            /// </summary>
            public uint eip_object;
            /// <summary>
            /// Entry address of module
            /// </summary>
            public uint eip;
            /// <summary>
            /// Object number to which the ESP is relative
            /// </summary>
            public uint esp_object;
            /// <summary>
            /// Starting stack address of module
            /// </summary>
            public uint esp;
            /// <summary>
            /// Size of one page
            /// </summary>
            public uint page_size;
            /// <summary>
            /// Shift left bits for page offsets
            /// </summary>
            public uint page_off_shift;
            /// <summary>
            /// Total size of the fixup information
            /// </summary>
            public uint fixup_size;
            /// <summary>
            /// Checksum for fixup information
            /// </summary>
            public uint fixup_checksum;
            /// <summary>
            /// Size of memory resident tables
            /// </summary>
            public uint loader_size;
            /// <summary>
            /// Checksum for loader section
            /// </summary>
            public uint loader_checksum;
            /// <summary>
            /// Object table offset
            /// </summary>
            public uint obj_table_off;
            /// <summary>
            /// Object table count
            /// </summary>
            public uint obj_no;
            /// <summary>
            /// Object page table offset
            /// </summary>
            public uint obj_page_table_off;
            /// <summary>
            /// Object iterated pages offset
            /// </summary>
            public uint obj_iter_pages_off;
            /// <summary>
            /// Resource table offset
            /// </summary>
            public uint resource_table_off;
            /// <summary>
            /// Entries in resource table
            /// </summary>
            public uint resource_entries;
            /// <summary>
            /// Resident name table offset
            /// </summary>
            public uint resident_names_off;
            /// <summary>
            /// Entry table offset
            /// </summary>
            public uint entry_table_off;
            /// <summary>
            /// Module format directives table offset
            /// </summary>
            public uint directives_off;
            /// <summary>
            /// Entries in module format directives table
            /// </summary>
            public uint directives_no;
            /// <summary>
            /// Fixup page table offset
            /// </summary>
            public uint fixup_page_table_off;
            /// <summary>
            /// Fixup record table offset
            /// </summary>
            public uint fixup_record_table_off;
            /// <summary>
            /// Import module name table offset
            /// </summary>
            public uint import_module_table_off;
            /// <summary>
            /// Entries in the import module name table
            /// </summary>
            public uint import_module_entries;
            /// <summary>
            /// Import procedure name table offset
            /// </summary>
            public uint import_proc_table_off;
            /// <summary>
            /// Per-page checksum table offset
            /// </summary>
            public uint perpage_checksum_off;
            /// <summary>
            /// Data pages offset
            /// </summary>
            public uint data_pages_off;
            /// <summary>
            /// Number of preload pages for this module
            /// </summary>
            public uint preload_pages_no;
            /// <summary>
            /// Non-resident name table offset
            /// </summary>
            public uint nonresident_name_table_off;
            /// <summary>
            /// Number of bytes in the non-resident name table
            /// </summary>
            public uint nonresident_name_table_len;
            /// <summary>
            /// Non-resident name table checksum
            /// </summary>
            public uint nonresident_name_table_checksum;
            /// <summary>
            /// The auto data segment object number
            /// </summary>
            public uint auto_ds_obj_no;
            /// <summary>
            /// Debug information offset
            /// </summary>
            public uint debug_info_off;
            /// <summary>
            /// Debug information length
            /// </summary>
            public uint debug_info_len;
            /// <summary>
            /// Instance pages in preload section
            /// </summary>
            public uint instance_preload_no;
            /// <summary>
            /// Instance pages in demand section
            /// </summary>
            public uint instance_demand_no;
            /// <summary>
            /// Heap size added to the auto ds object
            /// </summary>
            public uint heap_size;
        }
   }
}