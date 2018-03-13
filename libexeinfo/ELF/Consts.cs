namespace libexeinfo
{
    public partial class ELF
    {
        const int EI_NIDENT = 16;

        const uint eFlagsPariscMask = 0xFFFF0000;
        /// <summary>Architecture version</summary>
        const uint EF_PARISC_ARCH = 0x0000FFFF;
        /// <summary>PA-RISC 1.0</summary>
        const uint EFA_PARISC_1_0 = 0x020B;
        /// <summary>PA-RISC 1.1</summary>
        const uint EFA_PARISC_1_1 = 0x0210;
        /// <summary>PA-RISC 2.0</summary>
        const uint EFA_PARISC_2_0 = 0x0214;

        static readonly byte[] ELFMAG = {0x7F, 0x45, 0x4C, 0x46};

        static readonly string[] SectionNames =
        {
            // This section holds uninitialized data that contribute to the program's memory image. By definition, the system initializes the data with zeros when the program begins to run. The section occupies no file space, as indicated by the section type, SHT_NOBITS.
            ".bss",
            // This section holds uninitialized data that contribute to the program's memory image. By definition, the system initializes the data with zeros when the program begins to run. The section occupies no file space, as indicated by the section type, SHT_NOBITS.
            ".lbss",
            // This section holds version control information.
            ".comment",
            // These sections hold initialized data that contribute to the program's memory image.
            ".data",
            // These sections hold initialized data that contribute to the program's memory image.
            ".ldata",
            // These sections hold initialized data that contribute to the program's memory image.
            ".data1",
            // These sections hold initialized data that contribute to the program's memory image.
            ".ldata1",
            // This section holds information for symbolic debugging. The contents are unspecified. All section names with the prefix .debug are reserved for future use in the ABI.
            ".debug",
            // This section holds dynamic linking information. The section's attributes will include the SHF_ALLOC bit. Whether the SHF_WRITE bit is set is processor specific. See Chapter 5 for more information.
            ".dynamic",
            // This section holds strings needed for dynamic linking, most commonly the strings that represent the names associated with symbol table entries. See Chapter 5 for more information.
            ".dynstr",
            // This section holds the dynamic linking symbol table, as described in ``Symbol Table''. See Chapter 5 for more information.
            ".dynsym",
            // This section holds the unwind function table.
            ".eh_frame",
            // This section holds executable instructions that contribute to the process termination code. That is, when a program exits normally, the system arranges to execute the code in this section.
            ".fini",
            // This section holds an array of function pointers that contributes to a single termination array for the executable or shared object containing the section.
            ".fini_array",
            // This section holds the global offset table. See ``Coding Examples'' in Chapter 3, ``Special Sections'' in Chapter 4, and ``Global Offset Table'' in Chapter 5 of the processor supplement for more information.
            ".got",
            // This section holds the global offset table. See ``Coding Examples'' in Chapter 3, ``Special Sections'' in Chapter 4, and ``Global Offset Table'' in Chapter 5 of the processor supplement for more information.
            ".lgot",
            // This section holds a symbol hash table. See ``Hash Table'' in Chapter 5 for more information.
            ".hash",
            // This section holds executable instructions that contribute to the process initialization code. When a program starts to run, the system arranges to execute the code in this section before calling the main program entry point (called main for C programs).
            ".init",
            // This section holds an array of function pointers that contributes to a single initialization array for the executable or shared object containing the section.
            ".init_array",
            // This section holds the path name of a program interpreter. If the file has a loadable segment that includes relocation, the sections' attributes will include the SHF_ALLOC bit; otherwise, that bit will be off. See Chapter 5 for more information.
            ".interp",
            // This section holds line number information for symbolic debugging, which describes the correspondence between the source program and the machine code. The contents are unspecified.
            ".line",
            // This section holds information in the format that ``Note Section''. in Chapter 5 describes.
            ".note",
            // This section holds the procedure linkage table. See ``Special Sections'' in Chapter 4 and ``Procedure Linkage Table'' in Chapter 5 of the processor supplement for more information.
            ".lplt",
            // This section holds the procedure linkage table. See ``Special Sections'' in Chapter 4 and ``Procedure Linkage Table'' in Chapter 5 of the processor supplement for more information.
            ".plt",
            // This section holds an array of function pointers that contributes to a single pre-initialization array for the executable or shared object containing the section.
            ".preinit_array",
            // These sections hold relocation information, as described in ``Relocation''. If the file has a loadable segment that includes relocation, the sections' attributes will include the SHF_ALLOC bit; otherwise, that bit will be off. Conventionally, name is supplied by the section to which the relocations apply. Thus a relocation section for .text normally would have the name .rel.text or .rela.text.
            ".relname",
            // These sections hold relocation information, as described in ``Relocation''. If the file has a loadable segment that includes relocation, the sections' attributes will include the SHF_ALLOC bit; otherwise, that bit will be off. Conventionally, name is supplied by the section to which the relocations apply. Thus a relocation section for .text normally would have the name .rel.text or .rela.text.
            ".relaname",
            // These sections hold read-only data that typically contribute to a non-writable segment in the process image. See ``Program Header'' in Chapter 5 for more information.
            ".rodata",
            // These sections hold read-only data that typically contribute to a non-writable segment in the process image. See ``Program Header'' in Chapter 5 for more information.
            ".lrodata",
            // These sections hold read-only data that typically contribute to a non-writable segment in the process image. See ``Program Header'' in Chapter 5 for more information.
            ".rodata1",
            // These sections hold read-only data that typically contribute to a non-writable segment in the process image. See ``Program Header'' in Chapter 5 for more information.
            ".lrodata1",
            // This section holds section names.
            ".shstrtab",
            // This section holds strings, most commonly the strings that represent the names associated with symbol table entries. If the file has a loadable segment that includes the symbol string table, the section's attributes will include the SHF_ALLOC bit; otherwise, that bit will be off.
            ".strtab",
            // This section holds a symbol table, as ``Symbol Table''. in this chapter describes. If the file has a loadable segment that includes the symbol table, the section's attributes will include the SHF_ALLOC bit; otherwise, that bit will be off.
            ".symtab",
            // This section holds the special symbol table section index array, as described above. The section's attributes will include the SHF_ALLOC bit if the associated symbol table section does; otherwise that bit will be off.
            ".symtab_shndx",
            // This section holds uninitialized thread-local data that contribute to the program's memory image. By definition, the system initializes the data with zeros when the data is instantiated for each new execution flow. The section occupies no file space, as indicated by the section type, SHT_NOBITS. Implementations need not support thread-local storage.
            ".tbss",
            // This section holds initialized thread-local data that contributes to the program's memory image. A copy of its contents is instantiated by the system for each new execution flow. Implementations need not support thread-local storage.
            ".tdata",
            // This section holds the ``text,'' or executable instructions, of a program.
            ".text",
            // This section holds the ``text,'' or executable instructions, of a program.
            ".ltext",
            // Names beginning .ARM.exidx name sections containing index entries for section unwinding.
            ".ARM.exidx",
            // Names beginning .ARM.extab name sections containing exception unwinding information.
            ".ARM.extab",
            // Names a section that contains a BPABI DLL dynamic linking pre-emption map.
            ".ARM.preemptmap",
            // Names a section that contains build attributes. 
            ".ARM.attributes",
            // Name sections used by the Debugging Overlaid Programs ABI extension.
            ".ARM.debug_overlay",
            // Name sections used by the Debugging Overlaid Programs ABI extension.
            ".ARM.overlay_table",
            // This section holds initialized short data that contribute to the program memory image.
            ".sdata",
            // This section holds uninitialized short data that contribute to the program memory image. By definition, the system initializes the data with zeros when the program begins to run.
            ".sbss",
            // This section holds 4 byte read-only literals that contribute to the program memory image. Its purpose is to provide a list of unique 4-byte literals used by a program.
            ".lit4",
            // This section holds 8 byte read-only literals that contribute to the program memory image. Its purpose is to provide a list of unique 4-byte literals used by a program.
            ".lit8",
            // This section provides information on the program register usage to the system.
            ".reginfo",
            // This section contains information on each of the libraries used at static link time
            ".liblist",
            // This section provides additional dynamic linking information about symbols in an executable file that conflict with symbols defined in the dynamic shared libraries with which the file is linked.
            ".conflict",
            // This section contains a global pointer table. The global pointer table is described in "Global Data Area" in this chapter. The section is named .gptab.sbss,.gptab.sdata, gptab.bss, or .gptab.data depending on which data section the particular .gptab refers.
            ".gptab",
            // This section name is reserved and the contents of this type of section are unspecified. The section contents can be ignored.
            ".ucode",
            // This section contains symbol table information as emitted by the MIPS compilers. Its content is described in Chapter 10 of the MIPS Assembly Language Programmer’s Guide, order number ASM-01-DOC, (Copyright  1989, MIPS Computer Systems, Inc.). The information in this section is dependent on the location of other sections in the file; if an object is relocated, the section must be updated. Discard this section if an object file is relocated and the ABI compliant system does not update the section.
            ".mdebug",
            // This relocation section contains run-time entries for the .data and .sdata sections.
            ".rel.dyn",
            // This section holds DWARF 2.0 abbreviation tables.
            ".debug_abbrev",
            // This section holds the DWARF 2.0 lookup table to find the debugging information for an object by address.
            ".debug_aranges",
            // This section holds DWARF 2.0 virtual unwind information.
            ".debug_frame",
            // This section holds DWARF 2.0 debugging information entries.
            ".debug_info",
            // This section holds DWARF 2.0 line number information.
            ".debug_line",
            // This section holds DWARF 2.0 location lists.
            ".debug_loc",
            // This section holds DWARF 2.0 macro information.
            ".debug_macinfo",
            // This section holds the DWARF 2.0 lookup table to find the debugging information for an object by name.
            ".debug_pubnames",
            // This section holds string values for DWARF 2.0 attributes.
            ".debug_str",
            // Product-specific extension bits
            ".PARISC.archext",
            // Millicode
            ".PARISC.milli",
            // Unwind table
            ".PARISC.unwind",
            // Stack unwind and exception handling information
            ".PARISC.unwind_info",
            // This section contains tags. The size (sh_entsize) of each entry in this section is 8 and the alignment (sh_addralign) is 4. The relocation section .rela.tags, associated with the .tags section, should have the SHF_EXCLUDE attribute.
            ".tags",
            // This section contains data that enable a program to locate its tags. Locating tags is described in Tags below.
            ".taglist",
            // This section, which appears in object files only (not executable or shared objects), contains one entry for each entry in the .tags section. Each entry has STB_LOCAL binding and is of type STT_NOTYPE. The st_shndx and st_value fields of the entries specify the index of the section and the section offset to which the tag applies, respectively.
            ".tagsym",
            // This section on some operating systems (e.g. BeOS) contains application resources
            ".rsrc"
        };
    }
}