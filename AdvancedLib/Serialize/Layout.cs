using BinarySerializer;
using BinarySerializer.Nintendo.GBA;
using System;
using System.Drawing;


namespace AdvancedLib.Serialize;

public class Layout : BinarySerializable
{
    public Pointer[] layoutPointers { get; set; }
    public byte[] indicies
    {
        get
        {
            byte[] a = new byte[4096 * size.Width * size.Height * 4];
            for (int i = 0; i < size.Width * size.Height * 4; i++)
            {
                Array.Copy(layoutBlocks[i].data, 0, a, 4096 * i, 4096);
            }
            return a;
        }
        set
        {
            if (value.Length != 4096 * size.Width * size.Height * 4) return;
            for (int i = 0; i < size.Width * size.Height * 4; i++)
            {
                layoutBlocks[i].data = value[(4096 * i)..(4096 * (i+1))];
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

    public CompressedBlock<byte>[] layoutBlocks { get; set; }
    public override void SerializeImpl(SerializerObject s)
    {
        int partsCount = size.Width * size.Height * 4;
        int paddingLen = 4 * (32 - partsCount);
        layoutBlocks = s.InitializeArray(layoutBlocks, partsCount);

        Pointer basePointer = s.CurrentPointer;

        layoutPointers = s.SerializePointerArray(layoutPointers, partsCount, PointerSize.Pointer16, basePointer, name:nameof(layoutPointers));
        s.SerializePadding(paddingLen);

        s.DoArray(layoutBlocks, (obj, i, name) => {
            s.Goto(layoutPointers[i]);
            return s.SerializeObject<CompressedBlock<byte>>(obj, onPreSerialize: x => x.dataLength = 4096, name: name);
        }, name: nameof(layoutBlocks));

        s.SerializePadding(4-(s.CurrentAbsoluteOffset%4), name: "Alignment");
    }
    public override void RecalculateSize()
    {
        int position = 0;

        position = 32;

        for (int i = 0; i < layoutBlocks.Length; i++)
        {
            CompressedBlock<byte> block = layoutBlocks[i];
            layoutPointers[i] = new Pointer(position, layoutPointers[i].File, layoutPointers[i].Anchor, PointerSize.Pointer16);
            block.RecalculateSize();
            position += (int)block.SerializedSize;
        }

        base.RecalculateSize();
    }
}