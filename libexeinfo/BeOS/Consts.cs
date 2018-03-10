//
// Consts.cs
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

namespace libexeinfo.BeOS
{
    // TODO: Find examples of 'CSTR', 'TEXT', and 'MMSG' with data
    public static class Consts
    {
        internal const int RESOURCES_HEADER_MAGIC = 0x444F1000;

        // String enums not supported in C# *sad*
        public const string B_AFFINE_TRANSFORM_TYPE      = "AMTX";
        public const string B_ALIGNMENT_TYPE             = "ALGN";
        public const string B_ANY_TYPE                   = "ANYT";
        public const string B_ATOM_TYPE                  = "ATOM";
        public const string B_ATOMREF_TYPE               = "ATMR";
        public const string B_BOOL_TYPE                  = "BOOL";
        public const string B_CHAR_TYPE                  = "CHAR";
        public const string B_COLOR_8_BIT_TYPE           = "CLRB";
        public const string B_DOUBLE_TYPE                = "DBLE";
        public const string B_FLOAT_TYPE                 = "FLOT";
        public const string B_GRAYSCALE_8_BIT_TYPE       = "GRYB";
        public const string B_INT16_TYPE                 = "SHRT";
        public const string B_INT32_TYPE                 = "LONG";
        public const string B_INT64_TYPE                 = "LLNG";
        public const string B_INT8_TYPE                  = "BYTE";
        public const string B_LARGE_ICON_TYPE            = "ICON";
        public const string B_MEDIA_PARAMETER_GROUP_TYPE = "BMCG";
        public const string B_MEDIA_PARAMETER_TYPE       = "BMCT";
        public const string B_MEDIA_PARAMETER_WEB_TYPE   = "BMCW";
        public const string B_MESSAGE_TYPE               = "MSGG";
        public const string B_MESSENGER_TYPE             = "MSNG";
        public const string B_MIME_TYPE                  = "MIME";
        public const string B_MINI_ICON_TYPE             = "MICN";
        public const string B_MONOCHROME_1_BIT_TYPE      = "MNOB";
        public const string B_OBJECT_TYPE                = "OPTR";
        public const string B_OFF_T_TYPE                 = "OFFT";
        public const string B_PATTERN_TYPE               = "PATN";
        public const string B_POINTER_TYPE               = "PNTR";
        public const string B_POINT_TYPE                 = "BPNT";
        public const string B_PROPERTY_INFO_TYPE         = "SCTD";
        public const string B_RAW_TYPE                   = "RAWT";
        public const string B_RECT_TYPE                  = "RECT";
        public const string B_REF_TYPE                   = "RREF";
        public const string B_RGB_32_BIT_TYPE            = "RGBB";
        public const string B_RGB_COLOR_TYPE             = "RGBC";
        public const string B_SIZE_TYPE                  = "SIZE";
        public const string B_SIZE_T_TYPE                = "SIZT";
        public const string B_SSIZE_T_TYPE               = "SSZT";
        public const string B_STRING_TYPE                = "CSTR";
        public const string B_STRING_LIST_TYPE           = "STRL";
        public const string B_TIME_TYPE                  = "TIME";
        public const string B_UINT16_TYPE                = "USHT";
        public const string B_UINT32_TYPE                = "ULNG";
        public const string B_UINT64_TYPE                = "ULLG";
        public const string B_UINT8_TYPE                 = "UBYT";
        public const string B_VECTOR_ICON_TYPE           = "VICN";
        public const string B_XATTR_TYPE                 = "XATR";
        public const string B_NETWORK_ADDRESS_TYPE       = "NWAD";
        public const string B_MIME_STRING_TYPE           = "MIMS";
        public const string B_ASCII_TYPE                 = "TEXT";
        public const string B_VERSION_INFO_TYPE          = "APPV";
        public const string B_APP_FLAGS_TYPE             = "APPF";
        public const string B_PICT_FORMAT                = "PICT";
        public const string B_PNG_FORMAT                 = "PNG ";

        /// <summary>
        ///     BeOS system palette as ARGB values
        /// </summary>
        public static readonly uint[] ArgbSystemPalette =
        {
            0xFF000000, 0xFF080808, 0xFF101010, 0xFF181818, 0xFF202020, 0xFF282828, 0xFF303030, 0xFF383838, 0xFF404040,
            0xFF484848, 0xFF505050, 0xFF585858, 0xFF606060, 0xFF686868, 0xFF707070, 0xFF787878, 0xFF808080, 0xFF888888,
            0xFF909090, 0xFF989898, 0xFFA0A0A0, 0xFFA8A8A8, 0xFFB0B0B0, 0xFFB8B8B8, 0xFFC0C0C0, 0xFFC8C8C8, 0xFFD0D0D0,
            0xFFD8D8D8, 0xFFE0E0E0, 0xFFE8E8E8, 0xFFF0F0F0, 0xFFF8F8F8, 0xFF0000FF, 0xFF0000E5, 0xFF0000CC, 0xFF0000B3,
            0xFF00009A, 0xFF000081, 0xFF000069, 0xFF000050, 0xFF000037, 0xFF00001E, 0xFFFF0000, 0xFFE40000, 0xFFCB0000,
            0xFFB20000, 0xFF990000, 0xFF800000, 0xFF690000, 0xFF500000, 0xFF370000, 0xFF1E0000, 0xFF00FF00, 0xFF00E400,
            0xFF00CB00, 0xFF00B200, 0xFF009900, 0xFF008000, 0xFF006900, 0xFF005000, 0xFF003700, 0xFF001E00, 0xFF009833,
            0xFFFFFFFF, 0xFFCBFFFF, 0xFFCBFFCB, 0xFFCBFF98, 0xFFCBFF66, 0xFFCBFF33, 0xFFCBFF00, 0xFF98FFFF, 0xFF98FFCB,
            0xFF98FF98, 0xFF98FF66, 0xFF98FF33, 0xFF98FF00, 0xFF66FFFF, 0xFF66FFCB, 0xFF66FF98, 0xFF66FF66, 0xFF66FF33,
            0xFF66FF00, 0xFF33FFFF, 0xFF33FFCB, 0xFF33FF98, 0xFF33FF66, 0xFF33FF33, 0xFF33FF00, 0xFFFF98FF, 0xFFFF98CB,
            0xFFFF9898, 0xFFFF9866, 0xFFFF9833, 0xFFFF9800, 0xFF0066FF, 0xFF0066CB, 0xFFCBCBFF, 0xFFCBCBCB, 0xFFCBCB98,
            0xFFCBCB66, 0xFFCBCB33, 0xFFCBCB00, 0xFF98CBFF, 0xFF98CBCB, 0xFF98CB98, 0xFF98CB66, 0xFF98CB33, 0xFF98CB00,
            0xFF66CBFF, 0xFF66CBCB, 0xFF66CB98, 0xFF66CB66, 0xFF66CB33, 0xFF66CB00, 0xFF33CBFF, 0xFF33CBCB, 0xFF33CB98,
            0xFF33CB66, 0xFF33CB33, 0xFF33CB00, 0xFFFF66FF, 0xFFFF66CB, 0xFFFF6698, 0xFFFF6666, 0xFFFF6633, 0xFFFF6600,
            0xFF006698, 0xFF006666, 0xFFCB98FF, 0xFFCB98CB, 0xFFCB9898, 0xFFCB9866, 0xFFCB9833, 0xFFCB9800, 0xFF9898FF,
            0xFF9898CB, 0xFF989898, 0xFF989866, 0xFF989833, 0xFF989800, 0xFF6698FF, 0xFF6698CB, 0xFF669898, 0xFF669866,
            0xFF669833, 0xFF669800, 0xFF3398FF, 0xFF3398CB, 0xFF339898, 0xFF339866, 0xFF339833, 0xFF339800, 0xFFE68600,
            0xFFFF33CB, 0xFFFF3398, 0xFFFF3366, 0xFFFF3333, 0xFFFF3300, 0xFF006633, 0xFF006600, 0xFFCB66FF, 0xFFCB66CB,
            0xFFCB6698, 0xFFCB6666, 0xFFCB6633, 0xFFCB6600, 0xFF9866FF, 0xFF9866CB, 0xFF986698, 0xFF986666, 0xFF986633,
            0xFF986600, 0xFF6666FF, 0xFF6666CB, 0xFF666698, 0xFF666666, 0xFF666633, 0xFF666600, 0xFF3366FF, 0xFF3366CB,
            0xFF336698, 0xFF336666, 0xFF336633, 0xFF336600, 0xFFFF00FF, 0xFFFF00CB, 0xFFFF0098, 0xFFFF0066, 0xFFFF0033,
            0xFFFFAF13, 0xFF0033FF, 0xFF0033CB, 0xFFCB33FF, 0xFFCB33CB, 0xFFCB3398, 0xFFCB3366, 0xFFCB3333, 0xFFCB3300,
            0xFF9833FF, 0xFF9833CB, 0xFF983398, 0xFF983366, 0xFF983333, 0xFF983300, 0xFF6633FF, 0xFF6633CB, 0xFF663398,
            0xFF663366, 0xFF663333, 0xFF663300, 0xFF3333FF, 0xFF3333CB, 0xFF333398, 0xFF333366, 0xFF333333, 0xFF333300,
            0xFFFFCB66, 0xFFFFCB98, 0xFFFFCBCB, 0xFFFFCBFF, 0xFF003398, 0xFF003366, 0xFF003333, 0xFF003300, 0xFFCB00FF,
            0xFFCB00CB, 0xFFCB0098, 0xFFCB0066, 0xFFCB0033, 0xFFFFE346, 0xFF9800FF, 0xFF9800CB, 0xFF980098, 0xFF980066,
            0xFF980033, 0xFF980000, 0xFF6600FF, 0xFF6600CB, 0xFF660098, 0xFF660066, 0xFF660033, 0xFF660000, 0xFF3300FF,
            0xFF3300CB, 0xFF330098, 0xFF330066, 0xFF330033, 0xFF330000, 0xFFFFCB33, 0xFFFFCB00, 0xFFFFFF00, 0xFFFFFF33,
            0xFFFFFF66, 0xFFFFFF98, 0xFFFFFFCB, 0x00FFFFFF
        };
    }
}