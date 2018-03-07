//
// StringHandlers.cs
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
using System.Text;

namespace libexeinfo
{
    public static class StringHandlers
    {
        /// <summary>
        ///     Converts a null-terminated (aka C string) ASCII byte array to a C# string
        /// </summary>
        /// <returns>The corresponding C# string</returns>
        /// <param name="CString">A null-terminated (aka C string) ASCII byte array</param>
        public static string CToString(byte[] CString)
        {
            return CToString(CString, Encoding.ASCII);
        }

        /// <summary>
        ///     Converts a null-terminated (aka C string) byte array with the specified encoding to a C# string
        /// </summary>
        /// <returns>The corresponding C# string</returns>
        /// <param name="CString">A null-terminated (aka C string) byte array in the specified encoding</param>
        /// <param name="encoding">Encoding.</param>
        /// <param name="twoBytes">Set if encoding uses 16-bit characters.</param>
        /// <param name="start">Start decodint at this position</param>
        public static string CToString(byte[] CString, Encoding encoding, bool twoBytes = false, int start = 0)
        {
            if(CString == null) return null;

            int len = 0;

            for(int i = start; i < CString.Length; i++)
            {
                if(CString[i] == 0)
                    if(twoBytes)
                    {
                        if(i + 1 < CString.Length && CString[i + 1] == 0)
                        {
                            len++;
                            break;
                        }

                        //                      if((i + 1) == CString.Length)
                        //                            break;
                    }
                    else
                        break;

                len++;
            }

            byte[] dest = new byte[len];
            Array.Copy(CString, start, dest, 0, len);

            return len == 0 ? "" : encoding.GetString(dest);
        }

        /// <summary>
        ///     Converts a length-prefixed (aka Pascal string) ASCII byte array to a C# string
        /// </summary>
        /// <returns>The corresponding C# string</returns>
        /// <param name="PascalString">A length-prefixed (aka Pascal string) ASCII byte array</param>
        public static string PascalToString(byte[] PascalString)
        {
            return PascalToString(PascalString, Encoding.ASCII);
        }

        /// <summary>
        ///     Converts a length-prefixed (aka Pascal string) ASCII byte array to a C# string
        /// </summary>
        /// <returns>The corresponding C# string</returns>
        /// <param name="PascalString">A length-prefixed (aka Pascal string) ASCII byte array</param>
        /// <param name="encoding">Encoding.</param>
        /// <param name="start">Start decodint at this position</param>
        public static string PascalToString(byte[] PascalString, Encoding encoding, int start = 0)
        {
            if(PascalString == null) return null;

            byte length = PascalString[start];
            int  len    = 0;

            for(int i = start + 1; i < length + 1 && i < PascalString.Length; i++)
            {
                if(PascalString[i] == 0) break;

                len++;
            }

            byte[] dest = new byte[len];
            Array.Copy(PascalString, start + 1, dest, 0, len);

            return len == 0 ? "" : encoding.GetString(dest);
        }

        /// <summary>
        ///     Converts a space (' ', 0x20, ASCII SPACE) padded ASCII byte array to a C# string
        /// </summary>
        /// <returns>The corresponding C# string</returns>
        /// <param name="SpacePaddedString">A space (' ', 0x20, ASCII SPACE) padded ASCII byte array</param>
        public static string SpacePaddedToString(byte[] SpacePaddedString)
        {
            return SpacePaddedToString(SpacePaddedString, Encoding.ASCII);
        }

        /// <summary>
        ///     Converts a space (' ', 0x20, ASCII SPACE) padded ASCII byte array to a C# string
        /// </summary>
        /// <returns>The corresponding C# string</returns>
        /// <param name="SpacePaddedString">A space (' ', 0x20, ASCII SPACE) padded ASCII byte array</param>
        /// <param name="encoding">Encoding.</param>
        /// <param name="start">Start decodint at this position</param>
        public static string SpacePaddedToString(byte[] SpacePaddedString, Encoding encoding, int start = 0)
        {
            if(SpacePaddedString == null) return null;

            int len = start;

            for(int i = SpacePaddedString.Length; i >= start; i--)
            {
                if(i == start) return "";

                if(SpacePaddedString[i - 1] == 0x20) continue;

                len = i;
                break;
            }

            return len == 0 ? "" : encoding.GetString(SpacePaddedString, start, len);
        }

        /// <summary>
        ///     Converts an OSTA compressed unicode byte array to a C# string
        /// </summary>
        /// <returns>The C# string.</returns>
        /// <param name="dstring">OSTA compressed unicode byte array.</param>
        public static string DecompressUnicode(byte[] dstring)
        {
            ushort unicode;
            byte   compId = dstring[0];
            string temp   = "";

            if(compId != 8 && compId != 16) return null;

            for(int byteIndex = 1; byteIndex < dstring.Length;)
            {
                if(compId == 16) unicode = (ushort)(dstring[byteIndex++] << 8);
                else unicode             = 0;

                if(byteIndex < dstring.Length) unicode |= dstring[byteIndex++];

                if(unicode == 0) break;

                temp += Encoding.Unicode.GetString(BitConverter.GetBytes(unicode));
            }

            return temp;
        }
    }
}