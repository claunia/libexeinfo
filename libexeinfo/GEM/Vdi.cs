//
// Vdi.cs
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
    public static partial class GEM
    {
        /// <summary>Palette for monochrome</summary>
        public static int[] Palette1 = {0x00FFFFFF, 0x00000000};

        /// <summary>Palette for 2 planes (4 colors)</summary>
        public static int[] Palette2 = {0x00FFFFFF, 0x00FF0000, 0x0000FF00, 0x00000000};

        /// <summary>Palette for 3 planes (8 colors)</summary>
        public static int[] Palette3 =
            {0x00FFFFFF, 0x00FF0000, 0x0000FF00, 0x00FFFF00, 0x000000FF, 0x00FF00FF, 0x0000FFFF, 0x00000000};

        /// <summary>Palette for 4 planes (16 colors)</summary>
        public static int[] Palette4 =
        {
            // Atari TT palette
            0x00FFFFFF, 0x00FF0000, 0x0000FF00, 0x00FFFF00, 0x000000FF, 0x00FF00FF, 0x0000FFFF, 0x00AAAAAA, 0x00666666,
            0x00FF9999, 0x0099FF99, 0x00FFFF99, 0x009999FF, 0x00FF99FF, 0x0099FFFF, 0x00000000
            // Atari ST palette
            // 0x00FFFFFF, 0x00FF0000, 0x0000FF00, 0x00FFFF00, 0x000000FF, 0x00FF00FF, 0x0000FFFF, 0x00555555,
            // 0x00333333, 0x00FF3333, 0x0033FF33, 0x00FFFF33, 0x003333FF, 0x00FF33FF, 0x0033FFFF, 0x00000000
        };

        /// <summary>Palette for 8 planes (256 colors), from Atari TT</summary>
        public static int[] Palette8 =
        {
            0x00FFFFFF, 0x00FF0000, 0x0000FF00, 0x00FFFF00, 0x000000FF, 0x00FF00FF, 0x0000FFFF, 0x00AAAAAA, 0x00666666,
            0x00FF9999, 0x0099FF99, 0x00FFFF99, 0x009999FF, 0x00FF99FF, 0x0099FFFF, 0x00000000, 0x00FFFFFF, 0x00EEEEEE,
            0x00DDDDDD, 0x00CCCCCC, 0x00BBBBBB, 0x00AAAAAA, 0x00999999, 0x00888888, 0x00777777, 0x00666666, 0x00555555,
            0x00444444, 0x00333333, 0x00222222, 0x00111111, 0x00000000, 0x00FF0000, 0x00FF0011, 0x00FF0022, 0x00FF0033,
            0x00FF0044, 0x00FF0055, 0x00FF0066, 0x00FF0077, 0x00FF0088, 0x00FF0099, 0x00FF00AA, 0x00FF00BB, 0x00FF00CC,
            0x00FF00DD, 0x00FF00EE, 0x00FF00FF, 0x00EE00FF, 0x00DD00FF, 0x00CC00FF, 0x00BB00FF, 0x00AA00FF, 0x009900FF,
            0x008800FF, 0x007700FF, 0x006600FF, 0x005500FF, 0x004400FF, 0x003300FF, 0x002200FF, 0x001100FF, 0x000000FF,
            0x000011FF, 0x000022FF, 0x000033FF, 0x000044FF, 0x000055FF, 0x000066FF, 0x000077FF, 0x000088FF, 0x000099FF,
            0x0000AAFF, 0x0000BBFF, 0x0000CCFF, 0x0000DDFF, 0x0000EEFF, 0x0000FFFF, 0x0000FFEE, 0x0000FFDD, 0x0000FFCC,
            0x0000FFBB, 0x0000FFAA, 0x0000FF99, 0x0000FF88, 0x0000FF77, 0x0000FF66, 0x0000FF55, 0x0000FF44, 0x0000FF33,
            0x0000FF22, 0x0000FF11, 0x0000FF00, 0x0011FF00, 0x0022FF00, 0x0033FF00, 0x0044FF00, 0x0055FF00, 0x0066FF00,
            0x0077FF00, 0x0088FF00, 0x0099FF00, 0x00AAFF00, 0x00BBFF00, 0x00CCFF00, 0x00DDFF00, 0x00EEFF00, 0x00FFFF00,
            0x00FFEE00, 0x00FFDD00, 0x00FFCC00, 0x00FFBB00, 0x00FFAA00, 0x00FF9900, 0x00FF8800, 0x00FF7700, 0x00FF6600,
            0x00FF5500, 0x00FF4400, 0x00FF3300, 0x00FF2200, 0x00FF1100, 0x00BB0000, 0x00BB0011, 0x00BB0022, 0x00BB0033,
            0x00BB0044, 0x00BB0055, 0x00BB0066, 0x00BB0077, 0x00BB0088, 0x00BB0099, 0x00BB00AA, 0x00BB00BB, 0x00AA00BB,
            0x009900BB, 0x008800BB, 0x007700BB, 0x006600BB, 0x005500BB, 0x004400BB, 0x003300BB, 0x002200BB, 0x001100BB,
            0x000000BB, 0x000011BB, 0x000022BB, 0x000033BB, 0x000044BB, 0x000055BB, 0x000066BB, 0x000077BB, 0x000088BB,
            0x000099BB, 0x0000AABB, 0x0000BBBB, 0x0000BBAA, 0x0000BB99, 0x0000BB88, 0x0000BB77, 0x0000BB66, 0x0000BB55,
            0x0000BB44, 0x0000BB33, 0x0000BB22, 0x0000BB11, 0x0000BB00, 0x0011BB00, 0x0022BB00, 0x0033BB00, 0x0044BB00,
            0x0055BB00, 0x0066BB00, 0x0077BB00, 0x0088BB00, 0x0099BB00, 0x00AABB00, 0x00BBBB00, 0x00BBAA00, 0x00BB9900,
            0x00BB8800, 0x00BB7700, 0x00BB6600, 0x00BB5500, 0x00BB4400, 0x00BB3300, 0x00BB2200, 0x00BB1100, 0x00770000,
            0x00770011, 0x00770022, 0x00770033, 0x00770044, 0x00770055, 0x00770066, 0x00770077, 0x00660077, 0x00550077,
            0x00440077, 0x00330077, 0x00220077, 0x00110077, 0x00000077, 0x00001177, 0x00002277, 0x00003377, 0x00004477,
            0x00005577, 0x00006677, 0x00007777, 0x00007766, 0x00007755, 0x00007744, 0x00007733, 0x00007722, 0x00007711,
            0x00007700, 0x00117700, 0x00227700, 0x00337700, 0x00447700, 0x00557700, 0x00667700, 0x00777700, 0x00776600,
            0x00775500, 0x00774400, 0x00773300, 0x00772200, 0x00771100, 0x00440000, 0x00440011, 0x00440022, 0x00440033,
            0x00440044, 0x00330044, 0x00220044, 0x00110044, 0x00000044, 0x00001144, 0x00002244, 0x00003344, 0x00004444,
            0x00004433, 0x00004422, 0x00004411, 0x00004400, 0x00114400, 0x00224400, 0x00334400, 0x00444400, 0x00443300,
            0x00442200, 0x00441100, 0x00FFFFFF, 0x00000000
        };

        /// <summary>Palette for alpha mask</summary>
        public static int[] PaletteMask = {0x00000000, -16777216};

        public static byte[] FlipPlane(byte[] data, int width)
        {
            byte[] flipped = new byte[data.Length];
            int    pos     = 0;
            int    w       = width / 8;

            // This flips the image.
            while(pos < flipped.Length)
            {
                for(int i = 0; i < w; i++)
                {
                    byte b = data[pos + i];
                    flipped[pos       + i] =  (byte)(b  >> 7);
                    flipped[pos       + i] += (byte)((b >> 5) & 0x02);
                    flipped[pos       + i] += (byte)((b >> 3) & 0x04);
                    flipped[pos       + i] += (byte)((b >> 1) & 0x08);
                    flipped[pos       + i] += (byte)((b << 1) & 0x10);
                    flipped[pos       + i] += (byte)((b << 3) & 0x20);
                    flipped[pos       + i] += (byte)((b << 5) & 0x40);
                    flipped[pos       + i] += (byte)(b  << 7);
                }

                pos += w;
            }

            return flipped;
        }

        public static int[] PlaneToRaster(byte[] data, byte[] mask, int width, int height, int planes)
        {
            int[] pixels = PlaneToRaster(data, width, height, planes);
            int[] masked = PlaneToRasterIndexed(mask, width, height, 1);

            for(int i = 0; i < pixels.Length; i++) pixels[i] += PaletteMask[masked[i]];

            return pixels;
        }

        public static int[] PlaneToRaster(byte[] data, int width, int height, int planes)
        {
            int[] pixels  = PlaneToRasterIndexed(data, width, height, planes);
            int[] palette = null;

            switch(planes)
            {
                case 1:
                    palette = Palette1;
                    break;
                case 2:
                    palette = Palette2;
                    break;
                case 3:
                    palette = Palette3;
                    break;
                case 4:
                    palette = Palette4;
                    break;
                case 8:
                    palette = Palette8;
                    break;
                // What to do with other pixel formats?
                default: return null;
            }

            for(int i = 0; i < pixels.Length; i++) pixels[i] = palette[pixels[i]];

            return pixels;
        }

        public static int[] PlaneToRasterIndexed(byte[] data, int width, int height, int planes)
        {
            // No more than 24-bit RGB
            if(planes > 24) return null;

            int planeSize = width * height / 8;

            int[] pixels = new int[width * height];
            for(int p = 0; p < planes; p++)
            {
                int pixNum = 0;

                byte[] plane = new byte[planeSize];
                Array.Copy(data, p * planeSize, plane, 0, planeSize);
                plane = FlipPlane(plane, width);

                for(int b = 0; b < planeSize; b++)
                {
                    for(int i = 0; i < 8; i++) pixels[pixNum++] |= ((plane[b] & (1 << i)) >> i) << p;
                }
            }

            return pixels;
        }
    }
}