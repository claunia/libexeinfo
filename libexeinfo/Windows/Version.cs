//
// Version.cs
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
    public static class Version
    {
        /// <summary>
        ///     Converts a <see cref="VersionFileType" /> to string
        /// </summary>
        /// <returns>The string.</returns>
        /// <param name="type">
        ///     <see cref="VersionFileType" />
        /// </param>
        public static string TypeToString(VersionFileType type)
        {
            switch(type)
            {
                case VersionFileType.VFT_APP:        return "Application";
                case VersionFileType.VFT_DLL:        return "Dynamic-link library";
                case VersionFileType.VFT_DRV:        return "Device driver";
                case VersionFileType.VFT_FONT:       return "Font";
                case VersionFileType.VFT_STATIC_LIB: return "Static-link library";
                case VersionFileType.VFT_UNKNOWN:    return "Unknown";
                case VersionFileType.VFT_VXD:        return "Virtual device";
                default:                             return $"Unknown type code {(uint)type}";
            }
        }

        /// <summary>
        ///     Converts a <see cref="VersionFileSubtype" /> to string, considering file type to be a driver
        /// </summary>
        /// <returns>The string.</returns>
        /// <param name="subtype">
        ///     <see cref="VersionFileSubtype" />
        /// </param>
        public static string DriverToString(VersionFileSubtype subtype)
        {
            switch(subtype)
            {
                case VersionFileSubtype.VFT2_DRV_COMM:              return "Communications";
                case VersionFileSubtype.VFT2_DRV_DISPLAY:           return "Display";
                case VersionFileSubtype.VFT2_DRV_INSTALLABLE:       return "Installable";
                case VersionFileSubtype.VFT2_DRV_KEYBOARD:          return "Keyboard";
                case VersionFileSubtype.VFT2_DRV_LANGUAGE:          return "Language";
                case VersionFileSubtype.VFT2_DRV_MOUSE:             return "Mouse";
                case VersionFileSubtype.VFT2_DRV_NETWORK:           return "Network";
                case VersionFileSubtype.VFT2_DRV_PRINTER:           return "Printer";
                case VersionFileSubtype.VFT2_DRV_SOUND:             return "Sound";
                case VersionFileSubtype.VFT2_DRV_SYSTEM:            return "System";
                case VersionFileSubtype.VFT2_DRV_VERSIONED_PRINTER: return "Versioned";
                case VersionFileSubtype.VFT2_UNKNOWN:               return "Unknown";
                default:                                            return $"Unknown type code {(uint)subtype}";
            }
        }

        /// <summary>
        ///     Converts a <see cref="VersionFileSubtype" /> to string, considering file type to be a font
        /// </summary>
        /// <returns>The string.</returns>
        /// <param name="subtype">
        ///     <see cref="VersionFileSubtype" />
        /// </param>
        public static string FontToString(VersionFileSubtype subtype)
        {
            switch(subtype)
            {
                case VersionFileSubtype.VFT2_FONT_RASTER:   return "Raster";
                case VersionFileSubtype.VFT2_FONT_TRUETYPE: return "TrueType";
                case VersionFileSubtype.VFT2_FONT_VECTOR:   return "Vector";
                case VersionFileSubtype.VFT2_UNKNOWN:       return "Unknown";
                default:                                    return $"Unknown type code {(uint)subtype}";
            }
        }

        /// <summary>
        ///     Converts a <see cref="VersionFileOS" /> to string
        /// </summary>
        /// <returns>The string.</returns>
        /// <param name="os">
        ///     <see cref="VersionFileOS" />
        /// </param>
        public static string OsToString(VersionFileOS os)
        {
            switch(os)
            {
                case VersionFileOS.VOS_DOS:             return "DOS";
                case VersionFileOS.VOS_NT:              return "Windows NT";
                case VersionFileOS.VOS_WINDOWS16:       return "16-bit Windows";
                case VersionFileOS.VOS_WINDOWS32:       return "32-bit Windows";
                case VersionFileOS.VOS_OS216:           return "16-bit OS/2";
                case VersionFileOS.VOS_OS232:           return "32-bit OS/2";
                case VersionFileOS.VOS_PM16:            return "16-bit Presentation Manager";
                case VersionFileOS.VOS_PM32:            return "32-bit Presentation Manager";
                case VersionFileOS.VOS_UNKNOWN:         return "Unknown";
                case VersionFileOS.VOS_DOS_NT:          return "DOS running under Windows NT";
                case VersionFileOS.VOS_DOS_WINDOWS16:   return "16-bit Windows running under DOS";
                case VersionFileOS.VOS_DOS_WINDOWS32:   return "32-bit Windows running under DOS";
                case VersionFileOS.VOS_DOS_PM16:        return "16-bit Presentation Manager running under DOS";
                case VersionFileOS.VOS_DOS_PM32:        return "32-bit Presentation Manager running under DOS";
                case VersionFileOS.VOS_NT_WINDOWS16:    return "16-bit Windows running under Windows NT";
                case VersionFileOS.VOS_NT_WINDOWS32:    return "32-bit Windows running under Windows NT";
                case VersionFileOS.VOS_NT_PM16:         return "16-bit Presentation Manager running under Windows NT";
                case VersionFileOS.VOS_NT_PM32:         return "32-bit Presentation Manager running under Windows NT";
                case VersionFileOS.VOS_OS216_WINDOWS16: return "16-bit Windows running under 16-bit OS/2";
                case VersionFileOS.VOS_OS216_WINDOWS32: return "32-bit Windows running under 16-bit OS/2";
                case VersionFileOS.VOS_OS216_PM16:      return "16-bit Presentation Manager running under 16-bit OS/2";
                case VersionFileOS.VOS_OS216_PM32:      return "32-bit Presentation Manager running under 16-bit OS/2";
                case VersionFileOS.VOS_OS232_WINDOWS16: return "16-bit Windows running under 32-bit OS/2";
                case VersionFileOS.VOS_OS232_WINDOWS32: return "32-bit Windows running under 32-bit OS/2";
                case VersionFileOS.VOS_OS232_PM16:      return "16-bit Presentation Manager running under 32-bit OS/2";
                case VersionFileOS.VOS_OS232_PM32:      return "32-bit Presentation Manager running under 32-bit OS/2";
                default:                                return $"Unknown OS code {(uint)os}";
            }
        }
    }
}