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
	/// Represents a Microsoft New Executable
	/// </summary>
	public partial class NE
    {
		/// <summary>
		/// The <see cref="FileStream"/> that contains the executable represented by this instance
		/// </summary>
		public readonly FileStream BaseStream;
		/// <summary>
		/// Header for this executable
		/// </summary>
		public readonly NEHeader Header;
		/// <summary>
		/// If true this instance correctly represents a Microsoft New Executable
		/// </summary>
		public readonly bool IsNE;
		public readonly MZ BaseExecutable;
        public readonly ResourceTable Resources;
        public readonly Version[] Versions;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:libexeinfo.NE"/> class.
		/// </summary>
		/// <param name="path">Executable path.</param>
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

		/// <summary>
		/// Initializes a new instance of the <see cref="T:libexeinfo.NE"/> class.
		/// </summary>
		/// <param name="stream">Stream containing the executable.</param>
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

		/// <summary>
		/// Identifies if the specified executable is a Microsoft New Executable
		/// </summary>
		/// <returns><c>true</c> if the specified executable is a Microsoft New Executable, <c>false</c> otherwise.</returns>
		/// <param name="path">Executable path.</param>
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

		/// <summary>
		/// Identifies if the specified executable is a Microsoft New Executable
		/// </summary>
		/// <returns><c>true</c> if the specified executable is a Microsoft New Executable, <c>false</c> otherwise.</returns>
		/// <param name="stream">Stream containing the executable.</param>
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
