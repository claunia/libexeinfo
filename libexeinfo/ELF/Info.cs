using System.Text;

namespace libexeinfo
{
    public partial class ELF : IExecutable
    {
        public string Information
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                string processorFlags;

                if(Header.e_flags == 0) processorFlags = "None";
                else
                    switch(Header.e_machine)
                    {
                        case eMachine.EM_ARM:
                        case eMachine.EM_AARCH64:
                            processorFlags =
                                $"{(eFlagsArm)(Header.e_flags & eFlagsArmMask)} ABI version {Header.e_flags >> 24}";
                            break;
                        case eMachine.EM_MIPS:
                        case eMachine.EM_MIPS_RS3_LE:
                        case eMachine.EM_MIPS_X:
                            processorFlags = $"{(eFlagsMips)Header.e_flags}";
                            break;
                        case eMachine.EM_PARISC:
                            processorFlags =
                                $"{(eFlagsArm)(Header.e_flags & eFlagsPariscMask)} architecture version {(eFlagsPaRiscArchitecture)(Header.e_flags & EF_PARISC_ARCH)}";
                            break;
                        default:
                            processorFlags = $"{Header.e_flags}";
                            break;
                    }

                sb.AppendLine("Executable and Linkable Format (ELF):");
                sb.AppendFormat("\tObject class: {0}", eiClassToString(Header.ei_class)).AppendLine();
                sb.AppendFormat("\tObject endian: {0}", eiDataToString(Header.ei_data)).AppendLine();
                sb.AppendFormat("\tObject OS ABI: {0}", eiOsAbiToString(Header.ei_osabi)).AppendLine();
                sb.AppendFormat("\tObject type: {0}", eTypeToString(Header.e_type)).AppendLine();
                sb.AppendFormat("\tArchitecture: {0}", eMachineToString(Header.e_machine)).AppendLine();
                sb.AppendFormat("\tObject file version: {0}", Header.e_version).AppendLine();
                sb.AppendFormat("\tEntry point virtual address: {0}",
                                Header.ei_class == eiClass.ELFCLASS64
                                    ? $"0x{Header.e_entry:X16}"
                                    : $"0x{Header.e_entry:X8}").AppendLine();
                sb.AppendFormat("\tProgram header starts at {0}, contains {1} entries of {2} bytes", Header.e_phoff,
                                Header.e_phnum, Header.e_phentsize).AppendLine();
                sb.AppendFormat("\tSection header starts at {0}, contains {1} entries of {2} bytes", Header.e_shoff,
                                Header.e_shnum, Header.e_shentsize).AppendLine();
                sb.AppendFormat("\tProcessor specific flags: {0}", processorFlags).AppendLine();
                sb.AppendFormat("\tHeader is {0} bytes long", Header.e_ehsize).AppendLine();
                sb.AppendFormat("\tString table is at index {0} of section header", Header.e_shstrndx).AppendLine();

                if(!string.IsNullOrEmpty(interpreter)) sb.AppendFormat("\tInterpreter: {0}", interpreter).AppendLine();

                for(int i = 1; i < sections.Length; i++)
                {
                    sb.AppendFormat("\tSection {0}:", i).AppendLine();
                    sb.AppendFormat("\t\tName: {0}", sectionNames[i]).AppendLine();
                    sb.AppendFormat("\t\tType: {0}", sections[i].sh_type).AppendLine();
                    sb.AppendFormat("\t\tFlags: {0}", sections[i].sh_flags).AppendLine();
                    sb.AppendFormat("\t\tVirtual address: {0}",
                                    Header.ei_class == eiClass.ELFCLASS64
                                        ? $"0x{sections[i].sh_addr:X16}"
                                        : $"0x{sections[i].sh_addr:X8}").AppendLine();
                    sb.AppendFormat("\t\tSection starts at {0} and is {1} bytes long", sections[i].sh_offset,
                                    sections[i].sh_size).AppendLine();
                    if(sections[i].sh_link > 0)
                        sb.AppendFormat("\t\tIndex to name: {0}", sections[i].sh_link).AppendLine();
                    sb.AppendFormat("\t\tAdditional information: {0}", sections[i].sh_info).AppendLine();
                    sb.AppendFormat("\t\tSection is aligned to {0} bytes", sections[i].sh_addralign).AppendLine();
                    if(sections[i].sh_entsize > 0)
                        sb.AppendFormat("\t\tIndex to name: {0}", sections[i].sh_entsize).AppendLine();
                }

                return sb.ToString();
            }
        }

        static string eiClassToString(eiClass eiclass)
        {
            switch(eiclass)
            {
                case eiClass.ELFCLASSNONE: return "None";
                case eiClass.ELFCLASS32:   return "32-bit";
                case eiClass.ELFCLASS64:   return "64-bit";
                default:                   return $"{eiclass}";
            }
        }

        static string eiDataToString(eiData eidata)
        {
            switch(eidata)
            {
                case eiData.ELFDATANONE: return "None";
                case eiData.ELFDATA2LSB: return "Little-endian";
                case eiData.ELFDATA2MSB: return "Big-endian";
                default:                 return $"{eidata}";
            }
        }

        static string eiOsAbiToString(eiOsabi eiosabi)
        {
            switch(eiosabi)
            {
                case eiOsabi.ELFOSABI_NONE:      return "None";
                case eiOsabi.ELFOSABI_HPUX:      return "HP-UX";
                case eiOsabi.ELFOSABI_NETBSD:    return "NetBSD";
                case eiOsabi.ELFOSABI_GNU:       return "GNU/Linux";
                case eiOsabi.ELFOSABI_SOLARIS:   return "Solaris";
                case eiOsabi.ELFOSABI_AIX:       return "AIX";
                case eiOsabi.ELFOSABI_IRIX:      return "IRIX";
                case eiOsabi.ELFOSABI_FREEBSD:   return "FreeBSD";
                case eiOsabi.ELFOSABI_TRU64:     return "Tru64 UNIX";
                case eiOsabi.ELFOSABI_MODESTO:   return "Modesto";
                case eiOsabi.ELFOSABI_OPENBSD:   return "OpenBSD";
                case eiOsabi.ELFOSABI_OPENVMS:   return "OpenVMS";
                case eiOsabi.ELFOSABI_NSK:       return "HP Non-Stop Kernel";
                case eiOsabi.ELFOSABI_AROS:      return "AROS";
                case eiOsabi.ELFOSABI_FENIXOS:   return "The FenixOS";
                case eiOsabi.ELFOSABI_CLOUDABI:  return "Nuxi CloudABI";
                case eiOsabi.ELFOSABI_OPENVOS:   return "OpenVOS";
                case eiOsabi.ELFOSABI_ARM_AEABI: return "ARM EABI";
                default:                         return $"{eiosabi}";
            }
        }

        static string eTypeToString(eType etype)
        {
            switch(etype)
            {
                case eType.ET_NONE: return "None";
                case eType.ET_REL:  return "Relocatable file";
                case eType.ET_EXEC: return "Executable file";
                case eType.ET_DYN:  return "Shared object file";
                case eType.ET_CORE: return "Core file";
                default:            return $"{etype}";
            }
        }

        static string eMachineToString(eMachine emachine)
        {
            switch(emachine)
            {
                case eMachine.EM_NONE: // 0,
                    return "No machine";
                case eMachine.EM_M32: // 1,
                    return "AT&T WE 32100";
                case eMachine.EM_SPARC: // 2,
                    return "SPARC";
                case eMachine.EM_386: // 3,
                    return "Intel 80386";
                case eMachine.EM_68K: // 4,
                    return "Motorola 68000";
                case eMachine.EM_88K: // 5,
                    return "Motorola 88000";
                case eMachine.EM_IAMCU: // 6,
                    return "Intel MCU";
                case eMachine.EM_860: // 7,
                    return "Intel 80860";
                case eMachine.EM_MIPS: // 8,
                    return "MIPS I Architecture";
                case eMachine.EM_S370: // 9,
                    return "IBM System/370 Processor";
                case eMachine.EM_MIPS_RS3_LE: // 10,
                    return "MIPS RS3000 Little-endian";
                case eMachine.EM_PARISC: // 15,
                    return "Hewlett-Packard PA-RISC";
                case eMachine.EM_VPP500: // 17,
                    return "Fujitsu VPP500";
                case eMachine.EM_SPARC32PLUS: // 18,
                    return "Enhanced instruction set SPARC";
                case eMachine.EM_960: // 19,
                    return "Intel 80960";
                case eMachine.EM_PPC: // 20,
                    return "PowerPC";
                case eMachine.EM_PPC64: // 21,
                    return "64-bit PowerPC";
                case eMachine.EM_S390: // 22,
                    return "IBM System/390 Processor";
                case eMachine.EM_SPU: // 23,
                    return "IBM SPU/SPC";
                case eMachine.EM_V800: // 36,
                    return "NEC V800";
                case eMachine.EM_FR20: // 37,
                    return "Fujitsu FR20";
                case eMachine.EM_RH32: // 38,
                    return "TRW RH-32";
                case eMachine.EM_RCE: // 39,
                    return "Motorola RCE";
                case eMachine.EM_ARM: // 40,
                    return "ARM 32-bit architecture (AARCH32)";
                case eMachine.EM_ALPHA: // 41,
                    return "Digital Alpha";
                case eMachine.EM_SH: // 42,
                    return "Hitachi SH";
                case eMachine.EM_SPARCV9: // 43,
                    return "SPARC Version 9";
                case eMachine.EM_TRICORE: // 44,
                    return "Siemens TriCore embedded processor";
                case eMachine.EM_ARC: // 45,
                    return "Argonaut RISC Core, Argonaut Technologies Inc.";
                case eMachine.EM_H8_300: // 46,
                    return "Hitachi H8/300";
                case eMachine.EM_H8_300H: // 47,
                    return "Hitachi H8/300H";
                case eMachine.EM_H8S: // 48,
                    return "Hitachi H8S";
                case eMachine.EM_H8_500: // 49,
                    return "Hitachi H8/500";
                case eMachine.EM_IA_64: // 50,
                    return "Intel IA-64 processor architecture";
                case eMachine.EM_MIPS_X: // 51,
                    return "Stanford MIPS-X";
                case eMachine.EM_COLDFIRE: // 52,
                    return "Motorola ColdFire";
                case eMachine.EM_68HC12: // 53,
                    return "Motorola M68HC12";
                case eMachine.EM_MMA: // 54,
                    return "Fujitsu MMA Multimedia Accelerator";
                case eMachine.EM_PCP: // 55,
                    return "Siemens PCP";
                case eMachine.EM_NCPU: // 56,
                    return "Sony nCPU embedded RISC processor";
                case eMachine.EM_NDR1: // 57,
                    return "Denso NDR1 microprocessor";
                case eMachine.EM_STARCORE: // 58,
                    return "Motorola Star*Core processor";
                case eMachine.EM_ME16: // 59,
                    return "Toyota ME16 processor";
                case eMachine.EM_ST100: // 60,
                    return "STMicroelectronics ST100 processor";
                case eMachine.EM_TINYJ: // 61,
                    return "Advanced Logic Corp. TinyJ embedded processor family";
                case eMachine.EM_X86_64: // 62,
                    return "AMD x86-64 architecture";
                case eMachine.EM_PDSP: // 63,
                    return "Sony DSP Processor";
                case eMachine.EM_PDP10: // 64,
                    return "Digital Equipment Corp. PDP-10";
                case eMachine.EM_PDP11: // 65,
                    return "Digital Equipment Corp. PDP-11";
                case eMachine.EM_FX66: // 66,
                    return "Siemens FX66 microcontroller";
                case eMachine.EM_ST9PLUS: // 67,
                    return "STMicroelectronics ST9+ 8/16 bit microcontroller";
                case eMachine.EM_ST7: // 68,
                    return "STMicroelectronics ST7 8-bit microcontroller";
                case eMachine.EM_68HC16: // 69,
                    return "Motorola MC68HC16 Microcontroller";
                case eMachine.EM_68HC11: // 70,
                    return "Motorola MC68HC11 Microcontroller";
                case eMachine.EM_68HC08: // 71,
                    return "Motorola MC68HC08 Microcontroller";
                case eMachine.EM_68HC05: // 72,
                    return "Motorola MC68HC05 Microcontroller";
                case eMachine.EM_SVX: // 73,
                    return "Silicon Graphics SVx";
                case eMachine.EM_ST19: // 74,
                    return "STMicroelectronics ST19 8-bit microcontroller";
                case eMachine.EM_VAX: // 75,
                    return "Digital VAX";
                case eMachine.EM_CRIS: // 76,
                    return "Axis Communications 32-bit embedded processor";
                case eMachine.EM_JAVELIN: // 77,
                    return "Infineon Technologies 32-bit embedded processor";
                case eMachine.EM_FIREPATH: // 78,
                    return "Element 14 64-bit DSP Processor";
                case eMachine.EM_ZSP: // 79,
                    return "LSI Logic 16-bit DSP Processor";
                case eMachine.EM_MMIX: // 80,
                    return "Donald Knuth's educational 64-bit processor";
                case eMachine.EM_HUANY: // 81,
                    return "Harvard University machine-independent object files";
                case eMachine.EM_PRISM: // 82,
                    return "SiTera Prism";
                case eMachine.EM_AVR: // 83,
                    return "Atmel AVR 8-bit microcontroller";
                case eMachine.EM_FR30: // 84,
                    return "Fujitsu FR30";
                case eMachine.EM_D10V: // 85,
                    return "Mitsubishi D10V";
                case eMachine.EM_D30V: // 86,
                    return "Mitsubishi D30V";
                case eMachine.EM_V850: // 87,
                    return "NEC v850";
                case eMachine.EM_M32R: // 88,
                    return "Mitsubishi M32R";
                case eMachine.EM_MN10300: // 89,
                    return "Matsushita MN10300";
                case eMachine.EM_MN10200: // 90,
                    return "Matsushita MN10200";
                case eMachine.EM_PJ: // 91,
                    return "picoJava";
                case eMachine.EM_OPENRISC: // 92,
                    return "OpenRISC 32-bit embedded processor";
                case eMachine.EM_ARC_COMPACT: // 93,
                    return "ARC International ARCompact processor";
                case eMachine.EM_XTENSA: // 94,
                    return "Tensilica Xtensa Architecture";
                case eMachine.EM_VIDEOCORE: // 95,
                    return "Alphamosaic VideoCore processor";
                case eMachine.EM_TMM_GPP: // 96,
                    return "Thompson Multimedia General Purpose Processor";
                case eMachine.EM_NS32K: // 97,
                    return "National Semiconductor 32000 series";
                case eMachine.EM_TPC: // 98,
                    return "Tenor Network TPC processor";
                case eMachine.EM_SNP1K: // 99,
                    return "Trebia SNP 1000 processor";
                case eMachine.EM_ST200: // 100,
                    return "STMicroelectronics ST200 microcontroller";
                case eMachine.EM_IP2K: // 101,
                    return "Ubicom IP2xxx microcontroller family";
                case eMachine.EM_MAX: // 102,
                    return "MAX Processor";
                case eMachine.EM_CR: // 103,
                    return "National Semiconductor CompactRISC microprocessor";
                case eMachine.EM_F2MC16: // 104,
                    return "Fujitsu F2MC16";
                case eMachine.EM_MSP430: // 105,
                    return "Texas Instruments embedded microcontroller msp430";
                case eMachine.EM_BLACKFIN: // 106,
                    return "Analog Devices Blackfin (DSP) processor";
                case eMachine.EM_SE_C33: // 107,
                    return "S1C33 Family of Seiko Epson processors";
                case eMachine.EM_SEP: // 108,
                    return "Sharp embedded microprocessor";
                case eMachine.EM_ARCA: // 109,
                    return "Arca RISC Microprocessor";
                case eMachine.EM_UNICORE: // 110,
                    return "Microprocessor series from PKU-Unity Ltd. and MPRC of Peking University";
                case eMachine.EM_EXCESS: // 111,
                    return "eXcess: 16/32/64-bit configurable embedded CPU";
                case eMachine.EM_DXP: // 112,
                    return "Icera Semiconductor Inc. Deep Execution Processor";
                case eMachine.EM_ALTERA_NIOS2: // 113,
                    return "Altera Nios II soft-core processor";
                case eMachine.EM_CRX: // 114,
                    return "National Semiconductor CompactRISC CRX microprocessor";
                case eMachine.EM_XGATE: // 115,
                    return "Motorola XGATE embedded processor";
                case eMachine.EM_C166: // 116,
                    return "Infineon C16x/XC16x processor";
                case eMachine.EM_M16C: // 117,
                    return "Renesas M16C series microprocessors";
                case eMachine.EM_DSPIC30F: // 118,
                    return "Microchip Technology dsPIC30F Digital Signal Controller";
                case eMachine.EM_CE: // 119,
                    return "Freescale Communication Engine RISC core";
                case eMachine.EM_M32C: // 120,
                    return "Renesas M32C series microprocessors";
                case eMachine.EM_TSK3000: // 131,
                    return "Altium TSK3000 core";
                case eMachine.EM_RS08: // 132,
                    return "Freescale RS08 embedded processor";
                case eMachine.EM_SHARC: // 133,
                    return "Analog Devices SHARC family of 32-bit DSP processors";
                case eMachine.EM_ECOG2: // 134,
                    return "Cyan Technology eCOG2 microprocessor";
                case eMachine.EM_SCORE7: // 135,
                    return "Sunplus S+core7 RISC processor";
                case eMachine.EM_DSP24: // 136,
                    return "New Japan Radio (NJR) 24-bit DSP Processor";
                case eMachine.EM_VIDEOCORE3: // 137,
                    return "Broadcom VideoCore III processor";
                case eMachine.EM_LATTICEMICO32: // 138,
                    return "RISC processor for Lattice FPGA architecture";
                case eMachine.EM_SE_C17: // 139,
                    return "Seiko Epson C17 family";
                case eMachine.EM_TI_C6000: // 140,
                    return "The Texas Instruments TMS320C6000 DSP family";
                case eMachine.EM_TI_C2000: // 141,
                    return "The Texas Instruments TMS320C2000 DSP family";
                case eMachine.EM_TI_C5500: // 142,
                    return "The Texas Instruments TMS320C55x DSP family";
                case eMachine.EM_TI_ARP32: // 143,
                    return "Texas Instruments Application Specific RISC Processor, 32bit fetch";
                case eMachine.EM_TI_PRU: // 144,
                    return "Texas Instruments Programmable Realtime Unit";
                case eMachine.EM_MMDSP_PLUS: // 160,
                    return "STMicroelectronics 64bit VLIW Data Signal Processor";
                case eMachine.EM_CYPRESS_M8C: // 161,
                    return "Cypress M8C microprocessor";
                case eMachine.EM_R32C: // 162,
                    return "Renesas R32C series microprocessors";
                case eMachine.EM_TRIMEDIA: // 163,
                    return "NXP Semiconductors TriMedia architecture family";
                case eMachine.EM_QDSP6: // 164,
                    return "QUALCOMM DSP6 Processor";
                case eMachine.EM_8051: // 165,
                    return "Intel 8051 and variants";
                case eMachine.EM_STXP7X: // 166,
                    return "STMicroelectronics STxP7x family of configurable and extensible RISC processors";
                case eMachine.EM_NDS32: // 167,
                    return "Andes Technology compact code size embedded RISC processor family";
                case eMachine.EM_ECOG1: // 168,
                    return "Cyan Technology eCOG1X family";
                case eMachine.EM_MAXQ30: // 169,
                    return "Dallas Semiconductor MAXQ30 Core Micro-controllers";
                case eMachine.EM_XIMO16: // 170,
                    return "New Japan Radio (NJR) 16-bit DSP Processor";
                case eMachine.EM_MANIK: // 171,
                    return "M2000 Reconfigurable RISC Microprocessor";
                case eMachine.EM_CRAYNV2: // 172,
                    return "Cray Inc. NV2 vector architecture";
                case eMachine.EM_RX: // 173,
                    return "Renesas RX family";
                case eMachine.EM_METAG: // 174,
                    return "Imagination Technologies META processor architecture";
                case eMachine.EM_MCST_ELBRUS: // 175,
                    return "MCST Elbrus general purpose hardware architecture";
                case eMachine.EM_ECOG16: // 176,
                    return "Cyan Technology eCOG16 family";
                case eMachine.EM_CR16: // 177,
                    return "National Semiconductor CompactRISC CR16 16-bit microprocessor";
                case eMachine.EM_ETPU: // 178,
                    return "Freescale Extended Time Processing Unit";
                case eMachine.EM_SLE9X: // 179,
                    return "Infineon Technologies SLE9X core";
                case eMachine.EM_L10M: // 180,
                    return "Intel L10M";
                case eMachine.EM_K10M: // 181,
                    return "Intel K10M";
                case eMachine.EM_AARCH64: // 183,
                    return "ARM 64-bit architecture (AARCH64)";
                case eMachine.EM_AVR32: // 185,
                    return "Atmel Corporation 32-bit microprocessor family";
                case eMachine.EM_STM8: // 186,
                    return "STMicroeletronics STM8 8-bit microcontroller";
                case eMachine.EM_TILE64: // 187,
                    return "Tilera TILE64 multicore architecture family";
                case eMachine.EM_TILEPRO: // 188,
                    return "Tilera TILEPro multicore architecture family";
                case eMachine.EM_MICROBLAZE: // 189,
                    return "Xilinx MicroBlaze 32-bit RISC soft processor core";
                case eMachine.EM_CUDA: // 190,
                    return "NVIDIA CUDA architecture";
                case eMachine.EM_TILEGX: // 191,
                    return "Tilera TILE-Gx multicore architecture family";
                case eMachine.EM_CLOUDSHIELD: // 192,
                    return "CloudShield architecture family";
                case eMachine.EM_COREA_1ST: // 193,
                    return "KIPO-KAIST Core-A 1st generation processor family";
                case eMachine.EM_COREA_2ND: // 194,
                    return "KIPO-KAIST Core-A 2nd generation processor family";
                case eMachine.EM_ARC_COMPACT2: // 195,
                    return "Synopsys ARCompact V2";
                case eMachine.EM_OPEN8: // 196,
                    return "Open8 8-bit RISC soft processor core";
                case eMachine.EM_RL78: // 197,
                    return "Renesas RL78 family";
                case eMachine.EM_VIDEOCORE5: // 198,
                    return "Broadcom VideoCore V processor";
                case eMachine.EM_78KOR: // 199,
                    return "Renesas 78KOR family";
                case eMachine.EM_56800EX: // 200,
                    return "Freescale 56800EX Digital Signal Controller (DSC)";
                case eMachine.EM_BA1: // 201,
                    return "Beyond BA1 CPU architecture";
                case eMachine.EM_BA2: // 202,
                    return "Beyond BA2 CPU architecture";
                case eMachine.EM_XCORE: // 203,
                    return "XMOS xCORE processor family";
                case eMachine.EM_MCHP_PIC: // 204,
                    return "Microchip 8-bit PIC(r) family";
                case eMachine.EM_INTEL205: // 205,
                    return "Reserved by Intel";
                case eMachine.EM_INTEL206: // 206,
                    return "Reserved by Intel";
                case eMachine.EM_INTEL207: // 207,
                    return "Reserved by Intel";
                case eMachine.EM_INTEL208: // 208,
                    return "Reserved by Intel";
                case eMachine.EM_INTEL209: // 209,
                    return "Reserved by Intel";
                case eMachine.EM_KM32: // 210,
                    return "KM211 KM32 32-bit processor";
                case eMachine.EM_KMX32: // 211,
                    return "KM211 KMX32 32-bit processor";
                case eMachine.EM_KMX16: // 212,
                    return "KM211 KMX16 16-bit processor";
                case eMachine.EM_KMX8: // 213,
                    return "KM211 KMX8 8-bit processor";
                case eMachine.EM_KVARC: // 214,
                    return "KM211 KVARC processor";
                case eMachine.EM_CDP: // 215,
                    return "Paneve CDP architecture family";
                case eMachine.EM_COGE: // 216,
                    return "Cognitive Smart Memory Processor";
                case eMachine.EM_COOL: // 217,
                    return "Bluechip Systems CoolEngine";
                case eMachine.EM_NORC: // 218,
                    return "Nanoradio Optimized RISC";
                case eMachine.EM_CSR_KALIMBA: // 219,
                    return "CSR Kalimba architecture family";
                case eMachine.EM_Z80: // 220,
                    return "Zilog Z80";
                case eMachine.EM_VISIUM: // 221,
                    return "Controls and Data Services VISIUMcore processor";
                case eMachine.EM_FT32: // 222,
                    return "FTDI Chip FT32 high performance 32-bit RISC architecture";
                case eMachine.EM_MOXIE: // 223,
                    return "Moxie processor family";
                case eMachine.EM_AMDGPU: // 224,
                    return "AMD GPU architecture";
                case eMachine.EM_RISCV: // 243,
                    return "RISC-V";
                case eMachine.EM_ALPHA_OLD: // 0x9026,
                    return "Digital Alpha";
                case eMachine.EM_CYGNUS_V850: // 0x9080,
                    return "Bogus old v850 magic number, used by old tools.";
                case eMachine.EM_CYGNUS_M32R: // 0x9041,
                    return "Bogus old m32r magic number, used by old tools.";
                case eMachine.EM_S390_OLD: // 0xA390,
                    return "This is the old interim value for S/390 architecture.";
                case eMachine.EM_FRV: // 0x5441
                    return "Fujitsu FR-V";
                default: return $"{emachine}";
            }
        }
    }
}