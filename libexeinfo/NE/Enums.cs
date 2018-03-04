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

namespace libexeinfo
{
    public partial class NE
    {
        /// <summary>
        ///     Application flags.
        /// </summary>
        [Flags]
        public enum ApplicationFlags : byte
        {
            FullScreen    = 1,
            GUICompatible = 2,
            Errors        = 1 << 5,
            NonConforming = 1 << 6,
            DLL           = 1 << 7
        }

        /// <summary>
        ///     OS/2 flags.
        /// </summary>
        [Flags]
        public enum OS2Flags : byte
        {
            LongFilename      = 1 << 0,
            ProtectedMode2    = 1 << 1,
            ProportionalFonts = 1 << 2,
            GangloadArea      = 1 << 3
        }

        /// <summary>
        ///     Resource types.
        /// </summary>
        public enum Os2ResourceTypes : ushort
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

        /// <summary>
        ///     Program flags.
        /// </summary>
        [Flags]
        public enum ProgramFlags : byte
        {
            NoDGroup       = 0,
            SingleDGroup   = 1,
            MultipleDGroup = 2,
            GlobalInit     = 1 << 2,
            ProtectedMode  = 1 << 3,
            i86            = 1 << 4,
            i286           = 1 << 5,
            i386           = 1 << 6,
            i87            = 1 << 7
        }

        /// <summary>
        ///     Resource flags.
        /// </summary>
        [Flags]
        public enum ResourceFlags : ushort
        {
            Moveable    = 0x10,
            Pure        = 0x20,
            Preload     = 0x40,
            Discardable = 0x1000,
            SegmentAligned = 0x8000
        }

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

        [Flags]
        public enum SegmentFlags : ushort
        {
            /// <summary>
            ///     Segment data is iterated
            /// </summary>
            Iterated = 0x08,
            /// <summary>
            ///     Segment is not fixed
            /// </summary>
            Moveable = 0x10,
            /// <summary>
            ///     Segment can be shared
            /// </summary>
            Shared = 0x20,
            /// <summary>
            ///     Segment will be preloaded; read-only if this is a data segment
            /// </summary>
            Preload = 0x40,
            /// <summary>
            ///     Code segment is execute only; data segment is read-only
            /// </summary>
            Eronly = 0x80,
            /// <summary>
            ///     Segment has relocation records
            /// </summary>
            Relocinfo = 0x100,
            /// <summary>
            ///     Segment is conforming
            /// </summary>
            Conform = 0x200,
            /// <summary>
            ///     Discardable
            /// </summary>
            Discardable = 0x1000,
            /// <summary>
            ///     32-bit code segment
            /// </summary>
            Code32 = 0x2000,
            /// <summary>
            ///     Length of segment and minimum allocation size are in units of segment sector size
            /// </summary>
            Huge = 0x4000
        }

        public enum SegmentType : ushort
        {
            Code = 0,
            Data = 1
        }

        /// <summary>
        ///     Target operating system.
        /// </summary>
        public enum TargetOS : byte
        {
            Unknown = 0,
            OS2     = 1,
            Windows = 2,
            DOS     = 3,
            Win32   = 4,
            Borland = 5
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
}