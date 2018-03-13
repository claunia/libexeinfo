using System.Runtime.InteropServices;

namespace libexeinfo
{
    public partial class ELF
    {
        [StructLayout(LayoutKind.Sequential)]
        struct Elf32_Ehdr
        {
            /// <summary>
            ///     A file's first 4 bytes hold a ``magic number,'' identifying the file as an ELF object file.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] ei_mag;
            /// <summary>Identifies the file's class, or capacity.</summary>
            public eiClass ei_class;
            /// <summary>
            ///     Specifies the encoding of both the data structures used by object file container and data contained in object
            ///     file sections.
            /// </summary>
            public eiData ei_data;
            /// <summary>
            ///     Specifies the ELF header version number.
            /// </summary>
            public eiVersion ei_version;
            /// <summary>
            ///     Identifies the OS- or ABI-specific ELF extensions used by this file. Some fields in other ELF structures have flags
            ///     and values that have operating system and/or ABI specific meanings; the interpretation of those fields is
            ///     determined by the value of this byte. If the object file does not use any extensions, it is recommended that this
            ///     byte be set to 0. If the value for this byte is 64 through 255, its meaning depends on the value of the e_machine
            ///     header member. The ABI processor supplement for an architecture can define its own associated set of values for
            ///     this byte in this range. If the processor supplement does not specify a set of values, one of the following values
            ///     shall be used, where 0 can also be taken to mean unspecified.
            /// </summary>
            public eiOsabi ei_osabi;
            /// <summary>
            ///     Identifies the version of the ABI to which the object is targeted. This field is used to distinguish among
            ///     incompatible versions of an ABI. The interpretation of this version number is dependent on the ABI identified by
            ///     the EI_OSABI field. If no values are specified for the EI_OSABI field by the processor supplement or no version
            ///     values are specified for the ABI determined by a particular value of the EI_OSABI byte, the value 0 shall be used
            ///     for the EI_ABIVERSION byte; it indicates unspecified.
            /// </summary>
            public byte ei_abiversion;
            /// <summary>
            ///     This value marks the beginning of the unused bytes in e_ident. These bytes are reserved and set to zero; programs
            ///     that read object files should ignore them. The value of EI_PAD will change in the future if currently unused bytes
            ///     are given meanings.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            public byte[] ei_pad;

            /// <summary>
            ///     This member identifies the object file type.
            /// </summary>
            public eType e_type;
            /// <summary>
            ///     This member's value specifies the required architecture for an individual file.
            /// </summary>
            public eMachine e_machine;
            /// <summary>
            ///     This member identifies the object file version.
            /// </summary>
            public eVersion e_version;
            /// <summary>
            ///     This member gives the virtual address to which the system first transfers control, thus starting the process. If
            ///     the file has no associated entry point, this member holds zero.
            /// </summary>
            public uint e_entry;
            /// <summary>
            ///     This member holds the program header table's file offset in bytes. If the file has no program header table, this
            ///     member holds zero.
            /// </summary>
            public uint e_phoff;
            /// <summary>
            ///     This member holds the section header table's file offset in bytes. If the file has no section header table, this
            ///     member holds zero.
            /// </summary>
            public uint e_shoff;
            /// <summary>
            ///     This member holds processor-specific flags associated with the file. Flag names take the form EF_machine_flag.
            /// </summary>
            public uint e_flags;
            /// <summary>
            ///     This member holds the ELF header's size in bytes.
            /// </summary>
            public ushort e_ehsize;
            /// <summary>
            ///     This member holds the size in bytes of one entry in the file's program header table; all entries are the same size.
            /// </summary>
            public ushort e_phentsize;
            /// <summary>
            ///     This member holds the number of entries in the program header table. Thus the product of e_phentsize and e_phnum
            ///     gives the table's size in bytes. If a file has no program header table, e_phnum holds the value zero.
            /// </summary>
            public ushort e_phnum;
            /// <summary>
            ///     This member holds a section header's size in bytes. A section header is one entry in the section header table; all
            ///     entries are the same size.
            /// </summary>
            public ushort e_shentsize;
            /// <summary>
            ///     This member holds the number of entries in the section header table. Thus the product of e_shentsize and e_shnum
            ///     gives the section header table's size in bytes. If a file has no section header table, e_shnum holds the value
            ///     zero.
            ///     If the number of sections is greater than or equal to SHN_LORESERVE (0xff00), this member has the value zero and
            ///     the actual number of section header table entries is contained in the sh_size field of the section header at index
            ///     0. (Otherwise, the sh_size member of the initial entry contains 0.)
            /// </summary>
            public ushort e_shnum;
            /// <summary>
            ///     This member holds the section header table index of the entry associated with the section name string table. If the
            ///     file has no section name string table, this member holds the value SHN_UNDEF.
            ///     If the section name string table section index is greater than or equal to SHN_LORESERVE (0xff00), this member has
            ///     the value SHN_XINDEX (0xffff) and the actual index of the section name string table section is contained in the
            ///     sh_link field of the section header at index 0. (Otherwise, the sh_link member of the initial entry contains 0.)
            /// </summary>
            public ushort e_shstrndx;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct Elf64_Ehdr
        {
            /// <summary>
            ///     A file's first 4 bytes hold a ``magic number,'' identifying the file as an ELF object file.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] ei_mag;
            /// <summary>Identifies the file's class, or capacity.</summary>
            public eiClass ei_class;
            /// <summary>
            ///     Specifies the encoding of both the data structures used by object file container and data contained in object
            ///     file sections.
            /// </summary>
            public eiData ei_data;
            /// <summary>
            ///     Specifies the ELF header version number.
            /// </summary>
            public eiVersion ei_version;
            /// <summary>
            ///     Identifies the OS- or ABI-specific ELF extensions used by this file. Some fields in other ELF structures have flags
            ///     and values that have operating system and/or ABI specific meanings; the interpretation of those fields is
            ///     determined by the value of this byte. If the object file does not use any extensions, it is recommended that this
            ///     byte be set to 0. If the value for this byte is 64 through 255, its meaning depends on the value of the e_machine
            ///     header member. The ABI processor supplement for an architecture can define its own associated set of values for
            ///     this byte in this range. If the processor supplement does not specify a set of values, one of the following values
            ///     shall be used, where 0 can also be taken to mean unspecified.
            /// </summary>
            public eiOsabi ei_osabi;
            /// <summary>
            ///     Identifies the version of the ABI to which the object is targeted. This field is used to distinguish among
            ///     incompatible versions of an ABI. The interpretation of this version number is dependent on the ABI identified by
            ///     the EI_OSABI field. If no values are specified for the EI_OSABI field by the processor supplement or no version
            ///     values are specified for the ABI determined by a particular value of the EI_OSABI byte, the value 0 shall be used
            ///     for the EI_ABIVERSION byte; it indicates unspecified.
            /// </summary>
            public byte ei_abiversion;
            /// <summary>
            ///     This value marks the beginning of the unused bytes in e_ident. These bytes are reserved and set to zero; programs
            ///     that read object files should ignore them. The value of EI_PAD will change in the future if currently unused bytes
            ///     are given meanings.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            public byte[] ei_pad;
            /// <summary>
            ///     This member identifies the object file type.
            /// </summary>
            public eType e_type;
            /// <summary>
            ///     This member's value specifies the required architecture for an individual file.
            /// </summary>
            public eMachine e_machine;
            /// <summary>
            ///     This member identifies the object file version.
            /// </summary>
            public eVersion e_version;
            /// <summary>
            ///     This member gives the virtual address to which the system first transfers control, thus starting the process. If
            ///     the file has no associated entry point, this member holds zero.
            /// </summary>
            public ulong e_entry;
            /// <summary>
            ///     This member holds the program header table's file offset in bytes. If the file has no program header table, this
            ///     member holds zero.
            /// </summary>
            public ulong e_phoff;
            /// <summary>
            ///     This member holds the section header table's file offset in bytes. If the file has no section header table, this
            ///     member holds zero.
            /// </summary>
            public ulong e_shoff;
            /// <summary>
            ///     This member holds processor-specific flags associated with the file. Flag names take the form EF_machine_flag.
            /// </summary>
            public uint e_flags;
            /// <summary>
            ///     This member holds the ELF header's size in bytes.
            /// </summary>
            public ushort e_ehsize;
            /// <summary>
            ///     This member holds the size in bytes of one entry in the file's program header table; all entries are the same size.
            /// </summary>
            public ushort e_phentsize;
            /// <summary>
            ///     This member holds the number of entries in the program header table. Thus the product of e_phentsize and e_phnum
            ///     gives the table's size in bytes. If a file has no program header table, e_phnum holds the value zero.
            /// </summary>
            public ushort e_phnum;
            /// <summary>
            ///     This member holds a section header's size in bytes. A section header is one entry in the section header table; all
            ///     entries are the same size.
            /// </summary>
            public ushort e_shentsize;
            /// <summary>
            ///     This member holds the number of entries in the section header table. Thus the product of e_shentsize and e_shnum
            ///     gives the section header table's size in bytes. If a file has no section header table, e_shnum holds the value
            ///     zero.
            ///     If the number of sections is greater than or equal to SHN_LORESERVE (0xff00), this member has the value zero and
            ///     the actual number of section header table entries is contained in the sh_size field of the section header at index
            ///     0. (Otherwise, the sh_size member of the initial entry contains 0.)
            /// </summary>
            public ushort e_shnum;
            /// <summary>
            ///     This member holds the section header table index of the entry associated with the section name string table. If the
            ///     file has no section name string table, this member holds the value SHN_UNDEF.
            ///     If the section name string table section index is greater than or equal to SHN_LORESERVE (0xff00), this member has
            ///     the value SHN_XINDEX (0xffff) and the actual index of the section name string table section is contained in the
            ///     sh_link field of the section header at index 0. (Otherwise, the sh_link member of the initial entry contains 0.)
            /// </summary>
            public ushort e_shstrndx;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct Elf32_Shdr
        {
            /// <summary>
            ///     This member specifies the name of the section. Its value is an index into the section header string table section,
            ///     giving the location of a null-terminated string.
            /// </summary>
            public uint sh_name;
            /// <summary>
            ///     This member categorizes the section's contents and semantics.
            /// </summary>
            public shType sh_type;
            /// <summary>
            ///     Sections support 1-bit flags that describe miscellaneous attributes.
            /// </summary>
            public shFlags sh_flags;
            /// <summary>
            ///     If the section will appear in the memory image of a process, this member gives the address at which the section's
            ///     first byte should reside. Otherwise, the member contains 0.
            /// </summary>
            public uint sh_addr;
            /// <summary>
            ///     This member's value gives the byte offset from the beginning of the file to the first byte in the section. One
            ///     section type, SHT_NOBITS described below, occupies no space in the file, and its sh_offset member locates the
            ///     conceptual placement in the file.
            /// </summary>
            public uint sh_offset;
            /// <summary>
            ///     This member gives the section's size in bytes. Unless the section type is SHT_NOBITS, the section occupies sh_size
            ///     bytes in the file. A section of type SHT_NOBITS may have a non-zero size, but it occupies no space in the file.
            /// </summary>
            public uint sh_size;
            /// <summary>
            ///     This member holds a section header table index link, whose interpretation depends on the section type.
            /// </summary>
            public uint sh_link;
            /// <summary>
            ///     This member holds extra information, whose interpretation depends on the section type.
            /// </summary>
            public uint sh_info;
            /// <summary>
            ///     Some sections have address alignment constraints. For example, if a section holds a doubleword, the system must
            ///     ensure doubleword alignment for the entire section. The value of sh_addr must be congruent to 0, modulo the value
            ///     of sh_addralign. Currently, only 0 and positive integral powers of two are allowed. Values 0 and 1 mean the section
            ///     has no alignment constraints.
            /// </summary>
            public uint sh_addralign;
            /// <summary>
            ///     Some sections hold a table of fixed-size entries, such as a symbol table. For such a section, this member gives the
            ///     size in bytes of each entry. The member contains 0 if the section does not hold a table of fixed-size entries.
            /// </summary>
            public uint sh_entsize;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct Elf64_Shdr
        {
            /// <summary>
            ///     This member specifies the name of the section. Its value is an index into the section header string table section,
            ///     giving the location of a null-terminated string.
            /// </summary>
            public uint sh_name;
            /// <summary>
            ///     This member categorizes the section's contents and semantics.
            /// </summary>
            public shType sh_type;
            /// <summary>
            ///     Sections support 1-bit flags that describe miscellaneous attributes.
            /// </summary>
            public shFlags64 sh_flags;
            /// <summary>
            ///     If the section will appear in the memory image of a process, this member gives the address at which the section's
            ///     first byte should reside. Otherwise, the member contains 0.
            /// </summary>
            public ulong sh_addr;
            /// <summary>
            ///     This member's value gives the byte offset from the beginning of the file to the first byte in the section. One
            ///     section type, SHT_NOBITS described below, occupies no space in the file, and its sh_offset member locates the
            ///     conceptual placement in the file.
            /// </summary>
            public ulong sh_offset;
            /// <summary>
            ///     This member gives the section's size in bytes. Unless the section type is SHT_NOBITS, the section occupies sh_size
            ///     bytes in the file. A section of type SHT_NOBITS may have a non-zero size, but it occupies no space in the file.
            /// </summary>
            public ulong sh_size;
            /// <summary>
            ///     This member holds a section header table index link, whose interpretation depends on the section type.
            /// </summary>
            public uint sh_link;
            /// <summary>
            ///     This member holds extra information, whose interpretation depends on the section type.
            /// </summary>
            public uint sh_info;
            /// <summary>
            ///     Some sections have address alignment constraints. For example, if a section holds a doubleword, the system must
            ///     ensure doubleword alignment for the entire section. The value of sh_addr must be congruent to 0, modulo the value
            ///     of sh_addralign. Currently, only 0 and positive integral powers of two are allowed. Values 0 and 1 mean the section
            ///     has no alignment constraints.
            /// </summary>
            public ulong sh_addralign;
            /// <summary>
            ///     Some sections hold a table of fixed-size entries, such as a symbol table. For such a section, this member gives the
            ///     size in bytes of each entry. The member contains 0 if the section does not hold a table of fixed-size entries.
            /// </summary>
            public ulong sh_entsize;
        }

        struct Elf32_Chdr
        {
            /// <summary>
            ///     This member specifies the compression algorithm.
            /// </summary>
            public uint ch_type;
            /// <summary>
            ///     This member provides the size in bytes of the uncompressed data.
            /// </summary>
            public uint ch_size;
            /// <summary>
            ///     Specifies the required alignment for the uncompressed data.
            /// </summary>
            public uint ch_addralign;
        }

        struct Elf64_Chdr
        {
            /// <summary>
            ///     This member specifies the compression algorithm.
            /// </summary>
            public chType ch_type;
            public uint ch_reserved;
            /// <summary>
            ///     This member provides the size in bytes of the uncompressed data.
            /// </summary>
            public ulong ch_size;
            /// <summary>
            ///     Specifies the required alignment for the uncompressed data.
            /// </summary>
            public ulong ch_addralign;
        }
    }
}