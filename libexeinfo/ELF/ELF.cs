using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace libexeinfo
{
    public partial class ELF : IExecutable
    {
        List<Architecture> architectures;
        Elf64_Ehdr         Header;

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
        public OperatingSystem           RequiredOperatingSystem { get; }
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
                    Header           = BigEndianMarshal.ByteArrayToStructureBigEndian<Elf64_Ehdr>(buffer);
                    Header.e_type    = (eType)Swapping.Swap((ushort)Header.e_type);
                    Header.e_machine = (eMachine)Swapping.Swap((ushort)Header.e_machine);
                    Header.e_version = (eVersion)Swapping.Swap((uint)Header.e_version);
                    break;
                default: return;
            }

            if(Header.ei_data != eiData.ELFDATA2LSB && Header.ei_data != eiData.ELFDATA2MSB ||
               Header.ei_version != eiVersion.EV_CURRENT) return;

            List<string> strings = new List<string>();

            if(strings.Count > 0)
            {
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