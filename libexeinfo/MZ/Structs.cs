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

using System.Runtime.InteropServices;

namespace libexeinfo
{
    public partial class MZ
    {
        /// <summary>
        ///     Header of a DOS relocatable executable
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MZHeader
        {
            public ushort signature;
            public ushort bytes_in_last_block;
            public ushort blocks_in_file;
            public ushort num_relocs;
            public ushort header_paragraphs;
            public ushort min_extra_paragraphs;
            public ushort max_extra_paragraphs;
            public ushort ss;
            public ushort sp;
            public ushort checksum;
            public ushort ip;
            public ushort cs;
            public ushort reloc_table_offset;
            public ushort overlay_number;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public ushort[] reserved;
            public ushort   oem_id;
            public ushort   oem_info;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public ushort[] reserved2;
            public uint     new_offset;
        }

        /// <summary>
        ///     Entry in the relocation table
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct RelocationTableEntry
        {
            public ushort offset;
            public ushort segment;
        }
    }
}