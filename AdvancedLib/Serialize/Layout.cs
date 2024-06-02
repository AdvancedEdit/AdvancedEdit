using BinarySerializer;
using BinarySerializer.GBA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedLib.Serialize
{
    public class Layout : BinarySerializable
    {
        Pointer?[] layoutPointers { get; set; }
        public byte[] indicies
        {
            get
            {
                byte[] a = new byte[4096 * 16];
                Array.Copy(tileParts[ 0], 0, a, 4096 * 0, 4096);
                Array.Copy(tileParts[ 1], 0, a, 4096 * 1, 4096);
                Array.Copy(tileParts[ 2], 0, a, 4096 * 2, 4096);
                Array.Copy(tileParts[ 3], 0, a, 4096 * 3, 4096);
                Array.Copy(tileParts[ 4], 0, a, 4096 * 4, 4096);
                Array.Copy(tileParts[ 5], 0, a, 4096 * 5, 4096);
                Array.Copy(tileParts[ 6], 0, a, 4096 * 6, 4096);
                Array.Copy(tileParts[ 7], 0, a, 4096 * 7, 4096);
                Array.Copy(tileParts[ 8], 0, a, 4096 * 8, 4096);
                Array.Copy(tileParts[ 9], 0, a, 4096 * 9, 4096);
                Array.Copy(tileParts[10], 0, a, 4096 *10, 4096);
                Array.Copy(tileParts[11], 0, a, 4096 *11, 4096);
                Array.Copy(tileParts[12], 0, a, 4096 *12, 4096);
                Array.Copy(tileParts[13], 0, a, 4096 *13, 4096);
                Array.Copy(tileParts[14], 0, a, 4096 *14, 4096);
                Array.Copy(tileParts[15], 0, a, 4096 *15, 4096);
                return a;
            }
            set
            {
                if (value.Length != 4096 * 16) return;
                tileParts[ 0] = value[(4096 *  0)..(4096 *  1)];
                tileParts[ 1] = value[(4096 *  1)..(4096 *  2)];
                tileParts[ 2] = value[(4096 *  2)..(4096 *  3)];
                tileParts[ 3] = value[(4096 *  3)..(4096 *  4)];
                tileParts[ 4] = value[(4096 *  4)..(4096 *  5)];
                tileParts[ 5] = value[(4096 *  5)..(4096 *  6)];
                tileParts[ 6] = value[(4096 *  6)..(4096 *  7)];
                tileParts[ 7] = value[(4096 *  7)..(4096 *  8)];
                tileParts[ 8] = value[(4096 *  8)..(4096 *  9)];
                tileParts[ 9] = value[(4096 *  9)..(4096 * 10)];
                tileParts[10] = value[(4096 * 10)..(4096 * 11)];
                tileParts[11] = value[(4096 * 11)..(4096 * 12)];
                tileParts[12] = value[(4096 * 12)..(4096 * 13)];
                tileParts[13] = value[(4096 * 13)..(4096 * 14)];
                tileParts[14] = value[(4096 * 14)..(4096 * 15)];
                tileParts[15] = value[(4096 * 15)..(4096 * 16)];
            }
        }
        public byte[,] indicies2d
        {
            get
            {
                // sqrt(indicies.Length) = 256
                byte[,] temp = new byte[256, 256];
                Buffer.BlockCopy(indicies, 0, temp, 0, 256 * 256 * sizeof(byte));
                return temp;
            }
            set
            {
                if (value.Length != 256 * 256) throw new Exception("Bad input array!");
                Buffer.BlockCopy(value, 0, indicies, 0, 256 * 256 * sizeof(byte));
            }
        }
        byte[][] tileParts = new byte[16][];
        public override void SerializeImpl(SerializerObject s)
        {
            Pointer basePointer = s.CurrentPointer;
            layoutPointers = s.SerializePointerArray(layoutPointers, 16, PointerSize.Pointer16, basePointer);
            for (int i = 0; i < layoutPointers.Length; i++)
            {
                s.DoAtEncoded(layoutPointers[i], new LZSSEncoder(), () => {
                    tileParts[i] = s.SerializeArray<byte>(tileParts[i], 4096, "tilePart");
                });
            }
        }
    }
}
