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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Claunia.Encoding;

namespace libexeinfo
{
    /// <summary>
    ///     Represents a DOS relocatable executable
    /// </summary>
    public partial class MZ : IExecutable
    {
        public GEM.ColorIcon[] GemColorIcons;
        /// <summary>
        ///     Header for this executable
        /// </summary>
        internal MZHeader                 Header;
        public   GEM.GemResourceExtension ResourceExtension;
        public   GEM.MagiCResourceHeader  ResourceHeader;
        public   GEM.TreeObjectNode[]     ResourceObjectRoots;
        public   Stream                   ResourceStream;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:libexeinfo.MZ" /> class.
        /// </summary>
        /// <param name="path">Executable path.</param>
        public MZ(string path)
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
        ///     Initializes a new instance of the <see cref="T:libexeinfo.MZ" /> class.
        /// </summary>
        /// <param name="stream">Stream containing the executable.</param>
        public MZ(Stream stream)
        {
            BaseStream = stream;
            Initialize();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:libexeinfo.MZ" /> class.
        /// </summary>
        /// <param name="data">Byte array containing the executable.</param>
        public MZ(byte[] data)
        {
            BaseStream = new MemoryStream(data);
            Initialize();
        }

        /// <summary>
        ///     The <see cref="FileStream" /> that contains the executable represented by this instance
        /// </summary>
        public Stream BaseStream  { get; }
        public bool   IsBigEndian => false;
        /// <summary>
        ///     If true this instance correctly represents a DOS relocatable executable
        /// </summary>
        public bool                      Recognized              { get; private set; }
        public string                    Type                    { get; private set; }
        public IEnumerable<Architecture> Architectures           => new[] {Architecture.I86};
        public OperatingSystem           RequiredOperatingSystem { get; private set; }
        public IEnumerable<string>       Strings                 { get; private set; }
        public IEnumerable<Segment> Segments { get; }

        void Initialize()
        {
            Recognized = false;
            if(BaseStream == null) return;

            byte[] buffer = new byte[Marshal.SizeOf(typeof(MZHeader))];

            BaseStream.Position = 0;
            BaseStream.Read(buffer, 0, buffer.Length);
            IntPtr hdrPtr = Marshal.AllocHGlobal(buffer.Length);
            Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
            Header = (MZHeader)Marshal.PtrToStructure(hdrPtr, typeof(MZHeader));
            Marshal.FreeHGlobal(hdrPtr);
            Recognized = Header.signature == SIGNATURE;

            if(!Recognized) return;

            Type                    = "DOS Executable (MZ)";
            RequiredOperatingSystem = new OperatingSystem {Name = "DOS"};

            if(ResourceStream == null) return;

            buffer                  = new byte[Marshal.SizeOf(typeof(GEM.GemResourceHeader))];
            ResourceStream.Position = 0;
            ResourceStream.Read(buffer, 0, buffer.Length);
            GEM.GemResourceHeader gemResourceHeader =
                BigEndianMarshal.ByteArrayToStructureLittleEndian<GEM.GemResourceHeader>(buffer);

            if(gemResourceHeader.rsh_vrsn != 0 && gemResourceHeader.rsh_vrsn != 1 && gemResourceHeader.rsh_vrsn != 3 &&
               gemResourceHeader.rsh_vrsn != 4 && gemResourceHeader.rsh_vrsn != 5) return;

            RequiredOperatingSystem = new OperatingSystem {Name = "PC-GEM"};

            if(gemResourceHeader.rsh_vrsn == 3)
            {
                buffer                  = new byte[Marshal.SizeOf(typeof(GEM.MagiCResourceHeader))];
                ResourceStream.Position = 0;
                ResourceStream.Read(buffer, 0, buffer.Length);
                ResourceHeader =
                    BigEndianMarshal.ByteArrayToStructureLittleEndian<GEM.MagiCResourceHeader>(buffer);
                RequiredOperatingSystem = new OperatingSystem {Name = "MagiC"};
            }
            else ResourceHeader = GEM.GemToMagiC(gemResourceHeader);

            if((ResourceHeader.rsh_vrsn & 4) == 4)
            {
                buffer                  = new byte[Marshal.SizeOf(typeof(GEM.GemResourceExtension))];
                ResourceStream.Position = ResourceHeader.rsh_rssize;
                ResourceStream.Read(buffer, 0, buffer.Length);
                ResourceExtension = BigEndianMarshal.ByteArrayToStructureLittleEndian<GEM.GemResourceExtension>(buffer);

                GemColorIcons =
                    GEM.GetColorIcons(ResourceStream, ResourceExtension.color_ic, false, Encoding.GemEncoding);
            }

            List<string> strings = new List<string>();

            if(ResourceHeader.rsh_ntree > 0)
            {
                ResourceStream.Position = ResourceHeader.rsh_trindex;
                int[]  treeOffsets      = new int[ResourceHeader.rsh_ntree];
                byte[] tmp              = new byte[4];

                for(int i = 0; i < ResourceHeader.rsh_ntree; i++)
                {
                    ResourceStream.Read(tmp, 0, 4);
                    treeOffsets[i] = BitConverter.ToInt32(tmp, 0);
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
                        GEM.ObjectNode node = BigEndianMarshal.ByteArrayToStructureLittleEndian<GEM.ObjectNode>(buffer);
                        nodes.Add(node);
                        if(((GEM.ObjectFlags)node.ob_flags).HasFlag(GEM.ObjectFlags.Lastob)) break;
                    }

                    List<short> knownNodes = new List<short>();
                    ResourceObjectRoots[i] =
                        GEM.ProcessResourceObject(nodes, ref knownNodes, 0, ResourceStream, strings, false,
                                                  Encoding.GemEncoding);
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
                    nodes[i] = BigEndianMarshal.ByteArrayToStructureLittleEndian<GEM.ObjectNode>(buffer);
                }

                List<short> knownNodes = new List<short>();
                ResourceObjectRoots    = new GEM.TreeObjectNode[1];
                // TODO: Correct encoding?
                ResourceObjectRoots[0] =
                    GEM.ProcessResourceObject(nodes, ref knownNodes, 0, ResourceStream, strings, false,
                                              Encoding.GemEncoding);
            }

            if(strings.Count > 0)
            {
                strings.Sort();
                Strings = strings.Distinct();
            }
        }

        /// <summary>
        ///     Identifies if the specified executable is a DOS relocatable executable
        /// </summary>
        /// <returns><c>true</c> if the specified executable is a DOS relocatable executable, <c>false</c> otherwise.</returns>
        /// <param name="path">Executable path.</param>
        public static bool Identify(string path)
        {
            FileStream exeFs = File.Open(path, FileMode.Open, FileAccess.Read);
            return Identify(exeFs);
        }

        /// <summary>
        ///     Identifies if the specified executable is a DOS relocatable executable
        /// </summary>
        /// <returns><c>true</c> if the specified executable is a DOS relocatable executable, <c>false</c> otherwise.</returns>
        /// <param name="stream">Stream containing the executable.</param>
        public static bool Identify(FileStream stream)
        {
            byte[] buffer = new byte[Marshal.SizeOf(typeof(MZHeader))];

            stream.Position = 0;
            stream.Read(buffer, 0, buffer.Length);
            IntPtr hdrPtr = Marshal.AllocHGlobal(buffer.Length);
            Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
            MZHeader mzHdr = (MZHeader)Marshal.PtrToStructure(hdrPtr, typeof(MZHeader));
            Marshal.FreeHGlobal(hdrPtr);

            return mzHdr.signature == SIGNATURE;
        }
    }
}