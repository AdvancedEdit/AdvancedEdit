using BinarySerializer;
using BinarySerializer.GBA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdvancedLib.Serialize
{
    public class Minimap : BinarySerializable
    {
        public byte[] indicies
        {
            get
            {
                return data.data;
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
        }
        
        public CompressedBlock data;

        public override void SerializeImpl(SerializerObject s)
        {
            data = s.SerializeObject<CompressedBlock>(data, name: "minimap data");
        }
    }
}
