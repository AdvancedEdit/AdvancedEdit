using BinarySerializer;

namespace AdvancedLib.Types {
    public class Tile
    {
        public byte[,] indicies;
        public Tile(byte[,] indicies)
        {
            this.indicies = indicies;
        }
        public static Tile[] GenerateTiles(byte[] indicies)
        {
            int tileno = (int)Math.Ceiling(indicies.Length / 64f);
            Tile[] tiles = new Tile[tileno];
            for (int t = 0; t < tileno; t++)
            {
                byte[,] tile = new byte[8, 8];
                for (int y = 0; y < 8; y++)
                {
                    for (int x = 0; x < 8; x++)
                    {
                        if ((y * 8 + x) < indicies.Length)
                        {
                            tile[x, y] = indicies[y * 8 + (x + t * 64)];
                        }
                        else
                        {
                            tile[x, y] = 0;
                        }
                    }
                }
                tiles[t] = new Tile(tile);
            }
            return tiles;
        }
        public static byte[] GetTileBytes(Tile[] tiles)
        {
            int totalIndices = tiles.Length * 64;
            List<byte> indicesList = new List<byte>();

            foreach (Tile tile in tiles)
            {
                byte[,] tileData = tile.indicies;

                for (int y = 0; y < 8; y++)
                {
                    for (int x = 0; x < 8; x++)
                    {
                        if (indicesList.Count < totalIndices)
                        {
                            indicesList.Add(tileData[x, y]);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            return indicesList.ToArray();
        }
    }
}