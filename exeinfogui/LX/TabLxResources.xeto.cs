//
// TabLxResources.xeto.cs
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
using exeinfogui.NE;
using exeinfogui.Os2;
using exeinfogui.Win16;
using exeinfogui.Windows;
using Eto.Forms;
using Eto.Serialization.Xaml;

namespace exeinfogui.LX
{
    public class TabLxResources : TabPage
    {
        PanelHexDump           panelHexDump;
        PanelNeAccelerators    panelNeAccelerators;
        PanelNeStrings         panelNeStrings;
        PanelOs2Bitmap         panelOs2Bitmap;
        Panel                  pnlResource;
        TreeGridItemCollection treeData;
        TreeGridView           treeResources;
        PanelWindowsIcon panelWindowsIcon;

        public TabLxResources()
        {
            XamlReader.Load(this);

            treeResources.Columns.Add(new GridColumn {HeaderText = "Type", DataCell  = new TextBoxCell(0)});
            treeResources.Columns.Add(new GridColumn {HeaderText = "Size", DataCell  = new TextBoxCell(1)});

            treeResources.AllowMultipleSelection =  false;
            treeResources.SelectionChanged       += TreeResourcesOnSelectionChanged;

            panelNeStrings      = new PanelNeStrings();
            panelNeAccelerators = new PanelNeAccelerators();
            panelHexDump        = new PanelHexDump();
            panelOs2Bitmap      = new PanelOs2Bitmap();
            panelWindowsIcon = new PanelWindowsIcon();
        }

        public void Update(IEnumerable<libexeinfo.NE.ResourceType> resourceTypes)
        {
            treeData = new TreeGridItemCollection();

            foreach(libexeinfo.NE.ResourceType resourceType in resourceTypes.OrderBy(r => r.name))
            {
                TreeGridItem root = new TreeGridItem
                {
                    Values = new object[] {$"{resourceType.name}", null, null, null}
                };

                foreach(libexeinfo.NE.Resource resource in resourceType.resources.OrderBy(r => r.name))
                    root.Children.Add(new TreeGridItem
                    {
                        Values = new object[]
                        {
                            $"{resource.name}", $"{resource.data.Length}", $"{resourceType.name}", resource
                        }
                    });

                treeData.Add(root);
            }

            treeResources.DataStore = treeData;
        }

        void TreeResourcesOnSelectionChanged(object sender, EventArgs eventArgs)
        {
            if(!(((TreeGridItem)treeResources.SelectedItem)?.Values[3] is libexeinfo.NE.Resource resource))
            {
                pnlResource.Content = null;
                return;
            }

            byte[] data = ((libexeinfo.NE.Resource)((TreeGridItem)treeResources.SelectedItem).Values[3]).data;

            switch(((TreeGridItem)treeResources.SelectedItem).Values[2])
            {
                case "RT_STRING":
                    pnlResource.Content = panelNeStrings;
                    panelNeStrings.Update(data, libexeinfo.NE.TargetOS.OS2);
                    break;
                case "RT_ACCELTABLE":
                    pnlResource.Content = panelNeAccelerators;
                    panelNeAccelerators.Update(data, libexeinfo.NE.TargetOS.OS2);
                    break;
                case "RT_BITMAP":
                case "RT_POINTER":
                    // TODO: Some do not contain valid OS/2 bitmaps
                    try
                    {
                        pnlResource.Content = panelOs2Bitmap;
                        panelOs2Bitmap.Update(data);
                    }
                    catch { goto default; }

                    break;
                case "RT_MENU":
                    if(BitConverter.ToUInt32(data, 0) == 40)
                    {
                        // Some OS/2 executables contain Windows "RT_ICON" resources, in OS/2 NE format
                        pnlResource.Content = panelWindowsIcon;
                        panelWindowsIcon.Update(data);
                        break;
                    }

                    goto default;
                default:
                    pnlResource.Content = panelHexDump;
                    panelHexDump.Update(data);
                    break;
            }
        }
    }
}