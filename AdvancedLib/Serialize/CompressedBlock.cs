using BinarySerializer;
using BinarySerializer.GBA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedLib.Serialize
{
    public class CompressedBlock : BinarySerializable
    {
        public byte[] data;
        public override void SerializeImpl(SerializerObject s)
        {
            s.DoEncoded(new LZSSEncoder(), () =>
            {
                data = s.SerializeArray(data, 4096, "CompressedBlock");
            });
        }
    }
}
