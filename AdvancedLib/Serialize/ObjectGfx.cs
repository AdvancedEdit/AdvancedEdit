using AdvancedLib.Types;
using BinarySerializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedLib.Serialize
{
    public class ObjectGfx : BinarySerializable
    {
        /*public byte[] indicies
        {
            get
            {
                return data.;
            }
            set
            {
                data.data = value;
            }
        }
        public byte[,] indicies2d
        {
            get
            {
                // sqrt(indicies.Length) = 256
                byte[,] temp = new byte[8 * 8, 8 * 8];
                Buffer.BlockCopy(indicies, 0, temp, 0, 8 * 8 * 64 * sizeof(byte));
                return temp;
            }
            set
            {
                if (value.Length != 8 * 8 * 64) throw new Exception("Bad input array!");
                Buffer.BlockCopy(value, 0, indicies, 0, 8 * 8 * 64 * sizeof(byte));
            }
        }*/

        public Tile4[] tiles { get => tileObject.data; set => tileObject.data = value; }
        public CompressedObjectBlock<Tile4> tileObject;
        public ushort objectLength { get; set; }
        public int rawSize { get { return tiles.Length * 32; } }

        public override void SerializeImpl(SerializerObject s)
        {
            Pointer basePointer = s.CurrentPointer;
            s.DoAt(basePointer, () => {
                s.SerializePadding(1);
                objectLength = s.Serialize<ushort>(objectLength, nameof(objectLength));
            });
            tileObject = s.SerializeObject<CompressedObjectBlock<Tile4>>(tileObject, onPreSerialize: x => x.dataLength = (ushort)(objectLength/32u), name: "object data");
        }
    }
    public class MultiObjectGfx : BinarySerializable
    {
        /*public byte[] indicies
        {
            get
            {
                return data.;
            }
            set
            {
                data.data = value;
            }
        }
        public byte[,] indicies2d
        {
            get
            {
                // sqrt(indicies.Length) = 256
                byte[,] temp = new byte[8 * 8, 8 * 8];
                Buffer.BlockCopy(indicies, 0, temp, 0, 8 * 8 * 64 * sizeof(byte));
                return temp;
            }
            set
            {
                if (value.Length != 8 * 8 * 64) throw new Exception("Bad input array!");
                Buffer.BlockCopy(value, 0, indicies, 0, 8 * 8 * 64 * sizeof(byte));
            }
        }*/

        public CompressedObjectBlock<Tile4>[] tileObjects;
        public Pointer[] objectPointers { get; set; }
        public ushort[] objectLengths { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Pointer basePointer = s.CurrentPointer;
            
            // Assume 2 because im dumb and every track has 2
            objectPointers = s.SerializePointerArray(objectPointers, 2, PointerSize.Pointer16, basePointer, name: nameof(objectPointers));
            
            // This was removing every element when I wrote back
            //objectPointers = objectPointers[0..(objectPointers.Length - 1)]; // Remove empty array element
            objectLengths ??= new ushort[objectPointers.Length];
            tileObjects ??= new CompressedObjectBlock<Tile4>[objectPointers.Length];

            for (int i = 0; i < objectPointers.Length; i++) {
                // Kinda hacky solution but it is a 24 bit number
                s.DoAt(objectPointers[i], () => {
                    s.SerializePadding(0);
                    objectLengths[i] = s.Serialize<ushort>(objectLengths[i], nameof(objectLengths));
                });
                s.Goto(objectPointers[i]);
                tileObjects[i] = s.SerializeObject<CompressedObjectBlock<Tile4>>(tileObjects[i], onPreSerialize: x => x.dataLength = (ushort)(objectLengths[i] / 32u), name: "object data");

            }
        }
        public override void RecalculateSize()
        {
            int position = 0;

            position += 32;

            for (int i = 0; i < tileObjects.Length; i++)
            {
                CompressedObjectBlock<Tile4> block = tileObjects[i];
                objectPointers[i] = new Pointer(position, objectPointers[i].File, objectPointers[i].Anchor, PointerSize.Pointer16);
                block.RecalculateSize();
                position += (int)block.SerializedSize;
            }

            base.RecalculateSize();
        }
    }
}
