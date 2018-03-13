// ReSharper disable InconsistentNaming

using System;

namespace libexeinfo
{
    public partial class ELF
    {
        enum eiClass : byte
        {
            /// <summary>Invalid class</summary>
            ELFCLASSNONE = 0,
            /// <summary>32-bit objects</summary>
            ELFCLASS32 = 1,
            /// <summary>64-bit objects</summary>
            ELFCLASS64 = 2
        }

        enum eiData : byte
        {
            /// <summary>Invalid data encoding</summary>
            ELFDATANONE = 0,
            /// <summary>Little-endian</summary>
            ELFDATA2LSB = 1,
            /// <summary>Big-endian</summary>
            ELFDATA2MSB = 2
        }

        enum eiOsabi : byte
        {
            /// <summary>No extensions or unspecified</summary>
            ELFOSABI_NONE = 0,
            /// <summary>Hewlett-Packard HP-UX</summary>
            ELFOSABI_HPUX = 1,
            /// <summary>NetBSD</summary>
            ELFOSABI_NETBSD = 2,
            /// <summary>GNU</summary>
            ELFOSABI_GNU = 3,
            /// <summary>Linux, historical - alias for ELFOSABI_GNU</summary>
            ELFOSABI_LINUX = ELFOSABI_GNU,
            /// <summary>Sun Solaris</summary>
            ELFOSABI_SOLARIS = 6,
            /// <summary>AIX</summary>
            ELFOSABI_AIX = 7,
            /// <summary>IRIX</summary>
            ELFOSABI_IRIX = 8,
            /// <summary>FreeBSD</summary>
            ELFOSABI_FREEBSD = 9,
            /// <summary>Compaq TRU64 UNIX</summary>
            ELFOSABI_TRU64 = 10,
            /// <summary>Novell Modesto</summary>
            ELFOSABI_MODESTO = 11,
            /// <summary>Open BSD</summary>
            ELFOSABI_OPENBSD = 12,
            /// <summary>Open VMS</summary>
            ELFOSABI_OPENVMS = 13,
            /// <summary>Hewlett-Packard Non-Stop Kernel</summary>
            ELFOSABI_NSK = 14,
            /// <summary>Amiga Research OS</summary>
            ELFOSABI_AROS = 15,
            /// <summary>The FenixOS highly scalable multi-core OS</summary>
            ELFOSABI_FENIXOS = 16,
            /// <summary>Nuxi CloudABI</summary>
            ELFOSABI_CLOUDABI = 17,
            /// <summary>Stratus Technologies OpenVOS</summary>
            ELFOSABI_OPENVOS = 18,
            /// <summary>The object contains symbol versioning extensions</summary>
            ELFOSABI_ARM_AEABI = 64,
            /// <summary>ARM</summary>
            ELFOSABI_ARM = 97,
            /// <summary>Standalone (embedded applications)</summary>
            ELFOSABI_STANDALONE = 255
        }

        enum eiVersion : byte
        {
            EV_NONE    = (byte)eVersion.EV_NONE,
            EV_CURRENT = (byte)eVersion.EV_CURRENT
        }

        enum eMachine : ushort
        {
            /// <summary>No machine</summary>
            EM_NONE = 0,
            /// <summary>AT&amp;T WE 32100</summary>
            EM_M32 = 1,
            /// <summary>SPARC</summary>
            EM_SPARC = 2,
            /// <summary>Intel 80386</summary>
            EM_386 = 3,
            /// <summary>Motorola 68000</summary>
            EM_68K = 4,
            /// <summary>Motorola 88000</summary>
            EM_88K = 5,
            /// <summary>Intel MCU</summary>
            EM_IAMCU = 6,
            /// <summary>Intel 80860</summary>
            EM_860 = 7,
            /// <summary>MIPS I Architecture</summary>
            EM_MIPS = 8,
            /// <summary>IBM System/370 Processor</summary>
            EM_S370 = 9,
            /// <summary>MIPS RS3000 Little-endian</summary>
            EM_MIPS_RS3_LE = 10,
            /// <summary>Hewlett-Packard PA-RISC</summary>
            EM_PARISC = 15,
            /// <summary>Fujitsu VPP500</summary>
            EM_VPP500 = 17,
            /// <summary>Enhanced instruction set SPARC</summary>
            EM_SPARC32PLUS = 18,
            /// <summary>Intel 80960</summary>
            EM_960 = 19,
            /// <summary>PowerPC</summary>
            EM_PPC = 20,
            /// <summary>64-bit PowerPC</summary>
            EM_PPC64 = 21,
            /// <summary>IBM System/390 Processor</summary>
            EM_S390 = 22,
            /// <summary>IBM SPU/SPC</summary>
            EM_SPU = 23,
            /// <summary>NEC V800</summary>
            EM_V800 = 36,
            /// <summary>Fujitsu FR20</summary>
            EM_FR20 = 37,
            /// <summary>TRW RH-32</summary>
            EM_RH32 = 38,
            /// <summary>Motorola RCE</summary>
            EM_RCE = 39,
            /// <summary>ARM 32-bit architecture (AARCH32)</summary>
            EM_ARM = 40,
            /// <summary>Digital Alpha</summary>
            EM_ALPHA = 41,
            /// <summary>Hitachi SH</summary>
            EM_SH = 42,
            /// <summary>SPARC Version 9</summary>
            EM_SPARCV9 = 43,
            /// <summary>Siemens TriCore embedded processor</summary>
            EM_TRICORE = 44,
            /// <summary>Argonaut RISC Core, Argonaut Technologies Inc.</summary>
            EM_ARC = 45,
            /// <summary>Hitachi H8/300</summary>
            EM_H8_300 = 46,
            /// <summary>Hitachi H8/300H</summary>
            EM_H8_300H = 47,
            /// <summary>Hitachi H8S</summary>
            EM_H8S = 48,
            /// <summary>Hitachi H8/500</summary>
            EM_H8_500 = 49,
            /// <summary>Intel IA-64 processor architecture</summary>
            EM_IA_64 = 50,
            /// <summary>Stanford MIPS-X</summary>
            EM_MIPS_X = 51,
            /// <summary>Motorola ColdFire</summary>
            EM_COLDFIRE = 52,
            /// <summary>Motorola M68HC12</summary>
            EM_68HC12 = 53,
            /// <summary>Fujitsu MMA Multimedia Accelerator</summary>
            EM_MMA = 54,
            /// <summary>Siemens PCP</summary>
            EM_PCP = 55,
            /// <summary>Sony nCPU embedded RISC processor</summary>
            EM_NCPU = 56,
            /// <summary>Denso NDR1 microprocessor</summary>
            EM_NDR1 = 57,
            /// <summary>Motorola Star*Core processor</summary>
            EM_STARCORE = 58,
            /// <summary>Toyota ME16 processor</summary>
            EM_ME16 = 59,
            /// <summary>STMicroelectronics ST100 processor</summary>
            EM_ST100 = 60,
            /// <summary>Advanced Logic Corp. TinyJ embedded processor family</summary>
            EM_TINYJ = 61,
            /// <summary>AMD x86-64 architecture</summary>
            EM_X86_64 = 62,
            /// <summary>Sony DSP Processor</summary>
            EM_PDSP = 63,
            /// <summary>Digital Equipment Corp. PDP-10</summary>
            EM_PDP10 = 64,
            /// <summary>Digital Equipment Corp. PDP-11</summary>
            EM_PDP11 = 65,
            /// <summary>Siemens FX66 microcontroller</summary>
            EM_FX66 = 66,
            /// <summary>STMicroelectronics ST9+ 8/16 bit microcontroller</summary>
            EM_ST9PLUS = 67,
            /// <summary>STMicroelectronics ST7 8-bit microcontroller</summary>
            EM_ST7 = 68,
            /// <summary>Motorola MC68HC16 Microcontroller</summary>
            EM_68HC16 = 69,
            /// <summary>Motorola MC68HC11 Microcontroller</summary>
            EM_68HC11 = 70,
            /// <summary>Motorola MC68HC08 Microcontroller</summary>
            EM_68HC08 = 71,
            /// <summary>Motorola MC68HC05 Microcontroller</summary>
            EM_68HC05 = 72,
            /// <summary>Silicon Graphics SVx</summary>
            EM_SVX = 73,
            /// <summary>STMicroelectronics ST19 8-bit microcontroller</summary>
            EM_ST19 = 74,
            /// <summary>Digital VAX</summary>
            EM_VAX = 75,
            /// <summary>Axis Communications 32-bit embedded processor</summary>
            EM_CRIS = 76,
            /// <summary>Infineon Technologies 32-bit embedded processor</summary>
            EM_JAVELIN = 77,
            /// <summary>Element 14 64-bit DSP Processor</summary>
            EM_FIREPATH = 78,
            /// <summary>LSI Logic 16-bit DSP Processor</summary>
            EM_ZSP = 79,
            /// <summary>Donald Knuth's educational 64-bit processor</summary>
            EM_MMIX = 80,
            /// <summary>Harvard University machine-independent object files</summary>
            EM_HUANY = 81,
            /// <summary>SiTera Prism</summary>
            EM_PRISM = 82,
            /// <summary>Atmel AVR 8-bit microcontroller</summary>
            EM_AVR = 83,
            /// <summary>Fujitsu FR30</summary>
            EM_FR30 = 84,
            /// <summary>Mitsubishi D10V</summary>
            EM_D10V = 85,
            /// <summary>Mitsubishi D30V</summary>
            EM_D30V = 86,
            /// <summary>NEC v850</summary>
            EM_V850 = 87,
            /// <summary>Mitsubishi M32R</summary>
            EM_M32R = 88,
            /// <summary>Matsushita MN10300</summary>
            EM_MN10300 = 89,
            /// <summary>Matsushita MN10200</summary>
            EM_MN10200 = 90,
            /// <summary>picoJava</summary>
            EM_PJ = 91,
            /// <summary>OpenRISC 32-bit embedded processor</summary>
            EM_OPENRISC = 92,
            /// <summary>ARC International ARCompact processor</summary>
            EM_ARC_COMPACT = 93,
            /// <summary>Tensilica Xtensa Architecture</summary>
            EM_XTENSA = 94,
            /// <summary>Alphamosaic VideoCore processor</summary>
            EM_VIDEOCORE = 95,
            /// <summary>Thompson Multimedia General Purpose Processor</summary>
            EM_TMM_GPP = 96,
            /// <summary>National Semiconductor 32000 series</summary>
            EM_NS32K = 97,
            /// <summary>Tenor Network TPC processor</summary>
            EM_TPC = 98,
            /// <summary>Trebia SNP 1000 processor</summary>
            EM_SNP1K = 99,
            /// <summary>STMicroelectronics ST200 microcontroller</summary>
            EM_ST200 = 100,
            /// <summary>Ubicom IP2xxx microcontroller family</summary>
            EM_IP2K = 101,
            /// <summary>MAX Processor</summary>
            EM_MAX = 102,
            /// <summary>National Semiconductor CompactRISC microprocessor</summary>
            EM_CR = 103,
            /// <summary>Fujitsu F2MC16</summary>
            EM_F2MC16 = 104,
            /// <summary>Texas Instruments embedded microcontroller msp430</summary>
            EM_MSP430 = 105,
            /// <summary>Analog Devices Blackfin (DSP) processor</summary>
            EM_BLACKFIN = 106,
            /// <summary>S1C33 Family of Seiko Epson processors</summary>
            EM_SE_C33 = 107,
            /// <summary>Sharp embedded microprocessor</summary>
            EM_SEP = 108,
            /// <summary>Arca RISC Microprocessor</summary>
            EM_ARCA = 109,
            /// <summary>Microprocessor series from PKU-Unity Ltd. and MPRC of Peking University</summary>
            EM_UNICORE = 110,
            /// <summary>eXcess: 16/32/64-bit configurable embedded CPU</summary>
            EM_EXCESS = 111,
            /// <summary>Icera Semiconductor Inc. Deep Execution Processor</summary>
            EM_DXP = 112,
            /// <summary>Altera Nios II soft-core processor</summary>
            EM_ALTERA_NIOS2 = 113,
            /// <summary>National Semiconductor CompactRISC CRX microprocessor</summary>
            EM_CRX = 114,
            /// <summary>Motorola XGATE embedded processor</summary>
            EM_XGATE = 115,
            /// <summary>Infineon C16x/XC16x processor</summary>
            EM_C166 = 116,
            /// <summary>Renesas M16C series microprocessors</summary>
            EM_M16C = 117,
            /// <summary>Microchip Technology dsPIC30F Digital Signal Controller</summary>
            EM_DSPIC30F = 118,
            /// <summary>Freescale Communication Engine RISC core</summary>
            EM_CE = 119,
            /// <summary>Renesas M32C series microprocessors</summary>
            EM_M32C = 120,
            /// <summary>Altium TSK3000 core</summary>
            EM_TSK3000 = 131,
            /// <summary>Freescale RS08 embedded processor</summary>
            EM_RS08 = 132,
            /// <summary>Analog Devices SHARC family of 32-bit DSP processors</summary>
            EM_SHARC = 133,
            /// <summary>Cyan Technology eCOG2 microprocessor</summary>
            EM_ECOG2 = 134,
            /// <summary>Sunplus S+core7 RISC processor</summary>
            EM_SCORE7 = 135,
            /// <summary>New Japan Radio (NJR) 24-bit DSP Processor</summary>
            EM_DSP24 = 136,
            /// <summary>Broadcom VideoCore III processor</summary>
            EM_VIDEOCORE3 = 137,
            /// <summary>RISC processor for Lattice FPGA architecture</summary>
            EM_LATTICEMICO32 = 138,
            /// <summary>Seiko Epson C17 family</summary>
            EM_SE_C17 = 139,
            /// <summary>The Texas Instruments TMS320C6000 DSP family</summary>
            EM_TI_C6000 = 140,
            /// <summary>The Texas Instruments TMS320C2000 DSP family</summary>
            EM_TI_C2000 = 141,
            /// <summary>The Texas Instruments TMS320C55x DSP family</summary>
            EM_TI_C5500 = 142,
            /// <summary>Texas Instruments Application Specific RISC Processor, 32bit fetch</summary>
            EM_TI_ARP32 = 143,
            /// <summary>Texas Instruments Programmable Realtime Unit</summary>
            EM_TI_PRU = 144,
            /// <summary>STMicroelectronics 64bit VLIW Data Signal Processor</summary>
            EM_MMDSP_PLUS = 160,
            /// <summary>Cypress M8C microprocessor</summary>
            EM_CYPRESS_M8C = 161,
            /// <summary>Renesas R32C series microprocessors</summary>
            EM_R32C = 162,
            /// <summary>NXP Semiconductors TriMedia architecture family</summary>
            EM_TRIMEDIA = 163,
            /// <summary>QUALCOMM DSP6 Processor</summary>
            EM_QDSP6 = 164,
            /// <summary>Intel 8051 and variants</summary>
            EM_8051 = 165,
            /// <summary>STMicroelectronics STxP7x family of configurable and extensible RISC processors</summary>
            EM_STXP7X = 166,
            /// <summary>Andes Technology compact code size embedded RISC processor family</summary>
            EM_NDS32 = 167,
            /// <summary>Cyan Technology eCOG1X family</summary>
            EM_ECOG1 = 168,
            /// <summary>Cyan Technology eCOG1X family</summary>
            EM_ECOG1X = 168,
            /// <summary>Dallas Semiconductor MAXQ30 Core Micro-controllers</summary>
            EM_MAXQ30 = 169,
            /// <summary>New Japan Radio (NJR) 16-bit DSP Processor</summary>
            EM_XIMO16 = 170,
            /// <summary>M2000 Reconfigurable RISC Microprocessor</summary>
            EM_MANIK = 171,
            /// <summary>Cray Inc. NV2 vector architecture</summary>
            EM_CRAYNV2 = 172,
            /// <summary>Renesas RX family</summary>
            EM_RX = 173,
            /// <summary>Imagination Technologies META processor architecture</summary>
            EM_METAG = 174,
            /// <summary>MCST Elbrus general purpose hardware architecture</summary>
            EM_MCST_ELBRUS = 175,
            /// <summary>Cyan Technology eCOG16 family</summary>
            EM_ECOG16 = 176,
            /// <summary>National Semiconductor CompactRISC CR16 16-bit microprocessor</summary>
            EM_CR16 = 177,
            /// <summary>Freescale Extended Time Processing Unit</summary>
            EM_ETPU = 178,
            /// <summary>Infineon Technologies SLE9X core</summary>
            EM_SLE9X = 179,
            /// <summary>Intel L10M</summary>
            EM_L10M = 180,
            /// <summary>Intel K10M</summary>
            EM_K10M = 181,
            /// <summary>ARM 64-bit architecture (AARCH64)</summary>
            EM_AARCH64 = 183,
            /// <summary>Atmel Corporation 32-bit microprocessor family</summary>
            EM_AVR32 = 185,
            /// <summary>STMicroeletronics STM8 8-bit microcontroller</summary>
            EM_STM8 = 186,
            /// <summary>Tilera TILE64 multicore architecture family</summary>
            EM_TILE64 = 187,
            /// <summary>Tilera TILEPro multicore architecture family</summary>
            EM_TILEPRO = 188,
            /// <summary>Xilinx MicroBlaze 32-bit RISC soft processor core</summary>
            EM_MICROBLAZE = 189,
            /// <summary>NVIDIA CUDA architecture</summary>
            EM_CUDA = 190,
            /// <summary>Tilera TILE-Gx multicore architecture family</summary>
            EM_TILEGX = 191,
            /// <summary>CloudShield architecture family</summary>
            EM_CLOUDSHIELD = 192,
            /// <summary>KIPO-KAIST Core-A 1st generation processor family</summary>
            EM_COREA_1ST = 193,
            /// <summary>KIPO-KAIST Core-A 2nd generation processor family</summary>
            EM_COREA_2ND = 194,
            /// <summary>Synopsys ARCompact V2</summary>
            EM_ARC_COMPACT2 = 195,
            /// <summary>Open8 8-bit RISC soft processor core</summary>
            EM_OPEN8 = 196,
            /// <summary>Renesas RL78 family</summary>
            EM_RL78 = 197,
            /// <summary>Broadcom VideoCore V processor</summary>
            EM_VIDEOCORE5 = 198,
            /// <summary>Renesas 78KOR family</summary>
            EM_78KOR = 199,
            /// <summary>Freescale 56800EX Digital Signal Controller (DSC)</summary>
            EM_56800EX = 200,
            /// <summary>Beyond BA1 CPU architecture</summary>
            EM_BA1 = 201,
            /// <summary>Beyond BA2 CPU architecture</summary>
            EM_BA2 = 202,
            /// <summary>XMOS xCORE processor family</summary>
            EM_XCORE = 203,
            /// <summary>Microchip 8-bit PIC(r) family</summary>
            EM_MCHP_PIC = 204,
            /// <summary>Reserved by Intel</summary>
            EM_INTEL205 = 205,
            /// <summary>Reserved by Intel</summary>
            EM_INTEL206 = 206,
            /// <summary>Reserved by Intel</summary>
            EM_INTEL207 = 207,
            /// <summary>Reserved by Intel</summary>
            EM_INTEL208 = 208,
            /// <summary>Reserved by Intel</summary>
            EM_INTEL209 = 209,
            /// <summary>KM211 KM32 32-bit processor</summary>
            EM_KM32 = 210,
            /// <summary>KM211 KMX32 32-bit processor</summary>
            EM_KMX32 = 211,
            /// <summary>KM211 KMX16 16-bit processor</summary>
            EM_KMX16 = 212,
            /// <summary>KM211 KMX8 8-bit processor</summary>
            EM_KMX8 = 213,
            /// <summary>KM211 KVARC processor</summary>
            EM_KVARC = 214,
            /// <summary>Paneve CDP architecture family</summary>
            EM_CDP = 215,
            /// <summary>Cognitive Smart Memory Processor</summary>
            EM_COGE = 216,
            /// <summary>Bluechip Systems CoolEngine</summary>
            EM_COOL = 217,
            /// <summary>Nanoradio Optimized RISC</summary>
            EM_NORC = 218,
            /// <summary>CSR Kalimba architecture family</summary>
            EM_CSR_KALIMBA = 219,
            /// <summary>Zilog Z80</summary>
            EM_Z80 = 220,
            /// <summary>Controls and Data Services VISIUMcore processor</summary>
            EM_VISIUM = 221,
            /// <summary>FTDI Chip FT32 high performance 32-bit RISC architecture</summary>
            EM_FT32 = 222,
            /// <summary>Moxie processor family</summary>
            EM_MOXIE = 223,
            /// <summary>AMD GPU architecture</summary>
            EM_AMDGPU = 224,
            /// <summary>RISC-V</summary>
            EM_RISCV = 243,
            EM_ALPHA_OLD = 0x9026,
            /// <summary>Bogus old v850 magic number, used by old tools.</summary>
            EM_CYGNUS_V850 = 0x9080,
            /// <summary>Bogus old m32r magic number, used by old tools.</summary>
            EM_CYGNUS_M32R = 0x9041,
            /// <summary>This is the old interim value for S/390 architecture.</summary>
            EM_S390_OLD = 0xA390,
            /// <summary>Fujitsu FR-V</summary>
            EM_FRV = 0x5441
        }

        enum eType : ushort
        {
            /// <summary>No file type</summary>
            ET_NONE = 0,
            /// <summary>Relocatable file</summary>
            ET_REL = 1,
            /// <summary>Executable file</summary>
            ET_EXEC = 2,
            /// <summary>Shared object file</summary>
            ET_DYN = 3,
            /// <summary>Core file</summary>
            ET_CORE = 4,
            /// <summary>Operating system-specific</summary>
            ET_LOOS = 0xfe00,
            /// <summary>Operating system-specific</summary>
            ET_HIOS = 0xfeff,
            /// <summary>Processor-specific</summary>
            ET_LOPROC = 0xff00,
            /// <summary>Processor-specific</summary>
            ET_HIPROC = 0xffff
        }

        enum eVersion : uint
        {
            /// <summary>Invalid version</summary>
            EV_NONE = 0,
            /// <summary>Current version</summary>
            EV_CURRENT = 1
        }

        enum shType : uint
        {
            /// <summary>
            ///     This value marks the section header as inactive; it does not have an associated section. Other members of the
            ///     section header have undefined values.
            /// </summary>
            SHT_NULL = 0,
            /// <summary>
            ///     The section holds information defined by the program, whose format and meaning are determined solely by the
            ///     program.
            /// </summary>
            SHT_PROGBITS = 1,
            /// <summary>
            ///     These sections hold a symbol table. Currently, an object file may have only one section of each type, but this
            ///     restriction may be relaxed in the future. Typically, SHT_SYMTAB provides symbols for link editing, though it may
            ///     also be used for dynamic linking. As a complete symbol table, it may contain many symbols unnecessary for dynamic
            ///     linking. Consequently, an object file may also contain a SHT_DYNSYM section, which holds a minimal set of dynamic
            ///     linking symbols, to save space.
            /// </summary>
            SHT_SYMTAB = 2,
            /// <summary>The section holds a string table. An object file may have multiple string table sections.</summary>
            SHT_STRTAB = 3,
            /// <summary>
            ///     The section holds relocation entries with explicit addends, such as type Elf32_Rela for the 32-bit class of
            ///     object files or type Elf64_Rela for the 64-bit class of object files. An object file may have multiple relocation
            ///     sections.
            /// </summary>
            SHT_RELA = 4,
            /// <summary>
            ///     The section holds a symbol hash table. Currently, an object file may have only one hash table, but this
            ///     restriction may be relaxed in the future.
            /// </summary>
            SHT_HASH = 5,
            /// <summary>
            ///     The section holds information for dynamic linking. Currently, an object file may have only one dynamic
            ///     section, but this restriction may be relaxed in the future.
            /// </summary>
            SHT_DYNAMIC = 6,
            /// <summary>The section holds information that marks the file in some way.</summary>
            SHT_NOTE = 7,
            /// <summary>
            ///     A section of this type occupies no space in the file but otherwise resembles SHT_PROGBITS. Although this
            ///     section contains no bytes, the sh_offset member contains the conceptual file offset.
            /// </summary>
            SHT_NOBITS = 8,
            /// <summary>
            ///     The section holds relocation entries without explicit addends, such as type Elf32_Rel for the 32-bit class of
            ///     object files or type Elf64_Rel for the 64-bit class of object files. An object file may have multiple relocation
            ///     sections.
            /// </summary>
            SHT_REL = 9,
            /// <summary>This section type is reserved but has unspecified semantics.</summary>
            SHT_SHLIB = 10,
            /// <summary>
            ///     These sections hold a symbol table. Currently, an object file may have only one section of each type, but this
            ///     restriction may be relaxed in the future. Typically, SHT_SYMTAB provides symbols for link editing, though it may
            ///     also be used for dynamic linking. As a complete symbol table, it may contain many symbols unnecessary for dynamic
            ///     linking. Consequently, an object file may also contain a SHT_DYNSYM section, which holds a minimal set of dynamic
            ///     linking symbols, to save space.
            /// </summary>
            SHT_DYNSYM = 11,
            /// <summary>
            ///     This section contains an array of pointers to initialization functions. Each pointer in the array is taken as
            ///     a parameterless procedure with a void return.
            /// </summary>
            SHT_INIT_ARRAY = 14,
            /// <summary>
            ///     This section contains an array of pointers to termination functions. Each pointer in the array is taken as a
            ///     parameterless procedure with a void return.
            /// </summary>
            SHT_FINI_ARRAY = 15,
            /// <summary>
            ///     This section contains an array of pointers to functions that are invoked before all other initialization
            ///     functions. Each pointer in the array is taken as a parameterless procedure with a void return.
            /// </summary>
            SHT_PREINIT_ARRAY = 16,
            /// <summary>
            ///     This section defines a section group. A section group is a set of sections that are related and that must be
            ///     treated specially by the linker (see below for further details). Sections of type SHT_GROUP may appear only in
            ///     relocatable objects (objects with the ELF header e_type member set to ET_REL). The section header table entry for a
            ///     group section must appear in the section header table before the entries for any of the sections that are members
            ///     of the group.
            /// </summary>
            SHT_GROUP = 17,
            /// <summary>
            ///     This section is associated with a symbol table section and is required if any of the section header indexes
            ///     referenced by that symbol table contain the escape value SHN_XINDEX. The section is an array of Elf32_Word values.
            ///     Each value corresponds one to one with a symbol table entry and appear in the same order as those entries. The
            ///     values represent the section header indexes against which the symbol table entries are defined. Only if the
            ///     corresponding symbol table entry's st_shndx field contains the escape value SHN_XINDEX will the matching Elf32_Word
            ///     hold the actual section header index; otherwise, the entry must be SHN_UNDEF (0).
            /// </summary>
            SHT_SYMTAB_SHNDX = 18,
            /// <summary>Values from this to <see cref="SHT_HIOS" /> are reserved for operating system-specific semantics</summary>
            SHT_LOOS = 0x60000000,
            /// <summary>Values from <see cref="SHT_LOOS" /> to this are reserved for operating system-specific semantics</summary>
            SHT_HIOS = 0x6fffffff,
            /// <summary>Values from this to <see cref="SHT_HIPROC" /> are reserved for processor-specific semantics</summary>
            SHT_LOPROC = 0x70000000,
            /// <summary>Values from <see cref="SHT_LOPROC" /> to this are reserved for processor-specific semantics</summary>
            SHT_HIPROC = 0x7fffffff,
            /// <summary>Values from this to <see cref="SHT_HIUSER" /> are reserved for application programs</summary>
            SHT_LOUSER = 0x80000000,
            /// <summary>Values from <see cref="SHT_LOUSER" /> to this are reserved for application programs</summary>
            SHT_HIUSER = 0xffffffff
        }

        enum shTypeGnu : uint
        {
            /// <summary>This section contains the symbol versions that are provided.</summary>
            SHT_GNU_VERDEF = 0x6ffffffd,
            /// <summary>This section contains the symbol versions that are required.</summary>
            SHT_GNU_VERNEED = 0x6ffffffe,
            /// <summary>This section contains the Symbol Version Table.</summary>
            SHT_GNU_VERSYM = 0x6fffffff
        }

        enum shTypeAmd64 : uint
        {
            /// <summary>This section contains unwind function table entries for stack unwinding.</summary>
            SHT_X86_64_UNWIND = 0x70000001
        }

        enum shTypeArm : uint
        {
            /// <summary>Exception Index table</summary>
            SHT_ARM_EXIDX = 0x70000001,
            /// <summary>BPABI DLL dynamic linking pre-emption map</summary>
            SHT_ARM_PREEMPTMAP = 0x70000002,
            /// <summary>Object file compatibility attributes</summary>
            SHT_ARM_ATTRIBUTES = 0x70000003,
            /// <summary>See DBGOVL for details</summary>
            SHT_ARM_DEBUGOVERLAY = 0x70000004,
            /// <summary>See DBGOVL for details</summary>
            SHT_ARM_OVERLAYSECTION = 0x70000005
        }

        enum shTypeMips : uint
        {
            /// <summary>
            ///     The section contains information about the set of dynamic shared object libraries used when statically linking
            ///     a program. Each entry contains information such as the library name, timestamp, and version.
            /// </summary>
            SHT_MIPS_LIBLIST = 0x70000000,
            /// <summary>
            ///     The section contains a list of symbols in an executable whose definitions conflict with shared-object defined
            ///     symbols.
            /// </summary>
            SHT_MIPS_CONFLICT = 0x70000002,
            /// <summary>
            ///     The section contains the global pointer table. The global pointer table includes a list of possible global
            ///     data area sizes. The list allows the linker to provide the user with information on the optimal size criteria to
            ///     use for gp register relative addressing.
            /// </summary>
            SHT_MIPS_GPTAB = 0x70000003,
            /// <summary>This section type is reserved and the contents are unspecified. The section contents can be ignored.</summary>
            SHT_MIPS_UCODE = 0x70000004,
            /// <summary>
            ///     The section contains debug information specific to MIPS. An ABI-compliant application does not need to have a
            ///     section of this type.
            /// </summary>
            SHT_MIPS_DEBUG = 0x70000005,
            /// <summary>The section contains information regarding register usage information for the object file.</summary>
            SHT_MIPS_REGINFO = 0x70000006
        }

        enum shTypePaRisc : uint
        {
            /// <summary>Section contains product-specific extension bits</summary>
            SHT_PARISC_EXT = 0x70000000,
            /// <summary>Section contains unwind table entries</summary>
            SHT_PARISC_UNWIND = 0x70000001,
            /// <summary>Section contains debug information for optimized code</summary>
            SHT_PARISC_DOC = 0x70000002,
            /// <summary>Section contains code annotations</summary>
            SHT_PARISC_ANNOT = 0x70000003
        }

        [Flags]
        enum shFlags : uint
        {
            /// <summary>The section contains data that should be writable during process execution.</summary>
            SHF_WRITE = 0x1,
            /// <summary>
            ///     The section occupies memory during process execution. Some control sections do not reside in the memory image
            ///     of an object file; this attribute is off for those sections.
            /// </summary>
            SHF_ALLOC = 0x2,
            /// <summary>The section contains executable machine instructions.</summary>
            SHF_EXECINSTR = 0x4,
            /// <summary>
            ///     The data in the section may be merged to eliminate duplication. Unless the SHF_STRINGS flag is also set, the
            ///     data elements in the section are of a uniform size. The size of each element is specified in the section header's
            ///     sh_entsize field. If the SHF_STRINGS flag is also set, the data elements consist of null-terminated character
            ///     strings. The size of each character is specified in the section header's sh_entsize field. Each element in the
            ///     section is compared against other elements in sections with the same name, type and flags. Elements that would have
            ///     identical values at program run-time may be merged. Relocations referencing elements of such sections must be
            ///     resolved to the merged locations of the referenced values. Note that any relocatable values, including values that
            ///     would result in run-time relocations, must be analyzed to determine whether the run-time values would actually be
            ///     identical. An ABI-conforming object file may not depend on specific elements being merged, and an ABI-conforming
            ///     link editor may choose not to merge specific elements.
            /// </summary>
            SHF_MERGE = 0x10,
            /// <summary>
            ///     The data elements in the section consist of null-terminated character strings. The size of each character is
            ///     specified in the section header's sh_entsize field.
            /// </summary>
            SHF_STRINGS = 0x20,
            /// <summary>The sh_info field of this section header holds a section header table index.</summary>
            SHF_INFO_LINK = 0x40,
            /// <summary>
            ///     This flag adds special ordering requirements for link editors. The requirements apply if the sh_link field of
            ///     this section's header references another section (the linked-to section). If this section is combined with other
            ///     sections in the output file, it must appear in the same relative order with respect to those sections, as the
            ///     linked-to section appears with respect to sections the linked-to section is combined with.
            /// </summary>
            SHF_LINK_ORDER = 0x80,
            /// <summary>
            ///     This section requires special OS-specific processing (beyond the standard linking rules) to avoid incorrect
            ///     behavior. If this section has either an sh_type value or contains sh_flags bits in the OS-specific ranges for those
            ///     fields, and a link editor processing this section does not recognize those values, then the link editor should
            ///     reject the object file containing this section with an error.
            /// </summary>
            SHF_OS_NONCONFORMING = 0x100,
            /// <summary>
            ///     This section is a member (perhaps the only one) of a section group. The section must be referenced by a
            ///     section of type SHT_GROUP. The SHF_GROUP flag may be set only for sections contained in relocatable objects
            ///     (objects with the ELF header e_type member set to ET_REL). See below for further details.
            /// </summary>
            SHF_GROUP = 0x200,
            /// <summary>
            ///     This section holds Thread-Local Storage, meaning that each separate execution flow has its own distinct
            ///     instance of this data. Implementations need not support this flag.
            /// </summary>
            SHF_TLS = 0x400,
            /// <summary>
            ///     This flag identifies a section containing compressed data. SHF_COMPRESSED applies only to non-allocable
            ///     sections, and cannot be used in conjunction with SHF_ALLOC. In addition, SHF_COMPRESSED cannot be applied to
            ///     sections of type SHT_NOBITS. All relocations to a compressed section specifiy offsets to the uncompressed section
            ///     data. It is therefore necessary to decompress the section data before relocations can be applied. Each compressed
            ///     section specifies the algorithm independently. It is permissible for different sections in a given ELF object to
            ///     employ different compression algorithms. Compressed sections begin with a compression header structure that
            ///     identifies the compression algorithm.
            /// </summary>
            SHF_COMPRESSED = 0x800,
            /// <summary>All bits included in this mask are reserved for operating system-specific semantics</summary>
            SHF_MASKOS = 0x0ff00000,
            /// <summary>All bits included in this mask are reserved for processor-specific semantics</summary>
            SHF_MASKPROC = 0xf0000000
        }

        [Flags]
        enum shFlags64 : ulong
        {
            /// <summary>The section contains data that should be writable during process execution.</summary>
            SHF_WRITE = shFlags.SHF_WRITE,
            /// <summary>
            ///     The section occupies memory during process execution. Some control sections do not reside in the memory image
            ///     of an object file; this attribute is off for those sections.
            /// </summary>
            SHF_ALLOC = shFlags.SHF_ALLOC,
            /// <summary>The section contains executable machine instructions.</summary>
            SHF_EXECINSTR = shFlags.SHF_EXECINSTR,
            /// <summary>
            ///     The data in the section may be merged to eliminate duplication. Unless the SHF_STRINGS flag is also set, the
            ///     data elements in the section are of a uniform size. The size of each element is specified in the section header's
            ///     sh_entsize field. If the SHF_STRINGS flag is also set, the data elements consist of null-terminated character
            ///     strings. The size of each character is specified in the section header's sh_entsize field. Each element in the
            ///     section is compared against other elements in sections with the same name, type and flags. Elements that would have
            ///     identical values at program run-time may be merged. Relocations referencing elements of such sections must be
            ///     resolved to the merged locations of the referenced values. Note that any relocatable values, including values that
            ///     would result in run-time relocations, must be analyzed to determine whether the run-time values would actually be
            ///     identical. An ABI-conforming object file may not depend on specific elements being merged, and an ABI-conforming
            ///     link editor may choose not to merge specific elements.
            /// </summary>
            SHF_MERGE = shFlags.SHF_MERGE,
            /// <summary>
            ///     The data elements in the section consist of null-terminated character strings. The size of each character is
            ///     specified in the section header's sh_entsize field.
            /// </summary>
            SHF_STRINGS = shFlags.SHF_STRINGS,
            /// <summary>The sh_info field of this section header holds a section header table index.</summary>
            SHF_INFO_LINK = shFlags.SHF_INFO_LINK,
            /// <summary>
            ///     This flag adds special ordering requirements for link editors. The requirements apply if the sh_link field of
            ///     this section's header references another section (the linked-to section). If this section is combined with other
            ///     sections in the output file, it must appear in the same relative order with respect to those sections, as the
            ///     linked-to section appears with respect to sections the linked-to section is combined with.
            /// </summary>
            SHF_LINK_ORDER = shFlags.SHF_LINK_ORDER,
            /// <summary>
            ///     This section requires special OS-specific processing (beyond the standard linking rules) to avoid incorrect
            ///     behavior. If this section has either an sh_type value or contains sh_flags bits in the OS-specific ranges for those
            ///     fields, and a link editor processing this section does not recognize those values, then the link editor should
            ///     reject the object file containing this section with an error.
            /// </summary>
            SHF_OS_NONCONFORMING = shFlags.SHF_OS_NONCONFORMING,
            /// <summary>
            ///     This section is a member (perhaps the only one) of a section group. The section must be referenced by a
            ///     section of type SHT_GROUP. The SHF_GROUP flag may be set only for sections contained in relocatable objects
            ///     (objects with the ELF header e_type member set to ET_REL). See below for further details.
            /// </summary>
            SHF_GROUP = shFlags.SHF_GROUP,
            /// <summary>
            ///     This section holds Thread-Local Storage, meaning that each separate execution flow has its own distinct
            ///     instance of this data. Implementations need not support this flag.
            /// </summary>
            SHF_TLS = shFlags.SHF_TLS,
            /// <summary>
            ///     This flag identifies a section containing compressed data. SHF_COMPRESSED applies only to non-allocable
            ///     sections, and cannot be used in conjunction with SHF_ALLOC. In addition, SHF_COMPRESSED cannot be applied to
            ///     sections of type SHT_NOBITS. All relocations to a compressed section specifiy offsets to the uncompressed section
            ///     data. It is therefore necessary to decompress the section data before relocations can be applied. Each compressed
            ///     section specifies the algorithm independently. It is permissible for different sections in a given ELF object to
            ///     employ different compression algorithms. Compressed sections begin with a compression header structure that
            ///     identifies the compression algorithm.
            /// </summary>
            SHF_COMPRESSED = shFlags.SHF_COMPRESSED,
            SHF_MASKOS   = shFlags.SHF_MASKOS,
            SHF_MASKPROC = shFlags.SHF_MASKPROC
        }

        [Flags]
        enum shFlagsAmd64 : uint
        {
            /// <summary>
            ///     If an object file section does not have this flag set, then it may not hold more than 2GB and can be freely
            ///     referred to in objects using smaller code models. Otherwise, only objects using larger code models can refer to
            ///     them. For example, a medium code model object can refer to data in a section that sets this flag besides being able
            ///     to refer to data in a section that does not set it; likewise, a small code model object can refer only to code in a
            ///     section that does not set this flag
            /// </summary>
            SHF_X86_64_LARGE = 0x10000000
        }

        [Flags]
        enum shFlagsArm : uint
        {
            /// <summary>The content of this section should not be read by program executor</summary>
            SHF_ARM_NOREAD = 0x20000000
        }

        [Flags]
        enum shFlagsMips : uint
        {
            /// <summary>
            ///     The section contains data that must be part of the global data area during program execution. Data in this
            ///     area is addressable with a gp relative address. Any section with the SHF_MIPS_GPREL attribute must have a section
            ///     header index of one of the .gptab special sections in the sh_link member of its section header table entry.
            /// </summary>
            SHF_MIPS_GPREL = 0x10000000
        }

        [Flags]
        enum shFlagsParisc : uint
        {
            /// <summary>Section contains code compiled for static branch prediction</summary>
            SHF_PARISC_SBP = 0x80000000,
            /// <summary>Section should be allocated far from gp</summary>
            SHF_PARISC_HUGE = 0x40000000,
            /// <summary>Section should be near gp</summary>
            SHF_PARISC_SHORT = 0x20000000
        }

        [Flags]
        enum chType : uint
        {
            /// <summary>
            ///     The section data is compressed with the ZLIB algoritm. The compressed ZLIB data bytes begin with the byte
            ///     immediately following the compression header, and extend to the end of the section.
            /// </summary>
            ELFCOMPRESS_ZLIB = 1,
            /// <summary>Values from this to <see cref="ELFCOMPRESS_HIOS" /> are reserved for operating system-specific semantics</summary>
            ELFCOMPRESS_LOOS = 0x60000000,
            /// <summary>Values from <see cref="ELFCOMPRESS_LOOS" /> to this are reserved for operating system-specific semantics</summary>
            ELFCOMPRESS_HIOS = 0x6fffffff,
            /// <summary>Values from this to <see cref="ELFCOMPRESS_HIPROC" /> are reserved for processor-specific semantics</summary>
            ELFCOMPRESS_LOPROC = 0x70000000,
            /// <summary>Values from <see cref="ELFCOMPRESS_LOPROC" /> to this are reserved for processor-specific semantics</summary>
            ELFCOMPRESS_HIPROC = 0x7fffffff
        }

        enum eFlagsArm : uint
        {
            /// <summary>This masks an 8-bit version number, the version of the ABI to which this ELF file conforms.</summary>
            EF_ARM_ABIMASK = 0xFF000000,
            /// <summary>The ELF file contains BE-8 code, suitable for execution on an ARM Architecture v6 processor.</summary>
            EF_ARM_BE8 = 0x00800000,
            /// <summary>Legacy code (ABI version 4 and earlier) generated by gcc-arm-xxx might use these bits.</summary>
            EF_ARM_GCCMASK = 0x00400FFF,
            /// <summary>
            ///     Set in executable file headers (e_type = ET_EXEC or ET_DYN) to note that the executable file was built to
            ///     conform to the hardware floating-point procedure-call standard.
            /// </summary>
            EF_ARM_ABI_FLOAT_HARD = 0x00000400,
            /// <summary>
            ///     Set in executable file headers (e_type = ET_EXEC or ET_DYN) to note that the executable file was built to
            ///     conform to the software floating-point procedure-call standard.
            /// </summary>
            EF_ARM_ABI_FLOAT_SOFT = 0x00000200
        }

        [Flags]
        enum eFlagsMips : uint
        {
            /// <summary>
            ///     This bit is asserted when at least one .noreorder directive in an assembly language source contributes to the
            ///     object module.
            /// </summary>
            EF_MIPS_NOREORDER = 0x00000001,
            /// <summary>This bit is asserted when the file contains position-independent code that can be relocated in memory.</summary>
            EF_MIPS_PIC = 0x00000002,
            /// <summary>
            ///     This bit is asserted when the file contains code that follows standard calling sequence rules for calling
            ///     position-independent code. The code in this file is not necessarily position independent. The
            ///     <see cref="EF_MIPS_PIC" /> and <see cref="EF_MIPS_CPIC" /> flags must be mutually exclusive.
            /// </summary>
            EF_MIPS_CPIC = 0x00000004,
            /// <summary>Extension to the basic MIPS I architecture.</summary>
            EF_MIPS_ARCH_EXT1 = 0x10000000,
            /// <summary>Extension to the basic MIPS I architecture.</summary>
            EF_MIPS_ARCH_EXT2 = 0x20000000,
            /// <summary>Extension to the basic MIPS I architecture.</summary>
            EF_MIPS_ARCH_EXT3 = 0x40000000,
            /// <summary>Extension to the basic MIPS I architecture.</summary>
            EF_MIPS_ARCH_EXT4 = 0x80000000
        }

        [Flags]
        enum eFlagsPaRisc : uint
        {
            /// <summary>Trap nil pointer dereferences</summary>
            EF_PARISC_TRAPNIL = 0x00010000,
            /// <summary>Program uses arch. extensions</summary>
            EF_PARISC_EXT = 0x00020000,
            /// <summary>Program expects little-endian mode</summary>
            EF_PARISC_LSB = 0x00040000,
            /// <summary>Program expects wide mode</summary>
            EF_PARISC_WIDE = 0x00080000,
            /// <summary>Do not allow kernel-assisted branch prediction</summary>
            EF_PARISC_NO_KABP = 0x00100000,
            /// <summary>Allow lazy swap for dynamically-allocated program segments</summary>
            EF_PARISC_LAZYSWAP = 0x00400000
        }

        enum eFlagsPaRiscArchitecture : uint
        {
            /// <summary>PA-RISC 1.0</summary>
            EFA_PARISC_1_0 = 0x020B,
            /// <summary>PA-RISC 1.1</summary>
            EFA_PARISC_1_1 = 0x0210,
            /// <summary>PA-RISC 2.0</summary>
            EFA_PARISC_2_0 = 0x0214
        }

        enum GnuAbiSystem : uint
        {
            Linux = 0,
            Hurd = 1,
            Solaris = 2,
            kFreeBSD = 3,
            kNetBSD = 4
        }
    }
}