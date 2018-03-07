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

namespace libexeinfo
{
    // TODO: Tru64 COFF is slightly different
    public partial class COFF
    {
	    /// <summary>
	    ///     At the beginning of an object file, or immediately after the signature of an image file, is a standard COFF file
	    ///     header in the following format. Note that the Windows loader limits the number of sections to 96.
	    /// </summary>
	    [StructLayout(LayoutKind.Sequential /*, Pack = 2*/)]
        public struct COFFHeader
        {
	        /// <summary>
	        ///     The number that identifies the type of target machine. For more information, <see cref="MachineTypes" />.
	        /// </summary>
	        public MachineTypes machine;
	        /// <summary>
	        ///     The number of sections. This indicates the size of the section table, which immediately follows the headers.
	        /// </summary>
	        public ushort numberOfSections;
	        /// <summary>
	        ///     The low 32 bits of the number of seconds since 00:00 January 1, 1970 (a C run-time time_t value), that indicates
	        ///     when the file was created.
	        /// </summary>
	        public uint timeDateStamp;
	        /// <summary>
	        ///     The file offset of the COFF symbol table, or zero if no COFF symbol table is present. This value should be zero for
	        ///     an image because COFF debugging information is deprecated.
	        /// </summary>
	        public uint pointerToSymbolTable;
	        /// <summary>
	        ///     The number of entries in the symbol table. This data can be used to locate the string table, which immediately
	        ///     follows the symbol table. This value should be zero for an image because COFF debugging information is deprecated.
	        /// </summary>
	        public uint   numberOfSymbols;
            public ushort sizeOfOptionalHeader;
	        /// <summary>
	        ///     The flags that indicate the attributes of the file. For specific flag values, <see cref="Characteristics" />
	        /// </summary>
	        public Characteristics characteristics;
            public OptionalHeader  optionalHeader;
        }

	    /// <summary>
	    ///     The first eight fields of the optional header are standard fields that are defined for every implementation of
	    ///     COFF. These fields contain general information that is useful for loading and running an executable file. They are
	    ///     unchanged for the PE32+ format.
	    /// </summary>
	    [StructLayout(LayoutKind.Sequential /*, Pack = 2*/)]
        public struct OptionalHeader
        {
	        /// <summary>
	        ///     The unsigned integer that identifies the state of the image file. The most common number is 0x10B, which identifies
	        ///     it as a normal executable file. 0x107 identifies it as a ROM image, and 0x20B identifies it as a PE32+ executable.
	        /// </summary>
	        public ushort magic;
	        /// <summary>
	        ///     The linker major version number.
	        /// </summary>
	        public byte majorLinkerVersion;
	        /// <summary>
	        ///     The linker minor version number.
	        /// </summary>
	        public byte minorLinkerVersion;
	        /// <summary>
	        ///     The size of the code (text) section, or the sum of all code sections if there are multiple sections.
	        /// </summary>
	        public uint sizeOfCode;
	        /// <summary>
	        ///     The size of the initialized data section, or the sum of all such sections if there are multiple data sections.
	        /// </summary>
	        public uint sizeOfInitializedData;
	        /// <summary>
	        ///     The size of the uninitialized data section (BSS), or the sum of all such sections if there are multiple BSS
	        ///     sections.
	        /// </summary>
	        public uint sizeOfUninitializedData;
	        /// <summary>
	        ///     The address of the entry point relative to the image base when the executable file is loaded into memory. For
	        ///     program images, this is the starting address. For device drivers, this is the address of the initialization
	        ///     function. An entry point is optional for DLLs. When no entry point is present, this field must be zero.
	        /// </summary>
	        public uint addressOfEntryPoint;
	        /// <summary>
	        ///     The address that is relative to the image base of the beginning-of-code section when it is loaded into memory.
	        /// </summary>
	        public uint baseOfCode;
	        /// <summary>
	        ///     PE32 contains this additional field, which is absent in PE32+, following BaseOfCode.<br />
	        ///     The address that is relative to the image base of the beginning-of-data section when it is loaded into memory.
	        /// </summary>
	        public uint baseOfData;
        }

        [StructLayout(LayoutKind.Sequential /*, Pack = 2*/)]
        public struct SectionHeader
        {
	        /// <summary>
	        ///     An 8-byte, null-padded ASCII string. There is no terminating null if the string is
	        ///     exactly eight characters long. For longer names, this field contains a slash(/)
	        ///     followed by ASCII representation of a decimal number: this number is an offset into
	        ///     the string table.Executable images do not use a string table and do not support
	        ///     section names longer than eight characters. Long names in object files will be
	        ///     truncated if emitted to an executable file.
	        /// </summary>
	        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
            public string name;
	        /// <summary>
	        ///     Total size of the section when loaded into memory. If this value is greater than
	        ///     <see cref="sizeOfRawData" />, the section is zero-padded.This field is valid only
	        ///     for executable images and should be set to 0 for object files.
	        /// </summary>
	        public uint virtualSize;
	        /// <summary>
	        ///     For executable images this is the address of the first byte of the section, when
	        ///     loaded into memory, relative to the image base. For object files, this field is
	        ///     the address of the first byte before relocation is applied; for simplicity,
	        ///     compilers should set this to zero. Otherwise, it is an arbitrary value that is
	        ///     subtracted from offsets during relocation.
	        /// </summary>
	        public uint virtualAddress;
	        /// <summary>
	        ///     Size of the section (object file) or size of the initialized data on disk(image
	        ///     files). For executable image, this must be a multiple of FileAlignment from the
	        ///     optional header.If this is less than <see cref="virtualSize" /> the remainder of
	        ///     the section is zero filled. Because this field is rounded while the
	        ///     <see cref="virtualSize" /> field is not it is possible for this to be greater
	        ///     than <see cref="virtualSize" /> as well.When a section contains only
	        ///     uninitialized data, this field should be 0.
	        /// </summary>
	        public uint sizeOfRawData;
	        /// <summary>
	        ///     File pointer to sectionís first page within the COFF file. For executable images,
	        ///     this must be a multiple of FileAlignment from the optional header. For object
	        ///     files, the value should be aligned on a four-byte boundary for best performance.
	        ///     When a section contains only uninitialized data, this field should be 0.
	        /// </summary>
	        public uint pointerToRawData;
	        /// <summary>
	        ///     File pointer to beginning of relocation entries for the section. Set to 0 for
	        ///     executable images or if there are no relocations.
	        /// </summary>
	        public uint pointerToRelocations;
	        /// <summary>
	        ///     File pointer to beginning of line-number entries for the section.Set to 0 if
	        ///     there are no COFF line numbers.
	        /// </summary>
	        public uint pointerToLineNumbers;
	        /// <summary>
	        ///     Number of relocation entries for the section. Set to 0 for executable images.
	        /// </summary>
	        public ushort numberOfRelocations;
	        /// <summary>
	        ///     Number of line-number entries for the section.
	        /// </summary>
	        public ushort numberOfLineNumbers;
	        /// <summary>
	        ///     Flags describing sectionís characteristics. <see cref="SectionFlags" />
	        /// </summary>
	        public SectionFlags characteristics;
        }
    }
}