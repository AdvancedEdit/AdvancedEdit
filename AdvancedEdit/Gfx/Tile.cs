using AdvancedLib.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedEdit.Gfx
{
    public static class TileExtensions
    {
        public static Bitmap ToImage(this Tile tile, Palette64 palette)
        {
            Bitmap bmp = new Bitmap(8,8);
            int x, y;
            for (x = 0; x < bmp.Width; x++)
            {
                for (y = 0; y < bmp.Height; y++)
                {
                    Color newColor = palette[tile.indicies[x,y]].ToColor();
                    bmp.SetPixel(x, y, newColor);
                }
            }
            return bmp;
        }
    }
    public static class ColorExtensions
    {
        public static Color ToColor(this BgrColor color)
        {
            return Color.FromArgb(color.r * 8, color.g * 8, color.b * 8);
        }
    }
}
