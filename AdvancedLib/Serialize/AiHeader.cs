using BinarySerializer;

namespace AdvancedLib;

class AiHeader : BinarySerializable{
    byte zonesCount {get; set;}
    Pointer zonesPointer {get; set;}
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

    }
}