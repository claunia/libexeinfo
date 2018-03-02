//
// Info.cs
//
// Author:
//       Natalia Portillo <claunia@claunia.com>
//
// Copyright (c) 2017 Copyright © Claunia.com
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

using System.Text;
using Encoding = Claunia.Encoding.Encoding;

namespace libexeinfo
{
    public partial class AtariST
    {
        /// <summary>
        ///     Gets a string with human readable information for the Atari ST executable represented by this instance
        /// </summary>
        /// <value>Human readable information for this instance.</value>
        public string Information => GetInfo(Header, symbols);

        /// <summary>
        ///     Gets a string with human readable information for a given Atari ST header
        /// </summary>
        /// <returns>Human readable information for given Atari ST header.</returns>
        /// <param name="header">Atari ST executable header.</param>
        static string GetInfo(AtariHeader header, SymbolEntry[] symbols)
        {
            PrgFlags   flags   = (PrgFlags)(header.flags    & PF_FLAGS_MASK);
            PrgSharing sharing = (PrgSharing)((header.flags & PF_SHARE_MASK) >> 4);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Atari ST executable:");
            if(header.mint == MINT_SIGNATURE) sb.AppendLine("\tMiNT executable.");
            sb.AppendFormat("\t{0} bytes in text segment", header.text_len).AppendLine();
            sb.AppendFormat("\t{0} bytes in data segment", header.data_len).AppendLine();
            sb.AppendFormat("\t{0} bytes in BSS segment",  header.bss_len).AppendLine();
            sb.AppendFormat("\t{0} bytes in symbol table", header.symb_len).AppendLine();
            sb.AppendFormat("\tFlags: {0}",                flags).AppendLine();
            sb.AppendFormat("\tProcess sharing: {0}",      sharing).AppendLine();
            sb.AppendFormat("\t{0} fixups",                header.absflags == 0 ? "Has" : "Doesn't have").AppendLine();
            if(symbols                                                     == null || symbols.Length <= 0)
                return sb.ToString();

            sb.AppendLine("\tSymbol table:");
            for(int i = 0; i < symbols.Length; i++)
                sb.AppendFormat("\t\tSymbol {0}: \"{1}\", type {2}, value {3}", i,
                                StringHandlers.CToString(symbols[i].name, Encoding.AtariSTEncoding), symbols[i].type,
                                symbols[i].value).AppendLine();

            return sb.ToString();
        }
    }
}