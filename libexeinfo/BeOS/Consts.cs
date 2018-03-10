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
    }
}