//
// GemColor.cs
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
    public static class GemColor
    {
        internal static Color GemToEtoColor(libexeinfo.GEM.ObjectColors color)
        {
            switch(color)
            {
                case libexeinfo.GEM.ObjectColors.White: return Color.FromRgb((int)libexeinfo.GEM.ObjectColorsRgb.White);
                case libexeinfo.GEM.ObjectColors.Black: return Color.FromRgb((int)libexeinfo.GEM.ObjectColorsRgb.Black);
                case libexeinfo.GEM.ObjectColors.Red: return Color.FromRgb((int)libexeinfo.GEM.ObjectColorsRgb.Red);
                case libexeinfo.GEM.ObjectColors.Green: return Color.FromRgb((int)libexeinfo.GEM.ObjectColorsRgb.Green);
                case libexeinfo.GEM.ObjectColors.Blue: return Color.FromRgb((int)libexeinfo.GEM.ObjectColorsRgb.Blue);
                case libexeinfo.GEM.ObjectColors.Cyan: return Color.FromRgb((int)libexeinfo.GEM.ObjectColorsRgb.Cyan);
                case libexeinfo.GEM.ObjectColors.Yellow: return Color.FromRgb((int)libexeinfo.GEM.ObjectColorsRgb.Yellow);
                case libexeinfo.GEM.ObjectColors.Magenta: return Color.FromRgb((int)libexeinfo.GEM.ObjectColorsRgb.Magenta);
                case libexeinfo.GEM.ObjectColors.LightGray: return Color.FromRgb((int)libexeinfo.GEM.ObjectColorsRgb.LightGray);
                case libexeinfo.GEM.ObjectColors.Gray: return Color.FromRgb((int)libexeinfo.GEM.ObjectColorsRgb.Gray);
                case libexeinfo.GEM.ObjectColors.LightRed: return Color.FromRgb((int)libexeinfo.GEM.ObjectColorsRgb.LightRed);
                case libexeinfo.GEM.ObjectColors.LightGreen: return Color.FromRgb((int)libexeinfo.GEM.ObjectColorsRgb.LightGreen);
                case libexeinfo.GEM.ObjectColors.LightBlue: return Color.FromRgb((int)libexeinfo.GEM.ObjectColorsRgb.LightBlue);
                case libexeinfo.GEM.ObjectColors.LightCyan: return Color.FromRgb((int)libexeinfo.GEM.ObjectColorsRgb.LightCyan);
                case libexeinfo.GEM.ObjectColors.LightYellow: return Color.FromRgb((int)libexeinfo.GEM.ObjectColorsRgb.LightYellow);
                case libexeinfo.GEM.ObjectColors.LightMagenta: return Color.FromRgb((int)libexeinfo.GEM.ObjectColorsRgb.LightMagenta);
            }
            
            return Color.FromRgb(0);
        }
    }
}