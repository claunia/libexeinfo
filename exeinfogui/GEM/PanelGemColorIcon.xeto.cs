//
// PanelGemColorIcon.xeto.cs
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
using Eto.Forms;
using Eto.Serialization.Xaml;

namespace exeinfogui.GEM
{
    public class PanelGemColorIcon : Panel
    {
        int iconHeight;

        int       iconWidth;
        ImageView imgColorIcon;
        ImageView imgIcon;
        ImageView imgSelectedIcon;
        GridView  treePlanes;
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

        public PanelGemColorIcon()
        {
            XamlReader.Load(this);

            treePlanes.Columns.Add(new GridColumn
            {
                DataCell = new TextBoxCell
                {
                    Binding = Binding.Property<libexeinfo.GEM.ColorIconPlane, string>(i => $"{i.Planes}")
                },
                HeaderText = "Planes"
            });
            treePlanes.AllowMultipleSelection =  false;
            treePlanes.SelectedItemsChanged   += TreePlanesOnSelectedItemsChanged;
        }

        void TreePlanesOnSelectedItemsChanged(object sender, EventArgs eventArgs)
        {
            if(!(treePlanes.SelectedItem is libexeinfo.GEM.ColorIconPlane cicon))
            {
                imgColorIcon.Image    = null;
                imgSelectedIcon.Image = null;
                return;
            }

            imgColorIcon.Image    = GemColorIcon.GemColorIconToEto(cicon, iconWidth, iconHeight, false);
            imgSelectedIcon.Image = GemColorIcon.GemColorIconToEto(cicon, iconWidth, iconHeight, true);
        }

        public void Update(libexeinfo.GEM.TreeObjectNode node, libexeinfo.GEM.ColorIcon colorIcon)
        {
            txtFlags.Text           = node.flags == 0 ? "None" : node.flags.ToString();
            txtState.Text           = node.state == 0 ? "Normal" : node.state.ToString();
            txtCoordinates.Text     = $"{colorIcon.Monochrome.X},{colorIcon.Monochrome.Y}";
            txtSize.Text            = $"{colorIcon.Monochrome.Width}x{colorIcon.Monochrome.Height} pixels";
            txtCharater.Text        = $"{colorIcon.Monochrome.Character}";
            txtCharCoordinates.Text = $"{colorIcon.Monochrome.CharX},{colorIcon.Monochrome.CharY}";
            txtFgColor.Text         = $"{colorIcon.Monochrome.ForegroundColor}";
            txtBgColor.Text         = $"{colorIcon.Monochrome.BackgroundColor}";
            txtTextCoordinates.Text = $"{colorIcon.Monochrome.TextX},{colorIcon.Monochrome.TextY}";
            txtTextBoxSize.Text     = $"{colorIcon.Monochrome.TextWidth}x{colorIcon.Monochrome.TextHeight} pixels";
            txtText.Text            = colorIcon.Monochrome.Text;
            imgIcon.Image           = GemIcon.GemIconToEto(colorIcon.Monochrome);
            treePlanes.DataStore    = colorIcon.Color;
            iconWidth               = (int)colorIcon.Monochrome.Width;
            iconHeight              = (int)colorIcon.Monochrome.Height;
            treePlanes.SelectRow(0);
            if(colorIcon.Color != null && colorIcon.Color.Length >= 1 && colorIcon.Color[0] != null)
            {
                imgColorIcon.Image =
                    GemColorIcon.GemColorIconToEto(colorIcon.Color[0],                     iconWidth, iconHeight, false);
                imgSelectedIcon.Image = GemColorIcon.GemColorIconToEto(colorIcon.Color[0], iconWidth, iconHeight, true);
            }
            else
            {
                imgColorIcon.Image    = null;
                imgSelectedIcon.Image = null;
            }
        }
    }
}