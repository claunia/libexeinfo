using Eto.Forms;
using Eto.Serialization.Xaml;

namespace exeinfogui.GEM
{
    public class PanelGemIcon : Panel
    {
        ImageView imgIcon;
        TextBox   txtBgColor;
        TextBox   txtCharater;
        TextBox   txtCharCoordinates;
        TextBox   txtCoordinates;
        TextBox   txtFgColor;
        TextBox   txtFlags;
        TextBox   txtSize;
        TextBox   txtState;
        TextBox   txtText;
        TextBox   txtTextBoxSize;
        TextBox   txtTextCoordinates;

        public PanelGemIcon()
        {
            XamlReader.Load(this);
        }

        public void Update(libexeinfo.GEM.TreeObjectNode node)
        {
            txtFlags.Text           = node.flags == 0 ? "None" : node.flags.ToString();
            txtState.Text           = node.state == 0 ? "Normal" : node.state.ToString();
            txtCoordinates.Text     = $"{node.IconBlock.X},{node.IconBlock.Y}";
            txtSize.Text            = $"{node.IconBlock.Width}x{node.IconBlock.Height} pixels";
            txtCharater.Text        = $"{node.IconBlock.Character}";
            txtCharCoordinates.Text = $"{node.IconBlock.CharX},{node.IconBlock.CharY}";
            txtFgColor.Text         = $"{node.IconBlock.ForegroundColor}";
            txtBgColor.Text         = $"{node.IconBlock.BackgroundColor}";
            txtTextCoordinates.Text = $"{node.IconBlock.TextX},{node.IconBlock.TextY}";
            txtTextBoxSize.Text     = $"{node.IconBlock.TextWidth}x{node.IconBlock.TextHeight} pixels";
            txtText.Text            = node.IconBlock.Text;
            imgIcon.Image           = GemIcon.GemIconToEto(node.IconBlock);
        }
    }
}