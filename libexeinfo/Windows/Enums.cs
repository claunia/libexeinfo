﻿//
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

using System;

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

    /// <summary>
    ///     Version file flags.
    /// </summary>
    [Flags]
    public enum VersionFileFlags : uint
    {
        VS_FF_DEBUG        = 0x00000001,
        VS_FF_INFOINFERRED = 0x00000010,
        VS_FF_PATCHED      = 0x00000004,
        VS_FF_PRERELEASE   = 0x00000002,
        VS_FF_PRIVATEBUILD = 0x00000008,
        VS_FF_SPECIALBUILD = 0x00000020
    }

    /// <summary>
    ///     Version file operating system.
    /// </summary>
    public enum VersionFileOS : uint
    {
        VOS_DOS       = 0x00010000,
        VOS_NT        = 0x00040000,
        VOS_WINDOWS16 = 0x00000001,
        VOS_WINDOWS32 = 0x00000004,
        VOS_OS216     = 0x00020000,
        VOS_OS232     = 0x00030000,
        VOS_PM16      = 0x00000002,
        VOS_PM32      = 0x00000003,
        VOS_UNKNOWN   = 0x00000000,

        // Combinations, some have no sense
        VOS_DOS_NT          = 0x00050000,
        VOS_DOS_WINDOWS16   = 0x00010001,
        VOS_DOS_WINDOWS32   = 0x00010004,
        VOS_DOS_PM16        = 0x00010002,
        VOS_DOS_PM32        = 0x00010003,
        VOS_NT_WINDOWS16    = 0x00040001,
        VOS_NT_WINDOWS32    = 0x00040004,
        VOS_NT_PM16         = 0x00040002,
        VOS_NT_PM32         = 0x00040003,
        VOS_OS216_WINDOWS16 = 0x00020001,
        VOS_OS216_WINDOWS32 = 0x00020004,
        VOS_OS216_PM16      = 0x00020002,
        VOS_OS216_PM32      = 0x00020003,
        VOS_OS232_WINDOWS16 = 0x00030001,
        VOS_OS232_WINDOWS32 = 0x00030004,
        VOS_OS232_PM16      = 0x00030002,
        VOS_OS232_PM32      = 0x00030003
    }

    /// <summary>
    ///     Version file subtype.
    /// </summary>
    public enum VersionFileSubtype : uint
    {
        VFT2_UNKNOWN = 0x00000000,
        // Drivers
        VFT2_DRV_COMM              = 0x0000000A,
        VFT2_DRV_DISPLAY           = 0x00000004,
        VFT2_DRV_INSTALLABLE       = 0x00000008,
        VFT2_DRV_KEYBOARD          = 0x00000002,
        VFT2_DRV_LANGUAGE          = 0x00000003,
        VFT2_DRV_MOUSE             = 0x00000005,
        VFT2_DRV_NETWORK           = 0x00000006,
        VFT2_DRV_PRINTER           = 0x00000001,
        VFT2_DRV_SOUND             = 0x00000009,
        VFT2_DRV_SYSTEM            = 0x00000007,
        VFT2_DRV_VERSIONED_PRINTER = 0x0000000C,
        // Fonts
        VFT2_FONT_RASTER   = 0x00000001,
        VFT2_FONT_TRUETYPE = 0x00000003,
        VFT2_FONT_VECTOR   = 0x00000002
    }

    /// <summary>
    ///     Version file type.
    /// </summary>
    public enum VersionFileType : uint
    {
        VFT_APP        = 0x00000001,
        VFT_DLL        = 0x00000002,
        VFT_DRV        = 0x00000003,
        VFT_FONT       = 0x00000004,
        VFT_STATIC_LIB = 0x00000007,
        VFT_UNKNOWN    = 0x00000000,
        VFT_VXD        = 0x00000005
    }
}