//
// Version.cs
//
// Author:
//       Natalia Portillo <claunia@claunia.com>
//
// Copyright (c) 2017-2018 Copyright © Claunia.com
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
using System.Text;
using libexeinfo.Windows;

namespace libexeinfo
{
    public partial class PE
    {
        /// <summary>
        ///     Gets all the version resources from this instance
        /// </summary>
        /// <returns>The decoded version resources.</returns>
        List<Version> GetVersions()
        {
            return (from node in WindowsResourcesRoot.children
                    where node.id == (uint)ResourceTypes.RT_VERSION
                    from ids in node.children
                    from lang in ids.children
                    select new Version(lang.data, lang.name)).ToList();
        }

        /// <summary>
        ///     Represents a version ("RT_VERSION") resource
        /// </summary>
        public class Version
        {
            /// <summary>
            ///     Initializes a new instance of the <see cref="T:libexeinfo.NE.Version" /> class.
            /// </summary>
            /// <param name="data">Resource data.</param>
            /// <param name="resourceName">Resource name.</param>
            public Version(byte[] data, string resourceName = null)
            {
                if(data == null || data.Length < 5) return;

                Name = resourceName;

                StringsByLanguage = new Dictionary<string, Dictionary<string, string>>();

                VersionNode root = GetNode(data, 0, out int _);
                DecodeNode(root, null, null);
            }

            /// <summary>
            ///     This contains a list of all name=value strings pairs sorted by language
            /// </summary>
            /// <value>List of all name=value strings pairs sorted by language.</value>
            public Dictionary<string, Dictionary<string, string>> StringsByLanguage { get; }

            /// <summary>
            ///     File version.
            /// </summary>
            /// <value>The file version.</value>
            public string FileVersion { get; private set; }

            /// <summary>
            ///     Product version.
            /// </summary>
            /// <value>The product version.</value>
            public string ProductVersion { get; private set; }

            /// <summary>
            ///     File flags.
            /// </summary>
            /// <value>The file flags.</value>
            public VersionFileFlags FileFlags { get; private set; }

            /// <summary>
            ///     File operating system.
            /// </summary>
            /// <value>The file operating system.</value>
            public VersionFileOS FileOs { get; private set; }

            /// <summary>
            ///     File type.
            /// </summary>
            /// <value>The type of the file.</value>
            public VersionFileType FileType { get; private set; }

            /// <summary>
            ///     File subtype.
            /// </summary>
            /// <value>The file subtype.</value>
            public VersionFileSubtype FileSubtype { get; private set; }

            /// <summary>
            ///     File date.
            /// </summary>
            /// <value>The file date.</value>
            public DateTime FileDate { get; set; }

            /// <summary>
            ///     Resource name
            /// </summary>
            /// <value>The resource name.</value>
            public string Name { get; }

            static VersionNode GetNode(byte[] data, int startPosition, out int nodeLength)
            {
                nodeLength = 0;

                VersionNode node = new VersionNode
                {
                    wLength      = BitConverter.ToUInt16(data, startPosition              + nodeLength),
                    wValueLength = BitConverter.ToUInt16(data, startPosition + nodeLength + 2),
                    wType        = BitConverter.ToUInt16(data, startPosition + nodeLength + 4)
                };
                nodeLength += 6;

                MemoryStream nameMs = new MemoryStream();
                while(true)
                {
                    if(data[startPosition + nodeLength] == 0 && data[startPosition + nodeLength + 1] == 0) break;

                    nameMs.WriteByte(data[startPosition              + nodeLength]);
                    nameMs.WriteByte(data[startPosition + nodeLength + 1]);
                    nodeLength += 2;
                }

                node.szName =  Encoding.Unicode.GetString(nameMs.ToArray());
                nodeLength  += 2;

                if(nodeLength % 4 > 0) nodeLength += 4 - nodeLength % 4;

                int factor = node.wType == 1 ? 2 : 1;

                node.rgbData = new byte[node.wValueLength * factor];
                Array.Copy(data, startPosition + nodeLength, node.rgbData, 0, node.rgbData.Length);
                nodeLength += node.rgbData.Length;
                if(nodeLength % 4 > 0) nodeLength += 4 - nodeLength % 4;

                string foo = Encoding.Unicode.GetString(node.rgbData);

                List<VersionNode> children = new List<VersionNode>();

                while(nodeLength < node.wLength)
                {
                    children.Add(GetNode(data, startPosition + nodeLength, out int childLength));
                    nodeLength += childLength;
                }

                if(children.Count > 0) node.children = children.ToArray();

                return node;
            }

            void DecodeNode(VersionNode node, string parent, string grandparent)
            {
                if(node.szName == Consts.FixedFileInfoSig)
                {
                    IntPtr infoPtr = Marshal.AllocHGlobal(node.wValueLength);
                    Marshal.Copy(node.rgbData, 0, infoPtr, node.wValueLength);
                    FixedFileInfo info = (FixedFileInfo)Marshal.PtrToStructure(infoPtr, typeof(FixedFileInfo));
                    Marshal.FreeHGlobal(infoPtr);

                    FileVersion =
                        $"{(info.dwFileVersionMS & 0xFFFF0000) >> 16}.{info.dwFileVersionMS & 0xFFFF:D2}.{(info.dwFileVersionLS & 0xFFFF0000) >> 16}.{info.dwFileVersionLS & 0xFFFF}";
                    ProductVersion =
                        $"{(info.dwProductVersionMS & 0xFFFF0000) >> 16}.{info.dwProductVersionMS & 0xFFFF:D2}.{(info.dwProductVersionLS & 0xFFFF0000) >> 16}.{info.dwProductVersionLS & 0xFFFF}";
                    FileFlags   = (VersionFileFlags)(info.dwFileFlags & info.dwFileFlagsMask);
                    FileOs      = (VersionFileOS)info.dwFileOS;
                    FileType    = (VersionFileType)info.dwFileType;
                    FileSubtype = (VersionFileSubtype)info.dwFileSubtype;
                    FileDate    = DateTime.FromFileTime(info.dwFileDateMS * 0x100000000 + info.dwFileDateLS);
                }

                if(parent == Consts.StringFileInfo)
                {
                    Dictionary<string, string> strings = new Dictionary<string, string>();
                    StringsByLanguage.Add(node.szName, strings);
                }

                if(grandparent == Consts.StringFileInfo)
                    if(StringsByLanguage.TryGetValue(parent, out Dictionary<string, string> strings))
                        strings.Add(node.szName, Encoding.Unicode.GetString(node.rgbData));

                if(node.children == null) return;

                foreach(VersionNode n in node.children) DecodeNode(n, node.szName, parent);
            }
        }
    }
}