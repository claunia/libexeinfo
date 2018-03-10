//
// TabBeResources.xeto.cs
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
using System.Linq;
using System.Text;
using Eto.Forms;
using Eto.Serialization.Xaml;
using libexeinfo.BeOS;

namespace exeinfogui.BeOS
{
    public class TabBeResources : TabPage
    {
        bool           bigEndian;
        PanelBeIcon    panelBeIcon;
        PanelBeVersion panelBeVersion;
        PanelHexDump   panelHexDump;
        PanelText      panelText;
        Panel          pnlResource;
        TreeGridView   treeResources;

        public TabBeResources()
        {
            XamlReader.Load(this);

            treeResources.Columns.Add(new GridColumn {HeaderText = "Name", DataCell = new TextBoxCell(0)});
            treeResources.Columns.Add(new GridColumn {HeaderText = "ID", DataCell   = new TextBoxCell(1)});
            treeResources.Columns.Add(new GridColumn {HeaderText = "Size", DataCell = new TextBoxCell(2)});

            treeResources.AllowMultipleSelection =  false;
            treeResources.SelectionChanged       += TreeResourcesOnSelectionChanged;

            panelHexDump   = new PanelHexDump();
            panelText      = new PanelText();
            panelBeIcon    = new PanelBeIcon();
            panelBeVersion = new PanelBeVersion();
        }

        public void Update(IEnumerable<ResourceTypeBlock> resources, bool bigEndian)
        {
            TreeGridItemCollection treeData = new TreeGridItemCollection();

            foreach(ResourceTypeBlock type in resources.OrderBy(r => r.type))
            {
                TreeGridItem root = new TreeGridItem {Values = new object[] {$"{type.type}", null, null, null, null}};

                foreach(Resource resource in type.resources.OrderBy(r => r.name).ThenBy(r => r.id).ThenBy(r => r.index))
                {
                    TreeGridItem child = new TreeGridItem
                    {
                        Values = new object[]
                        {
                            $"{resource.name}", $"{resource.id}",
                            resource.data == null ? null : $"{resource.data.Length}", $"{type.type}", resource.data
                        }
                    };

                    root.Children.Add(child);
                }

                treeData.Add(root);
            }

            treeResources.DataStore = treeData;
            this.bigEndian          = bigEndian;
        }

        void TreeResourcesOnSelectionChanged(object sender, EventArgs eventArgs)
        {
            if(((TreeGridItem)treeResources.SelectedItem)?.Values[4] == null)
            {
                pnlResource.Content = null;
                return;
            }

            byte[] data = ((TreeGridItem)treeResources.SelectedItem)?.Values[4] as byte[];
            string type = ((TreeGridItem)treeResources.SelectedItem)?.Values[3] as string;

            switch(type)
            {
                case Consts.B_MIME_STRING_TYPE:
                    pnlResource.Content = panelText;
                    panelText.Update(data, Encoding.ASCII);
                    break;
                case Consts.B_LARGE_ICON_TYPE:
                case Consts.B_MINI_ICON_TYPE:
                    pnlResource.Content = panelBeIcon;
                    panelBeIcon.Update(data, type);
                    break;
                case Consts.B_VERSION_INFO_TYPE:
                    pnlResource.Content = panelBeVersion;
                    panelBeVersion.Update(data, bigEndian);
                    break;
                default:
                    pnlResource.Content = panelHexDump;
                    panelHexDump.Update(data);
                    break;
            }
        }
    }
}