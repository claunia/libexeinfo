using Eto.Forms;
using Eto.Serialization.Xaml;

namespace exeinfogui.GEM
{
    public class PanelGemGeneric : Panel
    {
        TextBox txtCoordinates;
        TextBox txtData;
        TextBox txtFlags;
        TextBox txtSize;
        TextBox txtState;

        public PanelGemGeneric()
        {
            XamlReader.Load(this);
        }

        public void Update(libexeinfo.GEM.TreeObjectNode node)
        {
            txtFlags.Text       = node.flags == 0 ? "None" : node.flags.ToString();
            txtState.Text       = node.state == 0 ? "Normal" : node.state.ToString();
            txtCoordinates.Text = $"{node.x},{node.y}";
            txtSize.Text        = $"{node.width}x{node.height} pixels";
            txtData.Text        = $"{node.data}";
        }
    }
}