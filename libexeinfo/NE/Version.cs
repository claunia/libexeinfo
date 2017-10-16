using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace libexeinfo
{
    public partial class NE
    {
        public class Version
        {
            public Dictionary<string, Dictionary<string, string>> StringsByLanguage { get; }

            string fileVersion;
            string productVersion;
            VersionFileFlags fileFlags;
            VersionFileOS fileOS;
            VersionFileType fileType;
            VersionFileSubtype fileSubtype;
            DateTime fileDate;

            public string FileVersion
            {
                get
                {
                    return fileVersion;
                }
            }

            public string ProductVersion
            {
                get
                {
                    return productVersion;
                }
            }

            public VersionFileFlags FileFlags
            {
                get
                {
                    return fileFlags;
                }
            }

            public VersionFileOS FileOS
            {
                get
                {
                    return fileOS;
                }
            }

            public VersionFileType FileType
            {
                get
                {
                    return fileType;
                }
            }

            public VersionFileSubtype FileSubtype
            {
                get
                {
                    return fileSubtype;
                }
            }

            public DateTime FileDate
            {
                get
                {
                    return fileDate;
                }
            }

            public Version(byte[] data)
            {
                if (data == null || data.Length < 5)
                    return;

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