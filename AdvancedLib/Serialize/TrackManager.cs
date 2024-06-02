using BinarySerializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedLib.Serialize;

public class TrackManager : BinarySerializable
{
    Track[] tracks { get; set; }
    Pointer[] trackPointers { get; set; }
    public override void SerializeImpl(SerializerObject s)
    {
        Pointer basePointer = s.GetPreDefinedPointer(Manager.region);

        s.DoAt(basePointer, () => {
            trackPointers = s.SerializePointerArray(trackPointers, 48, PointerSize.Pointer32, basePointer, name: nameof(trackPointers));
        });
        tracks ??= new Track[trackPointers.Length];
        for (int i = 0; i < trackPointers.Length; i++)
        {
            s.DoAt(trackPointers[i], () => {
                tracks[i] = s.SerializeObject<Track>(tracks[i], name: $"track{i}");
            });
        }
    }
}