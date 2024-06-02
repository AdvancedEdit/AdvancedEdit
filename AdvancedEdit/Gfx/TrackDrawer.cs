using AdvancedLib.Serialize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedEdit.Gfx
{
    internal class TrackDrawer
    {
        private Track track;
        private Bitmap trackCache;
        private Bitmap[] tileCache;

        private Size imageSize;

        public TrackDrawer()
        {
            
        }
        public void LoadTrack(Track t)
        {
            this.track = t;
            // TODO tracks with repeated tilesets
            var trackPalette = track.palette;
            for (int i = 0; i < track.tileset.tiles.Length; i++) {
                tileCache[i] = track.tileset.tiles[i].ToImage(trackPalette);
            }
            imageSize = new Size(track.trackWidth * 128 * 8, track.trackHeight * 128 * 8);
            track.layout.indicies
        }
        public void DrawTrack()
        {
            using (Bitmap image = new Bitmap(imageSize.Width, imageSize.Height))
            {

            }
        }

    }
}
