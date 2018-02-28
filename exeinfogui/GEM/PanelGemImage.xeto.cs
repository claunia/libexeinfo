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
            int    w    = node.BitBlock.Width / 8;
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

            return new Bitmap(node.BitBlock.Width, node.BitBlock.Height, PixelFormat.Format32bppRgb, pixels);
        }
    }
}