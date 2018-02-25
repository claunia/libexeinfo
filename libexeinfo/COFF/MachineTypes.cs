//
// MachineTypes.cs
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

namespace libexeinfo
{
    public partial class COFF
    {
        public static string MachineTypeToString(MachineTypes machine)
        {
            switch(machine)
            {
                case MachineTypes.IMAGE_FILE_MACHINE_UNKNOWN: return "Any machine";
                case MachineTypes.IMAGE_FILE_MACHINE_ALPHA:   return "Alpha AXP";
                case MachineTypes.IMAGE_FILE_MACHINE_ALPHA64: return "Alpha AXP 64-bit.";
                case MachineTypes.IMAGE_FILE_MACHINE_AM33:    return "Matsushita AM33";
                case MachineTypes.IMAGE_FILE_MACHINE_AMD64:   return "AMD64 / EM64T";
                case MachineTypes.IMAGE_FILE_MACHINE_ARM:     return "ARM little endian";
                case MachineTypes.IMAGE_FILE_MACHINE_ARM64:   return "ARM64 little endian";
                case MachineTypes.IMAGE_FILE_MACHINE_ARMNT:   return "ARM Thumb-2 little endian";
                case MachineTypes.IMAGE_FILE_MACHINE_EBC:     return "EFI byte code";
                case MachineTypes.IMAGE_FILE_MACHINE_I386:
                case MachineTypes.IMAGE_FILE_MACHINE_I386_AIX:
                    return "Intel 386 or later processors and compatible processors";
                case MachineTypes.IMAGE_FILE_MACHINE_IA64: return "Intel Itanium processor family";
                case MachineTypes.IMAGE_FILE_MACHINE_M32R: return "Mitsubishi M32R little endian";
                case MachineTypes.IMAGE_FILE_MACHINE_M68K:
                case MachineTypes.IMAGE_FILE_MACHINE_M68K_OTHER: return "Motorola 68000 series";
                case MachineTypes.IMAGE_FILE_MACHINE_MIPS16:     return "MIPS16";
                case MachineTypes.IMAGE_FILE_MACHINE_MIPSFPU:    return "MIPS with FPU";
                case MachineTypes.IMAGE_FILE_MACHINE_MIPSFPU16:  return "MIPS16 with FPU";
                case MachineTypes.IMAGE_FILE_MACHINE_POWERPC:    return "PowerPC little endian";
                case MachineTypes.IMAGE_FILE_MACHINE_POWERPCFP:  return "PowerPC with floating point support";
                case MachineTypes.IMAGE_FILE_MACHINE_MIPSEB:     return "MIPS R3000 or later (big endian)";
                case MachineTypes.IMAGE_FILE_MACHINE_R3000:      return "MIPS R3000 or later (little endian)";
                case MachineTypes.IMAGE_FILE_MACHINE_R4000:      return "MIPS R4000 or later (little endian)";
                case MachineTypes.IMAGE_FILE_MACHINE_R10000:     return "MIPS R10000 or later (little endian)";
                case MachineTypes.IMAGE_FILE_MACHINE_RISCV32:    return "RISC-V 32-bit address space";
                case MachineTypes.IMAGE_FILE_MACHINE_RISCV64:    return "RISC-V 64-bit address space";
                case MachineTypes.IMAGE_FILE_MACHINE_RISCV128:   return "RISC-V 128-bit address space";
                case MachineTypes.IMAGE_FILE_MACHINE_SH3:        return "Hitachi SH3";
                case MachineTypes.IMAGE_FILE_MACHINE_SH3DSP:     return "Hitachi SH3 DSP";
                case MachineTypes.IMAGE_FILE_MACHINE_SH4:        return "Hitachi SH4";
                case MachineTypes.IMAGE_FILE_MACHINE_SH5:        return "Hitachi SH5";
                case MachineTypes.IMAGE_FILE_MACHINE_THUMB:      return "ARM Thumb";
                case MachineTypes.IMAGE_FILE_MACHINE_WCEMIPSV2:  return "MIPS little-endian WCE v2";
                case MachineTypes.IMAGE_FILE_MACHINE_CLIPPER:    return "Clipper";
                case MachineTypes.IMAGE_FILE_MACHINE_WE32000:    return "WE32000 series";
                default:
                    return string.Format("Unknown machine type with code {0}", (ushort)machine);
            }
        }
    }
}