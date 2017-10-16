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

using System;
namespace libexeinfo
{
	public partial class MZ
	{
		public static void PrintInfo(Header header)
		{
			Console.WriteLine("DOS MZ executable:");
			Console.WriteLine("\tBlocks in file: {0}", header.blocks_in_file);
			Console.WriteLine("\t{0} bytes used in last block", header.bytes_in_last_block == 0 ? 512 : header.bytes_in_last_block);
			Console.WriteLine("\t{0} relocations present after the header", header.num_relocs);
			Console.WriteLine("\t{0} paragraphs in header", header.header_paragraphs);
			Console.WriteLine("\t{0} paragraphs of additional memory required", header.min_extra_paragraphs);
			Console.WriteLine("\t{0} paragraphs of additional memory requested", header.max_extra_paragraphs);
			Console.WriteLine("\tSegment address for SS: {0:X4}h", header.ss);
			Console.WriteLine("\tInitial value of SP: {0:X4}h", header.sp);
			Console.WriteLine("\tInitial value of IP: {0:X4}h", header.ip);
			Console.WriteLine("\tInitial value of CS: {0:X4}h", header.cs);
			Console.WriteLine("\tOffset to relocation table: {0}", header.reloc_table_offset);
			Console.WriteLine("\tFile contains {0} overlays", header.overlay_number);
			Console.WriteLine("\tFile checksum: 0x{0:X4}", header.checksum);
			Console.WriteLine("\tOEM ID: {0}", header.oem_id);
			Console.WriteLine("\tOEM information: 0x{0:X4}", header.oem_info);
			Console.WriteLine("\tOffset to new header: {0}", header.new_offset);
		}
	}
}
