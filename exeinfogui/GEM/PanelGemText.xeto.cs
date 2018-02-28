using Eto.Forms;
using Eto.Serialization.Xaml;

namespace exeinfogui.GEM
{
    public class PanelGemText : Panel
    {
        Label   lblTemplate;
        Label   lblText;
        Label   lblValidation;
        TextBox txtBorderColor;
        TextBox txtCoordinates;
        TextBox txtFill;
        TextBox txtFlags;
        TextBox txtFont;
        TextBox txtInsideColor;
        TextBox txtJustification;
        TextBox txtPreview;
        TextBox txtSize;
        TextBox txtState;
        TextBox txtTemplate;
        TextBox txtText;
        TextBox txtTextColor;
        TextBox txtThickness;
        TextBox txtTransparency;
        TextBox txtValidation;

        public PanelGemText()
        {
            XamlReader.Load(this);
        }

        public void Update(libexeinfo.GEM.TreeObjectNode node)
        {
            txtFlags.Text         = node.flags == 0 ? "None" : node.flags.ToString();
            txtState.Text         = node.state == 0 ? "Normal" : node.state.ToString();
            txtCoordinates.Text   = $"{node.x},{node.y}";
            txtSize.Text          = $"{node.width}x{node.height} pixels";
            txtBorderColor.Text   = $"{node.TedInfo.BorderColor}";
            txtFill.Text          = $"{node.TedInfo.Fill}";
            txtFont.Text          = $"{node.TedInfo.Font}";
            txtInsideColor.Text   = $"{node.TedInfo.InsideColor}";
            txtJustification.Text = $"{node.TedInfo.Justification}";
            txtTemplate.Text      = node.TedInfo.Template;
            txtText.Text          = node.TedInfo.Text;
            txtTextColor.Text     = $"{node.TedInfo.TextColor}";
            txtTransparency.Text  =
                node.TedInfo.Transparency ? "Transparent mode" : "Replace mode";
            txtValidation.Text                                    = node.TedInfo.Validation;
            if(node.TedInfo.Thickness      < 0) txtThickness.Text = $"{node.TedInfo.Thickness * -1} pixels outward";
            else if(node.TedInfo.Thickness < 0)
                txtThickness.Text = $"{node.TedInfo.Thickness} pixels inward";
            else
                txtThickness.Text = "None";

            if(string.IsNullOrWhiteSpace(node.TedInfo.Template))
            {
                txtTemplate.Visible = false;
                lblTemplate.Visible = false;
            }
            else
            {
                txtTemplate.Visible = true;
                lblTemplate.Visible = true;
            }

            if(string.IsNullOrWhiteSpace(node.TedInfo.Text))
            {
                txtText.Visible = false;
                lblText.Visible = false;
            }
            else
            {
                txtText.Visible = true;
                lblText.Visible = true;
            }

            if(string.IsNullOrWhiteSpace(node.TedInfo.Validation))
            {
                txtValidation.Visible = false;
                lblValidation.Visible = false;
            }
            else
            {
                txtValidation.Visible = true;
                lblValidation.Visible = true;
            }

            if(!string.IsNullOrEmpty(node.TedInfo.Template))
                if(string.IsNullOrEmpty(node.TedInfo.Text))
                    txtPreview.Text = node.TedInfo.Template;
                else
                {
                    char[] preview  = node.TedInfo.Template.ToCharArray();
                    char[] template = node.TedInfo.Text.ToCharArray();
                    if(template[0]       == '@')
                        for(int i = 0; i < template.Length; i++)
                            template[i] = ' ';

                    int templatePos = 0;

                    for(int i = 0; i < preview.Length; i++)
                    {
                        if(preview[i] != '_') continue;

                        if(templatePos >= template.Length) continue;

                        preview[i] = template[templatePos];
                        templatePos++;
                    }

                    txtPreview.Text = new string(preview);
                }
            else txtPreview.Text = txtText.Text;

            txtPreview.BackgroundColor = GemColor.GemToEtoColor(node.TedInfo.InsideColor);
            txtPreview.TextColor       = GemColor.GemToEtoColor(node.TedInfo.TextColor);
        }
    }
}