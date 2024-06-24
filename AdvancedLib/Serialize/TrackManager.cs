using BinarySerializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedLib.Serialize;

public class TrackManager : BinarySerializable
{
    public Track[] tracks { get; set; }
    public Pointer[] trackPointers { get; set; }
    public override void SerializeImpl(SerializerObject s)
    {
        Pointer basePointer = s.GetPreDefinedPointer(Manager.region);

        s.DoAt(basePointer, () =>
            trackPointers = s.SerializePointerArray(trackPointers, 48, PointerSize.Pointer32, basePointer, name: nameof(trackPointers))
        );
        tracks = s.InitializeArray(tracks, trackPointers.Length);
        
        for (int i = 0; i < trackPointers.Length; i++)
        {
            if (!(i == 20 || i == 21 || i == 22 || i == 23)) // battle tracks have messed up layouts?
            {
                s.Goto(trackPointers[i]);
                tracks[i] = s.SerializeObject<Track>(tracks[i], name: $"track{i}");
            } else
            {

            }
        }
    }
    public override void RecalculateSize()
    {
        int position = 0;

        position += trackPointers.Length * 4;

        for (int i = 0; i < tracks.Length; i++)
        {
            if (!(i==20|| i == 21 || i == 22 || i == 23))
            {
                trackPointers[i] = new Pointer(position, trackPointers[i].File, trackPointers[i].Anchor, trackPointers[i].Size);
                tracks[i].RecalculateSize();
                position += (int)tracks[i].SerializedSize;
            }
        }

        base.RecalculateSize();
    }
}