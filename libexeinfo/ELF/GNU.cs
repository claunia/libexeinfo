//
// GNU.cs
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
            foreach(byte b in note.contents) sb.AppendFormat("{0:x2}", b);

            return sb.ToString();
        }
    }
}