﻿//
// Enums.cs
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

namespace libexeinfo
{
    public enum Architecture
    {
        Unknown,
        Aarch64,
        Alpha,
        Am33,
        Amd64,
        AmdGpu,
        Arca,
        ARCompact,
        ARCompact2,
        Argonaut,
        Arm,
        Avr,
        Avr32,
        Ba1,
        Ba2,
        Blackfin,
        C16,
        CDP,
        Clipper,
        CloudShield,
        Coge,
        Coldfire,
        CommunicationEngine,
        CompactRisc,
        CompactRisc16,
        CompactRiscX,
        CoolEngine,
        CoreA,
        CoreA2,
        CrayNv2,
        Cris,
        Cuda,
        D10V,
        D30V,
        DeepExecutionProcessor,
        EfiByteCode,
        Elbrus,
        FR20,
        FR30,
        FRV,
        FT32,
        FX66,
        H8,
        Hobbit,
        Huany,
        I286,
        I386,
        I8051,
        I86,
        I860,
        I960,
        IA64,
        IP2K,
        Javelin,
        Kalimba,
        Lattice,
        M16C,
        M32C,
        M32R,
        M68HC05,
        M68HC08,
        M68HC11,
        M68HC12,
        M68HC16,
        M68K,
        M88K,
        M8C,
        MicroBlaze,
        Mips,
        Mips16,
        Mips3,
        NIOS2,
        Open8,
        OpenRisc,
        PaRisc,
        Pdp10,
        Pdp11,
        PicoJava,
        Power,
        PowerPc,
        PowerPc64,
        Prism,
        R32C,
        R78KOR,
        RiscV,
        S370,
        S390,
        Sh2,
        Sh3,
        Sh4,
        Sh5,
        Sharc,
        Sparc,
        Sparc64,
        Spu,
        StarCore,
        STM8,
        Thumb,
        Thumb2,
        Tile64,
        TileGx,
        TilePro,
        TinyJ,
        TriCore,
        TriMedia,
        V800,
        V850,
        Vax,
        VideoCore,
        We32000,
        Xcore,
        Xtensa,
        Z80
    }

    public static class Enums
    {
        public static readonly (Architecture arch, string shortName, string longName)[] ArchitectureName =
        {
            (Architecture.Unknown, "unknown", "Unknown"),
            (Architecture.Aarch64, "aarch64", "ARM64"),
            (Architecture.Alpha, "axp", "Alpha"), 
            (Architecture.Am33, "am33", "AM33"),
            (Architecture.Amd64, "amd64", "AMD64"),
            (Architecture.AmdGpu, "amdgpu", "AMD GPU"),
            (Architecture.Arca, "arca", "Arca RISC"), 
            (Architecture.ARCompact2, "arcompact2", "ARCompact 2"),
            (Architecture.ARCompact, "arcompact", "ARCompact"), 
            (Architecture.Argonaut, "arc", "Argonaut RISC Core"),
            (Architecture.Arm, "arm", "ARM"),
            (Architecture.Avr32, "avr32", "Atmel AVR32"), 
            (Architecture.Avr, "avr", "Atmel AVR"),
            (Architecture.Ba1, "ba1", "BA1"), 
            (Architecture.Ba2, "ba2", "BA2"),
            (Architecture.Blackfin, "blackfin", "Blackfin"), 
            (Architecture.C16, "c16", "Infineon C16x/XC16x"),
            (Architecture.CDP, "cdp", "Paneve CDP"),
            (Architecture.Clipper, "clipper", "Clipper"),
            (Architecture.CloudShield, "cloudshield", ""),
            (Architecture.Coge, "coge", "Cognitive Smart Memory Processor"),
            (Architecture.Coldfire, "coldfire", "Motorola ColdFire"),
            (Architecture.CommunicationEngine, "fce", "Freescale Communication Engine"),
            (Architecture.CompactRisc16, "cr16", "CompactRISC (16-bit)"),
            (Architecture.CompactRisc, "cr", "CompactRISC"),
            (Architecture.CompactRiscX, "crx", "CompactRISC X"),
            (Architecture.CoolEngine, "coole", "Bluechip Systems CoolEngine"), 
            (Architecture.CoreA2, "corea2", "Core-A (2nd gen.)"),
            (Architecture.CoreA, "corea", "Core-A"),
            (Architecture.CrayNv2, "craynv2", "Cray NV2"),
            (Architecture.Cris, "cris", "Axis Cris"), 
            (Architecture.Cuda, "cuda", "NVIDIA CUDA"), 
            (Architecture.D10V, "d10v", "Mitsubishi D10V"),
            (Architecture.D30V, "d30v", "Mitsubishi D30V"),
            (Architecture.DeepExecutionProcessor, "dep", "Deep Execution Processor"),
            (Architecture.EfiByteCode, "ebc", "EFI Byte Code"),
            (Architecture.Elbrus, "elbrus", "Elbrus"),
            (Architecture.FR20, "fr20", "Fujitsu FR20"),
            (Architecture.FR30, "fr30", "Fujitsu FR30"),
            (Architecture.FRV, "frv", "Fujitsu FR-V"), 
            (Architecture.FT32, "ft32", "FTDI FT32"),
            (Architecture.FX66, "fx66", "Siemens FX66"),
            (Architecture.H8, "h8", "Hitachi H8"),
            (Architecture.Hobbit, "hobbit", "AT&T Hobbit"),
            (Architecture.Huany, "huany", "Harvard University machine-independent"),
            (Architecture.I286, "i286", "Intel x86 (16-bit protected mode)"),
            (Architecture.I386, "ia32", "Intel IA-32"), 
            (Architecture.I8051, "8051", "Intel 8051"),
            (Architecture.I86, "i860", "Intel i860"),
            (Architecture.I86, "i86", "Intel x86 (16-bit)"),
            (Architecture.I960, "i960", "Intel 960"),
            (Architecture.IA64, "ia64", "Intel IA-64"),
            (Architecture.IP2K, "ip2k", "Ubicom IP2xxx"), 
            (Architecture.Javelin, "javelin", "Infineon Javelin"),
            (Architecture.Kalimba, "kalimba", "Kalimba"),
            (Architecture.Lattice, "lattice", "Lattice Mico32"), 
            (Architecture.M16C, "m16c", "Renesas M16C"),
            (Architecture.M32C, "m32c", "Renesas M32C"),
            (Architecture.M32R, "m32r", "Mitsubishi M32R"),
            (Architecture.M68HC05, "68hc05", "Motorola 68HC05"),
            (Architecture.M68HC08, "68hc08", "Motorola 68HC08"),
            (Architecture.M68HC11, "68hc11", "Motorola 68HC11"),
            (Architecture.M68HC12, "68hc12", "Motorola 68HC12"),
            (Architecture.M68HC16, "68hc16", "Motorola 68HC16"),
            (Architecture.M68K, "m68k", "Motorola 68000"),
            (Architecture.M88K, "m88k", "Motorola 88000"),
            (Architecture.M8C, "m8c", "Cypress M8C"),
            (Architecture.MicroBlaze, "microblaze", "MicroBlanse"),
            (Architecture.Mips16, "mips16", "MIPS16"),
            (Architecture.Mips3, "mips3", "MIPS III"), 
            (Architecture.Mips, "mips", "MIPS"),
            (Architecture.NIOS2, "nios2", "Altera Nios II"),
            (Architecture.Open8, "open8", "Open8"),
            (Architecture.OpenRisc, "openrisc", "OpenRisc"),
            (Architecture.PaRisc, "hppa", "HP PA-RISC"),
            (Architecture.Pdp10, "pdp10", "Digital PDP-10"),
            (Architecture.Pdp11, "pdp11", "Digital PDP-11"),
            (Architecture.PicoJava, "picojava", "PicoJava"),
            (Architecture.PowerPc64, "ppc64", "PowerPC (64-bit)"),
            (Architecture.PowerPc, "ppc", "PowerPC"),
            (Architecture.Power, "power", "POWER"), 
            (Architecture.Prism, "prism", "PRISM"),
            (Architecture.R32C, "r32c", "Renesas R32C"),
            (Architecture.R78KOR, "78kor", "Renesas 78KOR"),
            (Architecture.RiscV, "riscv", "RISC-V"),
            (Architecture.S370, "s370", "IBMSystem/370"),
            (Architecture.S390, "s390", "IBM S/390"), 
            (Architecture.Sh2, "sh2", "Hitachi SH2"),
            (Architecture.Sh3, "sh3", "Hitachi SH3"),
            (Architecture.Sh4, "sh4", "Hitachi SH4"),
            (Architecture.Sh5, "sh5", "Hitachi SH5"),
            (Architecture.Sharc, "sharc", "SHARC"),
            (Architecture.Sparc64, "sparc64", "SPARC (64-bit)"),
            (Architecture.Sparc, "sparc", "SPARC"),
            (Architecture.Spu, "spu", "IBM SPU"), 
            (Architecture.StarCore, "starcore", "Motorola Star*Core"),
            (Architecture.STM8, "stm8", "STM8"), 
            (Architecture.Thumb2, "thumb2", "ARM Thumb-2"),
            (Architecture.Thumb, "thumb", "ARM Thumb"),
            (Architecture.Tile64, "tile64", "Tile64"),
            (Architecture.TileGx, "tilegx", "TileGx"),
            (Architecture.TilePro, "tilepro", "Tile Pro"),
            (Architecture.TinyJ, "tinyj", "TinyJ"), 
            (Architecture.TriCore, "tricore", "TriCore"),
            (Architecture.TriMedia, "trimedia", "TriMedia"),
            (Architecture.V800, "v800", "NEC V800"),
            (Architecture.V850, "v850", "NEC V850"),
            (Architecture.Vax, "vax", "Digital VAX"),
            (Architecture.VideoCore, "videocore", "VideoCore"),
            (Architecture.We32000, "we32000", "WE32000"),
            (Architecture.Xcore, "xcore", "xCORE"),
            (Architecture.Xtensa, "xtensa", "Xtensa"),
            (Architecture.Z80, "z80", "Z80")
        };
    }
}