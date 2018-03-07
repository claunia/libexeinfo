//
// GemColorIcon.cs
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

using Eto.Drawing;

namespace exeinfogui.GEM
{
    public static class GemColorIcon
    {
        public static Bitmap GemColorIconToEto(libexeinfo.GEM.ColorIconPlane icon, int width, int height, bool selected)
        {
            if(selected && icon.SelectedData == null) return null;

            byte[] data = selected ? icon.SelectedData : icon.Data;
            byte[] mask = selected ? icon.SelectedMask : icon.Mask;

            int[] pixels = libexeinfo.GEM.PlaneToRaster(data, mask, width, height, icon.Planes);

            return new Bitmap(width, height, PixelFormat.Format32bppRgba, pixels);
        }
    }
}