//
// Geos.cs
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
// THE SOFTWARE.namespace libexeinfo.Geos

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace libexeinfo
{
    public partial class Geos : IExecutable
    {
        ApplicationHeader   applicationHeader;
        ApplicationHeaderV2 applicationHeader2;
        Export[]            exports;
        Encoding            geosEncoding = Claunia.Encoding.Encoding.GeosEncoding;
        GeodeHeader         header;
        GeodeHeaderV2       header2;
        Import[]            imports;
        bool                isNewHeader;
        SegmentDescriptor[] segments;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:libexeinfo.NE" /> class.
        /// </summary>
        /// <param name="path">Executable path.</param>
        public Geos(string path)
        {
            BaseStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            Initialize();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:libexeinfo.NE" /> class.
        /// </summary>
        /// <param name="stream">Stream containing the executable.</param>
        public Geos(Stream stream)
        {
            BaseStream = stream;
            Initialize();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:libexeinfo.NE" /> class.
        /// </summary>
        /// <param name="data">Byte array containing the executable.</param>
        public Geos(byte[] data)
        {
            BaseStream = new MemoryStream(data);
            Initialize();
        }

        public bool Recognized { get; private set; }
        public string Type =>
            isNewHeader ? "GEOS executable v2" : "GEOS executable";
        public Stream                    BaseStream              { get; }
        public bool                      IsBigEndian             => false;
        public IEnumerable<Architecture> Architectures           => new[] {Architecture.I86};
        public OperatingSystem           RequiredOperatingSystem { get; private set; }
        public IEnumerable<string>       Strings                 { get; private set; }
        public IEnumerable<Segment>      Segments                { get; private set; }

        void Initialize()
        {
            Recognized = false;
            if(BaseStream == null) return;

            BaseStream.Seek(0, SeekOrigin.Begin);
            byte[] buffer = new byte[Marshal.SizeOf(typeof(GeodeHeaderV2))];
            BaseStream.Read(buffer, 0, buffer.Length);
            header  = BigEndianMarshal.ByteArrayToStructureLittleEndian<GeodeHeader>(buffer);
            header2 = BigEndianMarshal.ByteArrayToStructureLittleEndian<GeodeHeaderV2>(buffer);

            Recognized = header.magic  == GEOS_ID  && header.type  == FileType.GFT_EXECUTABLE ||
                         header2.magic == GEOS2_ID && header2.type == FileType2.GFT_EXECUTABLE;

            if(!Recognized) return;

            isNewHeader             = header2.magic == GEOS2_ID;
            RequiredOperatingSystem = new OperatingSystem {Name = "GEOS", MajorVersion = isNewHeader ? 2 : 1};

            List<string> strings = new List<string>
            {
                StringHandlers.CToString(isNewHeader ? header2.name : header.name,           geosEncoding),
                StringHandlers.CToString(isNewHeader ? header2.copyright : header.copyright, geosEncoding),
                StringHandlers.CToString(isNewHeader ? header2.info : header.info,           geosEncoding)
            };

            uint segmentBase = 0;

            if(isNewHeader)
            {
                BaseStream.Position = Marshal.SizeOf(typeof(GeodeHeaderV2));
                buffer              = new byte[Marshal.SizeOf(typeof(ApplicationHeaderV2))];
                segmentBase         = (uint)Marshal.SizeOf(typeof(GeodeHeaderV2));
                BaseStream.Read(buffer, 0, buffer.Length);
                applicationHeader2 = BigEndianMarshal.ByteArrayToStructureLittleEndian<ApplicationHeaderV2>(buffer);
                imports            = new Import[applicationHeader2.imports];
                exports            = new Export[applicationHeader2.exports];
                segments           = new SegmentDescriptor[applicationHeader2.segments];
                strings.Add($"{StringHandlers.CToString(applicationHeader2.name, geosEncoding).Trim()}.{StringHandlers.CToString(applicationHeader2.extension, geosEncoding).Trim()}");
            }
            else
            {
                BaseStream.Position = Marshal.SizeOf(typeof(GeodeHeader));
                buffer              = new byte[Marshal.SizeOf(typeof(ApplicationHeader))];
                BaseStream.Read(buffer, 0, buffer.Length);
                applicationHeader = BigEndianMarshal.ByteArrayToStructureLittleEndian<ApplicationHeader>(buffer);
                imports           = new Import[applicationHeader.imports];
                exports           = new Export[applicationHeader.exports];
                segments          = new SegmentDescriptor[applicationHeader.segments];
                strings.Add($"{StringHandlers.CToString(applicationHeader.name, geosEncoding).Trim()}.{StringHandlers.CToString(applicationHeader.extension, geosEncoding).Trim()}");
            }

            buffer = new byte[Marshal.SizeOf(typeof(Import))];
            for(int i = 0; i < imports.Length; i++)
            {
                BaseStream.Read(buffer, 0, buffer.Length);
                imports[i] = BigEndianMarshal.ByteArrayToStructureLittleEndian<Import>(buffer);
                strings.Add(StringHandlers.CToString(imports[i].name, geosEncoding).Trim());
            }

            buffer = new byte[Marshal.SizeOf(typeof(Export))];
            for(int i = 0; i < exports.Length; i++)
            {
                BaseStream.Read(buffer, 0, buffer.Length);
                exports[i] = BigEndianMarshal.ByteArrayToStructureLittleEndian<Export>(buffer);
            }

            if(segments.Length > 0)
            {
                buffer = new byte[Marshal.SizeOf(typeof(SegmentDescriptor)) * segments.Length];
                BaseStream.Read(buffer, 0, buffer.Length);
                Segment[] mySegments = new Segment[segments.Length];

                for(int i = 0; i < segments.Length; i++)
                {
                    segments[i].length = BitConverter.ToUInt16(buffer, 2 * i);
                    segments[i].offset =
                        BitConverter.ToUInt32(buffer, 2 * segments.Length + 4 * i) + segmentBase;
                    segments[i].relocs_length = BitConverter.ToUInt16(buffer, 6 * segments.Length + 2 * i);
                    segments[i].flags =
                        (SegmentFlags)BitConverter.ToUInt16(buffer, 8 * segments.Length + 2 * i);

                    mySegments[i] = new Segment
                    {
                        Flags  = $"{segments[i].flags}",
                        Offset = segments[i].offset,
                        Size   = segments[i].length
                    };

                    if(i == 1) mySegments[i].Name                                                           = ".idata";
                    else if(segments[i].flags.HasFlag(SegmentFlags.HAF_CODE)) mySegments[i].Name            = ".text";
                    else if(segments[i].flags.HasFlag(SegmentFlags.HAF_OBJECT_RESOURCE)) mySegments[i].Name = ".rsrc";
                    else if(segments[i].flags.HasFlag(SegmentFlags.HAF_ZERO_INIT)) mySegments[i].Name       = ".bss";
                    else if(segments[i].flags.HasFlag(SegmentFlags.HAF_READ_ONLY)) mySegments[i].Name       = ".rodata";
                    else mySegments[i].Name                                                                 = ".data";
                }

                Segments = mySegments;
            }

            strings.Remove("");
            strings.Remove(null);
            Strings = strings;
        }

        /// <summary>
        ///     Identifies if the specified executable is a Microsoft/IBM Linear EXecutable
        /// </summary>
        /// <returns><c>true</c> if the specified executable is a Microsoft/IBM Linear EXecutable, <c>false</c> otherwise.</returns>
        /// <param name="path">Executable path.</param>
        public static bool Identify(string path)
        {
            FileStream baseStream = File.Open(path, FileMode.Open, FileAccess.Read);

            baseStream.Seek(0, SeekOrigin.Begin);
            byte[] buffer = new byte[Marshal.SizeOf(typeof(GeodeHeaderV2))];
            baseStream.Read(buffer, 0, buffer.Length);
            GeodeHeader   header  = BigEndianMarshal.ByteArrayToStructureLittleEndian<GeodeHeader>(buffer);
            GeodeHeaderV2 header2 = BigEndianMarshal.ByteArrayToStructureLittleEndian<GeodeHeaderV2>(buffer);

            return header.magic  == GEOS_ID  && header.type  == FileType.GFT_EXECUTABLE ||
                   header2.magic == GEOS2_ID && header2.type == FileType2.GFT_EXECUTABLE;
        }

        /// <summary>
        ///     Identifies if the specified executable is a Microsoft/IBM Linear EXecutable
        /// </summary>
        /// <returns><c>true</c> if the specified executable is a Microsoft/IBM Linear EXecutable, <c>false</c> otherwise.</returns>
        /// <param name="stream">Stream containing the executable.</param>
        public static bool Identify(FileStream stream)
        {
            FileStream baseStream = stream;
            baseStream.Seek(0, SeekOrigin.Begin);
            byte[] buffer = new byte[Marshal.SizeOf(typeof(GeodeHeaderV2))];
            baseStream.Read(buffer, 0, buffer.Length);
            GeodeHeader   header  = BigEndianMarshal.ByteArrayToStructureLittleEndian<GeodeHeader>(buffer);
            GeodeHeaderV2 header2 = BigEndianMarshal.ByteArrayToStructureLittleEndian<GeodeHeaderV2>(buffer);

            return header.magic  == GEOS_ID  && header.type  == FileType.GFT_EXECUTABLE ||
                   header2.magic == GEOS2_ID && header2.type == FileType2.GFT_EXECUTABLE;
        }
    }
}