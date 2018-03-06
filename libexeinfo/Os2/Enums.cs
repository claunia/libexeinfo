//
// Enums.cs
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

namespace libexeinfo.Os2
{
    /// <summary>
    ///     OS/2 flags.
    /// </summary>
    [Flags]
    public enum ExecutableFlags : byte
    {
        LongFilename      = 1 << 0,
        ProtectedMode2    = 1 << 1,
        ProportionalFonts = 1 << 2,
        GangloadArea      = 1 << 3
    }

    [Flags]
    public enum MenuAttribute : ushort
    {
        MIA_FRAMED   = 0x1000,
        MIA_CHECKED  = 0x2000,
        MIA_DISABLED = 0x4000,
        MIA_HILITED  = 0x8000
    }

    public enum MenuType : ushort
    {
        MIS_TEXT      = 0x0001,
        MIS_BITMAP    = 0x0002,
        MIS_SEPARATOR = 0x0004,
        MIS_OWNERDRAW = 0x0008,
        MIS_SUBMENU   = 0x0010,
        /// <summary>
        ///     multiple choice submenu
        /// </summary>
        MIS_MULTMENU        = 0x0020,
        MIS_SYSCOMMAND      = 0x0040,
        MIS_HELP            = 0x0080,
        MIS_STATIC          = 0x0100,
        MIS_BUTTONSEPARATOR = 0x0200,
        MIS_BREAK           = 0x0400,
        MIS_BREAKSEPARATOR  = 0x0800,
        /// <summary>
        ///     multiple choice group start
        /// </summary>
        MIS_GROUP = 0x1000,
        /// <summary>
        ///     In multiple choice submenus a style of 'single' denotes the item is a radiobutton.  Absence of this style defaults
        ///     the item to a checkbox.
        /// </summary>
        MIS_SINGLE = 0x2000
    }

    /// <summary>
    ///     Resource types.
    /// </summary>
    public enum ResourceTypes : ushort
    {
        /// <summary>mouse pointer shape</summary>
        RT_POINTER = 1,
        /// <summary>bitmap</summary>
        RT_BITMAP = 2,
        /// <summary>menu template</summary>
        RT_MENU = 3,
        /// <summary>dialog template</summary>
        RT_DIALOG = 4,
        /// <summary>string tables</summary>
        RT_STRING = 5,
        /// <summary>font directory</summary>
        RT_FONTDIR = 6,
        /// <summary>font</summary>
        RT_FONT = 7,
        /// <summary>accelerator tables</summary>
        RT_ACCELTABLE = 8,
        /// <summary>binary data</summary>
        RT_RCDATA = 9,
        /// <summary>error msg     tables</summary>
        RT_MESSAGE = 10,
        /// <summary>dialog include file name</summary>
        RT_DLGINCLUDE = 11,
        /// <summary>key to vkey tables</summary>
        RT_VKEYTBL = 12,
        /// <summary>key to UGL tables</summary>
        RT_KEYTBL = 13,
        /// <summary>glyph to character tables</summary>
        RT_CHARTBL = 14,
        /// <summary>screen display information</summary>
        RT_DISPLAYINFO = 15,
        /// <summary>function key area short form</summary>
        RT_FKASHORT = 16,
        /// <summary>function key area long form</summary>
        RT_FKALONG = 17,
        /// <summary>Help table for IPFC</summary>
        RT_HELPTABLE = 18,
        /// <summary>Help subtable for IPFC</summary>
        RT_HELPSUBTABLE = 19,
        /// <summary>DBCS uniq/font driver directory</summary>
        RT_FDDIR = 20,
        /// <summary>DBCS uniq/font driver</summary>
        RT_FD = 21
    }
}