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

namespace libexeinfo
{
    public partial class AtariST
	{
		/// <summary>
		/// Gets a string with human readable information for a given Atari ST header
		/// </summary>
		/// <returns>Human readable information for given Atari ST header.</returns>
		/// <param name="header">Atari ST executable header.</param>
		public static string GetInfo(AtariHeader header)
		{
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Atari ST executable:");
            sb.AppendFormat("\t{0} bytes in text segment", header.text_len).AppendLine();
            sb.AppendFormat("\t{0} bytes in data segment", header.data_len).AppendLine();
            sb.AppendFormat("\t{0} bytes in BSS segment", header.bss_len).AppendLine();
            sb.AppendFormat("\t{0} bytes in symbol table", header.symb_len).AppendLine();
			return sb.ToString();
		}

		/// <summary>
		/// Gets a string with human readable information for the Atari ST executable represented by this instance
		/// </summary>
		/// <returns>Human readable information for this instance.</returns>
		public string GetInfo()
        {
            return GetInfo(Header);
        }
	}
}
