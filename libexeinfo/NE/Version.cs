//
// Version.cs
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
using System.Text;

namespace libexeinfo
{
    public partial class NE
    {
        /// <summary>
        /// Gets all the version resources from this instance
        /// </summary>
        /// <returns>The decoded version resources.</returns>
        public List<Version> GetVersions()
        {
            List<Version> versions = new List<Version>();

            foreach (ResourceType type in Resources.types)
            {
                if ((type.id & 0x7FFF) == (int)ResourceTypes.RT_VERSION)
                {
                    foreach (Resource resource in type.resources)
                    {
                        Version vers = new Version(resource.data, resource.name);
                        versions.Add(vers);
                    }
                }
            }

            return versions;
        }

        /// <summary>
        /// Represents a version ("RT_VERSION") resource
        /// </summary>
        public class Version
        {
			/// <summary>
			/// This contains a list of all name=value strings pairs sorted by language
			/// </summary>
			/// <value>List of all name=value strings pairs sorted by language.</value>
			public Dictionary<string, Dictionary<string, string>> StringsByLanguage { get; }

            string fileVersion;
            string productVersion;
            VersionFileFlags fileFlags;
            VersionFileOS fileOS;
            VersionFileType fileType;
            VersionFileSubtype fileSubtype;
            DateTime fileDate;
            string name;

            /// <summary>
            /// File version.
            /// </summary>
            /// <value>The file version.</value>
            public string FileVersion
            {
                get
                {
                    return fileVersion;
                }
            }

            /// <summary>
            /// Product version.
            /// </summary>
            /// <value>The product version.</value>
            public string ProductVersion
            {
                get
                {
                    return productVersion;
                }
            }

            /// <summary>
            /// File flags.
            /// </summary>
            /// <value>The file flags.</value>
            public VersionFileFlags FileFlags
            {
                get
                {
                    return fileFlags;
                }
            }

            /// <summary>
            /// File operating system.
            /// </summary>
            /// <value>The file operating system.</value>
            public VersionFileOS FileOS
            {
                get
                {
                    return fileOS;
                }
            }

            /// <summary>
            /// File type.
            /// </summary>
            /// <value>The type of the file.</value>
            public VersionFileType FileType
            {
                get
                {
                    return fileType;
                }
            }

            /// <summary>
            /// File subtype.
            /// </summary>
            /// <value>The file subtype.</value>
            public VersionFileSubtype FileSubtype
            {
                get
                {
                    return fileSubtype;
                }
            }

            /// <summary>
            /// File date.
            /// </summary>
            /// <value>The file date.</value>
            public DateTime FileDate
            {
                get
                {
                    return fileDate;
                }
            }

            /// <summary>
            /// Resource name
            /// </summary>
            /// <value>The resource name.</value>
            public string Name
            {
                get { return name; }
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="T:libexeinfo.NE.Version"/> class.
            /// </summary>
            /// <param name="data">Resource data.</param>
            /// <param name="resourceName">Resource name.</param>
            public Version(byte[] data, string resourceName = null)
            {
                if (data == null || data.Length < 5)
                    return;

                name = resourceName;

                StringsByLanguage = new Dictionary<string, Dictionary<string, string>>();

                VersionNode root = GetNode(data, 0, out int rootLength);
                DecodeNode(root, null, null);
            }

            VersionNode GetNode(byte[] data, int startPosition, out int nodeLength)
            {
                nodeLength = 0;

                VersionNode node = new VersionNode
                {
                    cbNode = BitConverter.ToUInt16(data, startPosition + nodeLength),
                    cbData = BitConverter.ToUInt16(data, startPosition + nodeLength + 2)
                };
                nodeLength += 4;

                MemoryStream nameMs = new MemoryStream();
                while (data[startPosition + nodeLength] > 0)
                {
                    nameMs.WriteByte(data[startPosition + nodeLength]);
                    nodeLength++;
                }
                node.szName = Encoding.ASCII.GetString(nameMs.ToArray());
                nodeLength++;

                if (nodeLength % 4 > 0)
                    nodeLength += 4 - (nodeLength % 4);

                node.rgbData = new byte[node.cbData];
                Array.Copy(data, startPosition + nodeLength, node.rgbData, 0, node.cbData);
                nodeLength += node.cbData;
                if (nodeLength % 4 > 0)
                    nodeLength += 4 - (nodeLength % 4);

                List<VersionNode> children = new List<VersionNode>();

                while (nodeLength < node.cbNode)
                {
                    children.Add(GetNode(data, startPosition + nodeLength, out int childLength));
                    nodeLength += childLength;
                }

                if (children.Count > 0)
                    node.children = children.ToArray();

                return node;
            }

            void DecodeNode(VersionNode node, string parent, string grandparent)
            {
                if (node.szName == FixedFileInfoSig)
                {
                    IntPtr infoPtr = Marshal.AllocHGlobal(node.cbData);
                    Marshal.Copy(node.rgbData, 0, infoPtr, node.cbData);
                    FixedFileInfo info = (FixedFileInfo)Marshal.PtrToStructure(infoPtr, typeof(FixedFileInfo));
                    Marshal.FreeHGlobal(infoPtr);

                    fileVersion = string.Format("{0}.{1:D2}.{2}.{3}", (info.dwFileVersionMS & 0xFFFF0000) >> 16, info.dwFileVersionMS & 0xFFFF,
                                                (info.dwFileVersionLS & 0xFFFF0000) >> 16, info.dwFileVersionLS & 0xFFFF);
                    productVersion = string.Format("{0}.{1:D2}.{2}.{3}", (info.dwProductVersionMS & 0xFFFF0000) >> 16, info.dwProductVersionMS & 0xFFFF,
                                                (info.dwProductVersionLS & 0xFFFF0000) >> 16, info.dwProductVersionLS & 0xFFFF);
                    fileFlags = (VersionFileFlags)(info.dwFileFlags & info.dwFileFlagsMask);
                    fileOS = (VersionFileOS)info.dwFileOS;
                    fileType = (VersionFileType)info.dwFileType;
                    fileSubtype = (VersionFileSubtype)info.dwFileSubtype;
                    fileDate = DateTime.FromFileTime(info.dwFileDateMS * 0x100000000 + info.dwFileDateLS);
                }

                if (parent == StringFileInfo)
                {
                    Dictionary<string, string> strings = new Dictionary<string, string>();
                    StringsByLanguage.Add(node.szName, strings);
                }

                if (grandparent == StringFileInfo)
                {
                    if (StringsByLanguage.TryGetValue(parent, out Dictionary<string, string> strings))
                    {
                        Encoding encoding;

                        try
                        {
                            encoding = Encoding.GetEncoding(Convert.ToInt32(parent.Substring(4), 16));
                        }
                        catch
                        {
                            encoding = Encoding.ASCII;
                        }

                        strings.Add(node.szName, encoding.GetString(node.rgbData));
                    }
                }

                if (node.children != null)
                {
                    for (int i = 0; i < node.children.Length; i++)
                        DecodeNode(node.children[i], node.szName, parent);
                }
            }

			/// <summary>
			/// Converts a <see cref="VersionFileType"/> to string
			/// </summary>
			/// <returns>The string.</returns>
			/// <param name="type"><see cref="VersionFileType"/></param>
			public static string TypeToString(VersionFileType type)
            {
                switch (type)
                {
                    case VersionFileType.VFT_APP:
                        return "Application";
                    case VersionFileType.VFT_DLL:
                        return "Dynamic-link library";
                    case VersionFileType.VFT_DRV:
                        return "Device driver";
                    case VersionFileType.VFT_FONT:
                        return "Font";
                    case VersionFileType.VFT_STATIC_LIB:
                        return "Static-link library";
                    case VersionFileType.VFT_UNKNOWN:
                        return "Unknown";
                    case VersionFileType.VFT_VXD:
                        return "Virtual device";
                    default:
                        return string.Format("Unknown type code {0}", (uint)type);
                }
            }

			/// <summary>
			/// Converts a <see cref="VersionFileSubtype"/> to string, considering file type to be a driver
			/// </summary>
			/// <returns>The string.</returns>
			/// <param name="subtype"><see cref="VersionFileSubtype"/></param>
			public static string DriverToString(VersionFileSubtype subtype)
            {
                switch (subtype)
                {
                    case VersionFileSubtype.VFT2_DRV_COMM:
                        return "Communications";
                    case VersionFileSubtype.VFT2_DRV_DISPLAY:
                        return "Display";
                    case VersionFileSubtype.VFT2_DRV_INSTALLABLE:
                        return "Installable";
                    case VersionFileSubtype.VFT2_DRV_KEYBOARD:
                        return "Keyboard";
                    case VersionFileSubtype.VFT2_DRV_LANGUAGE:
                        return "Language";
                    case VersionFileSubtype.VFT2_DRV_MOUSE:
                        return "Mouse";
                    case VersionFileSubtype.VFT2_DRV_NETWORK:
                        return "Network";
                    case VersionFileSubtype.VFT2_DRV_PRINTER:
                        return "Printer";
                    case VersionFileSubtype.VFT2_DRV_SOUND:
                        return "Sound";
                    case VersionFileSubtype.VFT2_DRV_SYSTEM:
                        return "System";
                    case VersionFileSubtype.VFT2_DRV_VERSIONED_PRINTER:
                        return "Versioned";
                    case VersionFileSubtype.VFT2_UNKNOWN:
                        return "Unknown";
                    default:
                        return string.Format("Unknown type code {0}", (uint)subtype);
                }
            }

			/// <summary>
			/// Converts a <see cref="VersionFileSubtype"/> to string, considering file type to be a font
			/// </summary>
			/// <returns>The string.</returns>
			/// <param name="subtype"><see cref="VersionFileSubtype"/></param>
			public static string FontToString(VersionFileSubtype subtype)
            {
                switch (subtype)
                {
                    case VersionFileSubtype.VFT2_FONT_RASTER:
                        return "Raster";
                    case VersionFileSubtype.VFT2_FONT_TRUETYPE:
                        return "TrueType";
                    case VersionFileSubtype.VFT2_FONT_VECTOR:
                        return "Vector";
                    case VersionFileSubtype.VFT2_UNKNOWN:
                        return "Unknown";
                    default:
                        return string.Format("Unknown type code {0}", (uint)subtype);
                }
            }

			/// <summary>
			/// Converts a <see cref="VersionFileOS"/> to string
			/// </summary>
			/// <returns>The string.</returns>
			/// <param name="os"><see cref="VersionFileOS"/></param>
			public static string OsToString(VersionFileOS os)
            {
                switch (os)
                {
                    case VersionFileOS.VOS_DOS:
                        return "DOS";
                    case VersionFileOS.VOS_NT:
                        return "Windows NT";
                    case VersionFileOS.VOS_WINDOWS16:
                        return "16-bit Windows";
                    case VersionFileOS.VOS_WINDOWS32:
                        return "32-bit Windows";
                    case VersionFileOS.VOS_OS216:
                        return "16-bit OS/2";
                    case VersionFileOS.VOS_OS232:
                        return "32-bit OS/2";
                    case VersionFileOS.VOS_PM16:
                        return "16-bit Presentation Manager";
                    case VersionFileOS.VOS_PM32:
                        return "32-bit Presentation Manager";
                    case VersionFileOS.VOS_UNKNOWN:
                        return "Unknown";
                    case VersionFileOS.VOS_DOS_NT:
                        return "DOS running under Windows NT";
                    case VersionFileOS.VOS_DOS_WINDOWS16:
                        return "16-bit Windows running under DOS";
                    case VersionFileOS.VOS_DOS_WINDOWS32:
                        return "32-bit Windows running under DOS";
                    case VersionFileOS.VOS_DOS_PM16:
                        return "16-bit Presentation Manager running under DOS";
                    case VersionFileOS.VOS_DOS_PM32:
                        return "32-bit Presentation Manager running under DOS";
                    case VersionFileOS.VOS_NT_WINDOWS16:
                        return "16-bit Windows running under Windows NT";
                    case VersionFileOS.VOS_NT_WINDOWS32:
                        return "32-bit Windows running under Windows NT";
                    case VersionFileOS.VOS_NT_PM16:
                        return "16-bit Presentation Manager running under Windows NT";
                    case VersionFileOS.VOS_NT_PM32:
                        return "32-bit Presentation Manager running under Windows NT";
                    case VersionFileOS.VOS_OS216_WINDOWS16:
                        return "16-bit Windows running under 16-bit OS/2";
                    case VersionFileOS.VOS_OS216_WINDOWS32:
                        return "32-bit Windows running under 16-bit OS/2";
                    case VersionFileOS.VOS_OS216_PM16:
                        return "16-bit Presentation Manager running under 16-bit OS/2";
                    case VersionFileOS.VOS_OS216_PM32:
                        return "32-bit Presentation Manager running under 16-bit OS/2";
                    case VersionFileOS.VOS_OS232_WINDOWS16:
                        return "16-bit Windows running under 32-bit OS/2";
                    case VersionFileOS.VOS_OS232_WINDOWS32:
                        return "32-bit Windows running under 32-bit OS/2";
                    case VersionFileOS.VOS_OS232_PM16:
                        return "16-bit Presentation Manager running under 32-bit OS/2";
                    case VersionFileOS.VOS_OS232_PM32:
                        return "32-bit Presentation Manager running under 32-bit OS/2";
                    default:
                        return string.Format("Unknown OS code {0}", (uint)os);
                }
            }
        }
    }
}