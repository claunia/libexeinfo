//
// Bitmap.cs
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
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace libexeinfo.Windows
{
    public class Bitmap
    {
        public enum Compression : uint
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

        const int VISIBLE = -16777216;

        /// <summary>
        ///     This will decode an icon
        /// </summary>
        /// <param name="data">Data</param>
        /// <returns>A <see cref="DecodedBitmap" /> with the icon, null if the icon could not be decoded</returns>
        public static DecodedBitmap DecodeIcon(byte[] data)
        {
            long   pos    = 0;
            byte[] buffer = new byte[Marshal.SizeOf(typeof(BitmapInfoHeader))];
            Array.Copy(data, pos, buffer, 0, buffer.Length);
            BitmapInfoHeader bitmapFileHeader =
                BigEndianMarshal.ByteArrayToStructureLittleEndian<BitmapInfoHeader>(buffer);

            // Stop at unknown header
            if(bitmapFileHeader.HeaderSize != 40) return null;

            // Multiplanes not supported
            if(bitmapFileHeader.Planes != 1) return null;

            // TODO: Non paletted?
            pos           += bitmapFileHeader.HeaderSize;
            RGB[] palette = new RGB[1 << bitmapFileHeader.BitsPerPlane];
            buffer        = new byte[Marshal.SizeOf(typeof(RGB))];
            for(int i = 0; i < palette.Length; i++)
            {
                Array.Copy(data, pos, buffer, 0, buffer.Length);
                pos        += buffer.Length;
                palette[i] =  BigEndianMarshal.ByteArrayToStructureLittleEndian<RGB>(buffer);
            }

            // First let's do the icon itself
            bitmapFileHeader.Height /= 2;
            long dataLength         = 0;
            for(int y = 0; y < bitmapFileHeader.Height; y++)
            {
                int x = 0;
                while(x < bitmapFileHeader.Width)
                {
                    for(int k = 8 - bitmapFileHeader.BitsPerPlane; k >= 0; k -= (int)bitmapFileHeader.BitsPerPlane) x++;

                    dataLength++;
                }

                dataLength += dataLength % 2;
            }

            buffer = new byte[dataLength];
            Array.Copy(data, pos, buffer, 0, buffer.Length);

            DecodedBitmap icon = DecodeBitmap(bitmapFileHeader, palette, buffer);

            // Then the mask
            pos                           += dataLength;
            bitmapFileHeader.BitsPerPlane =  1;
            dataLength                    =  0;
            for(int y = 0; y < bitmapFileHeader.Height; y++)
            {
                int x = 0;
                while(x < bitmapFileHeader.Width)
                {
                    for(int k = 8 - bitmapFileHeader.BitsPerPlane; k >= 0; k -= (int)bitmapFileHeader.BitsPerPlane) x++;

                    dataLength++;
                }

                dataLength += dataLength % 2;
            }

            buffer = new byte[dataLength];
            Array.Copy(data, pos, buffer, 0, buffer.Length);

            DecodedBitmap mask = DecodeBitmap(bitmapFileHeader, palette, buffer);

            // Mask palette
            int[] argbPalette = new int[palette.Length];
            for(int c = 0; c < palette.Length; c++)
                argbPalette[c] = (palette[c].Red << 16) + (palette[c].Green << 8) + palette[c].Blue;

            DecodedBitmap bitmap = new DecodedBitmap
            {
                BitsPerPixel = icon.BitsPerPixel,
                Height       = icon.Height,
                Width        = icon.Width,
                Pixels       = new int[icon.Pixels.Length]
            };

            for(int px = 0; px                                         < bitmap.Pixels.Length; px++)
                bitmap.Pixels[px] = icon.Pixels[px] + (mask.Pixels[px] == argbPalette[0] ? VISIBLE : 0);

            // Need to reverse first all pixels then by line
            int[] pixels = bitmap.Pixels.Reverse().ToArray();
            for(int y = 0; y     < bitmap.Height; y++)
                for(int x = 0; x < bitmap.Width; x++)
                    bitmap.Pixels[y * bitmap.Width + (bitmap.Width - x - 1)] = pixels[y * bitmap.Width + x];

            return bitmap;
        }

        static DecodedBitmap DecodeBitmap(BitmapInfoHeader header, IList<RGB> palette, byte[] data)
        {
            if(header.Compression != Compression.None) return null;

            DecodedBitmap bitmap = new DecodedBitmap
            {
                BitsPerPixel = header.BitsPerPlane,
                Height       = header.Height,
                Width        = header.Width,
                Pixels       = new int[header.Width * header.Height]
            };

            int[] argbPalette = new int[palette.Count];

            for(int c = 0; c < palette.Count; c++)
                argbPalette[c] = (palette[c].Red << 16) + (palette[c].Green << 8) + palette[c].Blue;

            long pos = 0;

            for(int y = 0; y < bitmap.Height; y++)
            {
                int x = 0;
                while(x < bitmap.Width)
                {
                    for(int k = (int)(8 - bitmap.BitsPerPixel); k >= 0; k -= (int)bitmap.BitsPerPixel)
                    {
                        bitmap.Pixels[y * bitmap.Width                                      + x] =
                            argbPalette[(data[pos] >> k) & ((1 << (int)bitmap.BitsPerPixel) - 1)];
                        x++;

                        if(x == bitmap.Width) break;
                    }

                    pos++;
                }

                pos += pos % 2;
            }

            return bitmap;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BitmapInfoHeader
        {
            public uint        HeaderSize;
            public uint        Width;
            public uint        Height;
            public ushort      Planes;
            public ushort      BitsPerPlane;
            public Compression Compression;
            public uint        ImageSize;
            public uint        HorizontalResolution;
            public uint        VerticalResolution;
            public uint        ColorsInPalette;
            public uint        ImportantColors;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct RGB
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
}