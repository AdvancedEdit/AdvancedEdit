using BinarySerializer;

namespace AdvancedLib.Serialize;

public class AiTarget : BinarySerializable{
    byte data {get; set;}
    public byte intersection {
        get {
            return (byte)((data & 0xf0)>>8);
        }
        set {
            data = (byte)(((value & 0x0f)<<8) | (data & 0x0f));
        }
    }
    public byte speed {
        get {
            return (byte)(data & 0x0f);
        }
        set {
            data = (byte)((value & 0xf) | (data & 0xf0));
        }
    }
    public byte positionX {get; set;}
    public byte positionY {get; set;}
    public override void SerializeImpl(SerializerObject s)
    {
        data = s.Serialize(data, "speed and intersection");
        positionX = s.Serialize(positionX, nameof(positionX));
        positionY = s.Serialize(positionY, nameof(positionY));
    }
}