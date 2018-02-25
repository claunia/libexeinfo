using System;
using System.IO;
using Eto.Forms;
using Eto.Serialization.Xaml;
using libexeinfo;

namespace exeinfogui
{
    public class MainForm : Form
    {
        TextBox txtFile;
        TextBox txtType;
        TextArea txtInformation;

        public MainForm()
        {
            XamlReader.Load(this);
        }

        protected void OnBtnLoadClick(object sender, EventArgs e)
        {
            txtFile.Text = "";
            txtType.Text = "";
            txtInformation.Text ="";

            OpenFileDialog dlgOpen = new OpenFileDialog {Title = "Choose executable file", MultiSelect = false};

            if(dlgOpen.ShowDialog(this) != DialogResult.Ok) return;

            txtFile.Text = dlgOpen.FileName;

            FileStream exeFs = File.Open(dlgOpen.FileName, FileMode.Open, FileAccess.Read);

            MZ      mzExe   = new MZ(exeFs);
            NE      neExe   = new NE(exeFs);
            AtariST stExe   = new AtariST(exeFs);
            LX      lxExe   = new LX(exeFs);
            COFF    coffExe = new COFF(exeFs);
            PE      peExe   = new PE(exeFs);

            if(mzExe.IsMZ)
            {
                if(neExe.IsNE)
                {
                    txtType.Text        = "New Executable (NE)";
                    txtInformation.Text = neExe.GetInfo();
                }
                else if(lxExe.IsLX)
                {
                    txtType.Text        = "Linear eXecutable (LX)";
                    txtInformation.Text = lxExe.GetInfo();
                }
                else if(peExe.IsPE)
                {
                    txtType.Text        = "Portable Executable (PE)";
                    txtInformation.Text = peExe.GetInfo();
                }
                else
                    txtType.Text = "DOS Executable (MZ)";
                
                txtInformation.Text += mzExe.GetInfo();
            }
            else if(stExe.IsAtariST)
            {
                txtType.Text = "Atari ST executable";
                txtInformation.Text = stExe.GetInfo();
            }
            else if(coffExe.IsCOFF)
            {
                txtType.Text = "Common Object File Format (COFF)";
                txtInformation.Text = coffExe.GetInfo();
            }
            else
                txtType.Text = "Format not recognized";
        }

        protected void OnMnuAboutClick(object sender, EventArgs e)
        {
            new AboutDialog().ShowDialog(this);
        }

        protected void OnMnuQuitClick(object sender, EventArgs e)
        {
            Application.Instance.Quit();
        }
    }
}