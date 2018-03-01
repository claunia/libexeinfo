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
using System.Linq;
using exeinfogui.GEM;
using Eto.Forms;
using Eto.Serialization.Xaml;
using libexeinfo;

namespace exeinfogui
{
    public class MainForm : Form
    {
        ComboBox        cmbArch;
        Label           lblSubsystem;
        TabGemResources tabGemResources;
        TabControl      tabMain;
        TabPageStrings  tabStrings;
        TextBox         txtFile;
        TextArea        txtInformation;
        TextBox         txtOs;
        TextBox         txtSubsystem;
        TextBox         txtType;

        public MainForm()
        {
            XamlReader.Load(this);

            tabStrings      = new TabPageStrings {Visible  = false};
            tabGemResources = new TabGemResources {Visible = false};
            tabMain.Pages.Add(tabStrings);
            tabMain.Pages.Add(tabGemResources);
        }

        protected void OnBtnLoadClick(object sender, EventArgs e)
        {
            txtFile.Text        = "";
            txtType.Text        = "";
            txtInformation.Text = "";
            cmbArch.Items.Clear();
            lblSubsystem.Visible    = false;
            txtSubsystem.Visible    = false;
            tabStrings.Visible      = false;
            tabGemResources.Visible = false;

            OpenFileDialog dlgOpen = new OpenFileDialog {Title = "Choose executable file", MultiSelect = false};

            if(dlgOpen.ShowDialog(this) != DialogResult.Ok) return;

            txtFile.Text = dlgOpen.FileName;

            IExecutable mzExe         = new MZ(dlgOpen.FileName);
            IExecutable neExe         = new NE(dlgOpen.FileName);
            IExecutable stExe         = new AtariST(dlgOpen.FileName);
            IExecutable lxExe         = new LX(dlgOpen.FileName);
            IExecutable coffExe       = new COFF(dlgOpen.FileName);
            IExecutable peExe         = new PE(dlgOpen.FileName);
            IExecutable recognizedExe = null;

            if(mzExe.Recognized)
            {
                recognizedExe = mzExe;
                if(((MZ)mzExe).ResourceObjectRoots != null && ((MZ)mzExe).ResourceObjectRoots.Any())
                {
                    tabGemResources.Update(((MZ)mzExe).ResourceObjectRoots, ((MZ)mzExe).GemColorIcons);
                    tabGemResources.Visible = true;
                }
            }

            if(neExe.Recognized) recognizedExe = neExe;
            else if(lxExe.Recognized)
                recognizedExe = lxExe;
            else if(peExe.Recognized)
                recognizedExe = peExe;
            else if(stExe.Recognized)
            {
                recognizedExe = stExe;
                if(((AtariST)stExe).ResourceObjectRoots != null && ((AtariST)stExe).ResourceObjectRoots.Any())
                {
                    tabGemResources.Update(((AtariST)stExe).ResourceObjectRoots, ((AtariST)stExe).GemColorIcons);
                    tabGemResources.Visible = true;
                }
            }
            else if(coffExe.Recognized)
                recognizedExe = coffExe;
            else
                txtType.Text = "Format not recognized";

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

            if(recognizedExe.Strings != null && recognizedExe.Strings.Any())
            {
                tabStrings.Update(recognizedExe.Strings);
                tabStrings.Visible = true;
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