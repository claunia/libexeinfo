//
// Resources.cs
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
// FITPESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONPECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

namespace libexeinfo.Os2
{
    public static class Resources
    {
        /// <summary>
        ///     Gets the name of a resource type according to its identifier
        /// </summary>
        /// <returns>The resource type name.</returns>
        /// <param name="id">Resource type identifier.</param>
        public static string IdToName(ushort id)
        {
            switch(id)
            {
                case (int)ResourceTypes.RT_POINTER:      return "RT_POINTER";
                case (int)ResourceTypes.RT_BITMAP:       return "RT_BITMAP";
                case (int)ResourceTypes.RT_MENU:         return "RT_MENU";
                case (int)ResourceTypes.RT_DIALOG:       return "RT_DIALOG";
                case (int)ResourceTypes.RT_STRING:       return "RT_STRING";
                case (int)ResourceTypes.RT_FONTDIR:      return "RT_FONTDIR";
                case (int)ResourceTypes.RT_FONT:         return "RT_FONT";
                case (int)ResourceTypes.RT_ACCELTABLE:   return "RT_ACCELTABLE";
                case (int)ResourceTypes.RT_RCDATA:       return "RT_RCDATA";
                case (int)ResourceTypes.RT_MESSAGE:      return "RT_MESSAGE";
                case (int)ResourceTypes.RT_DLGINCLUDE:   return "RT_DLGINCLUDE";
                case (int)ResourceTypes.RT_VKEYTBL:      return "RT_VKEYTBL";
                case (int)ResourceTypes.RT_KEYTBL:       return "RT_KEYTBL";
                case (int)ResourceTypes.RT_CHARTBL:      return "RT_CHARTBL";
                case (int)ResourceTypes.RT_DISPLAYINFO:  return "RT_DISPLAYINFO";
                case (int)ResourceTypes.RT_FKASHORT:     return "RT_FKASHORT";
                case (int)ResourceTypes.RT_FKALONG:      return "RT_FKALONG";
                case (int)ResourceTypes.RT_HELPTABLE:    return "RT_HELPTABLE";
                case (int)ResourceTypes.RT_HELPSUBTABLE: return "RT_HELPSUBTABLE";
                case (int)ResourceTypes.RT_FDDIR:        return "RT_FDDIR";
                case (int)ResourceTypes.RT_FD:           return "RT_FD";
                default:                                 return $"{id}";
            }
        }
    }
}