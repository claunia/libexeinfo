//
// Info.cs
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

using System.Text;

namespace libexeinfo
{
    public partial class MZ
    {
        /// <summary>
        ///     Gets a string with human readable information for the MZ executable represented by this instance
        /// </summary>
        /// <value>Human readable information for this instance.</value>
        public string Information => GetInfo(Header);

        /// <summary>
        ///     Gets a string with human readable information for a given MZ header
        /// </summary>
        /// <returns>Human readable information for given MZ header.</returns>
        /// <param name="header">MZ executable header.</param>
        static string GetInfo(MZHeader header)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("DOS MZ executable:");
            sb.AppendFormat("\tBlocks in file: {0}", header.blocks_in_file).AppendLine();
            sb.AppendFormat("\t{0} bytes used in last block",
                            header.bytes_in_last_block == 0 ? 512 : header.bytes_in_last_block).AppendLine();
            sb.AppendFormat("\t{0} relocations present after the header",     header.num_relocs).AppendLine();
            sb.AppendFormat("\t{0} paragraphs in header",                     header.header_paragraphs).AppendLine();
            sb.AppendFormat("\t{0} paragraphs of additional memory required", header.min_extra_paragraphs)
              .AppendLine();
            sb.AppendFormat("\t{0} paragraphs of additional memory requested", header.max_extra_paragraphs)
              .AppendLine();
            sb.AppendFormat("\tSegment address for SS: {0:X4}h", header.ss).AppendLine();
            sb.AppendFormat("\tInitial value of SP: {0:X4}h",    header.sp).AppendLine();
            sb.AppendFormat("\tInitial value of IP: {0:X4}h",    header.ip).AppendLine();
            sb.AppendFormat("\tInitial value of CS: {0:X4}h",    header.cs).AppendLine();
            sb.AppendFormat("\tOffset to relocation table: {0}", header.reloc_table_offset).AppendLine();
            sb.AppendFormat("\tFile contains {0} overlays",      header.overlay_number).AppendLine();
            sb.AppendFormat("\tFile checksum: 0x{0:X4}",         header.checksum).AppendLine();
            sb.AppendFormat("\tOEM ID: {0}",                     header.oem_id).AppendLine();
            sb.AppendFormat("\tOEM information: 0x{0:X4}",       header.oem_info).AppendLine();
            sb.AppendFormat("\tOffset to new header: {0}",       header.new_offset).AppendLine();
            return sb.ToString();
        }
    }
}