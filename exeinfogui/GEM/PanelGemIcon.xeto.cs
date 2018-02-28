using System.Collections.Generic;
using Eto.Drawing;
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
            imgIcon.Image           = GemIconToEto(node);
        }

        static Bitmap GemIconToEto(libexeinfo.GEM.TreeObjectNode node)
        {
            const uint COLOR      = 0x00000000;
            const uint BACKGROUND = 0x00FFFFFF;
            const uint ALPHAMASK  = 0xFF000000;
            List<int>  pixels     = new List<int>();

            byte[] data = new byte[node.IconBlock.Data.Length];
            int    pos  = 0;
            int    w    = node.IconBlock.Width / 8;
            // This flips the image.
            while(pos < data.Length)
            {
                for(int i = 0; i < w; i++)
                {
                    byte b = node.IconBlock.Data[pos + i];
                    data[pos                         + i] =  (byte)(b  >> 7);
                    data[pos                         + i] += (byte)((b >> 5) & 0x02);
                    data[pos                         + i] += (byte)((b >> 3) & 0x04);
                    data[pos                         + i] += (byte)((b >> 1) & 0x08);
                    data[pos                         + i] += (byte)((b << 1) & 0x10);
                    data[pos                         + i] += (byte)((b << 3) & 0x20);
                    data[pos                         + i] += (byte)((b << 5) & 0x40);
                    data[pos                         + i] += (byte)(b  << 7);
                }

                pos += w;
            }

            byte[] mask = new byte[node.IconBlock.Mask.Length];
            pos         = 0;
            // This flips the mask.
            while(pos < data.Length)
            {
                for(int i = 0; i < w; i++)
                {
                    byte b = node.IconBlock.Mask[pos + i];
                    mask[pos                         + i] =  (byte)(b  >> 7);
                    mask[pos                         + i] += (byte)((b >> 5) & 0x02);
                    mask[pos                         + i] += (byte)((b >> 3) & 0x04);
                    mask[pos                         + i] += (byte)((b >> 1) & 0x08);
                    mask[pos                         + i] += (byte)((b << 1) & 0x10);
                    mask[pos                         + i] += (byte)((b << 3) & 0x20);
                    mask[pos                         + i] += (byte)((b << 5) & 0x40);
                    mask[pos                         + i] += (byte)(b  << 7);
                }

                pos += w;
            }

            for(pos = 0; pos < data.Length; pos++)
            {
                for(int i = 0; i                             < 8; i++)
                    pixels.Add((int)(((data[pos] & (1 << i)) != 0 ? COLOR : BACKGROUND) +
                                     ((mask[pos] & (1 << i)) != 0 ? ALPHAMASK : 0)));
            }

            return new Bitmap(node.IconBlock.Width, node.IconBlock.Height, PixelFormat.Format32bppRgba, pixels);
        }
    }
}