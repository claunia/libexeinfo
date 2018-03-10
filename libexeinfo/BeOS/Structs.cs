//
// Structs.cs
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

using System.Runtime.InteropServices;

// Structures thanks to Ingo Weinhold and Haiku
namespace libexeinfo.BeOS
{
    /// <summary>
    ///     Header of resource data
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    struct ResourcesHeader
    {
        /// <summary>
        ///     <see cref="Consts.RESOURCES_HEADER_MAGIC" />
        /// </summary>
        public uint magic;
        /// <summary>
        ///     How many resources are present
        /// </summary>
        public uint resource_count;
        /// <summary>
        ///     Offset to <see cref="ResourceIndexSectionHeader" />
        /// </summary>
        public uint index_section_offset;
        /// <summary>
        ///     Size of the admin section
        /// </summary>
        public uint admin_section_size;
        /// <summary>
        ///     Padding
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)]
        public uint[] pad;
    }

    /// <summary>
    ///     Header of the resource index, followed by as many <see cref="ResourceIndexEntry" /> as resources are in the file
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    struct ResourceIndexSectionHeader
    {
        /// <summary>
        ///     Offset to itself
        /// </summary>
        public uint index_section_offset;
        /// <summary>
        ///     Size in bytes of the whole index section
        /// </summary>
        public uint index_section_size;
        /// <summary>
        ///     Unused
        /// </summary>
        public uint unused_data1;
        /// <summary>
        ///     Offset to an unknown section
        /// </summary>
        public uint unknown_section_offset;
        /// <summary>
        ///     Size in bytes of the unknown section
        /// </summary>
        public uint unknown_section_size;
        /// <summary>
        ///     Unused
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 25)]
        public uint[] unused_data2;
        /// <summary>
        ///     Offset to <see cref="ResourceInfoBlock" /> table
        /// </summary>
        public uint info_table_offset;
        /// <summary>
        ///     Sizee of <see cref="ResourceInfoBlock" /> table
        /// </summary>
        public uint info_table_size;
        /// <summary>
        ///     Unused
        /// </summary>
        public uint unused_data3;
    }

    /// <summary>
    ///     A resource index entry
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    struct ResourceIndexEntry
    {
        /// <summary>
        ///     Offset to resource
        /// </summary>
        public uint offset;
        /// <summary>
        ///     Size of resource
        /// </summary>
        public uint size;
        /// <summary>
        ///     Padding
        /// </summary>
        public uint pad;
    }

    /// <summary>
    ///     Resource information, followed by <see cref="name_size" /> ASCII characters with the name
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    struct ResourceInfo
    {
        /// <summary>
        ///     Resource ID
        /// </summary>
        public int id;
        /// <summary>
        ///     Resource index, not related to index table
        /// </summary>
        public int index;
        /// <summary>
        ///     Size of name following this structure
        /// </summary>
        public ushort name_size;
    }

    /// <summary>
    ///     Separator between resource types
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    struct ResourceInfoSeparator
    {
        /// <summary>
        ///     Must be 0xFFFFFFFF
        /// </summary>
        public uint value1;
        /// <summary>
        ///     Must be 0xFFFFFFFF
        /// </summary>
        public uint value2;
    }

    /// <summary>
    ///     Resource info block, followed by <see cref="ResourceInfoSeparator" /> when the <see cref="info" /> array ends
    /// </summary>
    struct ResourceInfoBlock
    {
        /// <summary>
        ///     OSType code
        /// </summary>
        public uint type;
        /// <summary>
        ///     Array of <see cref="ResourceInfo" />. Unless a <see cref="ResourceInfoSeparator" /> is found.
        /// </summary>
        public ResourceInfo[] info;
    }

    /// <summary>
    ///     End of the resource info table
    /// </summary>
    struct ResourceInfoTableEnd
    {
        /// <summary>
        ///     Resource info table checksum
        /// </summary>
        public uint checksum;
        /// <summary>
        ///     Must be 0
        /// </summary>
        public uint terminator;
    }

    public class ResourceTypeBlock
    {
        public Resource[] resources;
        public string     type;
    }

    public class Resource
    {
        public byte[] data;
        public int    id;
        public int    index;
        public string name;
    }
}