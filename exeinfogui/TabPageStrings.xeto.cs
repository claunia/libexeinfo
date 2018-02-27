using System;
using System.Collections.Generic;
using System.Linq;
using Eto.Forms;
using Eto.Drawing;
using Eto.Serialization.Xaml;

namespace exeinfogui
{
    public class TabPageStrings : TabPage
    {
        GridView treeStrings;
            
        //IEnumerable<string> strings;
        
        public TabPageStrings(IEnumerable<string> strings)
        {
            XamlReader.Load(this);

            treeStrings.DataStore = strings;
            treeStrings.Columns.Add(new GridColumn
            {
                DataCell = new TextBoxCell {Binding = Binding.Property<string, string>(r => r)},
                HeaderText = "String"
            });
        }
    }
}
