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
	/// <summary>
	///     Represents a Microsoft/IBM Linear EXecutable
	/// </summary>
	// TODO: Big-endian (really needed?)
	public partial class LX
    {
        public readonly MZ BaseExecutable;
	    /// <summary>
	    ///     The <see cref="FileStream" /> that contains the executable represented by this instance
	    /// </summary>
	    public readonly FileStream BaseStream;
	    /// <summary>
	    ///     Header for this executable
	    /// </summary>
	    public readonly LXHeader Header;
	    /// <summary>
	    ///     If true this instance correctly represents a Microsoft/IBM Linear EXecutable
	    /// </summary>
	    public readonly bool IsLX;

	    /// <summary>
	    ///     Initializes a new instance of the <see cref="T:libexeinfo.NE" /> class.
	    /// </summary>
	    /// <param name="path">Executable path.</param>
	    public LX(string path)
        {
            IsLX           = false;
            BaseStream     = File.Open(path, FileMode.Open, FileAccess.Read);
            BaseExecutable = new MZ(BaseStream);
            if(BaseExecutable.IsMZ)
                if(BaseExecutable.Header.new_offset < BaseStream.Length)
                {
                    BaseStream.Seek(BaseExecutable.Header.new_offset, SeekOrigin.Begin);
                    byte[] buffer = new byte[Marshal.SizeOf(typeof(LXHeader))];
                    BaseStream.Read(buffer, 0, buffer.Length);
                    IntPtr hdrPtr = Marshal.AllocHGlobal(buffer.Length);
                    Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
                    Header = (LXHeader)Marshal.PtrToStructure(hdrPtr, typeof(LXHeader));
                    Marshal.FreeHGlobal(hdrPtr);
                    IsLX = Header.signature == Signature || Header.signature == Signature16;
                }
        }

	    /// <summary>
	    ///     Initializes a new instance of the <see cref="T:libexeinfo.NE" /> class.
	    /// </summary>
	    /// <param name="stream">Stream containing the executable.</param>
	    public LX(FileStream stream)
        {
            IsLX           = false;
            BaseStream     = stream;
            BaseExecutable = new MZ(BaseStream);
            if(BaseExecutable.IsMZ)
                if(BaseExecutable.Header.new_offset < BaseStream.Length)
                {
                    BaseStream.Seek(BaseExecutable.Header.new_offset, SeekOrigin.Begin);
                    byte[] buffer = new byte[Marshal.SizeOf(typeof(LXHeader))];
                    BaseStream.Read(buffer, 0, buffer.Length);
                    IntPtr hdrPtr = Marshal.AllocHGlobal(buffer.Length);
                    Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
                    Header = (LXHeader)Marshal.PtrToStructure(hdrPtr, typeof(LXHeader));
                    Marshal.FreeHGlobal(hdrPtr);
                    IsLX = Header.signature == Signature || Header.signature == Signature16;
                }
        }

	    /// <summary>
	    ///     Identifies if the specified executable is a Microsoft/IBM Linear EXecutable
	    /// </summary>
	    /// <returns><c>true</c> if the specified executable is a Microsoft/IBM Linear EXecutable, <c>false</c> otherwise.</returns>
	    /// <param name="path">Executable path.</param>
	    public static bool Identify(string path)
        {
            FileStream BaseStream     = File.Open(path, FileMode.Open, FileAccess.Read);
            MZ         BaseExecutable = new MZ(BaseStream);
            if(BaseExecutable.IsMZ)
                if(BaseExecutable.Header.new_offset < BaseStream.Length)
                {
                    BaseStream.Seek(BaseExecutable.Header.new_offset, SeekOrigin.Begin);
                    byte[] buffer = new byte[Marshal.SizeOf(typeof(LXHeader))];
                    BaseStream.Read(buffer, 0, buffer.Length);
                    IntPtr hdrPtr = Marshal.AllocHGlobal(buffer.Length);
                    Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
                    LXHeader Header = (LXHeader)Marshal.PtrToStructure(hdrPtr, typeof(LXHeader));
                    Marshal.FreeHGlobal(hdrPtr);
                    return Header.signature == Signature || Header.signature == Signature16;
                }

            return false;
        }

	    /// <summary>
	    ///     Identifies if the specified executable is a Microsoft/IBM Linear EXecutable
	    /// </summary>
	    /// <returns><c>true</c> if the specified executable is a Microsoft/IBM Linear EXecutable, <c>false</c> otherwise.</returns>
	    /// <param name="stream">Stream containing the executable.</param>
	    public static bool Identify(FileStream stream)
        {
            FileStream BaseStream     = stream;
            MZ         BaseExecutable = new MZ(BaseStream);
            if(BaseExecutable.IsMZ)
                if(BaseExecutable.Header.new_offset < BaseStream.Length)
                {
                    BaseStream.Seek(BaseExecutable.Header.new_offset, SeekOrigin.Begin);
                    byte[] buffer = new byte[Marshal.SizeOf(typeof(LXHeader))];
                    BaseStream.Read(buffer, 0, buffer.Length);
                    IntPtr hdrPtr = Marshal.AllocHGlobal(buffer.Length);
                    Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
                    LXHeader Header = (LXHeader)Marshal.PtrToStructure(hdrPtr, typeof(LXHeader));
                    Marshal.FreeHGlobal(hdrPtr);
                    return Header.signature == Signature || Header.signature == Signature16;
                }

            return false;
        }
    }
}