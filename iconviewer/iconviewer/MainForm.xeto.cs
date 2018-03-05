using System;
using System.IO;
using Eto.Drawing;
using Eto.Forms;
using Eto.Serialization.Xaml;
using Bitmap = libexeinfo.Os2.Bitmap;

namespace iconviewer
{
    public class MainForm : Form
    {
        GridView  grdIcons;
        ImageView imgIcon;
        TextBox   txtPath;

        public MainForm()
        {
            XamlReader.Load(this);

            grdIcons.Columns.Add(new GridColumn
            {
                DataCell                     =
                    new TextBoxCell {Binding = Binding.Property<Bitmap.DecodedBitmap, string>(b => $"{b.Type}")},
                HeaderText                   = "Command"
            });

            grdIcons.Columns.Add(new GridColumn
            {
                DataCell = new TextBoxCell
                {
                    Binding = Binding.Property<Bitmap.DecodedBitmap, string>(b => $"{b.Width}x{b.Height}")
                },
                HeaderText = "Size"
            });

            grdIcons.Columns.Add(new GridColumn
            {
                DataCell = new TextBoxCell
                {
                    Binding = Binding.Property<Bitmap.DecodedBitmap, string>(b => $"{1 << (int)b.BitsPerPixel}")
                },
                HeaderText = "Colors"
            });

            grdIcons.AllowMultipleSelection =  false;
            grdIcons.SelectionChanged       += GrdIconsOnSelectionChanged;
        }

        void GrdIconsOnSelectionChanged(object sender, EventArgs eventArgs)
        {
            if(!(grdIcons.SelectedItem is Bitmap.DecodedBitmap icon))
            {
                imgIcon.Image = null;
                return;
            }

            imgIcon.Image =
                new Eto.Drawing.Bitmap((int)icon.Width, (int)icon.Height, PixelFormat.Format32bppRgba, icon.Pixels);
        }

        protected void OnBtnPathClick(object sender, EventArgs e)
        {
            OpenFileDialog dlgOpenFileDialog                         = new OpenFileDialog {MultiSelect = false};
            dlgOpenFileDialog.Filters.Add(new FileFilter {Extensions = new[] {".ico"}});
            DialogResult result                                      = dlgOpenFileDialog.ShowDialog(this);

            if(result != DialogResult.Ok)
            {
                txtPath.Text       = "";
                imgIcon.Image      = null;
                grdIcons.DataStore = null;
                return;
            }

            txtPath.Text       = dlgOpenFileDialog.FileName;
            FileStream fstream = new FileStream(dlgOpenFileDialog.FileName, FileMode.Open);
            byte[]     data    = new byte[fstream.Length];
            fstream.Read(data, 0, data.Length);
            fstream.Dispose();

            Bitmap.DecodedBitmap[] icons = Bitmap.DecodeBitmap(data);
            imgIcon.Image                = new Eto.Drawing.Bitmap((int)icons[0].Width, (int)icons[0].Height,
                                                                  PixelFormat.Format32bppRgba, icons[0].Pixels);
            grdIcons.DataStore = icons;
            grdIcons.Visible   = icons.Length != 1;
        }

        protected void HandleAbout(object sender, EventArgs e)
        {
            new AboutDialog().ShowDialog(this);
        }

        protected void HandleQuit(object sender, EventArgs e)
        {
            Application.Instance.Quit();
        }
    }
}