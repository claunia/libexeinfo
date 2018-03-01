//
// TabGemResources.xeto.cs
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
using Eto.Forms;
using Eto.Serialization.Xaml;

namespace exeinfogui.GEM
{
    public class TabGemResources : TabPage
    {
        libexeinfo.GEM.ColorIcon[] colorIcons;
        PanelGemBox                panelBox;
        PanelGemColorIcon          panelColorIcon;
        PanelGemGeneric            panelGeneric;
        PanelGemIcon               panelIcon;
        PanelGemImage              panelImage;
        PanelGemString             panelString;
        PanelGemText               panelText;
        Panel                      pnlResource;
        TreeGridItemCollection     treeData;
        TreeGridView               treeResources;

        public TabGemResources()
        {
            XamlReader.Load(this);

            treeResources.Columns.Add(new GridColumn {HeaderText = "Type", DataCell = new TextBoxCell(0)});

            treeResources.AllowMultipleSelection =  false;
            treeResources.SelectionChanged       += TreeResourcesOnSelectionChanged;

            panelGeneric   = new PanelGemGeneric();
            panelString    = new PanelGemString();
            panelText      = new PanelGemText();
            panelBox       = new PanelGemBox();
            panelImage     = new PanelGemImage();
            panelIcon      = new PanelGemIcon();
            panelColorIcon = new PanelGemColorIcon();
        }

        public void Update(libexeinfo.GEM.TreeObjectNode[] roots, libexeinfo.GEM.ColorIcon[] cicons)
        {
            treeData = new TreeGridItemCollection();

            for(int i = 0; i < roots.Length; i++)
            {
                TreeGridItem root = new TreeGridItem {Values = new object[] {$"Root {i}", null}};

                AddObjectToTree(root, roots[i]);

                treeData.Add(root);
            }

            treeResources.DataStore = treeData;
            colorIcons              = cicons;
        }

        void TreeResourcesOnSelectionChanged(object sender, EventArgs eventArgs)
        {
            if(!(((TreeGridItem)treeResources.SelectedItem)?.Values[1] is libexeinfo.GEM.TreeObjectNode node))
            {
                pnlResource.Content = null;
                return;
            }

            switch(node.type)
            {
                case libexeinfo.GEM.ObjectTypes.G_BUTTON:
                case libexeinfo.GEM.ObjectTypes.G_STRING:
                case libexeinfo.GEM.ObjectTypes.G_TITLE:
                    panelString.Update(node);
                    pnlResource.Content = panelString;
                    break;
                case libexeinfo.GEM.ObjectTypes.G_TEXT:
                case libexeinfo.GEM.ObjectTypes.G_BOXTEXT:
                case libexeinfo.GEM.ObjectTypes.G_FTEXT:
                case libexeinfo.GEM.ObjectTypes.G_FBOXTEXT:
                    panelText.Update(node);
                    pnlResource.Content = panelText;
                    break;
                case libexeinfo.GEM.ObjectTypes.G_BOX:
                case libexeinfo.GEM.ObjectTypes.G_IBOX:
                case libexeinfo.GEM.ObjectTypes.G_BOXCHAR:
                    panelBox.Update(node);
                    pnlResource.Content = panelBox;
                    break;
                case libexeinfo.GEM.ObjectTypes.G_IMAGE:
                    panelImage.Update(node);
                    pnlResource.Content = panelImage;
                    break;
                case libexeinfo.GEM.ObjectTypes.G_ICON:
                    panelIcon.Update(node);
                    pnlResource.Content = panelIcon;
                    break;
                /*                case libexeinfo.GEM.ObjectTypes.G_USERDEF: break;*/
                case libexeinfo.GEM.ObjectTypes.G_CICON:
                    if(colorIcons == null || node.data >= colorIcons.Length || colorIcons[node.data] == null)
                        goto default;

                    panelColorIcon.Update(node, colorIcons[node.data]);
                    pnlResource.Content = panelColorIcon;
                    break;
                default:
                    panelGeneric.Update(node);
                    pnlResource.Content = panelGeneric;
                    break;
            }
        }

        static void AddObjectToTree(TreeGridItem parent, libexeinfo.GEM.TreeObjectNode node)
        {
            while(true)
            {
                TreeGridItem item = new TreeGridItem {Values = new object[] {$"{node.type}", node}};

                if(node.child != null) AddObjectToTree(item, node.child);

                parent.Children.Add(item);

                if(node.sibling != null)
                {
                    node = node.sibling;
                    continue;
                }

                break;
            }
        }
    }
}