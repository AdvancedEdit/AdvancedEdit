using AdvancedLib.Types;
using BinarySerializer;
using BinarySerializer.Nintendo.GBA;
using System.Drawing;
using System.Linq;

namespace AdvancedLib.Serialize;

public class Tileset : BinarySerializable
{
    Pointer?[] tilePointers { get; set; }
    public byte[] indicies { 
        get {
            byte[] a = new byte[4096 * 4];
            Array.Copy(tileBlocks[0].data, 0, a, 4096 * 0, 4096);
            Array.Copy(tileBlocks[1].data, 0, a, 4096 * 1, 4096);
            Array.Copy(tileBlocks[2].data, 0, a, 4096 * 2, 4096);
            Array.Copy(tileBlocks[3].data, 0, a, 4096 * 3, 4096);
            return a; 
        } set {
            if (value.Length != 4096 * 4) throw new Exception("Bad input array!");
            tileBlocks[0].data = value[(4096 * 0)..(4096 * 1)];
            tileBlocks[1].data = value[(4096 * 1)..(4096 * 2)];
            tileBlocks[2].data = value[(4096 * 2)..(4096 * 3)];
            tileBlocks[3].data = value[(4096 * 3)..(4096 * 4)];
        } }
    public byte[,] indicies2d
    {
        get
        {
            // sqrt(indicies.Length) = 128
            byte[,] temp = new byte[128, 128];
            Buffer.BlockCopy(indicies,0,temp,0, 128 * 128 * sizeof(byte));
            return temp;
        }
        set
        {
            if (value.Length != 128* 128) throw new Exception("Bad input array!");
            Buffer.BlockCopy(value, 0, indicies, 0, 128 * 128 * sizeof(byte));
        }
    }
    public Tile[] tiles
    {
        get
        {
            return Tile.GenerateTiles(indicies);
        }
        set
        {
            indicies = Tile.GetTileBytes(value);
        }
    }
    CompressedBlock<byte>[] tileBlocks;
    public override void SerializeImpl(SerializerObject s)
    {
        Pointer basePointer = s.CurrentPointer;
        tilePointers = s.SerializePointerArray(tilePointers,4,PointerSize.Pointer16, basePointer, name: nameof(tilePointers));
        s.SerializePadding(24); // make sure pointer table has length of 32
        tileBlocks = s.InitializeArray(tileBlocks, 4);
        for (int i = 0; i < tilePointers.Length; i++)
        {
            s.Goto(tilePointers[i]);
            tileBlocks[i] = s.SerializeObject<CompressedBlock<byte>>(tileBlocks[i], onPreSerialize: x => x.dataLength = 4096, name: $"layoutPart{i}");
        }
    }
    public override void RecalculateSize()
    {
        int position = 0;

        position = 32;

        for (int i = 0; i < tileBlocks.Length; i++)
        {
            CompressedBlock<byte> block = tileBlocks[i];
            tilePointers[i] = new Pointer(position, tilePointers[i].File, tilePointers[i].Anchor, PointerSize.Pointer16);
            block.RecalculateSize();
            position += (int)block.SerializedSize;
        }

        base.RecalculateSize();
    }
}