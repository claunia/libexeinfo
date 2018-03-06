using Eto.Drawing;
using Eto.Forms;
using Eto.Serialization.Xaml;
using Bitmap = libexeinfo.Windows.Bitmap;

namespace exeinfogui.Windows
{
    public class PanelWindowsIcon : Panel
    {
        ImageView    imgIcon;
        Label        lblColors;
        Label        lblSize;
        PanelHexDump panelHexDump;
        Panel        pnlPanel;
        TextBox      txtColors;
        TextBox      txtSize;

        public PanelWindowsIcon()
        {
            XamlReader.Load(this);
            panelHexDump     = new PanelHexDump();
            pnlPanel.Content = panelHexDump;
        }

        public void Update(byte[] data)
        {
            if(data == null)
            {
                imgIcon.Image     = null;
                lblSize.Text      = "No data";
                lblColors.Visible = false;
                lblSize.Visible   = false;
                txtColors.Visible = false;
                txtSize.Visible   = false;
                pnlPanel.Visible  = false;
                return;
            }

            Bitmap.DecodedBitmap icon;

            try { icon   = Bitmap.DecodeIcon(data); }
            catch { icon = null; }

            if(icon == null)
            {
                imgIcon.Image     = null;
                lblSize.Text      = "Undecoded";
                lblColors.Visible = false;
                lblSize.Visible   = false;
                txtColors.Visible = false;
                txtSize.Visible   = false;
                pnlPanel.Visible  = true;
                panelHexDump.Update(data);
                return;
            }

            txtSize.Text   = $"{icon.Width}x{icon.Height} pixels";
            txtColors.Text = $"{1 << (int)icon.BitsPerPixel} ({icon.BitsPerPixel} bpp)";
            imgIcon.Image  =
                new Eto.Drawing.Bitmap((int)icon.Width, (int)icon.Height, PixelFormat.Format32bppRgba, icon.Pixels);

            lblSize.Text      = "Size";
            lblColors.Visible = true;
            lblSize.Visible   = true;
            txtColors.Visible = true;
            txtSize.Visible   = true;
            pnlPanel.Visible  = false;
        }
    }
}