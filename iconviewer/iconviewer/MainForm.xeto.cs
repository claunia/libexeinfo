using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Eto.Forms;
using Eto.Drawing;
using Eto.Serialization.Xaml;
using Bitmap = libexeinfo.Os2.Bitmap;

namespace iconviewer
{
    public class MainForm : Form
    {
        ImageView imgIcon;
        TextBox txtPath;
        
        public MainForm()
        {
            XamlReader.Load(this);
        }

        protected void OnBtnPathClick(object sender, EventArgs e)
        {
            OpenFileDialog dlgOpenFileDialog = new OpenFileDialog {MultiSelect = false};
            dlgOpenFileDialog.Filters.Add(new FileFilter {Extensions = new[] {".ico"}});
            DialogResult result = dlgOpenFileDialog.ShowDialog(this);

            if(result != DialogResult.Ok)
            {
                txtPath.Text = "";
                imgIcon.Image = null;
                return;
            }

            txtPath.Text = dlgOpenFileDialog.FileName;
            FileStream fstream = new FileStream(dlgOpenFileDialog.FileName, FileMode.Open);
            byte[] data = new byte[fstream.Length];
            fstream.Read(data, 0, data.Length);
            fstream.Dispose();

            Bitmap.DecodedBitmap[] icons = libexeinfo.Os2.Bitmap.DecodeBitmap(data);
            imgIcon.Image = new Eto.Drawing.Bitmap((int)icons[0].Width, (int)icons[0].Height, PixelFormat.Format32bppRgba,
                                                   icons[0].Pixels);
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
