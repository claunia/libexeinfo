//
// PanelGemBox.xeto.cs
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

using Claunia.Encoding;
using Eto.Forms;
using Eto.Serialization.Xaml;

namespace exeinfogui.GEM
{
    public class PanelGemBox : Panel
    {
        Label   lblCharacter;
        Label   lblThickness;
        TextBox txtBorderColor;
        TextBox txtCharacter;
        TextBox txtCoordinates;
        TextBox txtFill;
        TextBox txtFlags;
        TextBox txtInsideColor;
        TextBox txtSize;
        TextBox txtState;
        TextBox txtTextColor;
        TextBox txtThickness;
        TextBox txtTransparency;

        public PanelGemBox()
        {
            XamlReader.Load(this);
        }

        public void Update(libexeinfo.GEM.TreeObjectNode node)
        {
            sbyte thickness = (sbyte)((node.data & 0xFF0000) >> 16);

            char character = Encoding.AtariSTEncoding.GetString(new[] {(byte)((node.data & 0xFF000000) >> 24)})[0];

            txtFlags.Text       = node.flags == 0 ? "None" : node.flags.ToString();
            txtState.Text       = node.state == 0 ? "Normal" : node.state.ToString();
            txtCoordinates.Text = $"{node.x},{node.y}";
            txtSize.Text        = $"{node.width}x{node.height} pixels";
            txtBorderColor.Text =
                ((libexeinfo.GEM.ObjectColors)((node.data & 0xFFFF & libexeinfo.GEM.BorderColorMask) >> 12)).ToString();
            txtFill.Text =
                ((libexeinfo.GEM.ObjectFillPattern)((node.data & 0xFFFF & libexeinfo.GEM.FillPatternMask) >> 4))
               .ToString();
            txtInsideColor.Text =
                ((libexeinfo.GEM.ObjectColors)((node.data & 0xFFFF & libexeinfo.GEM.InsideColorMask) >> 8)).ToString();
            txtTextColor.Text =
                ((libexeinfo.GEM.ObjectColors)((node.data & 0xFFFF & libexeinfo.GEM.TextColorMask) >> 8)).ToString();
            txtTransparency.Text = (node.data             & 0xFFFF & libexeinfo.GEM.TransparentColor) != 0
                                       ? "Transparent mode"
                                       : "Replace mode";
            if(thickness      < 0) txtThickness.Text = $"{thickness * -1} pixels outward";
            else if(thickness < 0)
                txtThickness.Text = $"{thickness} pixels inward";
            else
                txtThickness.Text = "None";
            txtCharacter.Text = new string(new []{character});

            if(node.type == libexeinfo.GEM.ObjectTypes.G_BOXCHAR)
            {
                txtCharacter.Visible = true;
                txtThickness.Visible = true;
                lblCharacter.Visible = true;
                lblThickness.Visible = true;
            }
            else
            {
                txtCharacter.Visible = false;
                txtThickness.Visible = false;
                lblCharacter.Visible = false;
                lblThickness.Visible = false;
            }
        }
    }
}