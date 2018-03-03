﻿//
// PanelNeStrings.xeto.cs
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

using Eto.Forms;
using Eto.Serialization.Xaml;

namespace exeinfogui.NE
{
    public class PanelNeStrings : Panel
    {
        GridView treeStrings;

        public PanelNeStrings()
        {
            XamlReader.Load(this);

            treeStrings.Columns.Add(new GridColumn
            {
                DataCell   = new TextBoxCell {Binding = Binding.Property<string, string>(r => r)},
                HeaderText = "String"
            });
        }

        public void Update(byte[] data, libexeinfo.NE.TargetOS targetOs)
        {
            treeStrings.DataStore = targetOs == libexeinfo.NE.TargetOS.OS2
                                        ? libexeinfo.NE.GetOs2Strings(data)
                                        : libexeinfo.NE.GetWindowsStrings(data);
        }
    }
}