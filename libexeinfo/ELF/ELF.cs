using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace libexeinfo
{
    public partial class ELF : IExecutable
    {
        List<Architecture> architectures;
        Elf64_Ehdr         Header;
        string             interpreter;
        string[]           sectionNames;
        Elf64_Shdr[]       sections;
        Dictionary<string, ElfNote> notes;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:libexeinfo.ELF" /> class.
        /// </summary>
        /// <param name="path">Executable path.</param>
        public ELF(string path)
        {
            BaseStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            string pathDir          = Path.GetDirectoryName(path);
            string filename         = Path.GetFileNameWithoutExtension(path);
            string testPath         = Path.Combine(pathDir, filename);
            string resourceFilePath = null;

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
        public IEnumerable<Segment>      Segments                { get; }

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
            Header     = BigEndianMarshal.ByteArrayToStructureLittleEndian<Elf64_Ehdr>(buffer);
            Recognized = Header.ei_mag.SequenceEqual(ELFMAG);

            if(!Recognized) return;

            Type          = "Executable and Linkable Format (ELF)";
            IsBigEndian   = Header.ei_data == eiData.ELFDATA2MSB;
            architectures = new List<Architecture>();

            switch(Header.ei_class)
            {
                case eiClass.ELFCLASS32:
                    Header = UpBits(buffer, Header.ei_data == eiData.ELFDATA2MSB);
                    break;
                case eiClass.ELFCLASS64:
                    if(Header.ei_data == eiData.ELFDATA2MSB)
                    {
                        Header           = BigEndianMarshal.ByteArrayToStructureBigEndian<Elf64_Ehdr>(buffer);
                        Header.e_type    = (eType)Swapping.Swap((ushort)Header.e_type);
                        Header.e_machine = (eMachine)Swapping.Swap((ushort)Header.e_machine);
                        Header.e_version = (eVersion)Swapping.Swap((uint)Header.e_version);
                    }
                    else Header = BigEndianMarshal.ByteArrayToStructureLittleEndian<Elf64_Ehdr>(buffer);

                    break;
                default: return;
            }

            if(Header.ei_data != eiData.ELFDATA2LSB && Header.ei_data != eiData.ELFDATA2MSB ||
               Header.ei_version != eiVersion.EV_CURRENT) return;

            List<string> strings = new List<string>();

            BaseStream.Position = (long)Header.e_shoff;
            buffer              = new byte[Header.e_shentsize];
            sections            = new Elf64_Shdr[Header.e_shnum];
            for(int i = 0; i < sections.Length; i++)
            {
                BaseStream.Read(buffer, 0, buffer.Length);
                switch(Header.ei_class)
                {
                    case eiClass.ELFCLASS32:
                        sections[i] = UpBitsSection(buffer, Header.ei_data == eiData.ELFDATA2MSB);
                        break;
                    case eiClass.ELFCLASS64:
                        if(Header.ei_data == eiData.ELFDATA2MSB)
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

            BaseStream.Position = (long)sections[Header.e_shstrndx].sh_offset;
            buffer              = new byte[sections[Header.e_shstrndx].sh_size];
            BaseStream.Read(buffer, 0, buffer.Length);
            sectionNames = new string[sections.Length];
            Segment[] segments = new Segment[sections.Length];

            int len = 0;
            int pos;

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

            // Sections that contain an array of null-terminated strings by definition
            for(int i = 0; i < sections.Length; i++)
            {
                BaseStream.Position = (long)sections[i].sh_offset;
                buffer              = new byte[sections[i].sh_size];
                BaseStream.Read(buffer, 0, buffer.Length);

                if(sectionNames[i] == ".interp" || sectionNames[i] == ".dynstr" || sectionNames[i] == ".comment")
                {
                    strStart = 0;
                    len      = 0;
                    for(int p = 0; p < buffer.Length; p++)
                    {
                        if(buffer[p] == 0x00)
                        {
                            if(len == 0) continue;

                            strings.Add(Encoding.ASCII.GetString(buffer, strStart, len));
                            if(sectionNames[i] == ".interp")
                                interpreter = Encoding.ASCII.GetString(buffer, strStart, len);
                            strStart = p + 1;
                            len      = 0;
                            continue;
                        }

                        len++;
                    }
                }
                else if(sectionNames[i].StartsWith(".note"))
                {
                    uint namesz = BitConverter.ToUInt32(buffer, 0);
                    uint descsz = BitConverter.ToUInt32(buffer, 4);
                    uint notetype = BitConverter.ToUInt32(buffer, 8);
                    pos = 12;

                    if(IsBigEndian)
                    {
                        namesz= Swapping.Swap(namesz);
                        descsz = Swapping.Swap(descsz);
                        notetype = Swapping.Swap(notetype);
                    }
                    
                    ElfNote note = new ElfNote
                    {
                        name = Encoding.ASCII.GetString(buffer, pos, (int)(namesz - 1)),
                        type = notetype,
                        contents = new byte[descsz]
                    };

                    pos += (int)namesz;

                    switch(Header.ei_class) {
                        case eiClass.ELFCLASS32: pos += pos % 4;
                            break;
                        case eiClass.ELFCLASS64: pos += pos % 8;
                            break;
                    }
                    
                    Array.Copy(buffer, pos, note.contents, 0, descsz);
                    
                    notes.Add(sectionNames[i], note);
                }
            }

            if(strings.Count > 0)
            {
                strings.Remove(null);
                strings.Remove("");
                strings.Sort();
                Strings = strings.Distinct();
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
    }
}