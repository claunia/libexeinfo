using System.Text;
using Eto.Forms;
using Eto.Serialization.Xaml;
using libexeinfo;
using libexeinfo.BeOS;

namespace exeinfogui.BeOS
{
    public class PanelBeVersion : Panel
    {
        TextBox  txtInternal;
        TextArea txtLongInfo;
        TextBox  txtMajorVersion;
        TextBox  txtMiddleVersion;
        TextBox  txtMinorVersion;
        TextBox  txtShortInfo;
        TextBox  txtVariety;

        public PanelBeVersion()
        {
            XamlReader.Load(this);
        }

        public void Update(byte[] data, bool bigEndian)
        {
            VersionInfo versionInfo = bigEndian
                                          ? BigEndianMarshal.ByteArrayToStructureBigEndian<VersionInfo>(data)
                                          : BigEndianMarshal.ByteArrayToStructureLittleEndian<VersionInfo>(data);

            txtMajorVersion.Text  = $"{versionInfo.major}";
            txtMiddleVersion.Text = $"{versionInfo.middle}";
            txtMinorVersion.Text  = $"{versionInfo.minor}";
            txtVariety.Text       = $"{versionInfo.variety}";
            txtInternal.Text      = $"{versionInfo.interna1}";
            txtShortInfo.Text     = StringHandlers.CToString(versionInfo.short_info, Encoding.UTF8);
            txtLongInfo.Text      = StringHandlers.CToString(versionInfo.long_info,  Encoding.UTF8);
        }
    }
}