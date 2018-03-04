using System.Collections.Generic;
using System.Linq;
using Eto.Forms;
using Eto.Serialization.Xaml;

namespace exeinfogui.NE
{
    public class PanelNeAccelerators : Panel
    {
        GridView grdAccelerators;
        Label    lblCodepage;
        TextBox  txtCodepage;

        public PanelNeAccelerators()
        {
            XamlReader.Load(this);

            grdAccelerators.Columns.Add(new GridColumn
            {
                DataCell   = new TextBoxCell {Binding = Binding.Property<Accelerator, string>(a => $"{a.Type}")},
                HeaderText = "Type"
            });

            grdAccelerators.Columns.Add(new GridColumn
            {
                DataCell   = new TextBoxCell {Binding = Binding.Property<Accelerator, string>(a => $"{a.Key}")},
                HeaderText = "Key"
            });

            grdAccelerators.Columns.Add(new GridColumn
            {
                DataCell   = new TextBoxCell {Binding = Binding.Property<Accelerator, string>(a => $"{a.Command}")},
                HeaderText = "Command"
            });
        }

        public void Update(byte[] data, libexeinfo.NE.TargetOS targetOs)
        {
            grdAccelerators.DataStore      = null;
            List<Accelerator> accelerators = new List<Accelerator>();

            if(targetOs == libexeinfo.NE.TargetOS.OS2)
            {
                libexeinfo.NE.Os2AcceleratorTable table = libexeinfo.NE.GetOs2Accelerators(data);
                lblCodepage.Visible                     = true;
                txtCodepage.Visible                     = true;
                txtCodepage.Text                        = $"{table.CodePage}";
                accelerators.AddRange(table.Accelerators.Select(accel => new Accelerator
                {
                    Type    = $"{accel.Type}",
                    Key     = accel.Type.HasFlag(libexeinfo.NE.Os2AcceleratorFlags.AF_VIRTUALKEY) ? $"{accel.Key}" :
                   $"'{(char)(byte)accel.Key}'",
                    Command = accel.Command
                }));
            }
            else
            {
                lblCodepage.Visible = false;
                txtCodepage.Visible = false;
                accelerators.AddRange(libexeinfo.NE.GetWinAccelerators(data)
                                                .Select(accel => new Accelerator
                                                 {
                                                     Type    = $"{accel.Flags}",
                                                     Key = accel.Flags.HasFlag(libexeinfo.NE.WinAcceleratorFlags.VirtualKey) ? $"{accel.Key}" :
                                                               $"'{(char)(byte)accel.Key}'",
                                                     Command = accel.Command
                                                 }));
            }

            if(accelerators.Count > 0) grdAccelerators.DataStore = accelerators.OrderBy(r => r.Command).ThenBy(r=>r.Key);
        }

        class Accelerator
        {
            public ushort Command;
            public string Key;
            public string Type;
        }
    }
}