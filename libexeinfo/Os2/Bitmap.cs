//
// Accelerator.cs
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

namespace libexeinfo.Os2
{
    public class Bitmap
    {
        /// <summary>
        ///     'IC', OS/2 only, icon
        /// </summary>
        public const ushort TYPE_ICON = 0x4349;
        /// <summary>
        ///     'BM', OS/2 and Windows, bitmap
        /// </summary>
        public const ushort TYPE_BITMAP = 0x4D42;
        /// <summary>
        ///     'PT', OS/2 only, cursor
        /// </summary>
        public const ushort TYPE_POINTER = 0x5450;
        /// <summary>
        ///     'CI', OS/2 only, color icon
        /// </summary>
        public const ushort TYPE_COLOR_ICON = 0x4943;
        /// <summary>
        ///     'CP', OS/2 only, color cursor
        /// </summary>
        public const ushort TYPE_COLOR_POINTER = 0x5043;
        /// <summary>
        ///     'BA', OS/2 only, bitmap array
        /// </summary>
        public const ushort TYPE_BITMAP_ARRAY = 0x4142;

        const int VISIBLE = -16777216;

        /// <summary>
        ///     This will decode a bitmap array, or if data is not an array, return an array with the single bitmap element
        /// </summary>
        /// <param name="data">Data</param>
        /// <returns>Array of <see cref="DecodedBitmap" />, one per bitmap, null if the bitmap could not be decoded</returns>
        public static DecodedBitmap[] DecodeBitmap(byte[] data)
        {
            long              pos = 0;
            BitmapArrayHeader bitmapArrayHeader;
            BitmapInfoHeader  bitmapFileHeader;
            byte[]            buffer = new byte[Marshal.SizeOf(typeof(BitmapInfoHeader))];

            Array.Copy(data, pos, buffer, 0, buffer.Length);
            bitmapArrayHeader = BigEndianMarshal.ByteArrayToStructureLittleEndian<BitmapArrayHeader>(buffer);
            bitmapFileHeader  = BigEndianMarshal.ByteArrayToStructureLittleEndian<BitmapInfoHeader>(buffer);

            List<DecodedBitmap> bitmaps = new List<DecodedBitmap>();

            do
            {
                Array.Copy(data, pos, buffer, 0, buffer.Length);
                bitmapArrayHeader = BigEndianMarshal.ByteArrayToStructureLittleEndian<BitmapArrayHeader>(buffer);
                long remaining;

                if(bitmapArrayHeader.Type == TYPE_BITMAP_ARRAY)
                {
                    remaining =  bitmapArrayHeader.Size - Marshal.SizeOf(typeof(BitmapArrayHeader));
                    pos       += Marshal.SizeOf(typeof(BitmapArrayHeader));
                }
                else
                {
                    remaining = 1;
                    pos       = 0;
                }

                while(remaining > 0)
                {
                    buffer = new byte[Marshal.SizeOf(typeof(BitmapInfoHeader))];
                    Array.Copy(data, pos, buffer, 0, buffer.Length);
                    bitmapFileHeader = BigEndianMarshal.ByteArrayToStructureLittleEndian<BitmapInfoHeader>(buffer);

                    // Stop at unknown header
                    if(bitmapFileHeader.Size != Marshal.SizeOf(typeof(BitmapInfoHeader))) break;

                    // Multiplanes not supported
                    if(bitmapFileHeader.Planes != 1) break;

                    // TODO: Non paletted?
                    pos           += bitmapFileHeader.Size;
                    RGB[] palette = new RGB[1 << bitmapFileHeader.BitsPerPlane];
                    buffer        = new byte[Marshal.SizeOf(typeof(RGB))];
                    for(int i = 0; i < palette.Length; i++)
                    {
                        Array.Copy(data, pos, buffer, 0, buffer.Length);
                        pos        += buffer.Length;
                        palette[i] =  BigEndianMarshal.ByteArrayToStructureLittleEndian<RGB>(buffer);
                    }

                    remaining -= bitmapFileHeader.Fix;
                    // rgb[1];
                    remaining -= 1;

                    // TODO (Optimize): Calculate real data length considering that every line is word-aligned (2-byte)
                    long dataLength = 0;
                    for(int y = 0; y < bitmapFileHeader.Y; y++)
                    {
                        int x = 0;
                        while(x < bitmapFileHeader.X)
                        {
                            for(int k = 8 - bitmapFileHeader.BitsPerPlane; k >= 0;
                                k -= (int)bitmapFileHeader.BitsPerPlane) x++;

                            dataLength++;
                        }

                        dataLength += dataLength % 2;
                    }

                    buffer = new byte[dataLength];
                    Array.Copy(data, bitmapFileHeader.Offset, buffer, 0, buffer.Length);

                    DecodedBitmap bitmap = DecodeBitmap(bitmapFileHeader, palette, buffer);

                    // Mask palette
                    int[] argbPalette = new int[palette.Length];
                    for(int c = 0; c < palette.Length; c++)
                        argbPalette[c] = (palette[c].Red << 16) + (palette[c].Green << 8) + palette[c].Blue;

                    // First image, then mask
                    if(bitmapFileHeader.Type == TYPE_ICON || bitmapFileHeader.Type == TYPE_POINTER)
                    {
                        int[] icon = new int[bitmap.Width * bitmap.Height / 2];
                        int[] mask = new int[bitmap.Width * bitmap.Height / 2];
                        Array.Copy(bitmap.Pixels, 0,           icon, 0, icon.Length);
                        Array.Copy(bitmap.Pixels, icon.Length, mask, 0, mask.Length);

                        bitmap.Pixels =  new int[bitmap.Width * bitmap.Height / 2];
                        bitmap.Height /= 2;
                        for(int px = 0; px                           < bitmap.Pixels.Length; px++)
                            bitmap.Pixels[px] = icon[px] + (mask[px] == argbPalette[0] ? VISIBLE : 0);
                    }
                    // We got the mask, now we need to read the color
                    else if(bitmapFileHeader.Type == TYPE_COLOR_ICON || bitmapFileHeader.Type == TYPE_COLOR_POINTER)
                    {
                        DecodedBitmap mask = bitmap;

                        buffer = new byte[Marshal.SizeOf(typeof(BitmapInfoHeader))];
                        Array.Copy(data, pos, buffer, 0, buffer.Length);
                        bitmapFileHeader = BigEndianMarshal.ByteArrayToStructureLittleEndian<BitmapInfoHeader>(buffer);

                        // Stop at unknown header
                        if(bitmapFileHeader.Size != Marshal.SizeOf(typeof(BitmapInfoHeader))) break;

                        // Multiplanes not supported
                        if(bitmapFileHeader.Planes != 1) break;

                        // TODO: Non paletted?
                        pos     += bitmapFileHeader.Size;
                        palette =  new RGB[1 << bitmapFileHeader.BitsPerPlane];
                        buffer  =  new byte[Marshal.SizeOf(typeof(RGB))];
                        for(int i = 0; i < palette.Length; i++)
                        {
                            Array.Copy(data, pos, buffer, 0, buffer.Length);
                            pos        += buffer.Length;
                            palette[i] =  BigEndianMarshal.ByteArrayToStructureLittleEndian<RGB>(buffer);
                        }

                        remaining -= bitmapFileHeader.Fix;
                        // rgb[1];
                        remaining -= 1;

                        // TODO (Optimize): Calculate real data length considering that every line is word-aligned (2-byte)
                        dataLength = 0;
                        for(int y = 0; y < bitmapFileHeader.Y; y++)
                        {
                            int x = 0;
                            while(x < bitmapFileHeader.X)
                            {
                                for(int k = 8 - bitmapFileHeader.BitsPerPlane; k >= 0;
                                    k -= (int)bitmapFileHeader.BitsPerPlane) x++;

                                dataLength++;
                            }

                            dataLength += dataLength % 2;
                        }

                        buffer = new byte[dataLength];
                        Array.Copy(data, bitmapFileHeader.Offset, buffer, 0, buffer.Length);

                        bitmap = DecodeBitmap(bitmapFileHeader, palette, buffer);

                        for(int px = 0; px < bitmap.Pixels.Length; px++)
                            bitmap.Pixels[px] = bitmap.Pixels[px] +
                                                (mask.Pixels[px   + bitmapFileHeader.X * bitmapFileHeader.Y] ==
                                                 argbPalette[0]
                                                     ? VISIBLE
                                                     : 0);
                    }
                    // Not an icon, all pixels are visible
                    else
                        for(int px = 0; px < bitmap.Pixels.Length; px++)
                            bitmap.Pixels[px] = bitmap.Pixels[px] + VISIBLE;

                    // OS/2 coordinate 0,0 is lower,left, so need to reverse first all pixels then by line
                    int[] pixels = bitmap.Pixels.Reverse().ToArray();
                    for(int y = 0; y     < bitmap.Height; y++)
                        for(int x = 0; x < bitmap.Width; x++)
                            bitmap.Pixels[y * bitmap.Width + (bitmap.Width - x - 1)] = pixels[y * bitmap.Width + x];

                    bitmaps.Add(bitmap);
                }

                pos = bitmapArrayHeader.Next;
            }
            while(bitmapArrayHeader.Next != 0);

            return bitmaps.ToArray();
        }

        // TODO: Mask is not correctly decoded on XGA icons (20x20 and 40x40)...
        static DecodedBitmap DecodeBitmap(BitmapInfoHeader header, IList<RGB> palette, byte[] data)
        {
            DecodedBitmap bitmap = new DecodedBitmap
            {
                BitsPerPixel = header.BitsPerPlane,
                Height       = header.Y,
                Width        = header.X,
                XHotspot     = header.XHotspot,
                YHostpot     = header.YHostpot,
                Pixels       = new int[header.X * header.Y]
            };

            switch(header.Type)
            {
                case TYPE_ICON:
                    bitmap.Type = "Icon";
                    break;
                case TYPE_BITMAP:
                    bitmap.Type = "Bitmap";
                    break;
                case TYPE_POINTER:
                    bitmap.Type = "Pointer";
                    break;
                case TYPE_COLOR_ICON:
                    bitmap.Type = "Color Icon";
                    break;
                case TYPE_COLOR_POINTER:
                    bitmap.Type = "Color Pointer";
                    break;
                default: return null;
            }

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
        public struct BitmapArrayHeader
        {
            public ushort Type;
            public uint   Size;
            public uint   Next;
            public ushort XDisplay;
            public ushort YDisplay;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BitmapInfoHeader
        {
            public ushort Type;
            public uint   Size;
            public short  XHotspot;
            public short  YHostpot;
            public uint   Offset;
            public uint   Fix;
            public ushort X;
            public ushort Y;
            public ushort Planes;
            public ushort BitsPerPlane;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct RGB
        {
            public byte Blue;
            public byte Green;
            public byte Red;
        }

        public class DecodedBitmap
        {
            public uint   BitsPerPixel;
            public uint   Height;
            public int[]  Pixels;
            public string Type;
            public uint   Width;
            public short  XHotspot;
            public short  YHostpot;
        }
    }
}