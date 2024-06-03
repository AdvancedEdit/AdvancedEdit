using AdvancedLib.Serialize;
using AdvancedLib.Types;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedEdit.Gfx
{
    internal class TrackDrawer
    {

        private Control control;
        private Track track;
        private Bitmap trackCache;
        private Tile[] tilePalette;
        private Bitmap[] tileCache;

        private byte[,] indicies;

        private Size imageSize;

        private float zoom = 1.0f;

        private Pen tilesetPen;

        public TrackDrawer(Control control)
        {
            this.control = control;
            tilesetPen = new Pen(Color.FromArgb(150, 255, 0, 0));
        }
        public void LoadTrack(Track t)
        {
            track = t;
            // TODO tracks with repeated tilesets
            var trackPalette = track.palette;
            tileCache = new Bitmap[256];
            for (int i = 0; i < track.tileset.tiles.Length; i++) {
                tileCache[i] = track.tileset.tiles[i].ToImage(trackPalette);
            }
            imageSize = new Size(track.trackWidth * 128 * 8, track.trackHeight * 128 * 8);
            indicies = track.layout.indicies2d;
            tilePalette = track.tileset.tiles;
            trackCache = new Bitmap(1, 1);
            RegenerateCache();
        }

        private void RegenerateCache()
        {
            trackCache.Dispose();

            int imageWidth = (int)(control.Width / zoom);
            int imageHeight = (tileCache.Length / (imageWidth / 8)) * 8;
            imageSize = new Size(imageWidth, imageHeight);

            int tileCountX = imageSize.Width / 8;
            int tileCountY = imageSize.Height / 8;

            trackCache = new Bitmap(imageSize.Width, imageSize.Height, PixelFormat.Format32bppPArgb);
            using (Graphics g = Graphics.FromImage(trackCache))
            {
                for (int x = 0; x < tileCountX; x++)
                {
                    for (int y = 0; y < tileCountY; y++)
                    {
                        Tile tile = tilePalette[indicies[x, y]];
                        g.DrawImage(tile.ToImage(track.palette), x * 8, y * 8);
                    }
                }
            }
        }

        public void DrawTrack(Graphics g, int selectedTile)
        {
            using (Bitmap image = new Bitmap(imageSize.Width, imageSize.Height))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                
                using (Graphics backBuffer = Graphics.FromImage(image))
                {
                    backBuffer.DrawImage(trackCache, 0, 0,
                                         imageSize.Width,
                                         imageSize.Height);

                    int tilePosX = (int)(selectedTile % 8);
                    int tilePosY = (int)(selectedTile / 8);
                    Point selectedTilePosition = new Point(tilePosX, tilePosY);

                    backBuffer.DrawRectangle(this.tilesetPen,
                                             selectedTilePosition.X * 8,
                                             selectedTilePosition.Y * 8,
                                             8 - 1,
                                             8 - 1);
                }
                g.DrawImage(image, 0, 0,
                            image.Width  * zoom,
                            image.Height * zoom);
            }
        }

    }
}
