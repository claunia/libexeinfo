//
// Consts.cs
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

using libexeinfo.Os2;

namespace libexeinfo
{
    public partial class NE
    {
        /// <summary>
        ///     New Executable signature, "NE"
        /// </summary>
        const ushort SIGNATURE = 0x454E;
        /// <summary>
        ///     Signature for a <see cref="FixedFileInfo" />
        /// </summary>
        const string FIXED_FILE_INFO_SIG = "VS_VERSION_INFO";
        /// <summary>
        ///     Signature for list of name=value strings inside a version resource
        /// </summary>
        const string STRING_FILE_INFO = "StringFileInfo";

        const ushort SEGMENT_TYPE_MASK    = 0x07;
        const ushort SEGMENT_FLAGS_MASK   = 0x3F8;
        const ushort SEGMENT_DISCARD_MASK = 0xF000;
        const ushort SEGMENT_IOPRVL_MASK  = 0xC00;
        public const ushort KNOWN_RSRC_FLAGS     = 0x1070;

        /// <summary>
        ///     Gets the name of a resource type according to its identifier
        /// </summary>
        /// <returns>The resource type name.</returns>
        /// <param name="id">Resource type identifier.</param>
        public static string ResourceIdToName(ushort id)
        {
            switch(id & 0x7FFF)
            {
                case (int)ResourceTypes.RT_ACCELERATOR:  return "RT_ACCELERATOR";
                case (int)ResourceTypes.RT_ANICURSOR:    return "RT_ANICURSOR";
                case (int)ResourceTypes.RT_ANIICON:      return "RT_ANIICON";
                case (int)ResourceTypes.RT_BITMAP:       return "RT_BITMAP";
                case (int)ResourceTypes.RT_CURSOR:       return "RT_CURSOR";
                case (int)ResourceTypes.RT_DIALOG:       return "RT_DIALOG";
                case (int)ResourceTypes.RT_DIALOGEX:     return "RT_DIALOGEX";
                case (int)ResourceTypes.RT_DLGINCLUDE:   return "RT_DLGINCLUDE";
                case (int)ResourceTypes.RT_DLGINIT:      return "RT_DLGINIT";
                case (int)ResourceTypes.RT_FONT:         return "RT_FONT";
                case (int)ResourceTypes.RT_FONTDIR:      return "RT_FONTDIR";
                case (int)ResourceTypes.RT_GROUP_CURSOR: return "RT_GROUP_CURSOR";
                case (int)ResourceTypes.RT_GROUP_ICON:   return "RT_GROUP_ICON";
                case (int)ResourceTypes.RT_HTML:         return "RT_HTML";
                case (int)ResourceTypes.RT_ICON:         return "RT_ICON";
                case (int)ResourceTypes.RT_MANIFEST:     return "RT_MANIFEST";
                case (int)ResourceTypes.RT_MENU:         return "RT_MENU";
                case (int)ResourceTypes.RT_MENUEX:       return "RT_MENUEX";
                case (int)ResourceTypes.RT_MESSAGETABLE: return "RT_MESSAGETABLE";
                case (int)ResourceTypes.RT_NEWBITMAP:    return "RT_NEWBITMAP";
                case (int)ResourceTypes.RT_PLUGPLAY:     return "RT_PLUGPLAY";
                case (int)ResourceTypes.RT_RCDATA:       return "RT_RCDATA";
                case (int)ResourceTypes.RT_STRING:       return "RT_STRING";
                case (int)ResourceTypes.RT_TOOLBAR:      return "RT_TOOLBAR";
                case (int)ResourceTypes.RT_VERSION:      return "RT_VERSION";
                case (int)ResourceTypes.RT_VXD:          return "RT_VXD";
                default:                                 return $"{id & 0x7FFF}";
            }
        }

        /// <summary>
        ///     Gets the name of a resource type according to its identifier
        /// </summary>
        /// <returns>The resource type name.</returns>
        /// <param name="id">Resource type identifier.</param>
        public static string ResourceIdToNameOs2(ushort id)
        {
            switch(id)
            {
                case (int)Os2.ResourceTypes.RT_POINTER:      return "RT_POINTER";
                case (int)Os2.ResourceTypes.RT_BITMAP:       return "RT_BITMAP";
                case (int)Os2.ResourceTypes.RT_MENU:         return "RT_MENU";
                case (int)Os2.ResourceTypes.RT_DIALOG:       return "RT_DIALOG";
                case (int)Os2.ResourceTypes.RT_STRING:       return "RT_STRING";
                case (int)Os2.ResourceTypes.RT_FONTDIR:      return "RT_FONTDIR";
                case (int)Os2.ResourceTypes.RT_FONT:         return "RT_FONT";
                case (int)Os2.ResourceTypes.RT_ACCELTABLE:   return "RT_ACCELTABLE";
                case (int)Os2.ResourceTypes.RT_RCDATA:       return "RT_RCDATA";
                case (int)Os2.ResourceTypes.RT_MESSAGE:      return "RT_MESSAGE";
                case (int)Os2.ResourceTypes.RT_DLGINCLUDE:   return "RT_DLGINCLUDE";
                case (int)Os2.ResourceTypes.RT_VKEYTBL:      return "RT_VKEYTBL";
                case (int)Os2.ResourceTypes.RT_KEYTBL:       return "RT_KEYTBL";
                case (int)Os2.ResourceTypes.RT_CHARTBL:      return "RT_CHARTBL";
                case (int)Os2.ResourceTypes.RT_DISPLAYINFO:  return "RT_DISPLAYINFO";
                case (int)Os2.ResourceTypes.RT_FKASHORT:     return "RT_FKASHORT";
                case (int)Os2.ResourceTypes.RT_FKALONG:      return "RT_FKALONG";
                case (int)Os2.ResourceTypes.RT_HELPTABLE:    return "RT_HELPTABLE";
                case (int)Os2.ResourceTypes.RT_HELPSUBTABLE: return "RT_HELPSUBTABLE";
                case (int)Os2.ResourceTypes.RT_FDDIR:        return "RT_FDDIR";
                case (int)Os2.ResourceTypes.RT_FD:           return "RT_FD";
                default:                                    return $"{id}";
            }
        }
    }
}