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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Claunia.Encoding;

namespace libexeinfo
{
    /// <summary>
    ///     Represents an Atari ST executable
    /// </summary>
    public partial class AtariST : IExecutable
    {
        public GEM.ColorIcon[]          GemColorIcons;
        public GEM.GemResourceExtension ResourceExtension;
        public GEM.MagiCResourceHeader  ResourceHeader;
        public GEM.TreeObjectNode[]     ResourceObjectRoots;
        public Stream                   ResourceStream;
        SymbolEntry[]                   symbols;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:libexeinfo.AtariST" /> class.
        /// </summary>
        /// <param name="path">Executable path.</param>
        public AtariST(string path)
        {
            BaseStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            string pathDir          = Path.GetDirectoryName(path);
            string filename         = Path.GetFileNameWithoutExtension(path);
            string testPath         = Path.Combine(pathDir, filename);
            string resourceFilePath = null;

            if(File.Exists(testPath         + ".rsc")) resourceFilePath = testPath + ".rsc";
            else if(File.Exists(testPath    + ".rsC"))
                resourceFilePath = testPath + ".rsC";
            else if(File.Exists(testPath    + ".rSc"))
                resourceFilePath = testPath + ".rSc";
            else if(File.Exists(testPath    + ".rSC"))
                resourceFilePath = testPath + ".rSC";
            else if(File.Exists(testPath    + ".Rsc"))
                resourceFilePath = testPath + ".Rsc";
            else if(File.Exists(testPath    + ".RsC"))
                resourceFilePath = testPath + ".RsC";
            else if(File.Exists(testPath    + ".RSc"))
                resourceFilePath = testPath + ".RSc";
            else if(File.Exists(testPath    + ".RSC"))
                resourceFilePath = testPath + ".RSC";

            if(resourceFilePath != null)
                ResourceStream = File.Open(resourceFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

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
        public OperatingSystem           RequiredOperatingSystem { get; private set; }
        public IEnumerable<string>       Strings                 { get; private set; }
        public IEnumerable<Segment> Segments { get; private set; }

        void Initialize()
        {
            Recognized = false;

            if(BaseStream == null) return;

            byte[] buffer       = new byte[Marshal.SizeOf(typeof(AtariHeader))];
            BaseStream.Position = 0;
            BaseStream.Read(buffer, 0, buffer.Length);
            Header               = BigEndianMarshal.ByteArrayToStructureBigEndian<AtariHeader>(buffer);
            Recognized           = Header.signature == SIGNATURE;
            List<string> strings = new List<string>();

            if(!Recognized) return;

            Type = "Atari ST executable";

            if(Header.symb_len != 0)
            {
                BaseStream.Position = 0x1C + Header.text_len + Header.data_len;
                buffer              = new byte[Marshal.SizeOf(typeof(SymbolEntry))];
                symbols             = new SymbolEntry[Header.symb_len / Marshal.SizeOf(typeof(SymbolEntry))];
                for(int i = 0; i < symbols.Length; i++)
                {
                    BaseStream.Read(buffer, 0, buffer.Length);
                    symbols[i]      = new SymbolEntry();
                    symbols[i]      = BigEndianMarshal.ByteArrayToStructureBigEndian<SymbolEntry>(buffer);
                    symbols[i].type = (SymbolType)Swapping.Swap((ushort)symbols[i].type);
                    strings.Add(StringHandlers.CToString(symbols[i].name, Encoding.AtariSTEncoding));
                }
            }

            Segments = new []
            {
                new Segment
                {
                    Name = ".text",
                    Flags = $"{(PrgFlags)(Header.flags & 0xFFCF)} {(PrgSharing)(Header.flags & PF_SHARE_MASK)}",
                    Offset = 0x1C,
                    Size = Header.text_len
                },
                new Segment
                {
                Name = ".data",
                Flags  = "",
                Offset = 0x1C + Header.text_len,
                Size   = Header.data_len
                },
                new Segment
                {
                    Name   = ".bss",
                    Flags  = "",
                    Offset = 0,
                    Size   = Header.bss_len
                }
            };
            
            RequiredOperatingSystem = new OperatingSystem {Name = Header.mint == MINT_SIGNATURE ? "MiNT" : "Atari TOS"};

            if(ResourceStream == null) return;

            buffer                  = new byte[Marshal.SizeOf(typeof(GEM.GemResourceHeader))];
            ResourceStream.Position = 0;
            ResourceStream.Read(buffer, 0, buffer.Length);
            GEM.GemResourceHeader gemResourceHeader =
                BigEndianMarshal.ByteArrayToStructureBigEndian<GEM.GemResourceHeader>(buffer);

            if(gemResourceHeader.rsh_vrsn != 0 && gemResourceHeader.rsh_vrsn != 1 && gemResourceHeader.rsh_vrsn != 3 &&
               gemResourceHeader.rsh_vrsn != 4 && gemResourceHeader.rsh_vrsn != 5) return;

            if(gemResourceHeader.rsh_vrsn == 3)
            {
                buffer                  = new byte[Marshal.SizeOf(typeof(GEM.MagiCResourceHeader))];
                ResourceStream.Position = 0;
                ResourceStream.Read(buffer, 0, buffer.Length);
                ResourceHeader =
                    BigEndianMarshal.ByteArrayToStructureBigEndian<GEM.MagiCResourceHeader>(buffer);
                RequiredOperatingSystem = new OperatingSystem {Name = "MagiC"};
            }
            else ResourceHeader = GEM.GemToMagiC(gemResourceHeader);

            if((ResourceHeader.rsh_vrsn & 4) == 4)
            {
                buffer                  = new byte[Marshal.SizeOf(typeof(GEM.GemResourceExtension))];
                ResourceStream.Position = ResourceHeader.rsh_rssize;
                ResourceStream.Read(buffer, 0, buffer.Length);
                ResourceExtension = BigEndianMarshal.ByteArrayToStructureBigEndian<GEM.GemResourceExtension>(buffer);

                GemColorIcons = GEM.GetColorIcons(ResourceStream, ResourceExtension.color_ic, true,
                                                  Encoding.AtariSTEncoding);
            }

            if(ResourceHeader.rsh_ntree > 0)
            {
                ResourceStream.Position = ResourceHeader.rsh_trindex;
                int[]  treeOffsets      = new int[ResourceHeader.rsh_ntree];
                byte[] tmp              = new byte[4];

                for(int i = 0; i < ResourceHeader.rsh_ntree; i++)
                {
                    ResourceStream.Read(tmp, 0, 4);
                    treeOffsets[i] = BitConverter.ToInt32(tmp.Reverse().ToArray(), 0);
                }

                ResourceObjectRoots = new GEM.TreeObjectNode[ResourceHeader.rsh_ntree];

                for(int i = 0; i < ResourceHeader.rsh_ntree; i++)
                {
                    if(treeOffsets[i] <= 0 || treeOffsets[i] >= ResourceStream.Length) continue;

                    ResourceStream.Position = treeOffsets[i];

                    List<GEM.ObjectNode> nodes = new List<GEM.ObjectNode>();
                    while(true)
                    {
                        buffer = new byte[Marshal.SizeOf(typeof(GEM.ObjectNode))];
                        ResourceStream.Read(buffer, 0, buffer.Length);
                        GEM.ObjectNode node = BigEndianMarshal.ByteArrayToStructureBigEndian<GEM.ObjectNode>(buffer);
                        nodes.Add(node);
                        if(((GEM.ObjectFlags)node.ob_flags).HasFlag(GEM.ObjectFlags.Lastob)) break;
                    }

                    List<short> knownNodes = new List<short>();
                    ResourceObjectRoots[i] =
                        GEM.ProcessResourceObject(nodes, ref knownNodes, 0, ResourceStream, strings, true,
                                                  Encoding.AtariSTEncoding);
                }
            }
            else if(ResourceHeader.rsh_nobs > 0)
            {
                GEM.ObjectNode[] nodes = new GEM.ObjectNode[ResourceHeader.rsh_nobs];

                ResourceStream.Position = ResourceHeader.rsh_object;
                for(short i = 0; i < ResourceHeader.rsh_nobs; i++)
                {
                    buffer = new byte[Marshal.SizeOf(typeof(GEM.ObjectNode))];
                    ResourceStream.Read(buffer, 0, buffer.Length);
                    nodes[i] = BigEndianMarshal.ByteArrayToStructureBigEndian<GEM.ObjectNode>(buffer);
                }

                List<short> knownNodes = new List<short>();
                ResourceObjectRoots    = new GEM.TreeObjectNode[1];
                ResourceObjectRoots[0] =
                    GEM.ProcessResourceObject(nodes, ref knownNodes, 0, ResourceStream, strings, true,
                                              Encoding.AtariSTEncoding);
            }

            if(strings.Count > 0)
            {
                strings.Sort();
                Strings = strings.Distinct();
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