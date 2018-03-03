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
    public partial class NE
    {
        /// <summary>
        ///     Header for a Microsoft New Executable
        /// </summary>
        [StructLayout(LayoutKind.Sequential /*, Pack = 2*/)]
        public struct NEHeader
        {
            public ushort           signature;
            public byte             linker_major;
            public byte             linker_minor;
            public ushort           entry_table_offset;
            public ushort           entry_table_length;
            public uint             crc;
            public ProgramFlags     program_flags;
            public ApplicationFlags application_flags;
            public byte             auto_data_segment_index;
            public ushort           initial_heap;
            public ushort           initial_stack;
            public uint             entry_point;
            public uint             stack_pointer;
            public ushort           segment_count;
            public ushort           reference_count;
            public ushort           nonresident_table_size;
            public ushort           segment_table_offset;
            public ushort           resource_table_offset;
            public ushort           resident_names_offset;
            public ushort           module_reference_offset;
            public ushort           imported_names_offset;
            public uint             nonresident_names_offset;
            public ushort           movable_entries;
            public ushort           alignment_shift;
            public ushort           resource_entries;
            public TargetOS         target_os;
            public OS2Flags         os2_flags;
            public ushort           return_thunks_offset;
            public ushort           segment_reference_thunks;
            public ushort           minimum_swap_area;
            public byte             os_minor;
            public byte             os_major;
        }

        /// <summary>
        ///     Resource table
        /// </summary>
        public struct ResourceTable
        {
            public ushort         alignment_shift;
            public ResourceType[] types;
        }

        /// <summary>
        ///     Resource type
        /// </summary>
        public struct ResourceType
        {
            public ushort     id;
            public ushort     count;
            public uint       reserved;
            public Resource[] resources;

            // Not sequentially stored
            public string name;
        }

        /// <summary>
        ///     Resource
        /// </summary>
        public struct Resource
        {
            public uint          dataOffset;
            public uint          length;
            public ResourceFlags flags;
            public ushort        id;
            public uint          reserved;

            // Not sequentially stored
            public string name;
            public byte[] data;
        }

        /// <summary>
        ///     Node in a version resource
        /// </summary>
        class VersionNode
        {
            public ushort        cbData;
            public ushort        cbNode;
            public VersionNode[] children;
            public byte[]        rgbData;
            public string        szName;
        }

        /// <summary>
        ///     Fixed file version info
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct FixedFileInfo
        {
            public uint dwSignature;
            public uint dwStrucVersion;
            public uint dwFileVersionMS;
            public uint dwFileVersionLS;
            public uint dwProductVersionMS;
            public uint dwProductVersionLS;
            public uint dwFileFlagsMask;
            public uint dwFileFlags;
            public uint dwFileOS;
            public uint dwFileType;
            public uint dwFileSubtype;
            public uint dwFileDateMS;
            public uint dwFileDateLS;
        }

        public struct ResidentName
        {
            /// <summary>
            ///     Text of string
            /// </summary>
            public string name;
            /// <summary>
            ///     Index in the entry table this string refers to
            /// </summary>
            public ushort entryTableIndex;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SegmentEntry
        {
            /// <summary>
            ///     Logical-sector offset to the contents of the segment data, relative to the beginning of the file.
            ///     Zero means no file data.
            /// </summary>
            public ushort dwLogicalSectorOffset;
            /// <summary>
            ///     Length of the segment in the file, in bytes. Zero means 64KiB
            /// </summary>
            public ushort dwSegmentLength;
            /// <summary>
            ///     Flag word, <see cref="SegmentResourceFlags" />, <see cref="SEGMENT_TYPE_MASK" />, <see cref="SEGMENT_FLAGS_MASK" />
            ///     , <see cref="SEGMENT_DISCARD_MASK" />
            /// </summary>
            public ushort dwFlags;
            /// <summary>
            ///     Minimum allocation size of the segment, in bytes. Total size of the segment. Zero means 64KiB
            /// </summary>
            public ushort dwMinimumAllocation;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Os2ResourceTableEntry
        {
            public ushort etype;
            public ushort ename;
        }
    }
}