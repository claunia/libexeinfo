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

using System;
using System.ComponentModel;
namespace libexeinfo
{
    public partial class NE
    {
		public const ushort Signature = 0x454E;
        public static readonly string FixedFileInfoSig = "VS_VERSION_INFO";
        public static readonly string StringFileInfo = "StringFileInfo";

		public static string IdToName(ushort id)
		{
			switch (id & 0x7FFF)
			{
				case (int)ResourceTypes.RT_ACCELERATOR:
					return "RT_ACCELERATOR";
				case (int)ResourceTypes.RT_ANICURSOR:
					return "RT_ANICURSOR";
				case (int)ResourceTypes.RT_ANIICON:
					return "RT_ANIICON";
				case (int)ResourceTypes.RT_BITMAP:
					return "RT_BITMAP";
				case (int)ResourceTypes.RT_CURSOR:
					return "RT_CURSOR";
				case (int)ResourceTypes.RT_DIALOG:
					return "RT_DIALOG";
				case (int)ResourceTypes.RT_DIALOGEX:
					return "RT_DIALOGEX";
				case (int)ResourceTypes.RT_DLGINCLUDE:
					return "RT_DLGINCLUDE";
				case (int)ResourceTypes.RT_DLGINIT:
					return "RT_DLGINIT";
				case (int)ResourceTypes.RT_FONT:
					return "RT_FONT";
				case (int)ResourceTypes.RT_FONTDIR:
					return "RT_FONTDIR";
				case (int)ResourceTypes.RT_GROUP_CURSOR:
					return "RT_GROUP_CURSOR";
				case (int)ResourceTypes.RT_GROUP_ICON:
					return "RT_GROUP_ICON";
				case (int)ResourceTypes.RT_HTML:
					return "RT_HTML";
				case (int)ResourceTypes.RT_ICON:
					return "RT_ICON";
				case (int)ResourceTypes.RT_MANIFEST:
					return "RT_MANIFEST";
				case (int)ResourceTypes.RT_MENU:
					return "RT_MENU";
				case (int)ResourceTypes.RT_MENUEX:
					return "RT_MENUEX";
				case (int)ResourceTypes.RT_MESSAGETABLE:
					return "RT_MESSAGETABLE";
				case (int)ResourceTypes.RT_NEWBITMAP:
					return "RT_NEWBITMAP";
				case (int)ResourceTypes.RT_PLUGPLAY:
					return "RT_PLUGPLAY";
				case (int)ResourceTypes.RT_RCDATA:
					return "RT_RCDATA";
				case (int)ResourceTypes.RT_STRING:
					return "RT_STRING";
				case (int)ResourceTypes.RT_TOOLBAR:
					return "RT_TOOLBAR";
				case (int)ResourceTypes.RT_VERSION:
					return "RT_VERSION";
				case (int)ResourceTypes.RT_VXD:
					return "RT_VXD";
				default:
					return string.Format("{0}", id & 0x7FFF);
			}
		}
	}
}
