//
// PanelWin16Version.xeto.cs
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using Eto.Forms;
using Eto.Serialization.Xaml;

namespace exeinfogui.Win16
{
    public class PanelWin16Version : Panel
    {
        ObservableCollection<StrByLang> stringsByLanguage;
        GridView                        treeLanguages;
        GridView                        treeStrings;
        TextBox                         txtFileDate;
        TextBox                         txtFileFlags;
        TextBox                         txtFileOs;
        TextBox                         txtFileSubtype;
        TextBox                         txtFileType;
        TextBox                         txtFileVersion;
        TextBox                         txtProductVersion;

        public PanelWin16Version()
        {
            XamlReader.Load(this);

            treeLanguages.Columns.Add(new GridColumn
            {
                DataCell   = new TextBoxCell {Binding = Binding.Property<StrByLang, string>(r => r.Name)},
                HeaderText = "Language (codepage)"
            });

            treeStrings.Columns.Add(new GridColumn
            {
                DataCell   = new TextBoxCell {Binding = Binding.Property<Strings, string>(r => r.Key)},
                HeaderText = "Key"
            });

            treeStrings.Columns.Add(new GridColumn
            {
                DataCell   = new TextBoxCell {Binding = Binding.Property<Strings, string>(r => r.Value)},
                HeaderText = "Value"
            });

            stringsByLanguage                    =  new ObservableCollection<StrByLang>();
            treeLanguages.SelectionChanged       += TreeLanguagesOnSelectionChanged;
            treeLanguages.AllowMultipleSelection =  false;
        }

        void TreeLanguagesOnSelectionChanged(object sender, EventArgs eventArgs)
        {
            treeStrings.DataStore = null;
            if(!(treeLanguages.SelectedItem is StrByLang strs)) return;

            List<Strings> strings = new List<Strings>();
            foreach(KeyValuePair<string, string> kvp in strs.Strings)
                strings.Add(new Strings {Key = kvp.Key, Value = kvp.Value});

            treeStrings.DataStore = strings;
        }

        public void Update(byte[] data)
        {
            libexeinfo.NE.Version version = new libexeinfo.NE.Version(data);
            txtFileDate.Text              = version.FileDate  != new DateTime(1601, 1, 1) ? $"{version.FileDate}" : "Not set";
            txtFileFlags.Text             = version.FileFlags == 0 ? "Normal" : $"{version.FileFlags}";
            txtFileOs.Text                = libexeinfo.NE.Version.OsToString(version.FileOS);

            if(version.FileType == libexeinfo.NE.VersionFileType.VFT_DRV)
                txtFileSubtype.Text = $"{libexeinfo.NE.Version.DriverToString(version.FileSubtype)} driver";
            else if(version.FileType == libexeinfo.NE.VersionFileType.VFT_DRV)
                txtFileSubtype.Text = $"{libexeinfo.NE.Version.FontToString(version.FileSubtype)} font";
            else if(version.FileSubtype > 0)
                txtFileSubtype.Text = $"{(uint)version.FileSubtype}";
            else
                txtFileSubtype.Text = "None";

            txtFileType.Text       = libexeinfo.NE.Version.TypeToString(version.FileType);
            txtFileVersion.Text    = $"{version.FileVersion}";
            txtProductVersion.Text = $"{version.ProductVersion}";

            stringsByLanguage.Clear();

            foreach(KeyValuePair<string, Dictionary<string, string>> strByLang in version.StringsByLanguage)
            {
                string cultureName;
                string encodingName;

                try { cultureName   = new CultureInfo(Convert.ToInt32(strByLang.Key.Substring(0, 4), 16)).DisplayName; }
                catch { cultureName = $"0x{Convert.ToInt32(strByLang.Key.Substring(0,            4), 16):X4}"; }

                try
                {
                    encodingName = Encoding.GetEncoding(Convert.ToInt32(strByLang.Key.Substring(4), 16)).EncodingName;
                }
                catch { encodingName = $"0x{Convert.ToInt32(strByLang.Key.Substring(4), 16):X4}"; }

                stringsByLanguage.Add(new StrByLang
                {
                    Name    = $"{cultureName} ({encodingName})",
                    Strings = strByLang.Value
                });
            }

            treeLanguages.DataStore = stringsByLanguage;
        }

        class StrByLang
        {
            public Dictionary<string, string> Strings;
            public string                     Name { get; set; }
        }

        class Strings
        {
            public string Key   { get; set; }
            public string Value { get; set; }
        }
    }
}