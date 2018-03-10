//
// MainForm.xeto.cs
//
// Author:
//       Natalia Portillo <claunia@claunia.com>
//
// Copyright (c) 2017-2018 Copyright © Claunia.com
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
using exeinfogui.BeOS;
using exeinfogui.GEM;
using exeinfogui.LE;
using exeinfogui.LX;
using exeinfogui.NE;
using exeinfogui.PE;
using Eto.Forms;
using Eto.Serialization.Xaml;
using libexeinfo;

namespace exeinfogui
{
    public class MainForm : Form
    {
        ComboBox        cmbArch;
        Label           lblSubsystem;
        TabBeResources  tabBeResources;
        TabGemResources tabGemResources;
        TabLeVxdVersion tabLeVxdVersion;
        TabLxResources  tabLxResources;
        TabControl      tabMain;
        TabNeResources  tabNeResources;
        TabPeResources  tabPeResources;
        TabPageSegments tabSegments;
        TabPageStrings  tabStrings;
        TextBox         txtFile;
        TextArea        txtInformation;
        TextBox         txtOs;
        TextBox         txtSubsystem;
        TextBox         txtType;

        public MainForm()
        {
            XamlReader.Load(this);

            tabSegments     = new TabPageSegments {Visible = false};
            tabStrings      = new TabPageStrings {Visible  = false};
            tabGemResources = new TabGemResources {Visible = false};
            tabNeResources  = new TabNeResources {Visible  = false};
            tabLeVxdVersion = new TabLeVxdVersion {Visible = false};
            tabLxResources  = new TabLxResources {Visible  = false};
            tabPeResources  = new TabPeResources {Visible  = false};
            tabBeResources  = new TabBeResources {Visible  = false};
            tabMain.Pages.Add(tabSegments);
            tabMain.Pages.Add(tabStrings);
            tabMain.Pages.Add(tabGemResources);
            tabMain.Pages.Add(tabNeResources);
            tabMain.Pages.Add(tabLeVxdVersion);
            tabMain.Pages.Add(tabLxResources);
            tabMain.Pages.Add(tabPeResources);
            tabMain.Pages.Add(tabBeResources);
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
            tabSegments.Visible     = false;
            tabNeResources.Visible  = false;
            tabLeVxdVersion.Visible = false;
            tabLxResources.Visible  = false;
            tabPeResources.Visible  = false;
            tabBeResources.Visible  = false;

            OpenFileDialog dlgOpen = new OpenFileDialog {Title = "Choose executable file", MultiSelect = false};

            if(dlgOpen.ShowDialog(this) != DialogResult.Ok) return;

            txtFile.Text        = dlgOpen.FileName;
            txtInformation.Text = "";
            txtOs.Text          = "";
            txtSubsystem.Text   = "";
            txtType.Text        = "";

            IExecutable mzExe         = new MZ(dlgOpen.FileName);
            IExecutable neExe         = new libexeinfo.NE(dlgOpen.FileName);
            IExecutable stExe         = new AtariST(dlgOpen.FileName);
            IExecutable lxExe         = new libexeinfo.LX(dlgOpen.FileName);
            IExecutable coffExe       = new COFF(dlgOpen.FileName);
            IExecutable peExe         = new libexeinfo.PE(dlgOpen.FileName);
            IExecutable geosExe       = new Geos(dlgOpen.FileName);
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

            if(neExe.Recognized)
            {
                recognizedExe = neExe;
                if(((libexeinfo.NE)neExe).Resources.types != null && ((libexeinfo.NE)neExe).Resources.types.Any())
                {
                    tabNeResources.Update(((libexeinfo.NE)neExe).Resources.types,
                                          ((libexeinfo.NE)neExe).Header.target_os);
                    tabNeResources.Visible = true;
                }
            }
            else if(lxExe.Recognized)
            {
                recognizedExe = lxExe;
                if(((libexeinfo.LX)lxExe).WinVersion != null)
                {
                    tabLeVxdVersion.Visible = true;
                    tabLeVxdVersion.Update(((libexeinfo.LX)lxExe).WinVersion);
                }

                if(((libexeinfo.LX)lxExe).neFormatResourceTable.types != null &&
                   ((libexeinfo.LX)lxExe).neFormatResourceTable.types.Any())
                {
                    tabLxResources.Update(((libexeinfo.LX)lxExe).neFormatResourceTable.types);
                    tabLxResources.Visible = true;
                }
            }
            else if(peExe.Recognized)
            {
                recognizedExe = peExe;
                if(((libexeinfo.PE)peExe).WindowsResourcesRoot?.children != null)
                {
                    tabPeResources.Update(((libexeinfo.PE)peExe).WindowsResourcesRoot);
                    tabPeResources.Visible = true;
                }

                if(((libexeinfo.PE)peExe).BeosResources != null)
                {
                    tabBeResources.Update(((libexeinfo.PE)peExe).BeosResources);
                    tabBeResources.Visible = true;
                }
            }
            else if(stExe.Recognized)
            {
                recognizedExe = stExe;
                if(((AtariST)stExe).ResourceObjectRoots != null && ((AtariST)stExe).ResourceObjectRoots.Any())
                {
                    tabGemResources.Update(((AtariST)stExe).ResourceObjectRoots, ((AtariST)stExe).GemColorIcons);
                    tabGemResources.Visible = true;
                }
            }
            else if(coffExe.Recognized) recognizedExe = coffExe;
            else if(geosExe.Recognized) recognizedExe = geosExe;
            else txtType.Text                         = "Format not recognized";

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

            if(recognizedExe.Segments != null && recognizedExe.Segments.Any())
            {
                tabSegments.Update(recognizedExe.Segments);
                tabSegments.Visible = true;
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