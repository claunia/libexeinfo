using System;
using Eto.Forms;
using Eto.Serialization.Xaml;

namespace exeinfogui.GEM
{
    public class TabGemResources : TabPage
    {
        PanelGemBox            panelBox;
        PanelGemGeneric        panelGeneric;
        PanelGemIcon           panelIcon;
        PanelGemImage          panelImage;
        PanelGemString         panelString;
        PanelGemText           panelText;
        Panel                  pnlResource;
        TreeGridItemCollection treeData;
        TreeGridView           treeResources;

        public TabGemResources()
        {
            XamlReader.Load(this);

            treeResources.Columns.Add(new GridColumn {HeaderText = "Type", DataCell = new TextBoxCell(0)});

            treeResources.AllowMultipleSelection =  false;
            treeResources.SelectionChanged       += TreeResourcesOnSelectionChanged;

            panelGeneric = new PanelGemGeneric();
            panelString  = new PanelGemString();
            panelText    = new PanelGemText();
            panelBox     = new PanelGemBox();
            panelImage   = new PanelGemImage();
            panelIcon    = new PanelGemIcon();
        }

        public void Update(libexeinfo.GEM.TreeObjectNode[] roots)
        {
            treeData = new TreeGridItemCollection();

            for(int i = 0; i < roots.Length; i++)
            {
                TreeGridItem root = new TreeGridItem {Values = new object[] {$"Root {i}", null}};

                AddObjectToTree(root, roots[i]);

                treeData.Add(root);
            }

            treeResources.DataStore = treeData;
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
                /*                case libexeinfo.GEM.ObjectTypes.G_USERDEF: break;
                                case libexeinfo.GEM.ObjectTypes.G_CICON: break;*/
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