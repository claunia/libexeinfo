//
// PanelGemImage.xeto.cs
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

using System.Collections.Generic;
using Eto.Drawing;
using Eto.Forms;
using Eto.Serialization.Xaml;

namespace exeinfogui.GEM
{
    public class PanelGemImage : Panel
    {
        ImageView imgImage;
        TextBox   txtColor;
        TextBox   txtCoordinates;
        TextBox   txtFlags;
        TextBox   txtSize;
        TextBox   txtState;

        public PanelGemImage()
        {
            XamlReader.Load(this);
        }

        public void Update(libexeinfo.GEM.TreeObjectNode node)
        {
            txtFlags.Text       = node.flags == 0 ? "None" : node.flags.ToString();
            txtState.Text       = node.state == 0 ? "Normal" : node.state.ToString();
            txtCoordinates.Text = $"{node.BitBlock.X},{node.BitBlock.Y}";
            txtSize.Text        = $"{node.BitBlock.Width}x{node.BitBlock.Height} pixels";
            txtColor.Text       = $"{node.BitBlock.Color}";
            imgImage.Image      = GemImageToEto(node);
        }

        static Bitmap GemImageToEto(libexeinfo.GEM.TreeObjectNode node)
        {
            Color color = GemColor.GemToEtoColor(node.BitBlock.Color);
            Color background;
            background = GemColor.GemToEtoColor(node.BitBlock.Color == libexeinfo.GEM.ObjectColors.White
                                                    ? libexeinfo.GEM.ObjectColors.Black
                                                    : libexeinfo.GEM.ObjectColors.White);

            List<Color> pixels = new List<Color>();

            byte[] data = new byte[node.BitBlock.Data.Length];
            int    pos  = 0;
            int    w    = (int)(node.BitBlock.Width / 8);
            // This flips the image.
            while(pos < data.Length)
            {
                for(int i = 0; i < w; i++)
                {
                    byte b = node.BitBlock.Data[pos + i];
                    data[pos                        + i] =  (byte)(b  >> 7);
                    data[pos                        + i] += (byte)((b >> 5) & 0x02);
                    data[pos                        + i] += (byte)((b >> 3) & 0x04);
                    data[pos                        + i] += (byte)((b >> 1) & 0x08);
                    data[pos                        + i] += (byte)((b << 1) & 0x10);
                    data[pos                        + i] += (byte)((b << 3) & 0x20);
                    data[pos                        + i] += (byte)((b << 5) & 0x40);
                    data[pos                        + i] += (byte)(b  << 7);
                }

                pos += w;
            }

            foreach(byte b in data)
                for(int i = 0; i              < 8; i++)
                    pixels.Add((b & (1 << i)) != 0 ? color : background);

            return new Bitmap((int)node.BitBlock.Width, (int)node.BitBlock.Height, PixelFormat.Format32bppRgb, pixels);
        }
    }
}