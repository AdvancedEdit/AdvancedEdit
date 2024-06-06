using System.Reflection.Metadata.Ecma335;
using BinarySerializer;

namespace AdvancedLib.Serialize;

public class AiZone : BinarySerializable{
    public ZoneShape shape {get; set;}
    public byte rawPositionX {get; set;}
    public byte rawPositionY {get; set;}
    public ushort positionX {
        get { return (ushort)(rawPositionX* 2); }
        set { rawPositionX = (byte)(value / 2); }
    }
    public ushort positionY {
        get { return (ushort)(rawPositionY* 2); }
        set { rawPositionY = (byte)(value / 2); }
    }
    public byte width {get; set;}
    public byte height {get; set;}
    public override void SerializeImpl(SerializerObject s)
    {
        shape = (ZoneShape)s.Serialize(
            (byte)shape,
            nameof(shape)
        );
        
        rawPositionX = s.Serialize(
            rawPositionX,
            nameof(rawPositionX)
        );

        rawPositionY = s.Serialize(
            rawPositionY,
            nameof(rawPositionY)
        );
        
        width = s.Serialize(
            width,
            nameof(width)
        );

        height = s.Serialize(
            height,
            nameof(height)
        );
    }
}
public enum ZoneShape {
    Rectange = 0x00,
    TriangleTopLeft = 0x01,
    TriangleTopRight = 0x02,
    TriangleBottomRight = 0x03,
    TriangleBottomLeft = 0x04,
}