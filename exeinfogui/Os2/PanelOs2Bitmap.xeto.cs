using System;
using Eto.Drawing;
using Eto.Forms;
using Eto.Serialization.Xaml;
using libexeinfo.Os2;
using Bitmap = libexeinfo.Os2.Bitmap;

namespace exeinfogui.Os2
{
    public class PanelOs2Bitmap : Panel
    {
        GridView     grdIcons;
        ImageView    imgIcon;
        Label        lblColors;
        Label        lblSize;
        Label        lblType;
        PanelHexDump panelHexDump;
        Panel        pnlPanel;
        TextBox      txtColors;
        TextBox      txtSize;
        TextBox      txtType;

        public PanelOs2Bitmap()
        {
            XamlReader.Load(this);
            grdIcons.Columns.Add(new GridColumn
            {
                DataCell                     =
                    new TextBoxCell {Binding = Binding.Property<DecodedBitmap, string>(b => $"{b.Type}")},
                HeaderText                   = "Command"
            });

            grdIcons.Columns.Add(new GridColumn
            {
                DataCell = new TextBoxCell
                {
                    Binding = Binding.Property<DecodedBitmap, string>(b => $"{b.Width}x{b.Height}")
                },
                HeaderText = "Size"
            });

            grdIcons.Columns.Add(new GridColumn
            {
                DataCell = new TextBoxCell
                {
                    Binding = Binding.Property<DecodedBitmap, string>(b => $"{1 << (int)b.BitsPerPixel}")
                },
                HeaderText = "Colors"
            });

            grdIcons.AllowMultipleSelection =  false;
            grdIcons.SelectionChanged       += GrdIconsOnSelectionChanged;
            panelHexDump                    =  new PanelHexDump();
            pnlPanel.Content                =  panelHexDump;
        }

        void GrdIconsOnSelectionChanged(object sender, EventArgs eventArgs)
        {
            if(!(grdIcons.SelectedItem is DecodedBitmap icon))
            {
                imgIcon.Image = null;
                return;
            }

            txtType.Text   = icon.Type;
            txtSize.Text   = $"{icon.Width}x{icon.Height} pixels";
            txtColors.Text = $"{1 << (int)icon.BitsPerPixel} ({icon.BitsPerPixel} bpp)";
            imgIcon.Image  =
                new Eto.Drawing.Bitmap((int)icon.Width, (int)icon.Height, PixelFormat.Format32bppRgba, icon.Pixels);
        }

        public void Update(byte[] data)
        {
            if(data == null)
            {
                imgIcon.Image     = null;
                grdIcons.Visible  = false;
                lblType.Text      = "No data";
                lblColors.Visible = false;
                lblSize.Visible   = false;
                txtType.Visible   = false;
                txtColors.Visible = false;
                txtSize.Visible   = false;
                pnlPanel.Visible  = false;
                return;
            }

            DecodedBitmap[] icons = Bitmap.DecodeBitmap(data);

            if(icons == null || icons.Length == 0)
                try
                {
                    libexeinfo.Windows.DecodedBitmap winIcon = null;

                    if(BitConverter.ToUInt32(data, 4) == 40)
                    {
                        byte[] cursor = new byte[data.Length - 4];
                        Array.Copy(data, 4, cursor, 0, cursor.Length);
                        winIcon = libexeinfo.Windows.Bitmap.DecodeIcon(cursor);
                    }
                    else if(BitConverter.ToUInt32(data, 0) == 40)
                        winIcon = libexeinfo.Windows.Bitmap.DecodeIcon(data);

                    if(winIcon != null)
                        icons = new[]
                        {
                            new DecodedBitmap
                            {
                                BitsPerPixel = winIcon.BitsPerPixel,
                                Height       = winIcon.Height,
                                Pixels       = winIcon.Pixels,
                                Type         = "Windows cursor",
                                Width        = winIcon.Width
                            }
                        };
                }
                catch { icons = null; }

            if(icons == null || icons.Length == 0)
            {
                imgIcon.Image     = null;
                grdIcons.Visible  = false;
                lblType.Text      = "Undecoded";
                lblColors.Visible = false;
                lblSize.Visible   = false;
                txtType.Visible   = false;
                txtColors.Visible = false;
                txtSize.Visible   = false;
                pnlPanel.Visible  = true;
                panelHexDump.Update(data);
                return;
            }

            txtType.Text   = icons[0].Type;
            txtSize.Text   = $"{icons[0].Width}x{icons[0].Height} pixels";
            txtColors.Text = $"{1 << (int)icons[0].BitsPerPixel} ({icons[0].BitsPerPixel} bpp)";
            imgIcon.Image  = new Eto.Drawing.Bitmap((int)icons[0].Width, (int)icons[0].Height,
                                                    PixelFormat.Format32bppRgba, icons[0].Pixels);
            grdIcons.DataStore   = icons;
            grdIcons.SelectedRow = 0;
            grdIcons.Visible     = icons.Length != 1;

            lblType.Text      = "Type";
            lblColors.Visible = true;
            lblSize.Visible   = true;
            txtType.Visible   = true;
            txtColors.Visible = true;
            txtSize.Visible   = true;
            pnlPanel.Visible  = false;
        }
    }
}