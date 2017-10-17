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
using static libexeinfo.COFF;

namespace libexeinfo
{
    public partial class PE
    {
        /// <summary>
        /// Header for a Microsoft New Executable
        /// </summary>
        [StructLayout(LayoutKind.Sequential/*, Pack = 2*/)]
        public struct PEHeader
        {
            /// <summary>
            /// After the MS-DOS stub, at the file offset specified at offset 0x3c, is a 4-byte signature that identifies the file as a PE format image file. This signature is "PE\0\0" (the letters "P" and "E" followed by two null bytes).
            /// </summary>
            public uint signature;
            public COFFHeader coff;
        }

        /// <summary>
        /// The next 21 fields are an extension to the COFF optional header format. They contain additional information that is required by the linker and loader in Windows.
        /// </summary>
        [StructLayout(LayoutKind.Sequential/*, Pack = 2*/)]
        public struct WindowsHeader
        {
            /// <summary>
            /// The preferred address of the first byte of image when loaded into memory; must be a multiple of 64 K. The default for DLLs is 0x10000000. The default for Windows CE EXEs is 0x00010000. The default for Windows NT, Windows 2000, Windows XP, Windows 95, Windows 98, and Windows Me is 0x00400000.
            /// </summary>
            public uint imageBase;
            /// <summary>
            /// The alignment (in bytes) of sections when they are loaded into memory. It must be greater than or equal to FileAlignment. The default is the page size for the architecture.
            /// </summary>
            public uint sectionAlignment;
            /// <summary>
            /// The alignment factor (in bytes) that is used to align the raw data of sections in the image file. The value should be a power of 2 between 512 and 64 K, inclusive. The default is 512. If the SectionAlignment is less than the architecture's page size, then FileAlignment must match SectionAlignment.
            /// </summary>
            public uint fileAlignment;
            /// <summary>
            /// The major version number of the required operating system.
            /// </summary>
            public ushort majorOperatingSystemVersion;
            /// <summary>
            /// The minor version number of the required operating system.
            /// </summary>
            public ushort minorOperatingSystemVersion;
            /// <summary>
            /// The major version number of the image.
            /// </summary>
            public ushort majorImageVersion;
            /// <summary>
            /// The minor version number of the image.
            /// </summary>
            public ushort minorImageVersion;
            /// <summary>
            /// The major version number of the subsystem.
            /// </summary>
            public ushort majorSubsystemVersion;
            /// <summary>
            /// The minor version number of the subsystem.
            /// </summary>
            public ushort minorSubsystemVersion;
            /// <summary>
            /// Reserved, must be zero.
            /// </summary>
            public uint win32VersionValue;
            /// <summary>
            /// The size (in bytes) of the image, including all headers, as the image is loaded in memory. It must be a multiple of SectionAlignment.
            /// </summary>
            public uint sizeOfImage;
            /// <summary>
            /// The combined size of an MS-DOS stub, PE header, and section headers rounded up to a multiple of FileAlignment.
            /// </summary>
            public uint sizeOfHeaders;
            /// <summary>
            /// The image file checksum. The algorithm for computing the checksum is incorporated into IMAGHELP.DLL. The following are checked for validation at load time: all drivers, any DLL loaded at boot time, and any DLL that is loaded into a critical Windows process.
            /// </summary>
            public uint checksum;
            /// <summary>
            /// The subsystem that is required to run this image. For more information, <see cref="Subsystems"/>.
            /// </summary>
            public Subsystems subsystem;
            /// <summary>
            /// For more information, <see cref="DllCharacteristics"/>.
            /// </summary>
            public DllCharacteristics dllCharacteristics;
            /// <summary>
            /// The size of the stack to reserve. Only SizeOfStackCommit is committed; the rest is made available one page at a time until the reserve size is reached.
            /// </summary>
            public uint sizeOfStackReserve;
            /// <summary>
            /// The size of the stack to commit.
            /// </summary>
            public uint sizeOfStackCommit;
            /// <summary>
            /// The size of the local heap space to reserve. Only SizeOfHeapCommit is committed; the rest is made available one page at a time until the reserve size is reached.
            /// </summary>
            public uint sizeOfHeapReserve;
            /// <summary>
            /// The size of the local heap space to commit.
            /// </summary>
            public uint sizeOfHeapCommit;
            /// <summary>
            /// Reserved, must be zero.
            /// </summary>
            public uint loaderFlags;
            /// <summary>
            /// The number of data-directory entries in the remainder of the optional header. Each describes a location and size.
            /// </summary>
            public uint numberOfRvaAndSizes;
        }

        /// <summary>
        /// The next 21 fields are an extension to the COFF optional header format. They contain additional information that is required by the linker and loader in Windows.
        /// </summary>
        [StructLayout(LayoutKind.Sequential/*, Pack = 2*/)]
        public struct WindowsHeader64
        {
            /// <summary>
            /// The preferred address of the first byte of image when loaded into memory; must be a multiple of 64 K. The default for DLLs is 0x10000000. The default for Windows CE EXEs is 0x00010000. The default for Windows NT, Windows 2000, Windows XP, Windows 95, Windows 98, and Windows Me is 0x00400000.
            /// </summary>
            public ulong imageBase;
            /// <summary>
            /// The alignment (in bytes) of sections when they are loaded into memory. It must be greater than or equal to FileAlignment. The default is the page size for the architecture.
            /// </summary>
            public uint sectionAlignment;
            /// <summary>
            /// The alignment factor (in bytes) that is used to align the raw data of sections in the image file. The value should be a power of 2 between 512 and 64 K, inclusive. The default is 512. If the SectionAlignment is less than the architecture's page size, then FileAlignment must match SectionAlignment.
            /// </summary>
            public uint fileAlignment;
            /// <summary>
            /// The major version number of the required operating system.
            /// </summary>
            public ushort majorOperatingSystemVersion;
            /// <summary>
            /// The minor version number of the required operating system.
            /// </summary>
            public ushort minorOperatingSystemVersion;
            /// <summary>
            /// The major version number of the image.
            /// </summary>
            public ushort majorImageVersion;
            /// <summary>
            /// The minor version number of the image.
            /// </summary>
            public ushort minorImageVersion;
            /// <summary>
            /// The major version number of the subsystem.
            /// </summary>
            public ushort majorSubsystemVersion;
            /// <summary>
            /// The minor version number of the subsystem.
            /// </summary>
            public ushort minorSubsystemVersion;
            /// <summary>
            /// Reserved, must be zero.
            /// </summary>
            public uint win32VersionValue;
            /// <summary>
            /// The size (in bytes) of the image, including all headers, as the image is loaded in memory. It must be a multiple of SectionAlignment.
            /// </summary>
            public uint sizeOfImage;
            /// <summary>
            /// The combined size of an MS-DOS stub, PE header, and section headers rounded up to a multiple of FileAlignment.
            /// </summary>
            public uint sizeOfHeaders;
            /// <summary>
            /// The image file checksum. The algorithm for computing the checksum is incorporated into IMAGHELP.DLL. The following are checked for validation at load time: all drivers, any DLL loaded at boot time, and any DLL that is loaded into a critical Windows process.
            /// </summary>
            public uint checksum;
            /// <summary>
            /// The subsystem that is required to run this image. For more information, <see cref="Subsystems"/>.
            /// </summary>
            public Subsystems subsystem;
            /// <summary>
            /// For more information, <see cref="DllCharacteristics"/>.
            /// </summary>
            public DllCharacteristics dllCharacteristics;
            /// <summary>
            /// The size of the stack to reserve. Only SizeOfStackCommit is committed; the rest is made available one page at a time until the reserve size is reached.
            /// </summary>
            public ulong sizeOfStackReserve;
            /// <summary>
            /// The size of the stack to commit.
            /// </summary>
            public ulong sizeOfStackCommit;
            /// <summary>
            /// The size of the local heap space to reserve. Only SizeOfHeapCommit is committed; the rest is made available one page at a time until the reserve size is reached.
            /// </summary>
            public ulong sizeOfHeapReserve;
            /// <summary>
            /// The size of the local heap space to commit.
            /// </summary>
            public ulong sizeOfHeapCommit;
            /// <summary>
            /// Reserved, must be zero.
            /// </summary>
            public uint loaderFlags;
            /// <summary>
            /// The number of data-directory entries in the remainder of the optional header. Each describes a location and size.
            /// </summary>
            public uint numberOfRvaAndSizes;
        }

        [StructLayout(LayoutKind.Sequential/*, Pack = 2*/)]
        public struct ImageDataDirectory
        {
            /// <summary>
            /// Relative virtual address of the table
            /// </summary>
            uint rva;
            /// <summary>
            /// The size in bytes
            /// </summary>
            uint size;
        }

        [StructLayout(LayoutKind.Sequential/*, Pack = 2*/)]
        public struct DebugDirectory
        {
            /// <summary>
            /// A reserved field intended to be used for flags, set to zero for now.
            /// </summary>
            public uint characteristics;
            /// <summary>
            /// Time and date the debug data was created. 
            /// </summary>
            public uint timeDateStamp;
            /// <summary>
            /// Major version number of the debug data format.
            /// </summary>
            public ushort majorVersion;
            /// <summary>
            /// Minor version number of the debug data format.
            /// </summary>
            public ushort minorVersion;
            /// <summary>
            /// Format of debugging information: this field enables support of multiple debuggers. <see cref="DebugTypes"/> for more information.
            /// </summary>
            public DebugTypes type;
            /// <summary>
            /// Size of the debug data (not including the debug directory itself). 
            /// </summary>
            public uint sizeOfData;
            /// <summary>
            /// Address of the debug data when loaded, relative to the image base. 
            /// </summary>
            public uint addressOfRawData;
            /// <summary>
            /// File pointer to the debug data. 
            /// </summary>
            public uint pointerToRawData;
        }

        [StructLayout(LayoutKind.Sequential/*, Pack = 2*/)]
        public struct ResourceDirectoryTable
        {
            /// <summary>
            /// Resource flags, reserved for future use; currently set to zero.
            /// </summary>
            public uint characteristics;
            /// <summary>
            /// Time the resource data was created by the resource compiler.
            /// </summary>
            public uint timeDateStamp;
            /// <summary>
            /// Major version number, set by the user. 
            /// </summary>
            public ushort majorVersion;
            /// <summary>
            /// Minor version number. 
            /// </summary>
            public ushort minorVersion;
            /// <summary>
            /// Number of directory entries, immediately following the table,
            /// that use strings to identify Type, Name, or Language
            /// (depending on the level of the table). 
            /// </summary>
            public ushort nameEntries;
            /// <summary>
            /// Number of directory entries, immediately following the Name
            /// entries, that use numeric identifiers for Type, Name, or Language.
            /// </summary>
            public ushort idEntries;
        }

        [StructLayout(LayoutKind.Sequential/*, Pack = 2*/)]
        public struct ResourceDirectoryEntries
        {
			/// <summary>
			/// Address of string that gives the Type, Name, or Language identifier, depending on level of table.
			/// or
			/// 32-bit integer that identifies Type, Name, or Language.
			/// </summary>
			public uint nameOrID;
			/// <summary>
			/// High bit 0: Address of a Resource Data Entry(a leaf).
            /// High bit 1: Lower 31 bits are the address of another Resource Directory Table(the next level down). 
            /// </summary>
            public uint rva;
        }

        [StructLayout(LayoutKind.Sequential/*, Pack = 2*/)]
        public struct ResourceDataEntry
        {
			/// <summary>
			/// Address of a unit of resource data in the Resource Data area.
			/// </summary>
			public uint rva;
			/// <summary>
			/// Size, in bytes, of the resource data pointed to by the Data RVA field.
            /// </summary>
            public uint size;
			/// <summary>
			/// Code page used to decode code point values within the resource data.Typically, the code page would be the Unicode code page.
			/// </summary>
			public uint codepage;
			/// <summary>
			/// Reserved (must be set to 0) 
			/// </summary>
			public uint reserved;
        }
    }
}