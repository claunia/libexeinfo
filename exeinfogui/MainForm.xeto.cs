//
// MainForm.xeto.cs
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
using System.IO;
using Eto.Forms;
using Eto.Serialization.Xaml;
using libexeinfo;

namespace exeinfogui
{
    public class MainForm : Form
    {
        TextBox  txtFile;
        TextArea txtInformation;
        TextBox  txtType;

        public MainForm()
        {
            XamlReader.Load(this);
        }

        protected void OnBtnLoadClick(object sender, EventArgs e)
        {
            txtFile.Text        = "";
            txtType.Text        = "";
            txtInformation.Text = "";

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
                txtType.Text        = "Atari ST executable";
                txtInformation.Text = stExe.GetInfo();
            }
            else if(coffExe.IsCOFF)
            {
                txtType.Text        = "Common Object File Format (COFF)";
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