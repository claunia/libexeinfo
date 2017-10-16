//
// NE.cs
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
using System.IO;
using System.Runtime.InteropServices;

namespace libexeinfo
{
    public partial class NE
    {
		public readonly FileStream BaseStream;
		public readonly NEHeader Header;
		public readonly bool IsNE;
		public readonly MZ BaseExecutable;
        public readonly ResourceTable Resources;
        public readonly Version[] Versions;

		public NE(string path)
		{
            IsNE = false;
			BaseStream = File.Open(path, FileMode.Open, FileAccess.Read);
            BaseExecutable = new MZ(BaseStream);
            if(BaseExecutable.IsMZ)
            {
				if(BaseExecutable.Header.new_offset < BaseStream.Length)
                {
					BaseStream.Seek(BaseExecutable.Header.new_offset, SeekOrigin.Begin);
					byte[] buffer = new byte[Marshal.SizeOf(typeof(NEHeader))];
					BaseStream.Read(buffer, 0, buffer.Length);
					IntPtr hdrPtr = Marshal.AllocHGlobal(buffer.Length);
					Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
                    Header = (NEHeader)Marshal.PtrToStructure(hdrPtr, typeof(NEHeader));
					Marshal.FreeHGlobal(hdrPtr);
                    IsNE = Header.signature == Signature;
                    Resources = GetResources(BaseStream, BaseExecutable.Header.new_offset, Header.resource_table_offset);
                    Versions = GetVersions().ToArray();
				}
			}
		}

		public NE(FileStream stream)
		{
			IsNE = false;
            BaseStream = stream;
			BaseExecutable = new MZ(BaseStream);
			if (BaseExecutable.IsMZ)
			{
				if (BaseExecutable.Header.new_offset < BaseStream.Length)
				{
					BaseStream.Seek(BaseExecutable.Header.new_offset, SeekOrigin.Begin);
					byte[] buffer = new byte[Marshal.SizeOf(typeof(NEHeader))];
					BaseStream.Read(buffer, 0, buffer.Length);
					IntPtr hdrPtr = Marshal.AllocHGlobal(buffer.Length);
					Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
					Header = (NEHeader)Marshal.PtrToStructure(hdrPtr, typeof(NEHeader));
					Marshal.FreeHGlobal(hdrPtr);
					IsNE = Header.signature == Signature;
					Resources = GetResources(BaseStream, BaseExecutable.Header.new_offset, Header.resource_table_offset);
					Versions = GetVersions().ToArray();
				}
			}
		}

		public bool Identify()
		{
			return IsNE;
		}

        public static bool Identify(string path)
		{
			FileStream BaseStream = File.Open(path, FileMode.Open, FileAccess.Read);
			MZ BaseExecutable = new MZ(BaseStream);
			if (BaseExecutable.IsMZ)
			{
				if (BaseExecutable.Header.new_offset < BaseStream.Length)
				{
					BaseStream.Seek(BaseExecutable.Header.new_offset, SeekOrigin.Begin);
					byte[] buffer = new byte[Marshal.SizeOf(typeof(NEHeader))];
					BaseStream.Read(buffer, 0, buffer.Length);
					IntPtr hdrPtr = Marshal.AllocHGlobal(buffer.Length);
					Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
					NEHeader Header = (NEHeader)Marshal.PtrToStructure(hdrPtr, typeof(NEHeader));
					Marshal.FreeHGlobal(hdrPtr);
					return Header.signature == Signature;
				}
			}

            return false;
		}
		
        public static bool Identify(FileStream stream)
		{
            FileStream BaseStream = stream;
			MZ BaseExecutable = new MZ(BaseStream);
			if (BaseExecutable.IsMZ)
			{
				if (BaseExecutable.Header.new_offset < BaseStream.Length)
				{
					BaseStream.Seek(BaseExecutable.Header.new_offset, SeekOrigin.Begin);
					byte[] buffer = new byte[Marshal.SizeOf(typeof(NEHeader))];
					BaseStream.Read(buffer, 0, buffer.Length);
					IntPtr hdrPtr = Marshal.AllocHGlobal(buffer.Length);
					Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
					NEHeader Header = (NEHeader)Marshal.PtrToStructure(hdrPtr, typeof(NEHeader));
					Marshal.FreeHGlobal(hdrPtr);
					return Header.signature == Signature;
				}
			}

			return false;
		}
	}
}
