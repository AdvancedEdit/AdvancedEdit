using BinarySerializer;
using BinarySerializer.GBA;
using System;
using System.Drawing;


namespace AdvancedLib.Serialize;

public class Layout : BinarySerializable
{
    public Pointer?[] layoutPointers { get; set; }
    public byte[] indicies
    {
        get
        {
            byte[] a = new byte[4096 * size.Width * size.Height * 4];
            for (int i = 0; i < size.Width * size.Height * 4; i++)
            {
                Array.Copy(tileParts[i], 0, a, 4096 * i, 4096);
            }
            return a;
        }
        set
        {
            if (value.Length != 4096 * size.Width * size.Height * 4) return;
            for (int i = 0; i < size.Width * size.Height * 4; i++)
            {
                tileParts[i] = value[(4096 * i)..(4096 * (i+1))];
            }
        }
    }
    public byte[,] indicies2d
    {
        get
        {
            // sqrt(indicies.Length) = 256
            byte[,] temp = new byte[size.Width*128, size.Height * 128];
            Buffer.BlockCopy(indicies, 0, temp, 0, size.Width * 128 * size.Height * 128 * sizeof(byte));
            return temp;
        }
        set
        {
            if (value.Length != size.Width * 128 * size.Height * 128) throw new Exception("Bad input array!");
            Buffer.BlockCopy(value, 0, indicies, 0, size.Width * 128 * size.Height * 128 * sizeof(byte));
        }
    }

    public Size size { get; set; }

    public byte[][] tileParts { get; set; }
    public override void SerializeImpl(SerializerObject s)
    {
        int partsCount = size.Width * size.Height * 4;
        tileParts = new byte[size.Width * size.Height * 4][];
        Pointer basePointer = s.CurrentPointer;

        layoutPointers = s.SerializePointerArray(layoutPointers, partsCount, PointerSize.Pointer16, basePointer, name:nameof(layoutPointers));

        for (int i = 0; i < partsCount; i++)
        {
            s.DoAtEncoded(layoutPointers[i], new LZSSEncoder(), () => 
                tileParts[i] = s.SerializeArray<byte>(tileParts[i], 4096, $"layoutPart{i}")
            );
        }
    }
}
