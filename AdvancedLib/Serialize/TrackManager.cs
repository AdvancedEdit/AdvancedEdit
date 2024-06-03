﻿using BinarySerializer;
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

        trackPointers = s.DoAt(basePointer, () =>
            s.SerializePointerArray(trackPointers, 48, PointerSize.Pointer32, basePointer, name: nameof(trackPointers))
        );
        tracks ??= new Track[trackPointers.Length];
        for (int i = 0; i < trackPointers.Length; i++)
        {
            if (!(i==20|| i == 21 || i == 22 || i == 23)) // battle tracks have messed up layouts?
            tracks[i] = s.DoAt(trackPointers[i], () => 
                s.SerializeObject<Track>(tracks[i], name: $"track{i}")
            );
        }
    }
}