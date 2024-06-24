using BinarySerializer;
using BinarySerializer.Nintendo.GBA;

namespace AdvancedLib.Types {
    /// <summary>
    /// A class for 8bpp tiles
    /// </summary>

    // TODO: Refactor Tile classes into Tile8 and Tile4
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
    public class Tile4 : BinarySerializable
    {
        public byte[] indicies
        {
            get
            {
                byte[] output = new byte[64];
                for (int i = 0; i < 32; i++)
                {
                    output[i * 2] = (byte)(rawTiles[i] & 0xf);
                    output[i * 2 + 1] = (byte)((rawTiles[i] & 0xf0) >> 8);
                }
                return output;
            }
            set
            {
                rawTiles = new byte[32];
                for (int i = 0; i < 32; i++)
                {
                    rawTiles[i] = (byte)(((value[i * 2]&0xf)<<8) | (value[i * 2 + 1] & 0xf));
                }
            }
        }
        public byte[] rawTiles { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            rawTiles = s.SerializeArray<byte>(rawTiles, 32, nameof(rawTiles));
        }
    }
}