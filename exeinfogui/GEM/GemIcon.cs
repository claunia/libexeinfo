//
// GemIcon.cs
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

using System.Collections.Generic;
using Eto.Drawing;

namespace exeinfogui.GEM
{
    public static class GemIcon
    {
        public static Bitmap GemIconToEto(libexeinfo.GEM.Icon icon)
        {
            const uint COLOR      = 0x00000000;
            const uint BACKGROUND = 0x00FFFFFF;
            const uint ALPHAMASK  = 0xFF000000;
            List<int>  pixels     = new List<int>();

            byte[] data = libexeinfo.GEM.FlipPlane(icon.Data, (int)icon.Width);
            byte[] mask = libexeinfo.GEM.FlipPlane(icon.Mask, (int)icon.Width);

            for(int pos = 0; pos < data.Length; pos++)
            {
                for(int i = 0; i                             < 8; i++)
                    pixels.Add((int)(((data[pos] & (1 << i)) != 0 ? COLOR : BACKGROUND) +
                                     ((mask[pos] & (1 << i)) != 0 ? ALPHAMASK : 0)));
            }

            return new Bitmap((int)icon.Width, (int)icon.Height, PixelFormat.Format32bppRgba, pixels);
        }
    }
}