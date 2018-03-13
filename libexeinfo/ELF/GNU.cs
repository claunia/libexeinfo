using System;
using System.Text;

namespace libexeinfo
{
    public partial class ELF
    {
        static GnuAbiTag DecodeGnuAbiTag(ElfNote note, bool bigEndian)
        {
            if(note.name != "GNU") return null;

            return bigEndian
                       ? new GnuAbiTag
                       {
                           system   = (GnuAbiSystem)Swapping.Swap(BitConverter.ToUInt32(note.contents, 0)),
                           major    = Swapping.Swap(BitConverter.ToUInt32(note.contents, 4)),
                           minor    = Swapping.Swap(BitConverter.ToUInt32(note.contents, 8)),
                           revision = Swapping.Swap(BitConverter.ToUInt32(note.contents, 0))
                       }
                       : new GnuAbiTag
                       {
                           system   = (GnuAbiSystem)BitConverter.ToUInt32(note.contents, 0),
                           major    = BitConverter.ToUInt32(note.contents, 4),
                           minor    = BitConverter.ToUInt32(note.contents, 8),
                           revision = BitConverter.ToUInt32(note.contents, 0)
                       };
        }

        static string DecodeGnuBuildId(ElfNote note)
        {
            StringBuilder sb = new StringBuilder();
            foreach(byte b in note.contents) { sb.AppendFormat("{0:x2}", b); }

            return sb.ToString();
        }
    }
} 