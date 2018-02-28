using System.Collections.Generic;
using Eto.Forms;
using Eto.Serialization.Xaml;

namespace exeinfogui
{
    public class TabPageStrings : TabPage
    {
        GridView treeStrings;

        public TabPageStrings()
        {
            XamlReader.Load(this);

            treeStrings.Columns.Add(new GridColumn
            {
                DataCell   = new TextBoxCell {Binding = Binding.Property<string, string>(r => r)},
                HeaderText = "String"
            });
        }

        public void Update(IEnumerable<string> strings)
        {
            treeStrings.DataStore = strings;
        }
    }
}