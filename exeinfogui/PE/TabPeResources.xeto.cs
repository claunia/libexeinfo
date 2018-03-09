//
// TabPeResources.xeto.cs
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
using System.Text;
using exeinfogui.Windows;
using Eto.Forms;
using Eto.Serialization.Xaml;

namespace exeinfogui.PE
{
    public class TabPeResources : TabPage
    {
        PanelHexDump     panelHexDump;
        PanelPeStrings   panelPeStrings;
        PanelPeVersion   panelPeVersion;
        PanelText        panelText;
        PanelWindowsIcon panelWindowsIcon;
        Panel            pnlResource;
        TreeGridView     treeResources;

        public TabPeResources()
        {
            XamlReader.Load(this);

            treeResources.Columns.Add(new GridColumn {HeaderText = "Identifier", DataCell = new TextBoxCell(0)});
            treeResources.Columns.Add(new GridColumn {HeaderText = "Size", DataCell       = new TextBoxCell(1)});

            treeResources.AllowMultipleSelection =  false;
            treeResources.SelectionChanged       += TreeResourcesOnSelectionChanged;

            panelPeVersion   = new PanelPeVersion();
            panelPeStrings   = new PanelPeStrings();
            panelHexDump     = new PanelHexDump();
            panelWindowsIcon = new PanelWindowsIcon();
            panelText        = new PanelText();
        }

        public void Update(libexeinfo.PE.ResourceNode root)
        {
            TreeGridItemCollection treeData = new TreeGridItemCollection();

            foreach(libexeinfo.PE.ResourceNode rootChild in root.children)
                treeData.Add(GetChildren(rootChild, rootChild.name));

            treeResources.DataStore = treeData;
        }

        static TreeGridItem GetChildren(libexeinfo.PE.ResourceNode node, string type)
        {
            string sizeStr = node.data == null ? null : $"{node.data.Length}";

            TreeGridItem item = new TreeGridItem {Values = new object[] {$"{node.name}", sizeStr, type, node.data}};

            if(node.children == null) return item;

            foreach(libexeinfo.PE.ResourceNode child in node.children) item.Children.Add(GetChildren(child, type));

            return item;
        }

        void TreeResourcesOnSelectionChanged(object sender, EventArgs eventArgs)
        {
            if(((TreeGridItem)treeResources.SelectedItem)?.Values[3] == null)
            {
                pnlResource.Content = null;
                return;
            }

            byte[] data = ((TreeGridItem)treeResources.SelectedItem)?.Values[3] as byte[];
            string type = ((TreeGridItem)treeResources.SelectedItem)?.Values[2] as string;

            switch(type)
            {
                case "RT_STRING":
                    pnlResource.Content = panelPeStrings;
                    panelPeStrings.Update(data);
                    break;
                case "RT_ICON":
                    pnlResource.Content = panelWindowsIcon;
                    panelWindowsIcon.Update(data);
                    break;
                case "RT_VERSION":
                    pnlResource.Content = panelPeVersion;
                    panelPeVersion.Update(data);
                    break;
                case "RT_MANIFEST":
                    pnlResource.Content = panelText;
                    panelText.Update(data, Encoding.UTF8);
                    break;
                default:
                    pnlResource.Content = panelHexDump;
                    panelHexDump.Update(data);
                    break;
            }
        }
    }
}