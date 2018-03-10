using System.Collections.Generic;
using System.Linq;
using Eto.Drawing;
using Eto.Forms;
using Eto.Serialization.Xaml;
using libexeinfo.BeOS;

namespace exeinfogui.BeOS
{
    public class PanelBeIcon : Panel
    {
        ImageView    imgIcon;
        Label        lblSize;
        PanelHexDump panelHexDump;
        Panel        pnlPanel;
        TextBox      txtSize;

        public PanelBeIcon()
        {
            XamlReader.Load(this);
            panelHexDump     = new PanelHexDump();
            pnlPanel.Content = panelHexDump;
        }

        public void Update(byte[] data, string type)
        {
            bool recognized = type == Consts.B_LARGE_ICON_TYPE || type == Consts.B_MINI_ICON_TYPE ||
                              type == Consts.B_PNG_FORMAT;

            if(type == Consts.B_PNG_FORMAT)
            {
                Bitmap png = new Bitmap(data);
                if(png.Width == 0 || png.Height == 0)
                {
                    txtSize.Text  = $"{png.Width}x{png.Height} pixels";
                    imgIcon.Image = png;

                    lblSize.Text     = "Size";
                    lblSize.Visible  = true;
                    txtSize.Visible  = true;
                    pnlPanel.Visible = false;
                    return;
                }

                recognized = false;
            }

            if(data == null || !recognized)
            {
                imgIcon.Image    = null;
                lblSize.Text     = "No data";
                lblSize.Visible  = false;
                txtSize.Visible  = false;
                pnlPanel.Visible = false;
                panelHexDump.Update(data);
                return;
            }

            int width  = type == Consts.B_LARGE_ICON_TYPE ? 32 : 16;
            int height = width;

            List<int> pixels = data.Select(p => (int)Consts.ArgbSystemPalette[p]).ToList();

            txtSize.Text  = $"{width}x{height} pixels";
            imgIcon.Image = new Bitmap(width, height, PixelFormat.Format32bppRgba, pixels);

            lblSize.Text     = "Size";
            lblSize.Visible  = true;
            txtSize.Visible  = true;
            pnlPanel.Visible = false;
        }
    }
}