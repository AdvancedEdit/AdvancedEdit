using BinarySerializer;

namespace AdvancedLib.Serialize;

public class TrackAI : BinarySerializable{
    byte zonesCount {get; set;}
    Pointer zonesPointer {get; set;}
    AiZone[] aiZones { get; set;}
    Pointer targetsPointer {get; set;}
    AiTarget[] targets { get; set;}
    public override void SerializeImpl(SerializerObject s)
    {
        Pointer basePointer = s.CurrentPointer;

        zonesCount = s.Serialize<byte>(
            zonesCount,
            nameof(zonesCount)
        );
        zonesPointer = s.SerializePointer(
            zonesPointer,
            PointerSize.Pointer8,
            anchor: basePointer,
            name:nameof(zonesPointer)
        );

        s.SerializePadding(1);

        targetsPointer = s.SerializePointer(
            targetsPointer,
            PointerSize.Pointer16,
            anchor: basePointer,
            name:nameof(targetsPointer)
        );

        s.Goto(zonesPointer);
        aiZones = s.SerializeObjectArray(
            aiZones,
            zonesCount,
            name: nameof(aiZones)
        );
        s.Goto(targetsPointer);
        targets = s.SerializeObjectArray(
            targets,
            zonesCount * 3,
            name: nameof(aiZones)
        );
    }
    public override void RecalculateSize()
    {
        int position = 5;

        zonesPointer = new Pointer(position, zonesPointer.File, zonesPointer.Anchor, zonesPointer.Size);
        position += zonesCount * 10;

        targetsPointer = new Pointer(position, targetsPointer.File, targetsPointer.Anchor, targetsPointer.Size);

        base.RecalculateSize();
    }
}