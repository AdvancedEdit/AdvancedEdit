using BinarySerializer;

namespace AdvancedLib.Serialize;

public class AiTarget : BinarySerializable{
    public ushort data1 { get; set; }
    public ushort data2 { get; set; }
    public ushort data3 { get; set; }
    public ushort data4 { get; set; }
    public override void SerializeImpl(SerializerObject s)
    {
        data1 = s.Serialize(data1, nameof(data1));
        data2 = s.Serialize(data2, nameof(data2));
        data3 = s.Serialize(data3, nameof(data3));
        data4 = s.Serialize(data4, nameof(data4));
    }
}