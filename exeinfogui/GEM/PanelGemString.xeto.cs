using Eto.Forms;
using Eto.Serialization.Xaml;

namespace exeinfogui.GEM
{
    public class PanelGemString : Panel
    {
        TextBox  txtCoordinates;
        TextBox  txtFlags;
        TextBox  txtSize;
        TextBox  txtState;
        TextArea txtString;

        public PanelGemString()
        {
            XamlReader.Load(this);
        }

        public void Update(libexeinfo.GEM.TreeObjectNode node)
        {
            txtFlags.Text       = node.flags == 0 ? "None" : node.flags.ToString();
            txtState.Text       = node.state == 0 ? "Normal" : node.state.ToString();
            txtCoordinates.Text = $"{node.x},{node.y}";
            txtSize.Text        = $"{node.width}x{node.height} pixels";
            txtString.Text      = node.String;
        }
    }
}