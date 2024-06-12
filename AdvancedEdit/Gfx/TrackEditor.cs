using AdvancedLib.Serialize;
using AdvancedLib.Types;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdvancedEdit.Components;

namespace AdvancedEdit.Gfx;
internal class TrackEditor
{
    private GridBox gridBox;
    private Track track;
    private Bitmap trackCache;
    private Tile[] tilePalette;
    private Bitmap[] tileCache;
    private byte[,] indicies;
    private Size imageSize;
    private int zoom = 1;
    private Pen tilesetPen;

    private byte? selectedTile = 0;

    public TrackEditor(GridBox gridBox)
    {
        this.gridBox = gridBox;

        // TODO implement more tools
        gridBox.TileClick += TileClick;

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
        gridBox.mapSize = new Size(track.trackWidth * 128, track.trackHeight * 128);
        RegenerateCache();
    }
    /// <summary>
    /// Regenerates the image for the track cache
    /// </summary>
    public void RegenerateCache()
    {
        trackCache.Dispose();
        int imageWidth = track.trackWidth * 128 * 8;
        int imageHeight = track.trackHeight * 128 * 8;
        imageSize = new Size(imageWidth, imageHeight);
        gridBox.Size = new Size(imageWidth * zoom, imageHeight * zoom);
        gridBox.CanvasSize = new Size(imageWidth * zoom, imageHeight * zoom);
        gridBox.BoxSize = new Size(8 * zoom, 8 * zoom);
        int tileCountX = imageSize.Width / 8;
        int tileCountY = imageSize.Height / 8;
        trackCache = new Bitmap(imageSize.Width, imageSize.Height, PixelFormat.Format32bppPArgb);
        using (Graphics g = Graphics.FromImage(trackCache))
        {
            for (int x = 0; x < tileCountX; x++)
            {
                for (int y = 0; y < tileCountY; y++)
                {
                    Tile tile = tilePalette[indicies[y, x]]; //Switching the order fixes tile rotation.
                    g.DrawImage(tile.ToImage(track.palette), x * 8, y * 8);
                }
            }
        }
        RefreshImage();
    }
    public void RefreshImage()
    {
        gridBox.SuspendLayout();
        gridBox.Image = trackCache;
        gridBox.ResumeLayout();
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
                /*int tilePosX = (int)(selectedTile % 8);
                int tilePosY = (int)(selectedTile / 8);
                Point selectedTilePosition = new Point(tilePosX, tilePosY);
                backBuffer.DrawRectangle(this.tilesetPen,
                                         selectedTilePosition.X * 8,
                                         selectedTilePosition.Y * 8,
                                         8 - 1,
                                         8 - 1);*/
            }

            /*g.DrawImage(image, 0, 0,
                        image.Width * zoom,
                        image.Height * zoom);*/
            gridBox.SuspendLayout();
            gridBox.Image = image;
            gridBox.ResumeLayout();
        }
    }
    public void SetTile(byte index, Point point)
    {
        indicies[point.Y, point.X] = index;

        using (Graphics g = Graphics.FromImage(trackCache))
        {
            Tile tile = tilePalette[indicies[point.Y, point.X]]; //Switching the order fixes tile rotation.
            g.DrawImage(tile.ToImage(track.palette), point.X * 8, point.Y * 8);
        }
    }
    public void TileClick(object sender, EventArgs _e)
    {
        var e = (TileClickArgs)_e;
        if (selectedTile is not null)
        {
            SetTile(selectedTile ?? 0, e.clickPoint);
        }
    }
}