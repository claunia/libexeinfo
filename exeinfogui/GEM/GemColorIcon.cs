using Eto.Drawing;

namespace exeinfogui.GEM
{
    public static class GemColorIcon
    {
        public static Bitmap GemColorIconToEto(libexeinfo.GEM.ColorIconPlane icon, int width, int height, bool selected)
        {
            if(selected && icon.SelectedData == null) return null;

            byte[] data = selected ? icon.SelectedData : icon.Data;
            byte[] mask = selected ? icon.SelectedMask : icon.Mask;

            int[] pixels = libexeinfo.GEM.PlaneToRaster(data, mask, width, height, icon.Planes);

            return new Bitmap(width, height, PixelFormat.Format32bppRgba, pixels);
        }
    }
}