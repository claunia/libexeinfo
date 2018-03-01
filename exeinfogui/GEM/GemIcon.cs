using System.Collections.Generic;
using Eto.Drawing;

namespace exeinfogui.GEM
{
    public static class GemIcon
    {
        public static Bitmap GemIconToEto(libexeinfo.GEM.Icon icon)
        {
            const uint COLOR      = 0x00000000;
            const uint BACKGROUND = 0x00FFFFFF;
            const uint ALPHAMASK  = 0xFF000000;
            List<int>  pixels     = new List<int>();

            byte[] data = libexeinfo.GEM.FlipPlane(icon.Data, icon.Width);
            byte[] mask = libexeinfo.GEM.FlipPlane(icon.Mask, icon.Width);

            for(int pos = 0; pos < data.Length; pos++)
            {
                for(int i = 0; i                             < 8; i++)
                    pixels.Add((int)(((data[pos] & (1 << i)) != 0 ? COLOR : BACKGROUND) +
                                     ((mask[pos] & (1 << i)) != 0 ? ALPHAMASK : 0)));
            }

            return new Bitmap(icon.Width, icon.Height, PixelFormat.Format32bppRgba, pixels);
        }
    }
}