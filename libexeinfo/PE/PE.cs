//
// PE.cs
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
// FITPESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONPECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace libexeinfo
{
	/// <summary>
	///     Represents a Microsoft Portable Executable
	/// </summary>
	public partial class PE
    {
        public readonly MZ BaseExecutable;
	    /// <summary>
	    ///     The <see cref="FileStream" /> that contains the executable represented by this instance
	    /// </summary>
	    public readonly FileStream BaseStream;
	    /// <summary>
	    ///     Header for this executable
	    /// </summary>
	    public readonly PEHeader Header;
	    /// <summary>
	    ///     If true this instance correctly represents a Microsoft Portable Executable
	    /// </summary>
	    public readonly bool            IsPE;
        public readonly WindowsHeader64 WinHeader;

	    /// <summary>
	    ///     Initializes a new instance of the <see cref="T:libexeinfo.PE" /> class.
	    /// </summary>
	    /// <param name="path">Executable path.</param>
	    public PE(string path)
        {
            IsPE           = false;
            BaseStream     = File.Open(path, FileMode.Open, FileAccess.Read);
            BaseExecutable = new MZ(BaseStream);
            if(BaseExecutable.IsMZ)
                if(BaseExecutable.Header.new_offset < BaseStream.Length)
                {
                    BaseStream.Seek(BaseExecutable.Header.new_offset, SeekOrigin.Begin);
                    byte[] buffer = new byte[Marshal.SizeOf(typeof(PEHeader))];
                    BaseStream.Read(buffer, 0, buffer.Length);
                    IntPtr hdrPtr = Marshal.AllocHGlobal(buffer.Length);
                    Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
                    Header = (PEHeader)Marshal.PtrToStructure(hdrPtr, typeof(PEHeader));
                    Marshal.FreeHGlobal(hdrPtr);
                    IsPE = Header.signature == Signature;

                    if(IsPE)
                        if(Header.coff.optionalHeader.magic == PE32Plus)
                        {
                            BaseStream.Position -= 4;
                            buffer              =  new byte[Marshal.SizeOf(typeof(WindowsHeader64))];
                            BaseStream.Read(buffer, 0, buffer.Length);
                            hdrPtr = Marshal.AllocHGlobal(buffer.Length);
                            Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
                            WinHeader = (WindowsHeader64)Marshal.PtrToStructure(hdrPtr, typeof(WindowsHeader64));
                            Marshal.FreeHGlobal(hdrPtr);
                        }
                        else
                        {
                            buffer = new byte[Marshal.SizeOf(typeof(WindowsHeader))];
                            BaseStream.Read(buffer, 0, buffer.Length);
                            hdrPtr = Marshal.AllocHGlobal(buffer.Length);
                            Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
                            WindowsHeader hdr32 = (WindowsHeader)Marshal.PtrToStructure(hdrPtr, typeof(WindowsHeader));
                            Marshal.FreeHGlobal(hdrPtr);
                            WinHeader = ToPlus(hdr32);
                        }
                }
        }

	    /// <summary>
	    ///     Initializes a new instance of the <see cref="T:libexeinfo.PE" /> class.
	    /// </summary>
	    /// <param name="stream">Stream containing the executable.</param>
	    public PE(FileStream stream)
        {
            IsPE           = false;
            BaseStream     = stream;
            BaseExecutable = new MZ(BaseStream);
            if(BaseExecutable.IsMZ)
                if(BaseExecutable.Header.new_offset < BaseStream.Length)
                {
                    BaseStream.Seek(BaseExecutable.Header.new_offset, SeekOrigin.Begin);
                    byte[] buffer = new byte[Marshal.SizeOf(typeof(PEHeader))];
                    BaseStream.Read(buffer, 0, buffer.Length);
                    IntPtr hdrPtr = Marshal.AllocHGlobal(buffer.Length);
                    Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
                    Header = (PEHeader)Marshal.PtrToStructure(hdrPtr, typeof(PEHeader));
                    Marshal.FreeHGlobal(hdrPtr);
                    IsPE = Header.signature == Signature;

                    if(IsPE)
                        if(Header.coff.optionalHeader.magic == PE32Plus)
                        {
                            BaseStream.Position -= 4;
                            buffer              =  new byte[Marshal.SizeOf(typeof(WindowsHeader64))];
                            BaseStream.Read(buffer, 0, buffer.Length);
                            hdrPtr = Marshal.AllocHGlobal(buffer.Length);
                            Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
                            WinHeader = (WindowsHeader64)Marshal.PtrToStructure(hdrPtr, typeof(WindowsHeader64));
                            Marshal.FreeHGlobal(hdrPtr);
                        }
                        else
                        {
                            buffer = new byte[Marshal.SizeOf(typeof(WindowsHeader))];
                            BaseStream.Read(buffer, 0, buffer.Length);
                            hdrPtr = Marshal.AllocHGlobal(buffer.Length);
                            Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
                            WindowsHeader hdr32 = (WindowsHeader)Marshal.PtrToStructure(hdrPtr, typeof(WindowsHeader));
                            Marshal.FreeHGlobal(hdrPtr);
                            WinHeader = ToPlus(hdr32);
                        }
                }
        }

	    /// <summary>
	    ///     Identifies if the specified executable is a Microsoft Portable Executable
	    /// </summary>
	    /// <returns><c>true</c> if the specified executable is a Microsoft Portable Executable, <c>false</c> otherwise.</returns>
	    /// <param name="path">Executable path.</param>
	    public static bool Identify(string path)
        {
            FileStream BaseStream     = File.Open(path, FileMode.Open, FileAccess.Read);
            MZ         BaseExecutable = new MZ(BaseStream);
            if(BaseExecutable.IsMZ)
                if(BaseExecutable.Header.new_offset < BaseStream.Length)
                {
                    BaseStream.Seek(BaseExecutable.Header.new_offset, SeekOrigin.Begin);
                    byte[] buffer = new byte[Marshal.SizeOf(typeof(PEHeader))];
                    BaseStream.Read(buffer, 0, buffer.Length);
                    IntPtr hdrPtr = Marshal.AllocHGlobal(buffer.Length);
                    Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
                    PEHeader Header = (PEHeader)Marshal.PtrToStructure(hdrPtr, typeof(PEHeader));
                    Marshal.FreeHGlobal(hdrPtr);
                    return Header.signature == Signature;
                }

            return false;
        }

	    /// <summary>
	    ///     Identifies if the specified executable is a Microsoft Portable Executable
	    /// </summary>
	    /// <returns><c>true</c> if the specified executable is a Microsoft Portable Executable, <c>false</c> otherwise.</returns>
	    /// <param name="stream">Stream containing the executable.</param>
	    public static bool Identify(FileStream stream)
        {
            FileStream BaseStream     = stream;
            MZ         BaseExecutable = new MZ(BaseStream);
            if(BaseExecutable.IsMZ)
                if(BaseExecutable.Header.new_offset < BaseStream.Length)
                {
                    BaseStream.Seek(BaseExecutable.Header.new_offset, SeekOrigin.Begin);
                    byte[] buffer = new byte[Marshal.SizeOf(typeof(PEHeader))];
                    BaseStream.Read(buffer, 0, buffer.Length);
                    IntPtr hdrPtr = Marshal.AllocHGlobal(buffer.Length);
                    Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
                    PEHeader Header = (PEHeader)Marshal.PtrToStructure(hdrPtr, typeof(PEHeader));
                    Marshal.FreeHGlobal(hdrPtr);
                    return Header.signature == Signature;
                }

            return false;
        }

        static WindowsHeader64 ToPlus(WindowsHeader header)
        {
            return new WindowsHeader64
            {
                imageBase                   = header.imageBase,
                sectionAlignment            = header.sectionAlignment,
                fileAlignment               = header.fileAlignment,
                majorOperatingSystemVersion = header.majorOperatingSystemVersion,
                minorOperatingSystemVersion = header.minorOperatingSystemVersion,
                majorImageVersion           = header.majorImageVersion,
                minorImageVersion           = header.minorImageVersion,
                majorSubsystemVersion       = header.majorSubsystemVersion,
                minorSubsystemVersion       = header.minorSubsystemVersion,
                win32VersionValue           = header.win32VersionValue,
                sizeOfImage                 = header.sizeOfImage,
                sizeOfHeaders               = header.sizeOfHeaders,
                checksum                    = header.checksum,
                subsystem                   = header.subsystem,
                dllCharacteristics          = header.dllCharacteristics,
                sizeOfStackReserve          = header.sizeOfStackReserve,
                sizeOfStackCommit           = header.sizeOfStackCommit,
                sizeOfHeapReserve           = header.sizeOfHeapReserve,
                sizeOfHeapCommit            = header.sizeOfHeapCommit,
                loaderFlags                 = header.loaderFlags,
                numberOfRvaAndSizes         = header.numberOfRvaAndSizes
            };
        }
    }
}