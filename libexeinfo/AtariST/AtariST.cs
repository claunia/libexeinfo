//
// AtariST.cs
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

using System.IO;
using System.Runtime.InteropServices;

namespace libexeinfo
{
	/// <summary>
	///     Represents an Atari ST executable
	/// </summary>
	public partial class AtariST
    {
	    /// <summary>
	    ///     The <see cref="FileStream" /> that contains the executable represented by this instance
	    /// </summary>
	    public readonly FileStream BaseStream;
	    /// <summary>
	    ///     Header for this executable
	    /// </summary>
	    public readonly AtariHeader Header;
	    /// <summary>
	    ///     If true this instance correctly represents an Atari ST executable
	    /// </summary>
	    public readonly bool IsAtariST;

	    /// <summary>
	    ///     Initializes a new instance of the <see cref="T:libexeinfo.AtariST" /> class.
	    /// </summary>
	    /// <param name="path">Executable path.</param>
	    public AtariST(string path)
        {
            byte[] buffer = new byte[Marshal.SizeOf(typeof(AtariHeader))];

            BaseStream          = File.Open(path, FileMode.Open, FileAccess.Read);
            BaseStream.Position = 0;
            BaseStream.Read(buffer, 0, buffer.Length);
            Header    = BigEndianMarshal.ByteArrayToStructureBigEndian<AtariHeader>(buffer);
            IsAtariST = Header.signature == SIGNATURE;
        }

	    /// <summary>
	    ///     Initializes a new instance of the <see cref="T:libexeinfo.AtariST" /> class.
	    /// </summary>
	    /// <param name="stream">Stream containing the executable.</param>
	    public AtariST(FileStream stream)
        {
            byte[] buffer = new byte[Marshal.SizeOf(typeof(AtariHeader))];

            BaseStream          = stream;
            BaseStream.Position = 0;
            BaseStream.Read(buffer, 0, buffer.Length);
            Header    = BigEndianMarshal.ByteArrayToStructureBigEndian<AtariHeader>(buffer);
            IsAtariST = Header.signature == SIGNATURE;
        }

	    /// <summary>
	    ///     Identifies if the specified executable is a Atari ST executable
	    /// </summary>
	    /// <returns><c>true</c> if the specified executable is a Atari ST executable, <c>false</c> otherwise.</returns>
	    /// <param name="path">Executable path.</param>
	    public static bool Identify(string path)
        {
            FileStream exeFs = File.Open(path, FileMode.Open, FileAccess.Read);
            return Identify(exeFs);
        }

	    /// <summary>
	    ///     Identifies if the specified executable is a Atari ST executable
	    /// </summary>
	    /// <returns><c>true</c> if the specified executable is a Atari ST executable, <c>false</c> otherwise.</returns>
	    /// <param name="stream">Stream containing the executable.</param>
	    public static bool Identify(FileStream stream)
        {
            byte[] buffer = new byte[Marshal.SizeOf(typeof(AtariHeader))];

            stream.Position = 0;
            stream.Read(buffer, 0, buffer.Length);
            AtariHeader hdr = BigEndianMarshal.ByteArrayToStructureBigEndian<AtariHeader>(buffer);

            return hdr.signature == SIGNATURE;
        }
    }
}