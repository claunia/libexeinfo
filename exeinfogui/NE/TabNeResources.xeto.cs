//
// TabNeResources.xeto.cs
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
using System.Collections.Generic;
using System.Linq;
using exeinfogui.Win16;
using Eto.Forms;
using Eto.Serialization.Xaml;

namespace exeinfogui.NE
{
    public class TabNeResources : TabPage
    {
        Panel                  pnlResource;
        TreeGridItemCollection treeData;
        TreeGridView           treeResources;
        PanelWin16Version panelWin16Version;
        PanelNeStrings panelNeStrings;

        public TabNeResources()
        {
            XamlReader.Load(this);

            treeResources.Columns.Add(new GridColumn {HeaderText = "Type", DataCell  = new TextBoxCell(0)});
            treeResources.Columns.Add(new GridColumn {HeaderText = "Size", DataCell  = new TextBoxCell(1)});
            treeResources.Columns.Add(new GridColumn {HeaderText = "Flags", DataCell = new TextBoxCell(2)});

            treeResources.AllowMultipleSelection =  false;
            treeResources.SelectionChanged       += TreeResourcesOnSelectionChanged;
            
            panelWin16Version = new PanelWin16Version();
            panelNeStrings = new PanelNeStrings();
        }

        public void Update(IEnumerable<libexeinfo.NE.ResourceType> resourceTypes, libexeinfo.NE.TargetOS os)
        {
            treeData = new TreeGridItemCollection();

            foreach(libexeinfo.NE.ResourceType resourceType in resourceTypes.OrderBy(r => r.name))
            {
                TreeGridItem root = new TreeGridItem
                {
                    Values = new object[] {$"{resourceType.name}", null, null, null, os, null}
                };

                foreach(libexeinfo.NE.Resource resource in resourceType.resources.OrderBy(r => r.name))
                    root.Children.Add(new TreeGridItem
                    {
                        Values = new object[]
                        {
                            $"{resource.name}", $"{resource.data.Length}",
                            $"{(libexeinfo.NE.ResourceFlags)((ushort)resource.flags & libexeinfo.NE.KNOWN_RSRC_FLAGS)}",
                            $"{resourceType.name}", os, resource
                        }
                    });

                treeData.Add(root);
            }

            treeResources.DataStore = treeData;
        }

        void TreeResourcesOnSelectionChanged(object sender, EventArgs eventArgs)
        {
            if(!(((TreeGridItem)treeResources.SelectedItem)?.Values[5] is libexeinfo.NE.Resource resource))
            {
                pnlResource.Content = null;
                return;
            }

            byte[] data = ((libexeinfo.NE.Resource)((TreeGridItem)treeResources.SelectedItem).Values[5]).data;
            
            switch(((TreeGridItem)treeResources.SelectedItem).Values[3])
            {
                case "RT_VERSION":
                    pnlResource.Content = panelWin16Version;
                    panelWin16Version.Update(data);
                    break;
                case "RT_STRING":
                    pnlResource.Content = panelNeStrings;
                    panelNeStrings.Update(data, (libexeinfo.NE.TargetOS)((TreeGridItem)treeResources.SelectedItem).Values[4]);
                    break;
                default:
                    pnlResource.Content = null;
                    break;
            }
        }
    }
}