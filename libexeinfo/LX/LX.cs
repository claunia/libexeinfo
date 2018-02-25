﻿//
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
	public partial class LX : IExecutable
    {
        MZ BaseExecutable;
	    /// <summary>
	    ///     The <see cref="FileStream" /> that contains the executable represented by this instance
	    /// </summary>
	    public Stream BaseStream { get; }
	    public bool IsBigEndian => false;
	    /// <summary>
	    ///     Header for this executable
	    /// </summary>
	    LXHeader Header;
	    /// <summary>
	    ///     If true this instance correctly represents a Microsoft/IBM Linear EXecutable
	    /// </summary>
	    public bool Recognized { get; private set;}
	    public string Type { get; private set;}

	    /// <summary>
	    ///     Initializes a new instance of the <see cref="T:libexeinfo.NE" /> class.
	    /// </summary>
	    /// <param name="path">Executable path.</param>
	    public LX(string path)
        {
            BaseStream     = File.Open(path, FileMode.Open, FileAccess.Read);
	        Initialize();
        }

	    /// <summary>
	    ///     Initializes a new instance of the <see cref="T:libexeinfo.NE" /> class.
	    /// </summary>
	    /// <param name="stream">Stream containing the executable.</param>
	    public LX(Stream stream)
        {
            BaseStream     = stream;
	        Initialize();
        }

	    /// <summary>
	    ///     Initializes a new instance of the <see cref="T:libexeinfo.NE" /> class.
	    /// </summary>
	    /// <param name="data">Byte array containing the executable.</param>
	    public LX(byte[] data)
	    {
		    BaseStream = new MemoryStream(data);
		    Initialize();
	    }

	    void Initialize()
	    {
		    Recognized = false;
		    if(BaseStream == null) return;
		    BaseExecutable = new MZ(BaseStream);
		    if(!BaseExecutable.Recognized) return;

		    if(BaseExecutable.Header.new_offset >= BaseStream.Length) return;

		    BaseStream.Seek(BaseExecutable.Header.new_offset, SeekOrigin.Begin);
		    byte[] buffer = new byte[Marshal.SizeOf(typeof(LXHeader))];
		    BaseStream.Read(buffer, 0, buffer.Length);
		    IntPtr hdrPtr = Marshal.AllocHGlobal(buffer.Length);
		    Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
		    Header = (LXHeader)Marshal.PtrToStructure(hdrPtr, typeof(LXHeader));
		    Marshal.FreeHGlobal(hdrPtr);
		    Recognized = Header.signature == Signature || Header.signature == Signature16;

		    if(!Recognized) return;
		    
		    Type = Header.signature == Signature16 ? "Linear Executable (LE)" : "Linear eXecutable (LX)";
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
	        if(!BaseExecutable.Recognized) return false;

	        if(BaseExecutable.Header.new_offset >= BaseStream.Length) return false;

	        BaseStream.Seek(BaseExecutable.Header.new_offset, SeekOrigin.Begin);
	        byte[] buffer = new byte[Marshal.SizeOf(typeof(LXHeader))];
	        BaseStream.Read(buffer, 0, buffer.Length);
	        IntPtr hdrPtr = Marshal.AllocHGlobal(buffer.Length);
	        Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
	        LXHeader Header = (LXHeader)Marshal.PtrToStructure(hdrPtr, typeof(LXHeader));
	        Marshal.FreeHGlobal(hdrPtr);
	        return Header.signature == Signature || Header.signature == Signature16;

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
	        if(!BaseExecutable.Recognized) return false;

	        if(BaseExecutable.Header.new_offset >= BaseStream.Length) return false;

	        BaseStream.Seek(BaseExecutable.Header.new_offset, SeekOrigin.Begin);
	        byte[] buffer = new byte[Marshal.SizeOf(typeof(LXHeader))];
	        BaseStream.Read(buffer, 0, buffer.Length);
	        IntPtr hdrPtr = Marshal.AllocHGlobal(buffer.Length);
	        Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
	        LXHeader Header = (LXHeader)Marshal.PtrToStructure(hdrPtr, typeof(LXHeader));
	        Marshal.FreeHGlobal(hdrPtr);
	        return Header.signature == Signature || Header.signature == Signature16;

        }
    }
}