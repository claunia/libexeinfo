//
// Enums.cs
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

// ReSharper disable InconsistentNaming

namespace libexeinfo
{
    public partial class COFF
    {
        /// <summary>
        ///     The Characteristics field contains flags that indicate attributes of the object or image file.
        /// </summary>
        [Flags]
        public enum Characteristics : ushort
        {
            /// <summary>
            ///     Image only, Windows CE, and Microsoft Windows NT and later. This indicates that the file does not contain base
            ///     relocations and must therefore be loaded at its preferred base address. If the base address is not available, the
            ///     loader reports an error. The default behavior of the linker is to strip base relocations from executable (EXE)
            ///     files.
            /// </summary>
            IMAGE_FILE_RELOCS_STRIPPED = 0x0001,
            /// <summary>
            ///     Image only. This indicates that the image file is valid and can be run. If this flag is not set, it indicates a
            ///     linker error.
            /// </summary>
            IMAGE_FILE_EXECUTABLE_IMAGE = 0x0002,
            /// <summary>
            ///     COFF line numbers have been removed. This flag is deprecated and should be zero.
            /// </summary>
            IMAGE_FILE_LINE_NUMS_STRIPPED = 0x0004,
            /// <summary>
            ///     COFF symbol table entries for local symbols have been removed. This flag is deprecated and should be zero.
            /// </summary>
            IMAGE_FILE_LOCAL_SYMS_STRIPPED = 0x0008,
            /// <summary>
            ///     Obsolete. Aggressively trim working set. This flag is deprecated for Windows 2000 and later and must be zero.
            /// </summary>
            IMAGE_FILE_AGGRESSIVE_WS_TRIM = 0x0010,
            /// <summary>
            ///     Application can handle > 2-GB addresses.
            /// </summary>
            IMAGE_FILE_LARGE_ADDRESS_AWARE = 0x0020,
            /// <summary>
            ///     Machine is based on a 16-bit-word architecture.
            /// </summary>
            IMAGE_FILE_16BIT_MACHINE = 0x0040,
            /// <summary>
            ///     Little endian: the least significant bit (LSB) precedes the most significant bit (MSB) in memory. This flag is
            ///     deprecated and should be zero.
            /// </summary>
            IMAGE_FILE_BYTES_REVERSED_LO = 0x0080,
            /// <summary>
            ///     Machine is based on a 32-bit-word architecture.
            /// </summary>
            IMAGE_FILE_32BIT_MACHINE = 0x0100,
            /// <summary>
            ///     Debugging information is removed from the image file.
            /// </summary>
            IMAGE_FILE_DEBUG_STRIPPED = 0x0200,
            /// <summary>
            ///     If the image is on removable media, fully load it and copy it to the swap file.
            /// </summary>
            IMAGE_FILE_REMOVABLE_RUN_FROM_SWAP = 0x0400,
            /// <summary>
            ///     If the image is on network media, fully load it and copy it to the swap file.
            /// </summary>
            IMAGE_FILE_NET_RUN_FROM_SWAP = 0x0800,
            /// <summary>
            ///     The image file is a system file, not a user program.
            /// </summary>
            IMAGE_FILE_SYSTEM = 0x1000,
            /// <summary>
            ///     The image file is a dynamic-link library (DLL). Such files are considered executable files for almost all purposes,
            ///     although they cannot be directly run.
            /// </summary>
            IMAGE_FILE_DLL = 0x2000,
            /// <summary>
            ///     The file should be run only on a uniprocessor machine.
            /// </summary>
            IMAGE_FILE_UP_SYSTEM_ONLY = 0x4000,
            /// <summary>
            ///     Big endian: the MSB precedes the LSB in memory. This flag is deprecated and should be zero.
            /// </summary>
            IMAGE_FILE_BYTES_REVERSED_HI = 0x8000
        }

        /// <summary>
        ///     The Machine field has one of the following values that specifies its CPU type. An image file can be run only on the
        ///     specified machine or on a system that emulates the specified machine.
        /// </summary>
        public enum MachineTypes : ushort
        {
            /// <summary>
            ///     The contents of this field are assumed to be applicable to any machine type
            /// </summary>
            IMAGE_FILE_MACHINE_UNKNOWN = 0x0,
            /// <summary>
            ///     Alpha AXP
            /// </summary>
            IMAGE_FILE_MACHINE_ALPHA = 0x184,
            /// <summary>
            ///     Alpha AXP 64-bit.
            /// </summary>
            IMAGE_FILE_MACHINE_ALPHA64 = 0x284,
            /// <summary>
            ///     Matsushita AM33
            /// </summary>
            IMAGE_FILE_MACHINE_AM33 = 0x1d3,
            /// <summary>
            ///     x64
            /// </summary>
            IMAGE_FILE_MACHINE_AMD64 = 0x8664,
            /// <summary>
            ///     ARM little endian
            /// </summary>
            IMAGE_FILE_MACHINE_ARM = 0x1c0,
            /// <summary>
            ///     ARM64 little endian
            /// </summary>
            IMAGE_FILE_MACHINE_ARM64 = 0xaa64,
            /// <summary>
            ///     ARM Thumb-2 little endian
            /// </summary>
            IMAGE_FILE_MACHINE_ARMNT = 0x1c4,
            /// <summary>
            ///     Clipper
            /// </summary>
            IMAGE_FILE_MACHINE_CLIPPER = 0x17f,
            /// <summary>
            ///     EFI byte code
            /// </summary>
            IMAGE_FILE_MACHINE_EBC = 0xebc,
            /// <summary>
            ///     Intel 386 or later processors and compatible processors
            /// </summary>
            IMAGE_FILE_MACHINE_I386 = 0x14c,
            /// <summary>
            ///     Intel 386 or later processors and compatible processors, used by AIX
            /// </summary>
            IMAGE_FILE_MACHINE_I386_AIX = 0x175,
            /// <summary>
            ///     Intel Itanium processor family
            /// </summary>
            IMAGE_FILE_MACHINE_IA64 = 0x200,
            /// <summary>
            ///     Mitsubishi M32R little endian
            /// </summary>
            IMAGE_FILE_MACHINE_M32R = 0x9041,
            /// <summary>
            ///     Motorola 68000 series
            /// </summary>
            IMAGE_FILE_MACHINE_M68K = 0x268,
            /// <summary>
            ///     Motorola 68000 series
            /// </summary>
            IMAGE_FILE_MACHINE_M68K_OTHER = 0x150,
            /// <summary>
            ///     MIPS16
            /// </summary>
            IMAGE_FILE_MACHINE_MIPS16 = 0x266,
            /// <summary>
            ///     MIPS with FPU
            /// </summary>
            IMAGE_FILE_MACHINE_MIPSFPU = 0x366,
            /// <summary>
            ///     MIPS16 with FPU
            /// </summary>
            IMAGE_FILE_MACHINE_MIPSFPU16 = 0x466,
            /// <summary>
            ///     PowerPC little endian
            /// </summary>
            IMAGE_FILE_MACHINE_POWERPC = 0x1f0,
            /// <summary>
            ///     Power PC with floating point support
            /// </summary>
            IMAGE_FILE_MACHINE_POWERPCFP = 0x1f1,
            /// <summary>
            ///     MIPS big endian
            /// </summary>
            IMAGE_FILE_MACHINE_MIPSEB = 0x160,
            /// <summary>
            ///     MIPS little endian
            /// </summary>
            IMAGE_FILE_MACHINE_R3000 = 0x162,
            /// <summary>
            ///     MIPS little endian
            /// </summary>
            IMAGE_FILE_MACHINE_R4000 = 0x166,
            /// <summary>
            ///     MIPS little endian
            /// </summary>
            IMAGE_FILE_MACHINE_R10000 = 0x168,
            /// <summary>
            ///     RISC-V 32-bit address space
            /// </summary>
            IMAGE_FILE_MACHINE_RISCV32 = 0x5032,
            /// <summary>
            ///     RISC-V 64-bit address space
            /// </summary>
            IMAGE_FILE_MACHINE_RISCV64 = 0x5064,
            /// <summary>
            ///     RISC-V 128-bit address space
            /// </summary>
            IMAGE_FILE_MACHINE_RISCV128 = 0x5128,
            /// <summary>
            ///     Hitachi SH3
            /// </summary>
            IMAGE_FILE_MACHINE_SH3 = 0x1a2,
            /// <summary>
            ///     Hitachi SH3 DSP
            /// </summary>
            IMAGE_FILE_MACHINE_SH3DSP = 0x1a3,
            /// <summary>
            ///     Hitachi SH4
            /// </summary>
            IMAGE_FILE_MACHINE_SH4 = 0x1a6,
            /// <summary>
            ///     Hitachi SH5
            /// </summary>
            IMAGE_FILE_MACHINE_SH5 = 0x1a8,
            /// <summary>
            ///     Thumb
            /// </summary>
            IMAGE_FILE_MACHINE_THUMB = 0x1c2,
            /// <summary>
            ///     MIPS little-endian WCE v2
            /// </summary>
            IMAGE_FILE_MACHINE_WCEMIPSV2 = 0x169,
            /// <summary>
            ///     WE32000
            /// </summary>
            IMAGE_FILE_MACHINE_WE32000 = 0x170
        }

        [Flags]
        public enum SectionFlags : uint
        {
            /// <summary>
            ///     Reserved for future use.
            /// </summary>
            IMAGE_SCN_TYPE_DSECT = 0x00000001,
            /// <summary>
            ///     Reserved for future use.
            /// </summary>
            IMAGE_SCN_TYPE_NOLOAD = 0x00000002,
            /// <summary>
            ///     Reserved for future use.
            /// </summary>
            IMAGE_SCN_TYPE_GROUP = 0x00000004,
            /// <summary>
            ///     Section should not be padded to next boundary.
            ///     This is obsolete and replaced by <see cref="IMAGE_SCN_ALIGN_1BYTES" />.
            ///     This is valid for object files only.
            /// </summary>
            IMAGE_SCN_TYPE_NO_PAD = 0x00000008,
            /// <summary>
            ///     Reserved for future use.
            /// </summary>
            IMAGE_SCN_TYPE_COPY = 0x00000010,
            /// <summary>
            ///     Section contains executable code.
            /// </summary>
            IMAGE_SCN_CNT_CODE = 0x00000020,
            /// <summary>
            ///     Section contains initialized data.
            /// </summary>
            IMAGE_SCN_CNT_INITIALIZED_DATA = 0x00000040,
            /// <summary>
            ///     Section contains uninitialized data.
            /// </summary>
            IMAGE_SCN_CNT_UNINITIALIZED_DATA = 0x00000080,
            /// <summary>
            ///     Reserved for future use.
            /// </summary>
            IMAGE_SCN_LNK_OTHER = 0x00000100,
            /// <summary>
            ///     Section contains comments or other information. The .drectve section has this type. This is valid for object files
            ///     only.
            /// </summary>
            IMAGE_SCN_LNK_INFO = 0x00000200,
            /// <summary>
            ///     Reserved for future use.
            /// </summary>
            IMAGE_SCN_TYPE_OVER = 0x00000400,
            /// <summary>
            ///     Section will not become part of the image. This is valid for object files only.
            /// </summary>
            IMAGE_SCN_LNK_REMOVE = 0x00000800,
            /// <summary>
            ///     Section contains COMDAT data. This is valid for object files only.
            /// </summary>
            IMAGE_SCN_LNK_COMDAT = 0x00001000,
            /// <summary>
            ///     Reserved for future use.
            /// </summary>
            IMAGE_SCN_MEM_FARDATA = 0x00008000,
            /// <summary>
            ///     Reserved for future use.
            /// </summary>
            IMAGE_SCN_MEM_16BIT = 0x00020000,
            /// <summary>
            ///     Reserved for future use.
            /// </summary>
            IMAGE_SCN_MEM_LOCKED = 0x00040000,
            /// <summary>
            ///     Reserved for future use.
            /// </summary>
            IMAGE_SCN_MEM_PRELOAD = 0x00080000,
            /// <summary>
            ///     Align data on a 1-byte boundary. This is valid for object files only.
            /// </summary>
            IMAGE_SCN_ALIGN_1BYTES = 0x00100000,
            /// <summary>
            ///     Align data on a 2-byte boundary. This is valid for object files only.
            /// </summary>
            IMAGE_SCN_ALIGN_2BYTES = 0x00200000,
            /// <summary>
            ///     Align data on a 4-byte boundary. This is valid for object files only.
            /// </summary>
            IMAGE_SCN_ALIGN_4BYTES = 0x00300000,
            /// <summary>
            ///     Align data on a 8-byte boundary. This is valid for object files only.
            /// </summary>
            IMAGE_SCN_ALIGN_8BYTES = 0x00400000,
            /// <summary>
            ///     Align data on a 16-byte boundary. This is valid for object files only.
            /// </summary>
            IMAGE_SCN_ALIGN_16BYTES = 0x00500000,
            /// <summary>
            ///     Align data on a 32-byte boundary. This is valid for object files only.
            /// </summary>
            IMAGE_SCN_ALIGN_32BYTES = 0x00600000,
            /// <summary>
            ///     Align data on a 64-byte boundary. This is valid for object files only.
            /// </summary>
            IMAGE_SCN_ALIGN_64BYTES = 0x00700000,
            /// <summary>
            ///     Align data on a 128-byte boundary. This is valid for object files only.
            /// </summary>
            IMAGE_SCN_ALIGN_128BYTES = 0x00800000,
            /// <summary>
            ///     Align data on a 256-byte boundary. This is valid for object files only.
            /// </summary>
            IMAGE_SCN_ALIGN_256BYTES = 0x00900000,
            /// <summary>
            ///     Align data on a 512-byte boundary. This is valid for object files only.
            /// </summary>
            IMAGE_SCN_ALIGN_512BYTES = 0x00A00000,
            /// <summary>
            ///     Align data on a 1024-byte boundary. This is valid for object files only.
            /// </summary>
            IMAGE_SCN_ALIGN_1024BYTES = 0x00B00000,
            /// <summary>
            ///     Align data on a 2048-byte boundary. This is valid for object files only.
            /// </summary>
            IMAGE_SCN_ALIGN_2048BYTES = 0x00C00000,
            /// <summary>
            ///     Align data on a 4096-byte boundary. This is valid for object files only.
            /// </summary>
            IMAGE_SCN_ALIGN_4096BYTES = 0x00D00000,
            /// <summary>
            ///     Align data on a 8192-byte boundary. This is valid for object files only.
            /// </summary>
            IMAGE_SCN_ALIGN_8192BYTES = 0x00E00000,
            /// <summary>
            ///     Section contains extended relocations.
            /// </summary>
            IMAGE_SCN_LNK_NRELOC_OVFL = 0x01000000,
            /// <summary>
            ///     Section can be discarded as needed.
            /// </summary>
            IMAGE_SCN_MEM_DISCARDABLE = 0x02000000,
            /// <summary>
            ///     Section cannot be cached.
            /// </summary>
            IMAGE_SCN_MEM_NOT_CACHED = 0x04000000,
            /// <summary>
            ///     Section is not pageable.
            /// </summary>
            IMAGE_SCN_MEM_NOT_PAGED = 0x08000000,
            /// <summary>
            ///     Section can be shared in memory.
            /// </summary>
            IMAGE_SCN_MEM_SHARED = 0x10000000,
            /// <summary>
            ///     Section can be executed as code.
            /// </summary>
            IMAGE_SCN_MEM_EXECUTE = 0x20000000,
            /// <summary>
            ///     Section can be read.
            /// </summary>
            IMAGE_SCN_MEM_READ = 0x40000000,
            /// <summary>
            ///     Section can be written to.
            /// </summary>
            IMAGE_SCN_MEM_WRITE = 0x80000000
        }

        public const uint IMAGE_SCN_ALIGN_MASK = 0x00F00000;
    }
}