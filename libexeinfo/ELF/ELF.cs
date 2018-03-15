//
// ELF.cs
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace libexeinfo
{
    public partial class ELF : IExecutable
    {
        Architecture[]              architectures;
        Elf64_Ehdr                  header;
        string                      interpreter;
        Dictionary<string, ElfNote> notes;
        Elf64_Phdr[]                programHeaders;
        string[]                    sectionNames;
        Elf64_Shdr[]                sections;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:libexeinfo.ELF" /> class.
        /// </summary>
        /// <param name="path">Executable path.</param>
        public ELF(string path)
        {
            BaseStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            Initialize();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:libexeinfo.ELF" /> class.
        /// </summary>
        /// <param name="stream">Stream containing the executable.</param>
        public ELF(Stream stream)
        {
            BaseStream = stream;
            Initialize();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:libexeinfo.ELF" /> class.
        /// </summary>
        /// <param name="data">Byte array containing the executable.</param>
        public ELF(byte[] data)
        {
            BaseStream = new MemoryStream(data);
            Initialize();
        }

        public bool                      Recognized              { get; private set; }
        public string                    Type                    { get; private set; }
        public IEnumerable<Architecture> Architectures           => architectures;
        public OperatingSystem           RequiredOperatingSystem { get; private set; }
        public IEnumerable<string>       Strings                 { get; private set; }
        public IEnumerable<Segment>      Segments                { get; private set; }

        /// <summary>
        ///     The <see cref="FileStream" /> that contains the executable represented by this instance
        /// </summary>
        public Stream BaseStream { get; }
        public bool IsBigEndian { get; private set; }

        void Initialize()
        {
            Recognized = false;
            if(BaseStream == null) return;

            byte[] buffer = new byte[Marshal.SizeOf(typeof(Elf64_Ehdr))];

            BaseStream.Position = 0;
            BaseStream.Read(buffer, 0, buffer.Length);
            header     = BigEndianMarshal.ByteArrayToStructureLittleEndian<Elf64_Ehdr>(buffer);
            Recognized = header.ei_mag.SequenceEqual(ELFMAG);

            if(!Recognized) return;

            Type          = "Executable and Linkable Format (ELF)";
            IsBigEndian   = header.ei_data == eiData.ELFDATA2MSB;
            architectures = new Architecture[1];

            switch(header.ei_class)
            {
                case eiClass.ELFCLASS32:
                    header = UpBits(buffer, header.ei_data == eiData.ELFDATA2MSB);
                    break;
                case eiClass.ELFCLASS64:
                    if(header.ei_data == eiData.ELFDATA2MSB)
                    {
                        header           = BigEndianMarshal.ByteArrayToStructureBigEndian<Elf64_Ehdr>(buffer);
                        header.e_type    = (eType)Swapping.Swap((ushort)header.e_type);
                        header.e_machine = (eMachine)Swapping.Swap((ushort)header.e_machine);
                        header.e_version = (eVersion)Swapping.Swap((uint)header.e_version);
                    }
                    else header = BigEndianMarshal.ByteArrayToStructureLittleEndian<Elf64_Ehdr>(buffer);

                    break;
                default: return;
            }

            architectures[0] = GetArchitecture(header.e_machine);

            if(header.ei_data != eiData.ELFDATA2LSB && header.ei_data != eiData.ELFDATA2MSB ||
               header.ei_version != eiVersion.EV_CURRENT) return;

            List<string> strings = new List<string>();

            if(header.e_phnum == 0 && header.e_shnum == 0) return;

            buffer              = new byte[header.e_phentsize];
            BaseStream.Position = (long)header.e_phoff;
            programHeaders      = new Elf64_Phdr[header.e_phnum];
            for(int i = 0; i < programHeaders.Length; i++)
            {
                BaseStream.Read(buffer, 0, buffer.Length);
                switch(header.ei_class)
                {
                    case eiClass.ELFCLASS32:
                        programHeaders[i] = UpBitsProgramHeader(buffer, header.ei_data == eiData.ELFDATA2MSB);
                        break;
                    case eiClass.ELFCLASS64:
                        if(header.ei_data == eiData.ELFDATA2MSB)
                        {
                            programHeaders[i] =
                                BigEndianMarshal.ByteArrayToStructureBigEndian<Elf64_Phdr>(buffer);
                            programHeaders[i].p_type  = (phType)Swapping.Swap((uint)programHeaders[i].p_type);
                            programHeaders[i].p_flags = (phFlags)Swapping.Swap((uint)programHeaders[i].p_flags);
                        }
                        else programHeaders[i] = BigEndianMarshal.ByteArrayToStructureLittleEndian<Elf64_Phdr>(buffer);

                        break;
                    default: return;
                }
            }

            int len;
            int pos;

            for(int i = 0; i < programHeaders.Length; i++)
            {
                if(programHeaders[i].p_type != phType.PT_INTERP && programHeaders[i].p_type != phType.PT_NOTE) continue;

                buffer              = new byte[programHeaders[i].p_filesz];
                BaseStream.Position = (long)programHeaders[i].p_offset;
                BaseStream.Read(buffer, 0, buffer.Length);

                switch(programHeaders[i].p_type)
                {
                    case phType.PT_INTERP:
                        interpreter = StringHandlers.CToString(buffer);
                        break;
                    case phType.PT_NOTE when buffer.Length > 12:
                        pos = 0;
                        while(pos < buffer.Length)
                        {
                            uint namesz   = BitConverter.ToUInt32(buffer, pos + 0);
                            uint descsz   = BitConverter.ToUInt32(buffer, pos + 4);
                            uint notetype = BitConverter.ToUInt32(buffer, pos + 8);
                            pos += 12;

                            if(IsBigEndian)
                            {
                                namesz   = Swapping.Swap(namesz);
                                descsz   = Swapping.Swap(descsz);
                                notetype = Swapping.Swap(notetype);
                            }

                            ElfNote note = new ElfNote
                            {
                                name     = Encoding.ASCII.GetString(buffer, pos, (int)(namesz - 1)),
                                type     = notetype,
                                contents = new byte[descsz]
                            };

                            pos += (int)namesz;

                            pos += pos % 4 != 0 ? 4 - pos % 4 : 0;

                            Array.Copy(buffer, pos, note.contents, 0, descsz);
                            pos += (int)descsz;
                            pos += pos % 4 != 0 ? 4 - pos % 4 : 0;

                            RequiredOperatingSystem = GetOsByNote(note, interpreter, IsBigEndian);
                        }

                        break;
                }
            }

            Segment[] segments;

            if(header.e_shnum == 0)
            {
                segments = new Segment[programHeaders.Length];
                for(int i = 0; i < programHeaders.Length; i++)
                {
                    segments[i] = new Segment
                    {
                        Flags  = $"{programHeaders[i].p_flags}",
                        Offset = (long)programHeaders[i].p_offset,
                        Size   = (long)programHeaders[i].p_filesz
                    };

                    switch(programHeaders[i].p_type)
                    {
                        case phType.PT_NULL: break;
                        case phType.PT_LOAD:
                            segments[i].Name = ".text";
                            break;
                        case phType.PT_DYNAMIC:
                            segments[i].Name = ".dynamic";
                            break;
                        case phType.PT_INTERP:
                            segments[i].Name = ".interp";
                            break;
                        case phType.PT_NOTE:
                            segments[i].Name = ".note";
                            break;
                        case phType.PT_SHLIB:
                            segments[i].Name = ".shlib";
                            break;
                        case phType.PT_PHDR: break;
                        case phType.PT_TLS:
                            segments[i].Name = ".tls";
                            break;
                        default:
                            segments[i].Name = programHeaders[i].p_flags.HasFlag(phFlags.PF_X)
                                                   ? ".text"
                                                   : programHeaders[i].p_flags.HasFlag(phFlags.PF_W)
                                                       ? ".data"
                                                       : ".rodata";
                            break;
                    }
                }

                if(RequiredOperatingSystem.Name == null)
                    RequiredOperatingSystem =
                        GetOsByInterpreter(interpreter, false, false, false, false, false, header.e_machine);

                return;
            }

            BaseStream.Position = (long)header.e_shoff;
            buffer              = new byte[header.e_shentsize];
            sections            = new Elf64_Shdr[header.e_shnum];

            // Read section 0, aka "the NULL section"
            BaseStream.Read(buffer, 0, buffer.Length);
            switch(header.ei_class)
            {
                case eiClass.ELFCLASS32:
                    sections[0] = UpBitsSection(buffer, header.ei_data == eiData.ELFDATA2MSB);
                    break;
                case eiClass.ELFCLASS64:
                    if(header.ei_data == eiData.ELFDATA2MSB)
                    {
                        sections[0]          = BigEndianMarshal.ByteArrayToStructureBigEndian<Elf64_Shdr>(buffer);
                        sections[0].sh_flags = (shFlags64)Swapping.Swap((ulong)sections[0].sh_flags);
                        sections[0].sh_type  = (shType)Swapping.Swap((uint)sections[0].sh_type);
                    }
                    else sections[0] = BigEndianMarshal.ByteArrayToStructureLittleEndian<Elf64_Shdr>(buffer);

                    break;
                default: return;
            }

            // Not a null section, in some ELFs, header contains incorrect pointer, but section header "should" be at end of file so check there
            if(sections[0].sh_type != shType.SHT_NULL)
            {
                BaseStream.Position = BaseStream.Length - header.e_shentsize * header.e_shnum;
                BaseStream.Read(buffer, 0, buffer.Length);
                switch(header.ei_class)
                {
                    case eiClass.ELFCLASS32:
                        sections[0] = UpBitsSection(buffer, header.ei_data == eiData.ELFDATA2MSB);
                        break;
                    case eiClass.ELFCLASS64:
                        if(header.ei_data == eiData.ELFDATA2MSB)
                        {
                            sections[0]          = BigEndianMarshal.ByteArrayToStructureBigEndian<Elf64_Shdr>(buffer);
                            sections[0].sh_flags = (shFlags64)Swapping.Swap((ulong)sections[0].sh_flags);
                            sections[0].sh_type  = (shType)Swapping.Swap((uint)sections[0].sh_type);
                        }
                        else sections[0] = BigEndianMarshal.ByteArrayToStructureLittleEndian<Elf64_Shdr>(buffer);

                        break;
                    default: return;
                }

                // Still not null section, stop gracefully
                if(sections[0].sh_type != shType.SHT_NULL) return;

                // Rewind
                BaseStream.Position = BaseStream.Length - header.e_shentsize * header.e_shnum;
            }
            // Rewind
            else BaseStream.Position = (long)header.e_shoff;

            for(int i = 0; i < sections.Length; i++)
            {
                BaseStream.Read(buffer, 0, buffer.Length);
                switch(header.ei_class)
                {
                    case eiClass.ELFCLASS32:
                        sections[i] = UpBitsSection(buffer, header.ei_data == eiData.ELFDATA2MSB);
                        break;
                    case eiClass.ELFCLASS64:
                        if(header.ei_data == eiData.ELFDATA2MSB)
                        {
                            sections[i]          = BigEndianMarshal.ByteArrayToStructureBigEndian<Elf64_Shdr>(buffer);
                            sections[i].sh_flags = (shFlags64)Swapping.Swap((ulong)sections[i].sh_flags);
                            sections[i].sh_type  = (shType)Swapping.Swap((uint)sections[i].sh_type);
                        }
                        else sections[i] = BigEndianMarshal.ByteArrayToStructureLittleEndian<Elf64_Shdr>(buffer);

                        break;
                    default: return;
                }
            }

            BaseStream.Position = (long)sections[header.e_shstrndx].sh_offset;
            buffer              = new byte[sections[header.e_shstrndx].sh_size];
            BaseStream.Read(buffer, 0, buffer.Length);
            sectionNames = new string[sections.Length];
            segments     = new Segment[sections.Length];

            for(int i = 0; i < sections.Length; i++)
            {
                pos = (int)sections[i].sh_name;
                len = 0;

                for(int p = pos; p < buffer.Length; p++)
                {
                    if(buffer[p] == 0x00) break;

                    len++;
                }

                sectionNames[i] = Encoding.ASCII.GetString(buffer, pos, len);
                strings.Add(sectionNames[i]);
                segments[i] = new Segment
                {
                    Flags  = $"{sections[i].sh_flags}",
                    Name   = sectionNames[i],
                    Offset = (long)sections[i].sh_offset,
                    Size   = (long)sections[i].sh_size
                };
            }

            int strStart = 0;
            len = 0;
            for(int p = 0; p < buffer.Length; p++)
            {
                if(buffer[p] == 0x00)
                {
                    if(len == 0) continue;

                    strings.Add(Encoding.ASCII.GetString(buffer, strStart, len));
                    strStart = p + 1;
                    len      = 0;
                    continue;
                }

                len++;
            }

            notes = new Dictionary<string, ElfNote>();
            bool beos     = false;
            bool haiku    = false;
            bool solaris  = false;
            bool lynxos   = false;
            bool skyos    = false;
            bool dellsysv = false;
            bool unixware = false;
            bool os2      = false;
            bool amigaos4 = false;
            bool aros     = false;
            bool morphos  = false;
            bool nonstop  = (uint)header.e_type == 100;

            // Sections that contain an array of null-terminated strings by definition
            for(int i = 0; i < sections.Length; i++)
            {
                BaseStream.Position = (long)sections[i].sh_offset;
                buffer              = new byte[sections[i].sh_size];
                BaseStream.Read(buffer, 0, buffer.Length);

                if(sectionNames[i] == ".interp"  || sectionNames[i] == ".dynstr" ||
                   sectionNames[i] == ".comment" ||
                   sectionNames[i] == ".strtab"  || sectionNames[i] == ".stab.indexstr" ||
                   sectionNames[i] == ".debug_str")
                {
                    strStart = 0;
                    len      = 0;
                    for(int p = 0; p < buffer.Length; p++)
                    {
                        if(buffer[p] == 0x00)
                        {
                            if(len == 0) continue;

                            string str                          = Encoding.ASCII.GetString(buffer, strStart, len);
                            if(str[str.Length - 1] == '\0') str = str.Substring(0, str.Length - 1);
                            if(!string.IsNullOrWhiteSpace(str))
                            {
                                strings.Add(str);
                                switch(sectionNames[i])
                                {
                                    case ".interp":
                                        interpreter = str;
                                        break;
                                    case ".comment":
                                        if(str.Contains("beos")) beos                           = true;
                                        if(str.Contains("haiku")) haiku                         = true;
                                        if(str.Contains("(Lynx)")) lynxos                       = true;
                                        if(str.StartsWith("Dell UNIX System V")) dellsysv       = true;
                                        if(str.Contains("uw7") || str.Contains("uw1")) unixware = true;
                                        break;
                                    case ".dynstr":
                                        if(str.Contains("SUNW")) solaris = true;
                                        switch(str)
                                        {
                                            case "libsky.so":
                                                skyos = true;
                                                break;
                                            case "DOSCALLS":
                                                os2 = true;
                                                break;
                                        }

                                        break;
                                }
                            }

                            strStart = p + 1;
                            len      = 0;
                            continue;
                        }

                        len++;
                    }
                }
                else if(sectionNames[i].StartsWith(".note") && buffer.Length > 12)
                {
                    pos = 0;
                    string noteAsString = Encoding.ASCII.GetString(buffer);

                    // Only IA-64 OpenVMS ELF uses 64-bit fields inside the note!
                    bool openVms = noteAsString.Contains("IPF/VMS");

                    while(pos < buffer.Length)
                    {
                        uint namesz;
                        uint descsz;
                        uint notetype;

                        if(openVms)
                        {
                            namesz   =  (uint)BitConverter.ToUInt64(buffer, pos);
                            descsz   =  (uint)BitConverter.ToUInt64(buffer, pos + 8);
                            notetype =  (uint)BitConverter.ToUInt64(buffer, pos + 16);
                            pos      += 24;
                        }
                        else
                        {
                            namesz   =  BitConverter.ToUInt32(buffer, pos);
                            descsz   =  BitConverter.ToUInt32(buffer, pos + 4);
                            notetype =  BitConverter.ToUInt32(buffer, pos + 8);
                            pos      += 12;
                        }

                        if(IsBigEndian)
                        {
                            namesz   = Swapping.Swap(namesz);
                            descsz   = Swapping.Swap(descsz);
                            notetype = Swapping.Swap(notetype);
                        }

                        ElfNote note = new ElfNote
                        {
                            name     = Encoding.ASCII.GetString(buffer, pos, (int)(namesz - 1)),
                            type     = notetype,
                            contents = new byte[descsz]
                        };

                        pos += (int)namesz;

                        pos += pos % (openVms ? 8 : 4) != 0 ? (openVms ? 8 : 4) - pos % (openVms ? 8 : 4) : 0;

                        Array.Copy(buffer, pos, note.contents, 0, descsz);
                        pos += (int)descsz;
                        pos += pos % (openVms ? 8 : 4) != 0 ? (openVms ? 8 : 4) - pos % (openVms ? 8 : 4) : 0;

                        string notename = note.type != 1 ? $"{sectionNames[i]}.{note.type}" : sectionNames[i];
                        if(!notes.ContainsKey(notename)) notes.Add(notename, note);
                    }
                }
            }

            for(int i = 0; i < sections.Length; i++)
            {
                if(sectionNames[i] != ".rodata") continue;

                buffer              = new byte[sections[i].sh_size];
                BaseStream.Position = (long)sections[i].sh_offset;
                BaseStream.Read(buffer, 0, buffer.Length);
                string rodataAsString = Encoding.ASCII.GetString(buffer);

                if(header.e_machine == eMachine.EM_PPC)
                {
                    if(rodataAsString.Contains("SUNW_OST_OSLIB")) solaris         = true;
                    else if(rodataAsString.Contains("newlib.library")) amigaos4   = true;
                    else if(rodataAsString.Contains("arosc.library")) aros        = true;
                    else if(rodataAsString.Contains("intuition.library")) morphos = true;
                }
                else if(rodataAsString.Contains("arosc.library")) aros = true;
            }

            if(notes.TryGetValue(".note.ABI-tag", out ElfNote abiTag))
                RequiredOperatingSystem = GetOsByNote(abiTag, interpreter, IsBigEndian);
            else if(notes.TryGetValue(".note.netbsd.ident", out ElfNote netbsdIdent))
                RequiredOperatingSystem = GetOsByNote(netbsdIdent, interpreter, IsBigEndian);
            else if(notes.TryGetValue(".note.minix.ident", out ElfNote minixIdent))
                RequiredOperatingSystem = GetOsByNote(minixIdent, interpreter, IsBigEndian);
            else if(notes.TryGetValue(".note.openbsd.ident", out ElfNote openbsdIdent))
                RequiredOperatingSystem = GetOsByNote(openbsdIdent, interpreter, IsBigEndian);
            else if(notes.TryGetValue(".note.tag", out ElfNote freebsdTag))
                RequiredOperatingSystem = GetOsByNote(freebsdTag, interpreter, IsBigEndian);
            else if(notes.TryGetValue(".note.ident", out ElfNote bsdiTag) && bsdiTag.name == "BSD/OS")
                RequiredOperatingSystem = GetOsByNote(bsdiTag, interpreter, IsBigEndian);
            else if(header.ei_osabi != eiOsabi.ELFOSABI_NONE && header.ei_osabi != eiOsabi.ELFOSABI_GNU &&
                    header.ei_osabi != eiOsabi.ELFOSABI_ARM  && header.ei_osabi != eiOsabi.ELFOSABI_ARM_AEABI)
                switch(header.ei_osabi)
                {
                    case eiOsabi.ELFOSABI_HPUX:
                        RequiredOperatingSystem = new OperatingSystem {Name = "HP-UX"};
                        break;
                    case eiOsabi.ELFOSABI_NETBSD:
                        RequiredOperatingSystem = new OperatingSystem {Name = "NetBSD"};
                        break;
                    case eiOsabi.ELFOSABI_SOLARIS:
                        RequiredOperatingSystem = new OperatingSystem {Name = "Solaris"};
                        break;
                    case eiOsabi.ELFOSABI_AIX:
                        RequiredOperatingSystem = new OperatingSystem {Name = "AIX"};
                        break;
                    case eiOsabi.ELFOSABI_IRIX:
                        RequiredOperatingSystem = new OperatingSystem {Name = "IRIX"};
                        break;
                    case eiOsabi.ELFOSABI_FREEBSD:
                        RequiredOperatingSystem = new OperatingSystem {Name = "FreeBSD"};
                        break;
                    case eiOsabi.ELFOSABI_TRU64:
                        RequiredOperatingSystem = new OperatingSystem {Name = "Tru64 UNIX"};
                        break;
                    case eiOsabi.ELFOSABI_MODESTO:
                        RequiredOperatingSystem = new OperatingSystem {Name = "Novell Modesto"};
                        break;
                    case eiOsabi.ELFOSABI_OPENBSD:
                        RequiredOperatingSystem = new OperatingSystem {Name = "OpenBSD"};
                        break;
                    case eiOsabi.ELFOSABI_OPENVMS:
                        RequiredOperatingSystem = new OperatingSystem {Name = "OpenVMS"};
                        break;
                    case eiOsabi.ELFOSABI_NSK:
                        RequiredOperatingSystem = new OperatingSystem {Name = "Non-Stop Kernel"};
                        break;
                    case eiOsabi.ELFOSABI_AROS:
                        RequiredOperatingSystem = new OperatingSystem {Name = "AROS"};
                        break;
                    case eiOsabi.ELFOSABI_FENIXOS:
                        RequiredOperatingSystem = new OperatingSystem {Name = "FenixOS"};
                        break;
                    case eiOsabi.ELFOSABI_CLOUDABI:
                        RequiredOperatingSystem = new OperatingSystem {Name = "CloudABI"};
                        break;
                    case eiOsabi.ELFOSABI_OPENVOS:
                        RequiredOperatingSystem = new OperatingSystem {Name = "OpenVOS"};
                        break;
                    case eiOsabi.ELFOSABI_STANDALONE:
                        RequiredOperatingSystem = new OperatingSystem {Name = "None"};
                        break;
                }
            else if(!string.IsNullOrEmpty(interpreter))
                RequiredOperatingSystem =
                    GetOsByInterpreter(interpreter, skyos, solaris, dellsysv, unixware, os2, header.e_machine);
            else if(beos) RequiredOperatingSystem     = new OperatingSystem {Name = "BeOS", MajorVersion = 4};
            else if(haiku) RequiredOperatingSystem    = new OperatingSystem {Name = "Haiku"};
            else if(lynxos) RequiredOperatingSystem   = new OperatingSystem {Name = "LynxOS"};
            else if(amigaos4) RequiredOperatingSystem = new OperatingSystem {Name = "AmigaOS", MajorVersion = 4};
            else if(aros) RequiredOperatingSystem     = new OperatingSystem {Name = "AROS"};
            else if(morphos) RequiredOperatingSystem  = new OperatingSystem {Name = "MorphOS"};
            else if(nonstop) RequiredOperatingSystem  = new OperatingSystem {Name = "Non-Stop Kernel"};

            if(strings.Count > 0)
            {
                strings.Remove(null);
                strings.Remove("");
                strings.Sort();
                Strings = strings.Distinct();
            }

            Segments = segments;
        }

        static OperatingSystem GetOsByNote(ElfNote note, string interpreter, bool bigendian)
        {
            if(note.type != 1) return new OperatingSystem {Name = interpreter};

            switch(note.name)
            {
                case "GNU":

                    GnuAbiTag gnuAbiTag = DecodeGnuAbiTag(note, bigendian);
                    if(gnuAbiTag != null)
                        if(gnuAbiTag.system == GnuAbiSystem.Linux && interpreter == "/system/bin/linker")
                            return new OperatingSystem
                            {
                                Name         = "Android",
                                MajorVersion = (int)gnuAbiTag.major,
                                MinorVersion = (int)gnuAbiTag.minor
                            };
                        else
                            return new OperatingSystem
                            {
                                Name         = $"{gnuAbiTag.system}",
                                MajorVersion = (int)gnuAbiTag.major,
                                MinorVersion = (int)gnuAbiTag.minor
                            };
                    else return new OperatingSystem {Name = interpreter};
                case "NetBSD":
                    uint netbsdVersionConstant          = BitConverter.ToUInt32(note.contents, 0);
                    if(bigendian) netbsdVersionConstant = Swapping.Swap(netbsdVersionConstant);

                    if(netbsdVersionConstant > 100000000)
                        return new OperatingSystem
                        {
                            Name         = "NetBSD",
                            MajorVersion = (int)(netbsdVersionConstant           / 100000000),
                            MinorVersion = (int)(netbsdVersionConstant / 1000000 % 100)
                        };

                    int nbsdMajor = 0;
                    int nbsdMinor = 0;

                    switch(netbsdVersionConstant)
                    {
                        case 1993070:
                            nbsdMajor = 0;
                            nbsdMinor = 9;
                            break;
                        case 1994100:
                            nbsdMajor = 1;
                            nbsdMinor = 0;
                            break;
                        case 199511:
                            nbsdMajor = 1;
                            nbsdMinor = 1;
                            break;
                        case 199609:
                            nbsdMajor = 1;
                            nbsdMinor = 2;
                            break;
                        case 199714:
                            nbsdMajor = 1;
                            nbsdMinor = 3;
                            break;
                        case 199905:
                            nbsdMajor = 1;
                            nbsdMinor = 4;
                            break;
                    }

                    return new OperatingSystem {Name = "NetBSD", MajorVersion = nbsdMajor, MinorVersion = nbsdMinor};
                case "Minix":
                    uint minixVersionConstant          = BitConverter.ToUInt32(note.contents, 0);
                    if(bigendian) minixVersionConstant = Swapping.Swap(minixVersionConstant);

                    if(minixVersionConstant > 100000000)
                        return new OperatingSystem
                        {
                            Name         = "MINIX",
                            MajorVersion = (int)(minixVersionConstant           / 100000000),
                            MinorVersion = (int)(minixVersionConstant / 1000000 % 100)
                        };
                    else return new OperatingSystem {Name = "MINIX"};
                case "OpenBSD": return new OperatingSystem {Name = "OpenBSD"};
                case "FreeBSD":
                    uint freebsdVersionConstant          = BitConverter.ToUInt32(note.contents, 0);
                    if(bigendian) freebsdVersionConstant = Swapping.Swap(freebsdVersionConstant);

                    FreeBSDTag freeBsdVersion = DecodeFreeBSDTag(freebsdVersionConstant);

                    return new OperatingSystem
                    {
                        Name         = "FreeBSD",
                        MajorVersion = (int)freeBsdVersion.major,
                        MinorVersion = (int)freeBsdVersion.minor
                    };
                case "BSD/OS":
                    string bsdiVersion = StringHandlers.CToString(note.contents);
                    if(bsdiVersion.Length == 3)
                        return new OperatingSystem
                        {
                            Name         = "BSD/OS",
                            MajorVersion = bsdiVersion[0] - 0x30,
                            MinorVersion = bsdiVersion[2] - 0x30
                        };

                    goto default;
                default: return new OperatingSystem {Name = interpreter};
            }
        }

        static OperatingSystem GetOsByInterpreter(string interpreter, bool skyos, bool     solaris, bool dellsysv,
                                                  bool   unixware,    bool os2,   eMachine machine)
        {
            switch(interpreter)
            {
                case "/usr/lib/ldqnx.so.2": return new OperatingSystem {Name = "QNX", MajorVersion = 6};

                case "/usr/dglib/libc.so.1": return new OperatingSystem {Name = "DG/UX"};

                case "/shlib/ld-bsdi.so": return new OperatingSystem {Name = "BSD/OS"};

                case "/usr/lib/libc.so.1" when skyos: return new OperatingSystem {Name = "SkyOS"};

                case "/usr/lib/ld.so.1" when solaris:
                case "/usr/lib/amd64/ld.so.1" when solaris: return new OperatingSystem {Name = "Solaris"};

                case "/usr/lib/libc.so.1" when unixware: return new OperatingSystem {Name = "UnixWare"};
                case "/usr/lib/libc.so.1" when dellsysv: return new OperatingSystem {Name = "Dell UNIX System V"};
                case "/usr/lib/libc.so.1" when os2:      return new OperatingSystem {Name = "OS/2"};
                // Other M68K UNIX don't use interpreter or use COFF
                case "/usr/lib/libc.so.1"
                    when machine == eMachine.EM_68K: return new OperatingSystem {Name = "Amiga UNIX"};
                case "/usr/lib32/libc.so.1"
                    when machine == eMachine.EM_MIPS: return new OperatingSystem {Name = "IRIX", MajorVersion = 6};
                default: return new OperatingSystem {Name = interpreter};
            }
        }

        static Elf64_Ehdr UpBits(byte[] buffer, bool bigEndian)
        {
            Elf32_Ehdr ehdr32 = bigEndian
                                    ? BigEndianMarshal.ByteArrayToStructureBigEndian<Elf32_Ehdr>(buffer)
                                    : BigEndianMarshal.ByteArrayToStructureLittleEndian<Elf32_Ehdr>(buffer);
            return new Elf64_Ehdr
            {
                ei_mag        = ehdr32.ei_mag,
                ei_class      = ehdr32.ei_class,
                ei_data       = ehdr32.ei_data,
                ei_version    = ehdr32.ei_version,
                ei_osabi      = ehdr32.ei_osabi,
                ei_abiversion = ehdr32.ei_abiversion,
                ei_pad        = ehdr32.ei_pad,
                e_type        = bigEndian ? (eType)Swapping.Swap((ushort)ehdr32.e_type) : ehdr32.e_type,
                e_machine     = bigEndian ? (eMachine)Swapping.Swap((ushort)ehdr32.e_machine) : ehdr32.e_machine,
                e_version     = bigEndian ? (eVersion)Swapping.Swap((uint)ehdr32.e_version) : ehdr32.e_version,
                e_entry       = ehdr32.e_entry,
                e_phoff       = ehdr32.e_phoff,
                e_shoff       = ehdr32.e_shoff,
                e_flags       = ehdr32.e_flags,
                e_ehsize      = ehdr32.e_ehsize,
                e_phentsize   = ehdr32.e_phentsize,
                e_phnum       = ehdr32.e_phnum,
                e_shentsize   = ehdr32.e_shentsize,
                e_shnum       = ehdr32.e_shnum,
                e_shstrndx    = ehdr32.e_shstrndx
            };
        }

        static Elf64_Shdr UpBitsSection(byte[] buffer, bool bigEndian)
        {
            Elf32_Shdr shdr32 = bigEndian
                                    ? BigEndianMarshal.ByteArrayToStructureBigEndian<Elf32_Shdr>(buffer)
                                    : BigEndianMarshal.ByteArrayToStructureLittleEndian<Elf32_Shdr>(buffer);
            return new Elf64_Shdr
            {
                sh_name      = shdr32.sh_name,
                sh_type      = bigEndian ? (shType)Swapping.Swap((uint)shdr32.sh_type) : shdr32.sh_type,
                sh_flags     = (shFlags64)(bigEndian ? Swapping.Swap((uint)shdr32.sh_flags) : (uint)shdr32.sh_flags),
                sh_addr      = shdr32.sh_addr,
                sh_offset    = shdr32.sh_offset,
                sh_size      = shdr32.sh_size,
                sh_link      = shdr32.sh_link,
                sh_info      = shdr32.sh_info,
                sh_addralign = shdr32.sh_addralign,
                sh_entsize   = shdr32.sh_entsize
            };
        }

        static Elf64_Phdr UpBitsProgramHeader(byte[] buffer, bool bigEndian)
        {
            Elf32_Phdr phdr32 = bigEndian
                                    ? BigEndianMarshal.ByteArrayToStructureBigEndian<Elf32_Phdr>(buffer)
                                    : BigEndianMarshal.ByteArrayToStructureLittleEndian<Elf32_Phdr>(buffer);
            return new Elf64_Phdr
            {
                p_offset = phdr32.p_offset,
                p_type   = (phType)(bigEndian ? Swapping.Swap((uint)phdr32.p_type) : (uint)phdr32.p_type),
                p_vaddr  = phdr32.p_vaddr,
                p_paddr  = phdr32.p_paddr,
                p_filesz = phdr32.p_filesz,
                p_memsz  = phdr32.p_memsz,
                p_flags  = phdr32.p_flags,
                p_align  = phdr32.p_align
            };
        }

        /// <summary>
        ///     Identifies if the specified executable is an Executable and Linkable Format
        /// </summary>
        /// <returns><c>true</c> if the specified executable is an Executable and Linkable Format, <c>false</c> otherwise.</returns>
        /// <param name="path">Executable path.</param>
        public static bool Identify(string path)
        {
            FileStream exeFs = File.Open(path, FileMode.Open, FileAccess.Read);
            return Identify(exeFs);
        }

        /// <summary>
        ///     Identifies if the specified executable is an Executable and Linkable Format
        /// </summary>
        /// <returns><c>true</c> if the specified executable is an Executable and Linkable Format, <c>false</c> otherwise.</returns>
        /// <param name="stream">Stream containing the executable.</param>
        public static bool Identify(FileStream stream)
        {
            byte[] buffer = new byte[Marshal.SizeOf(typeof(Elf32_Ehdr))];

            stream.Position = 0;
            stream.Read(buffer, 0, buffer.Length);
            IntPtr hdrPtr = Marshal.AllocHGlobal(buffer.Length);
            Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
            Elf32_Ehdr elfHdr = (Elf32_Ehdr)Marshal.PtrToStructure(hdrPtr, typeof(Elf32_Ehdr));
            Marshal.FreeHGlobal(hdrPtr);

            return elfHdr.ei_mag.SequenceEqual(ELFMAG);
        }

        static Architecture GetArchitecture(eMachine machine)
        {
            switch(machine)
            {
                case eMachine.EM_386:     return Architecture.I386;
                case eMachine.EM_68HC05:  return Architecture.M68HC05;
                case eMachine.EM_68HC08:  return Architecture.M68HC08;
                case eMachine.EM_68HC11:  return Architecture.M68HC11;
                case eMachine.EM_68HC12:  return Architecture.M68HC12;
                case eMachine.EM_68HC16:  return Architecture.M68HC16;
                case eMachine.EM_68K:     return Architecture.M68K;
                case eMachine.EM_78KOR:   return Architecture.R78KOR;
                case eMachine.EM_8051:    return Architecture.I8051;
                case eMachine.EM_860:     return Architecture.I860;
                case eMachine.EM_88K:     return Architecture.M88K;
                case eMachine.EM_960:     return Architecture.I960;
                case eMachine.EM_AARCH64: return Architecture.Aarch64;
                case eMachine.EM_ALPHA_OLD:
                case eMachine.EM_ALPHA: return Architecture.Alpha;
                case eMachine.EM_ALTERA_NIOS2: return Architecture.NIOS2;
                case eMachine.EM_AMDGPU:       return Architecture.AmdGpu;
                case eMachine.EM_ARCA:         return Architecture.Arca;
                case eMachine.EM_ARC_COMPACT2: return Architecture.ARCompact2;
                case eMachine.EM_ARC_COMPACT:  return Architecture.ARCompact;
                case eMachine.EM_ARC:          return Architecture.Argonaut;
                case eMachine.EM_ARM:          return Architecture.Arm;
                case eMachine.EM_AVR32:        return Architecture.Avr32;
                case eMachine.EM_AVR:          return Architecture.Avr;
                case eMachine.EM_BA1:          return Architecture.Ba1;
                case eMachine.EM_BA2:          return Architecture.Ba2;
                case eMachine.EM_BLACKFIN:     return Architecture.Blackfin;
                case eMachine.EM_C166:         return Architecture.C16;
                case eMachine.EM_CDP:          return Architecture.CDP;
                case eMachine.EM_CE:           return Architecture.CommunicationEngine;
                case eMachine.EM_CLOUDSHIELD:  return Architecture.CloudShield;
                case eMachine.EM_COGE:         return Architecture.Coge;
                case eMachine.EM_COLDFIRE:     return Architecture.Coldfire;
                case eMachine.EM_COOL:         return Architecture.CoolEngine;
                case eMachine.EM_COREA_1ST:    return Architecture.CoreA;
                case eMachine.EM_COREA_2ND:    return Architecture.CoreA2;
                case eMachine.EM_CR16:         return Architecture.CompactRisc16;
                case eMachine.EM_CRAYNV2:      return Architecture.CrayNv2;
                case eMachine.EM_CRIS:         return Architecture.Cris;
                case eMachine.EM_CR:           return Architecture.CompactRisc;
                case eMachine.EM_CRX:          return Architecture.CompactRiscX;
                case eMachine.EM_CSR_KALIMBA:  return Architecture.Kalimba;
                case eMachine.EM_CUDA:         return Architecture.Cuda;
                case eMachine.EM_CYGNUS_M32R:  return Architecture.M32R;
                case eMachine.EM_CYGNUS_V850:  return Architecture.V850;
                case eMachine.EM_CYPRESS_M8C:  return Architecture.M8C;
                case eMachine.EM_D10V:         return Architecture.D10V;
                case eMachine.EM_D30V:         return Architecture.D30V;
                case eMachine.EM_DXP:          return Architecture.DeepExecutionProcessor;
                case eMachine.EM_FR20:         return Architecture.FR20;
                case eMachine.EM_FR30:         return Architecture.FR30;
                case eMachine.EM_FRV:          return Architecture.FRV;
                case eMachine.EM_FT32:         return Architecture.FT32;
                case eMachine.EM_FX66:         return Architecture.FX66;
                case eMachine.EM_H8_300H:
                case eMachine.EM_H8_300:
                case eMachine.EM_H8_500:
                case eMachine.EM_H8S: return Architecture.H8;
                case eMachine.EM_HOBBIT:        return Architecture.Hobbit;
                case eMachine.EM_HUANY:         return Architecture.Huany;
                case eMachine.EM_IA_64:         return Architecture.IA64;
                case eMachine.EM_IP2K:          return Architecture.IP2K;
                case eMachine.EM_JAVELIN:       return Architecture.Javelin;
                case eMachine.EM_LATTICEMICO32: return Architecture.Lattice;
                case eMachine.EM_M16C:          return Architecture.M16C;
                case eMachine.EM_M32C:          return Architecture.M32C;
                case eMachine.EM_M32:           return Architecture.We32000;
                case eMachine.EM_M32R:          return Architecture.M32R;
                case eMachine.EM_MCST_ELBRUS:   return Architecture.Elbrus;
                case eMachine.EM_MICROBLAZE:    return Architecture.MicroBlaze;
                case eMachine.EM_MIPS:          return Architecture.Mips;
                case eMachine.EM_MIPS_RS3_LE:   return Architecture.Mips3;
                case eMachine.EM_MIPS_X:        return Architecture.Mips;
                case eMachine.EM_NS32K:         return Architecture.We32000;
                case eMachine.EM_OPEN8:         return Architecture.Open8;
                case eMachine.EM_OPENRISC:      return Architecture.OpenRisc;
                case eMachine.EM_PARISC:        return Architecture.PaRisc;
                case eMachine.EM_PDP10:         return Architecture.Pdp10;
                case eMachine.EM_PDP11:         return Architecture.Pdp11;
                case eMachine.EM_PJ:            return Architecture.PicoJava;
                case eMachine.EM_PPC64:         return Architecture.PowerPc64;
                case eMachine.EM_PPC:           return Architecture.PowerPc;
                case eMachine.EM_PRISM:         return Architecture.Prism;
                case eMachine.EM_R32C:          return Architecture.R32C;
                case eMachine.EM_RISCV:         return Architecture.RiscV;
                case eMachine.EM_S370:          return Architecture.S370;
                case eMachine.EM_S390_OLD:
                case eMachine.EM_S390: return Architecture.S390;
                case eMachine.EM_SHARC: return Architecture.Sharc;
                case eMachine.EM_SH:    return Architecture.Sh2;
                case eMachine.EM_SPARC32PLUS:
                case eMachine.EM_SPARC: return Architecture.Sparc;
                case eMachine.EM_SPARCV9:  return Architecture.Sparc64;
                case eMachine.EM_SPU:      return Architecture.Spu;
                case eMachine.EM_STARCORE: return Architecture.StarCore;
                case eMachine.EM_STM8:     return Architecture.STM8;
                case eMachine.EM_TILE64:   return Architecture.Tile64;
                case eMachine.EM_TILEGX:   return Architecture.TileGx;
                case eMachine.EM_TILEPRO:  return Architecture.TilePro;
                case eMachine.EM_TINYJ:    return Architecture.TinyJ;
                case eMachine.EM_TRICORE:  return Architecture.TriCore;
                case eMachine.EM_TRIMEDIA: return Architecture.TriMedia;
                case eMachine.EM_V800:     return Architecture.V800;
                case eMachine.EM_V850:     return Architecture.V850;
                case eMachine.EM_VAX:      return Architecture.Vax;
                case eMachine.EM_VIDEOCORE3:
                case eMachine.EM_VIDEOCORE5:
                case eMachine.EM_VIDEOCORE: return Architecture.VideoCore;
                case eMachine.EM_X86_64: return Architecture.Amd64;
                case eMachine.EM_XCORE:  return Architecture.Xcore;
                case eMachine.EM_XTENSA: return Architecture.Xtensa;
                case eMachine.EM_Z80:    return Architecture.Z80;
                default:                 return Architecture.Unknown;
            }
        }
    }
}