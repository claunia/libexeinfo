//
// Enums.cs
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

namespace libexeinfo.Windows
{
    /// <summary>
    ///     Resource types.
    /// </summary>
    public enum ResourceTypes : ushort
    {
        RT_ACCELERATOR  = 9,
        RT_ANICURSOR    = 21,
        RT_ANIICON      = 22,
        RT_BITMAP       = 2,
        RT_CURSOR       = 1,
        RT_DIALOG       = 5,
        RT_DIALOGEX     = 18,
        RT_DLGINCLUDE   = 17,
        RT_DLGINIT      = 240,
        RT_FONT         = 8,
        RT_FONTDIR      = 7,
        RT_GROUP_CURSOR = 12,
        RT_GROUP_ICON   = 14,
        RT_HTML         = 23,
        RT_ICON         = 3,
        RT_MANIFEST     = 24,
        RT_MENU         = 4,
        RT_MENUEX       = 15,
        RT_MESSAGETABLE = 11,
        RT_NEWBITMAP    = RT_NEW | RT_BITMAP,
        RT_PLUGPLAY     = 19,
        RT_RCDATA       = 10,
        RT_STRING       = 6,
        RT_TOOLBAR      = 241,
        RT_VERSION      = 16,
        RT_VXD          = 20,
        RT_NEW          = 0x2000
    }

    public enum BitmapCompression : uint
    {
        None           = 0,
        Rle8           = 1,
        Rle4           = 2,
        Bitfields      = 3,
        Jpeg           = 4,
        Png            = 5,
        AlphaBitfields = 6,
        Cmyk           = 11,
        CmykRle8       = 12,
        CmykRle4       = 13
    }
}