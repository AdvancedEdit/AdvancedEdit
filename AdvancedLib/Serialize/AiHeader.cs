using BinarySerializer;

namespace AdvancedLib.Serialize;

public class AiHeader : BinarySerializable{
    byte zonesCount {get; set;}
    Pointer zonesPointer {get; set;}
    Pointer targetsPointer {get; set;}
    public override void SerializeImpl(SerializerObject s)
    {
        zonesCount = s.Serialize<byte>(
            zonesCount,
            nameof(zonesCount)
        );
        zonesPointer = s.SerializePointer(
            zonesPointer,
            PointerSize.Pointer8,
            name:nameof(zonesPointer)
        );

        s.SerializePadding(1);

        targetsPointer = s.SerializePointer(
            targetsPointer,
            PointerSize.Pointer16,
            name:nameof(targetsPointer)
        );
    }
}