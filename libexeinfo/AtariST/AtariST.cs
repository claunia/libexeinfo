﻿//
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

using System;
using System.Collections.Generic;
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
            BaseStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            string pathDir = Path.GetDirectoryName(path);
            string filename = Path.GetFileNameWithoutExtension(path);
            string testPath = Path.Combine(pathDir, filename);
            string resourceFilePath = null;

            if(File.Exists(testPath         + ".rsc"))
                resourceFilePath = testPath + ".rsc";
            else if(File.Exists(testPath         + ".rsC"))
                resourceFilePath = testPath + ".rsC";
            else if(File.Exists(testPath         + ".rSc"))
                resourceFilePath = testPath + ".rSc";
            else if(File.Exists(testPath         + ".rSC"))
                resourceFilePath = testPath + ".rSC";
            else if(File.Exists(testPath         + ".Rsc"))
                resourceFilePath = testPath + ".Rsc";
            else if(File.Exists(testPath         + ".RsC"))
                resourceFilePath = testPath + ".RsC";
            else if(File.Exists(testPath         + ".RSc"))
                resourceFilePath = testPath + ".RSc";
            else if(File.Exists(testPath         + ".RSC"))
                resourceFilePath = testPath + ".RSC";

            if(resourceFilePath != null) resourceStream = File.Open(resourceFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            
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
        public AtariHeader               Header                  { get; private set; }
        public Stream                    BaseStream              { get; }
        public bool                      IsBigEndian             => true;
        public bool                      Recognized              { get; private set; }
        public string                    Type                    { get; private set; }
        public IEnumerable<Architecture> Architectures           => new[] {Architecture.M68K};
        public OperatingSystem           RequiredOperatingSystem =>
            new OperatingSystem {Name = Header.mint == MINT_SIGNATURE ? "MiNT" : "Atari TOS"};
        public IEnumerable<string> Strings { get; }
        public Stream resourceStream;
        public AtariResourceHeader ResourceHeader;
        public TreeObjectNode ResourceObjectRoot;

        void Initialize()
        {
            Recognized = false;

            if(BaseStream == null) return;

            byte[] buffer       = new byte[Marshal.SizeOf(typeof(AtariHeader))];
            BaseStream.Position = 0;
            BaseStream.Read(buffer, 0, buffer.Length);
            Header     = BigEndianMarshal.ByteArrayToStructureBigEndian<AtariHeader>(buffer);
            Recognized = Header.signature == SIGNATURE;

            if(!Recognized) return;
            
            Type = "Atari ST executable";

            if(resourceStream == null) return;
            
            buffer       = new byte[Marshal.SizeOf(typeof(AtariResourceHeader))];
            resourceStream.Position = 0;
            resourceStream.Read(buffer, 0, buffer.Length);
            ResourceHeader = BigEndianMarshal.ByteArrayToStructureBigEndian<AtariResourceHeader>(buffer);

            if(ResourceHeader.rsh_vrsn != 0 && ResourceHeader.rsh_vrsn != 1 && ResourceHeader.rsh_vrsn != 4 && ResourceHeader.rsh_vrsn != 5) return;

            if(ResourceHeader.rsh_nobs > 0)
            {
                ObjectNode[] nodes = new ObjectNode[ResourceHeader.rsh_nobs];

                resourceStream.Position = ResourceHeader.rsh_object;
                for(short i = 0; i < ResourceHeader.rsh_nobs; i++)
                {
                    buffer                  = new byte[Marshal.SizeOf(typeof(ObjectNode))];
                    resourceStream.Read(buffer, 0, buffer.Length);
                    nodes[i] = BigEndianMarshal.ByteArrayToStructureBigEndian<ObjectNode>(buffer);
                }
                
                List<short> knownNodes = new List<short>();
                ResourceObjectRoot = ProcessResourceObject(nodes, ref knownNodes, 0);
            }
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