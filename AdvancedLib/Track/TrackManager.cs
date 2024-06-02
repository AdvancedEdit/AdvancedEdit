using BinarySerializer;

namespace AdvancedLib.Track;

public class TrackManager{
    public static readonly uint trackTablePointer = 0x250000;
    public static readonly int trackCount = 48;

    public TrackManager(BinaryReader reader) {
        for (int i = 0; i < trackCount; i++){

        }
    }
    public void DecompressTracks(){
        for (int i = 0; i < trackCount; i++){

        }
    }
}