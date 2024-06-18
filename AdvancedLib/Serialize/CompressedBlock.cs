using BinarySerializer;
using BinarySerializer.GBA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedLib.Serialize
{
    public class CompressedBlock<T> : BinarySerializable where T : struct
    {
        public T[] data;
        public ushort dataLength;
        public override void SerializeImpl(SerializerObject s)
        {
            s.DoEncoded(new LZSSEncoder(), () =>
                data = s.SerializeArray<T>(data, dataLength, "CompressedBlock")
            );
        }
        public override void RecalculateSize()
        {
            base.RecalculateSize();
        }
    }
    public class CompressedObjectBlock<T> : BinarySerializable where T : BinarySerializable, new()
    {
        public T[] data;
        public ushort dataLength;
        public override void SerializeImpl(SerializerObject s)
        {
            s.DoEncoded(new LZSSEncoder(), () =>
                data = s.SerializeObjectArray<T>(data, dataLength, name: "CompressedBlock")
            );
        }
    }
}
