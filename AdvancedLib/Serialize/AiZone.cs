using System.Reflection.Metadata.Ecma335;
using BinarySerializer;

namespace AdvancedLib.Serialize;

public class AiZone : BinarySerializable{
    public byte data1 { get; set; }
    public ushort data2 { get; set; }
    public ushort data3 { get; set; }
    public ushort data4 { get; set; }
    public ushort data5 { get; set; }
    public override void SerializeImpl(SerializerObject s)
    {
        data1 = s.Serialize<byte>(data1, nameof(data1)); //shape
        data2 = s.Serialize<ushort>(data2, nameof(data1)); //xpos
        data3 = s.Serialize<ushort>(data3, nameof(data1)); //ypos
        data4 = s.Serialize<ushort>(data4, nameof(data1)); //width
        data5 = s.Serialize<ushort>(data5, nameof(data1)); //height (0 if triangle)
        s.SerializePadding(3);
    }
}
public enum ZoneShape {
    Rectange = 0x00,
    TriangleTopLeft = 0x01,
    TriangleTopRight = 0x02,
    TriangleBottomRight = 0x03,
    TriangleBottomLeft = 0x04,
}