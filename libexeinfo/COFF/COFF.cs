//
// COFF.cs
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
	///     Represents a Common Object File Format
	/// </summary>
	public partial class COFF
    {
	    /// <summary>
	    ///     The <see cref="FileStream" /> that contains the executable represented by this instance
	    /// </summary>
	    public readonly FileStream BaseStream;
	    /// <summary>
	    ///     Header for this executable
	    /// </summary>
	    public readonly COFFHeader Header;
        public readonly bool       IsBigEndian;
	    /// <summary>
	    ///     If true this instance correctly represents a Common Object File Format
	    /// </summary>
	    public readonly bool IsCOFF;

	    /// <summary>
	    ///     Initializes a new instance of the <see cref="T:libexeinfo.COFF" /> class.
	    /// </summary>
	    /// <param name="path">Executable path.</param>
	    public COFF(string path)
        {
            byte[] buffer = new byte[Marshal.SizeOf(typeof(COFFHeader))];

            BaseStream          = File.Open(path, FileMode.Open, FileAccess.Read);
            BaseStream.Position = 0;
            BaseStream.Read(buffer, 0, buffer.Length);
            IntPtr hdrPtr = Marshal.AllocHGlobal(buffer.Length);
            Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
            Header = (COFFHeader)Marshal.PtrToStructure(hdrPtr, typeof(COFFHeader));
            Marshal.FreeHGlobal(hdrPtr);
            IsCOFF = Header.optionalHeader.magic == STMAGIC || Header.optionalHeader.magic == OMAGIC ||
                     Header.optionalHeader.magic == JMAGIC  || Header.optionalHeader.magic == DMAGIC ||
                     Header.optionalHeader.magic == ZMAGIC  || Header.optionalHeader.magic == SHMAGIC;
            IsBigEndian = false;

            if(!IsCOFF)
            {
                Header = SwapHeader(Header);
                IsCOFF = Header.optionalHeader.magic == STMAGIC || Header.optionalHeader.magic == OMAGIC ||
                         Header.optionalHeader.magic == JMAGIC  || Header.optionalHeader.magic == DMAGIC ||
                         Header.optionalHeader.magic == ZMAGIC  || Header.optionalHeader.magic == SHMAGIC;
                IsBigEndian = !IsCOFF;
            }
        }

	    /// <summary>
	    ///     Initializes a new instance of the <see cref="T:libexeinfo.COFF" /> class.
	    /// </summary>
	    /// <param name="stream">Stream containing the executable.</param>
	    public COFF(FileStream stream)
        {
            byte[] buffer = new byte[Marshal.SizeOf(typeof(COFFHeader))];

            BaseStream          = stream;
            BaseStream.Position = 0;
            BaseStream.Read(buffer, 0, buffer.Length);
            IntPtr hdrPtr = Marshal.AllocHGlobal(buffer.Length);
            Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
            Header = (COFFHeader)Marshal.PtrToStructure(hdrPtr, typeof(COFFHeader));
            Marshal.FreeHGlobal(hdrPtr);
            IsCOFF = Header.optionalHeader.magic == STMAGIC || Header.optionalHeader.magic == OMAGIC ||
                     Header.optionalHeader.magic == JMAGIC  || Header.optionalHeader.magic == DMAGIC ||
                     Header.optionalHeader.magic == ZMAGIC  || Header.optionalHeader.magic == SHMAGIC;
            IsBigEndian = false;

            if(!IsCOFF)
            {
                Header = SwapHeader(Header);
                IsCOFF = Header.optionalHeader.magic == STMAGIC || Header.optionalHeader.magic == OMAGIC ||
                         Header.optionalHeader.magic == JMAGIC  || Header.optionalHeader.magic == DMAGIC ||
                         Header.optionalHeader.magic == ZMAGIC  || Header.optionalHeader.magic == SHMAGIC;
                IsBigEndian = !IsCOFF;
            }
        }

	    /// <summary>
	    ///     Identifies if the specified executable is a Common Object File Format
	    /// </summary>
	    /// <returns><c>true</c> if the specified executable is a Common Object File Format, <c>false</c> otherwise.</returns>
	    /// <param name="path">Executable path.</param>
	    public static bool Identify(string path)
        {
            FileStream exeFs = File.Open(path, FileMode.Open, FileAccess.Read);
            return Identify(exeFs);
        }

	    /// <summary>
	    ///     Identifies if the specified executable is a Common Object File Format
	    /// </summary>
	    /// <returns><c>true</c> if the specified executable is a Common Object File Format, <c>false</c> otherwise.</returns>
	    /// <param name="stream">Stream containing the executable.</param>
	    public static bool Identify(FileStream stream)
        {
            byte[] buffer = new byte[Marshal.SizeOf(typeof(COFFHeader))];

            stream.Position = 0;
            stream.Read(buffer, 0, buffer.Length);
            IntPtr hdrPtr = Marshal.AllocHGlobal(buffer.Length);
            Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
            COFFHeader COFFHdr = (COFFHeader)Marshal.PtrToStructure(hdrPtr, typeof(COFFHeader));
            Marshal.FreeHGlobal(hdrPtr);

            if(COFFHdr.optionalHeader.magic == STMAGIC || COFFHdr.optionalHeader.magic == OMAGIC ||
               COFFHdr.optionalHeader.magic == JMAGIC  || COFFHdr.optionalHeader.magic == DMAGIC ||
               COFFHdr.optionalHeader.magic == ZMAGIC  || COFFHdr.optionalHeader.magic == SHMAGIC) return true;

            COFFHdr = SwapHeader(COFFHdr);
            return COFFHdr.optionalHeader.magic == STMAGIC || COFFHdr.optionalHeader.magic == OMAGIC ||
                   COFFHdr.optionalHeader.magic == JMAGIC  || COFFHdr.optionalHeader.magic == DMAGIC ||
                   COFFHdr.optionalHeader.magic == ZMAGIC  || COFFHdr.optionalHeader.magic == SHMAGIC;
        }

        static COFFHeader SwapHeader(COFFHeader header)
        {
            COFFHeader swapped      = BigEndianMarshal.SwapStructureMembersEndian(header);
            swapped.characteristics = (Characteristics)Swapping.Swap((ushort)header.characteristics);
            swapped.machine         = (MachineTypes)Swapping.Swap((ushort)header.machine);
            swapped.optionalHeader  = BigEndianMarshal.SwapStructureMembersEndian(header.optionalHeader);
            return swapped;
        }
    }
}