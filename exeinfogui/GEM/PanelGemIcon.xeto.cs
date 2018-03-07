//
// PanelGemIcon.xeto.cs
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

using Eto.Forms;
using Eto.Serialization.Xaml;

namespace exeinfogui.GEM
{
    public class PanelGemIcon : Panel
    {
        ImageView imgIcon;
        TextBox   txtBgColor;
        TextBox   txtCharater;
        TextBox   txtCharCoordinates;
        TextBox   txtCoordinates;
        TextBox   txtFgColor;
        TextBox   txtFlags;
        TextBox   txtSize;
        TextBox   txtState;
        TextBox   txtText;
        TextBox   txtTextBoxSize;
        TextBox   txtTextCoordinates;

        public PanelGemIcon()
        {
            XamlReader.Load(this);
        }

        public void Update(libexeinfo.GEM.TreeObjectNode node)
        {
            txtFlags.Text           = node.flags == 0 ? "None" : node.flags.ToString();
            txtState.Text           = node.state == 0 ? "Normal" : node.state.ToString();
            txtCoordinates.Text     = $"{node.IconBlock.X},{node.IconBlock.Y}";
            txtSize.Text            = $"{node.IconBlock.Width}x{node.IconBlock.Height} pixels";
            txtCharater.Text        = $"{node.IconBlock.Character}";
            txtCharCoordinates.Text = $"{node.IconBlock.CharX},{node.IconBlock.CharY}";
            txtFgColor.Text         = $"{node.IconBlock.ForegroundColor}";
            txtBgColor.Text         = $"{node.IconBlock.BackgroundColor}";
            txtTextCoordinates.Text = $"{node.IconBlock.TextX},{node.IconBlock.TextY}";
            txtTextBoxSize.Text     = $"{node.IconBlock.TextWidth}x{node.IconBlock.TextHeight} pixels";
            txtText.Text            = node.IconBlock.Text;
            imgIcon.Image           = GemIcon.GemIconToEto(node.IconBlock);
        }
    }
}