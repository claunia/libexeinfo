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
using System.Linq;
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
        ComboBox cmbArch;

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

            IExecutable mzExe   = new MZ(exeFs);
            IExecutable neExe   = new NE(exeFs);
            IExecutable stExe   = new AtariST(exeFs);
            IExecutable lxExe   = new LX(exeFs);
            IExecutable coffExe = new COFF(exeFs);
            IExecutable peExe   = new PE(exeFs);

            if(mzExe.Recognized)
            {
                if(neExe.Recognized)
                {
                    txtType.Text        = neExe.Type;
                    txtInformation.Text = neExe.Information;
                    foreach(Architecture arch in neExe.Architectures)
                        cmbArch.Items.Add(Enums.ArchitectureName.FirstOrDefault(t => t.arch == arch).longName);
                }
                else if(lxExe.Recognized)
                {
                    txtType.Text        = lxExe.Type;
                    txtInformation.Text = lxExe.Information;
                    foreach(Architecture arch in lxExe.Architectures)
                        cmbArch.Items.Add(Enums.ArchitectureName.FirstOrDefault(t => t.arch == arch).longName);
                }
                else if(peExe.Recognized)
                {
                    txtType.Text        = peExe.Type;
                    txtInformation.Text = peExe.Information;
                    foreach(Architecture arch in peExe.Architectures)
                        cmbArch.Items.Add(Enums.ArchitectureName.FirstOrDefault(t => t.arch == arch).longName);
                }
                else
                {
                    txtType.Text = mzExe.Type;
                    foreach(Architecture arch in mzExe.Architectures)
                        cmbArch.Items.Add(Enums.ArchitectureName.FirstOrDefault(t => t.arch == arch).longName);
                }

                txtInformation.Text += mzExe.Information;
            }
            else if(stExe.Recognized)
            {
                txtType.Text        = stExe.Type;
                txtInformation.Text = stExe.Information;
                foreach(Architecture arch in stExe.Architectures)
                    cmbArch.Items.Add(Enums.ArchitectureName.FirstOrDefault(t => t.arch == arch).longName);
            }
            else if(coffExe.Recognized)
            {
                txtType.Text        = coffExe.Type;
                txtInformation.Text = coffExe.Information;
                foreach(Architecture arch in coffExe.Architectures)
                    cmbArch.Items.Add(Enums.ArchitectureName.FirstOrDefault(t => t.arch == arch).longName);
            }
            else
                txtType.Text = "Format not recognized";
            
            cmbArch.SelectedIndex = 0;

            exeFs.Close();
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