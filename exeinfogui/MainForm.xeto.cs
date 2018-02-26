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
        ComboBox cmbArch;
        Label    lblSubsystem;
        TextBox  txtFile;
        TextArea txtInformation;
        TextBox  txtOs;
        TextBox  txtSubsystem;
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
            cmbArch.Items.Clear();
            lblSubsystem.Visible = false;
            txtSubsystem.Visible = false;

            OpenFileDialog dlgOpen = new OpenFileDialog {Title = "Choose executable file", MultiSelect = false};

            if(dlgOpen.ShowDialog(this) != DialogResult.Ok) return;

            txtFile.Text = dlgOpen.FileName;

            FileStream exeFs = File.Open(dlgOpen.FileName, FileMode.Open, FileAccess.Read);

            IExecutable mzExe         = new MZ(exeFs);
            IExecutable neExe         = new NE(exeFs);
            IExecutable stExe         = new AtariST(exeFs);
            IExecutable lxExe         = new LX(exeFs);
            IExecutable coffExe       = new COFF(exeFs);
            IExecutable peExe         = new PE(exeFs);
            IExecutable recognizedExe = null;

            if(mzExe.Recognized) recognizedExe = mzExe;

            if(neExe.Recognized) recognizedExe = neExe;
            else if(lxExe.Recognized)
                recognizedExe = lxExe;
            else if(peExe.Recognized)
                recognizedExe = peExe;
            else if(stExe.Recognized)
                recognizedExe = stExe;
            else if(coffExe.Recognized)
                recognizedExe = coffExe;
            else
                txtType.Text = "Format not recognized";

            exeFs.Close();

            if(recognizedExe == null) return;

            txtType.Text        = recognizedExe.Type;
            txtInformation.Text = recognizedExe.Information;
            foreach(Architecture arch in recognizedExe.Architectures)
                cmbArch.Items.Add(Enums.ArchitectureName.FirstOrDefault(ar => ar.arch == arch).longName);
            cmbArch.SelectedIndex = 0;

            if(recognizedExe.RequiredOperatingSystem.MajorVersion > 0)
                txtOs.Text = $"{recognizedExe.RequiredOperatingSystem.Name}"          +
                             $" {recognizedExe.RequiredOperatingSystem.MajorVersion}" +
                             $".{recognizedExe.RequiredOperatingSystem.MinorVersion}";
            else txtOs.Text = recognizedExe.RequiredOperatingSystem.Name;

            if(!string.IsNullOrEmpty(recognizedExe.RequiredOperatingSystem.Subsystem))
            {
                lblSubsystem.Visible = true;
                txtSubsystem.Visible = true;
                txtSubsystem.Text    = recognizedExe.RequiredOperatingSystem.Subsystem;
            }
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