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
    public partial class AtariST : IExecutable
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:libexeinfo.AtariST" /> class.
        /// </summary>
        /// <param name="path">Executable path.</param>
        public AtariST(string path)
        {
            BaseStream = File.Open(path, FileMode.Open, FileAccess.Read);
            Initialize();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:libexeinfo.AtariST" /> class.
        /// </summary>
        /// <param name="stream">Stream containing the executable.</param>
        public AtariST(Stream stream)
        {
            BaseStream = stream;
            Initialize();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:libexeinfo.AtariST" /> class.
        /// </summary>
        /// <param name="data">Byte array containing the executable.</param>
        public AtariST(byte[] data)
        {
            BaseStream = new MemoryStream(data);
            Initialize();
        }

        /// <summary>
        ///     Header for this executable
        /// </summary>
        public AtariHeader Header      { get; private set; }
        public Stream      BaseStream  { get; }
        public bool        IsBigEndian => true;
        public bool        Recognized  { get; private set; }
        public string      Type        { get; private set; }

        void Initialize()
        {
            Recognized = false;

            if(BaseStream == null) return;

            byte[] buffer       = new byte[Marshal.SizeOf(typeof(AtariHeader))];
            BaseStream.Position = 0;
            BaseStream.Read(buffer, 0, buffer.Length);
            Header     = BigEndianMarshal.ByteArrayToStructureBigEndian<AtariHeader>(buffer);
            Recognized = Header.signature == SIGNATURE;

            if(Recognized) Type = "Atari ST executable";
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
        public static bool Identify(Stream stream)
        {
            byte[] buffer = new byte[Marshal.SizeOf(typeof(AtariHeader))];

            stream.Position = 0;
            stream.Read(buffer, 0, buffer.Length);
            AtariHeader hdr = BigEndianMarshal.ByteArrayToStructureBigEndian<AtariHeader>(buffer);

            return hdr.signature == SIGNATURE;
        }
    }
}