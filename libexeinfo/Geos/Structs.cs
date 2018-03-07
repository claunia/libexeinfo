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
// THE SOFTWARE.namespace libexeinfo.Geos

using System.Runtime.InteropServices;

namespace libexeinfo
{
    // Thanks to Marcus Gröber for structures
    // TODO: Format of resource segments
    public partial class Geos
    {
        /// <summary>ID for file types/icons</summary>
        [StructLayout(LayoutKind.Sequential)]
        struct Token
        {
            /// <summary>4 byte string</summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = GEOS_TOKENLEN)]
            public byte[] str;
            /// <summary>Manufacturer ID</summary>
            public ManufacturerId manufacturer;
        }

        /// <summary>Protocol/version number</summary>
        [StructLayout(LayoutKind.Sequential)]
        struct Protocol
        {
            /// <summary>Protocol</summary>
            public ushort major;
            /// <summary>Sub revision</summary>
            public ushort minor;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct Release
        {
            public ushort major;
            public ushort minor;
            public ushort change;
            public ushort engineering;
        }

        // Followed by followed by geosliblist, followed by exportlist, followed by segmentlist
        [StructLayout(LayoutKind.Sequential)]
        struct GeodeHeader
        {
            /// <summary>GEOS id magic: C7 45 CF 53</summary>
            public uint magic;
            /// <summary>
            ///     <see cref="FileType" />
            /// </summary>
            public FileType type;
            /// <summary>Flags ??? (always seen 0000h)</summary>
            public ushort flags;
            /// <summary>Release</summary>
            public Release release;
            /// <summary>Protocol/version</summary>
            public Protocol protocol;
            /// <summary>File type/icon</summary>
            public Token token;
            /// <summary>Tokenof creator application</summary>
            public Token creator;
            /// <summary>Long filename</summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = GEOS_LONGNAME)]
            public byte[] name;
            /// <summary>User file info</summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = GEOS_INFO)]
            public byte[] info;
            /// <summary>Copyright notice</summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
            public byte[] copyright;
        }

        /// <summary>Additional geode file header</summary>
        [StructLayout(LayoutKind.Sequential)]
        struct ApplicationHeader
        {
            /// <summary>This is actually not part of the app header, but for historical reasons, I keep it in... :-)</summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] unknown;
            /// <summary>Attributes <see cref="Attributes" /></summary>
            public Attributes _attr;
            /// <summary>Application type <see cref="ApplicationType" /></summary>
            public ApplicationType _type;
            /// <summary>Expected kernel protocol</summary>
            public Protocol kernel_protocol;
            /// <summary>Number of program segments</summary>
            public ushort _numseg;
            /// <summary>Number of included libraries</summary>
            public ushort _numlib;
            /// <summary>Number of exported locations</summary>
            public ushort _numexp;
            /// <summary>Default stack size</summary>
            public ushort stack_size;
            /// <summary>If application: segment/offset of ???</summary>
            public ushort app_off;
            public ushort app_seg;
            /// <summary>If application: item of resource with application token</summary>
            public ushort tokenres_item;
            /// <summary>If application: segment of resource with application token</summary>
            public ushort tokenres_segment;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] unknown2;
            /// <summary>Attributes <see cref="Attributes" /></summary>
            public Attributes attributes;
            /// <summary>Application type <see cref="ApplicationType" /></summary>
            public ApplicationType type;
            /// <summary>Release</summary>
            public Release release;
            /// <summary>Protocol/version</summary>
            public Protocol protocol;
            /// <summary>Possibly header checksum (???)</summary>
            public ushort crc;
            /// <summary>Internal filename (blank padded)</summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = GEOS_FNAME)]
            public byte[] name;
            /// <summary>Internal extension (blank padded)</summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = GEOS_FEXT)]
            public byte[] extension;
            /// <summary>File type/icon</summary>
            Token token;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] unknown3;
            /// <summary>If driver: entry location</summary>
            public ushort entry_off;
            public ushort entry_seg;
            /// <summary>If library: init location (?)</summary>
            public ushort init_off;
            public ushort init_seg;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] unknown4;
            /// <summary>Number of exported locations</summary>
            public ushort exports;
            /// <summary>Number of included libraries</summary>
            public ushort imports;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] unknown5;
            /// <summary>Number of program segments</summary>
            public ushort segments;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public byte[] unknown6;
        }

        /// <summary>Base type of "exported" array</summary>
        [StructLayout(LayoutKind.Sequential)]
        struct Export
        {
            /// <summary>Routine entry location</summary>
            public ushort offset;
            /// <summary>Routine entry location</summary>
            public ushort segment;
        }

        /// <summary>Base type of library array</summary>
        [StructLayout(LayoutKind.Sequential)]
        struct Import
        {
            /// <summary>library name</summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = GEOS_FNAME)]
            public byte[] name;
            /// <summary>library type</summary>
            public Attributes attributes;
            /// <summary>required lib protocol/version</summary>
            public Protocol protocol;
        }

        /// <summary>GEOS2 standard header</summary>
        [StructLayout(LayoutKind.Sequential)]
        struct GeodeHeaderV2
        {
            /// <summary>GEOS2 id magic: C7 45 CF 53</summary>
            public uint magic;
            /// <summary>Long filename</summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = GEOS_LONGNAME)]
            public byte[] name;
            /// <summary>
            ///     <see cref="FileType2" />
            /// </summary>
            public FileType2 type;
            /// <summary>Flags</summary>
            public ushort flags;
            /// <summary>Release</summary>
            public Release release;
            /// <summary>Protocol/version</summary>
            public Protocol protocol;
            /// <summary>File type/icon</summary>
            public Token token;
            /// <summary>Token of creator application</summary>
            public Token application;
            /// <summary>User file info</summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = GEOS_INFO)]
            public byte[] info;
            /// <summary>Copyright notice</summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
            public byte[] copyright;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] unknown;
            /// <summary>Creation date in DOS format</summary>
            public byte create_date;
            /// <summary>Creation time in DOS format</summary>
            public byte create_time;
            /// <summary>Password, encrypted as hex string</summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] password;
            /// <summary>not yet decoded</summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 44)]
            public byte[] unknown2;
        }

        /// <summary>Additional geode file header</summary>
        [StructLayout(LayoutKind.Sequential)]
        struct ApplicationHeaderV2
        {
            /// <summary>Attributes <see cref="Attributes" /></summary>
            public Attributes _attr;
            /// <summary>Application type <see cref="ApplicationType" /></summary>
            public ApplicationType _type;
            /// <summary>Expected kernel protocol</summary>
            public Protocol kernel_protocol;
            /// <summary>Number of program segments</summary>
            public ushort _numseg;
            /// <summary>Number of included libraries</summary>
            public ushort _numlib;
            /// <summary>Number of exported locations</summary>
            public ushort _numexp;
            /// <summary>Default stack size</summary>
            public ushort stack_size;
            /// <summary>If application: segment/offset of ???</summary>
            public ushort app_off;
            public ushort app_seg;
            /// <summary>If application: item of resource with application token</summary>
            public ushort tokenres_item;
            /// <summary>If application: segment of resource with application token</summary>
            public ushort tokenres_segment;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] unknown;
            /// <summary>Attributes <see cref="Attributes" /></summary>
            public Attributes attributes;
            /// <summary>Application type <see cref="ApplicationType" /></summary>
            public ApplicationType type;
            /// <summary>Release"</summary>
            public Release release;
            /// <summary>Protocol/version</summary>
            public Protocol protocol;
            /// <summary>Possibly header checksum (???)</summary>
            public ushort crc;
            /// <summary>Internal filename (blank padded)</summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = GEOS_FNAME)]
            public byte[] name;
            /// <summary>Internal extension (blank padded)</summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = GEOS_FEXT)]
            public byte[] extension;
            /// <summary>File type/icon</summary>
            public Token token;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] unknown2;
            /// <summary>If driver: entry location</summary>
            public ushort entry_off;
            public ushort entry_seg;
            /// <summary>If library: init location (?)</summary>
            public ushort init_off;
            public ushort init_seg;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] unknown3;
            /// <summary>Number of exported locations</summary>
            public ushort exports;
            /// <summary>Number of included libraries</summary>
            public ushort imports;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] unknown4;
            /// <summary>Number of program segments</summary>
            public ushort segments;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public byte[] unknown5;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct SegmentDescriptor
        {
            public ushort       length;
            public uint         offset;
            public ushort       relocs_length;
            public SegmentFlags flags;
        }
    }
}