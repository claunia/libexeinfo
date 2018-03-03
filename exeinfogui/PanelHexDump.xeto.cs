using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Sockets;
using Eto.Forms;
using Eto.Drawing;
using Eto.Serialization.Xaml;

namespace exeinfogui
{
    public class PanelHexDump : Panel
    {
        TextArea txtHexDump;
        public PanelHexDump()
        {
            XamlReader.Load(this);
        }

        public void Update(byte[] data)
        {
            txtHexDump.Text = "";
            for(long pos = 0; pos < data.LongLength; pos++)
            {
                if(pos > 0 && pos % 4 == 0) txtHexDump.Text += " ";
                txtHexDump.Text += $"{data[pos]:X2} ";
            }
        }
    }
}
