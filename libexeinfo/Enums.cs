namespace libexeinfo
{
    public enum Architecture
    {
        Unknown,
        Aarch64,
        Alpha,
        Am33,
        Amd64,
        Arm,
        Clipper,
        EfiByteCode,
        I86,
        I286,
        I386,
        I860,
        IA64,
        M32R,
        M68K,
        Mips,
        Mips3,
        Mips16,
        Power,
        PowerPc,
        PowerPc64,
        RiscV,
        Sh2,
        Sh3,
        Sh4,
        Sh5,
        Sparc,
        Sparc64,
        Thumb,
        Thumb2,
        We32000
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
            (Architecture.Arm, "arm", "ARM"),
            (Architecture.Clipper, "clipper", "Clipper"),
            (Architecture.EfiByteCode, "ebc", "EFI Byte Code"),
            (Architecture.I86, "i86", "Intel x86 (16-bit)"),
            (Architecture.I286, "i286", "Intel x86 (16-bit protected mode)"),
            (Architecture.I386, "ia32", "Intel IA-32"),
            (Architecture.I86, "i860", "Intel i860"),
            (Architecture.IA64, "ia64", "Intel IA-64"),
            (Architecture.M32R, "m32r", "Mitsubishi M32R"),
            (Architecture.M68K, "m68k", "Motorola 68000"),
            (Architecture.Mips, "mips", "MIPS"),
            (Architecture.Mips3, "mips3", "MIPS III"),
            (Architecture.Mips16, "mips16", "MIPS16"),
            (Architecture.Power, "power", "POWER"),
            (Architecture.PowerPc, "ppc", "PowerPC"),
            (Architecture.PowerPc64, "ppc64", "PowerPC (64-bit)"),
            (Architecture.RiscV, "riscv", "RISC-V"),
            (Architecture.Sh2, "sh2", "Hitachi SH2"),
            (Architecture.Sh3, "sh3", "Hitachi SH3"),
            (Architecture.Sh4, "sh4", "Hitachi SH4"),
            (Architecture.Sh5, "sh5", "Hitachi SH5"),
            (Architecture.Sparc, "sparc", "SPARC"),
            (Architecture.Sparc64, "sparc64", "SPARC (64-bit)"),
            (Architecture.Thumb, "thumb", "ARM Thumb"),
            (Architecture.Thumb2, "thumb2", "ARM Thumb-2"),
            (Architecture.We32000, "we32000", "WE32000")
        };
    }
    
}