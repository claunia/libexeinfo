//
// MZ.cs
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
    public partial class MZ
    {
        public readonly FileStream BaseStream;
        public readonly MZHeader Header;
        public readonly bool IsMZ;

        public MZ(string path)
        {
			byte[] buffer = new byte[Marshal.SizeOf(typeof(MZHeader))];

			BaseStream = File.Open(path, FileMode.Open, FileAccess.Read);
			BaseStream.Position = 0;
			BaseStream.Read(buffer, 0, buffer.Length);
			IntPtr hdrPtr = Marshal.AllocHGlobal(buffer.Length);
			Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
			Header = (MZHeader)Marshal.PtrToStructure(hdrPtr, typeof(MZHeader));
			Marshal.FreeHGlobal(hdrPtr);
            IsMZ = Header.signature == Signature;
		}
		
        public MZ(FileStream stream)
		{
			byte[] buffer = new byte[Marshal.SizeOf(typeof(MZHeader))];

            BaseStream = stream;
			BaseStream.Position = 0;
			BaseStream.Read(buffer, 0, buffer.Length);
			IntPtr hdrPtr = Marshal.AllocHGlobal(buffer.Length);
			Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
			Header = (MZHeader)Marshal.PtrToStructure(hdrPtr, typeof(MZHeader));
			Marshal.FreeHGlobal(hdrPtr);
			IsMZ = Header.signature == Signature;
		}

        public bool Identify()
        {
            return IsMZ;
        }

        public static bool Identify(string path)
        {
			FileStream exeFs = File.Open(path, FileMode.Open, FileAccess.Read);
            return Identify(exeFs);
		}

        public static bool Identify(FileStream stream)
		{
			byte[] buffer = new byte[Marshal.SizeOf(typeof(MZHeader))];

            stream.Position = 0;
			stream.Read(buffer, 0, buffer.Length);
			IntPtr hdrPtr = Marshal.AllocHGlobal(buffer.Length);
			Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
			MZHeader mzHdr = (MZHeader)Marshal.PtrToStructure(hdrPtr, typeof(MZHeader));
			Marshal.FreeHGlobal(hdrPtr);

            return mzHdr.signature == Signature;
		}
	}
}
