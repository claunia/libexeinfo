//
// Structs.cs
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

using System.Runtime.InteropServices;

namespace libexeinfo.Windows
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BitmapInfoHeader
    {
        public uint              HeaderSize;
        public uint              Width;
        public uint              Height;
        public ushort            Planes;
        public ushort            BitsPerPlane;
        public BitmapCompression BitmapCompression;
        public uint              ImageSize;
        public uint              HorizontalResolution;
        public uint              VerticalResolution;
        public uint              ColorsInPalette;
        public uint              ImportantColors;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Rgb
    {
        public byte Blue;
        public byte Green;
        public byte Red;
        public byte Reserved;
    }

    public class DecodedBitmap
    {
        public uint  BitsPerPixel;
        public uint  Height;
        public int[] Pixels;
        public uint  Width;
    }
}