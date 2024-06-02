using System.IO;
using AdvancedLib.Serialize;
using BinarySerializer;
using BinarySerializer.GBA;

/*
Notes:
Raw (usually) refers to the binary form of something
*/

namespace AdvancedLib;
public class Manager
{
    LinearFile file;

    Context context = new Context("", serializerLogger: new FileSerializerLogger("C:\\Users\\apler\\Downloads\\Log.txt"));

    public static Region region;

    ROMHeader header { get; set; }
    TrackManager trackManager { get; set; }
    
    public Manager() {
        context.AddPreDefinedPointers(new Dictionary<Region, long>()
        {
            [Region.USA] = 0x258000, 
            [Region.JPN] = 0x258000, // Should be correct, not tested
            [Region.PAL] = 0x258000, // ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
        });
    }

    /// <summary>
    /// Opens filestream to 
    /// </summary>
    /// <param name="path">Path the file</param>
    /// <returns>Open was a success?</returns>
    public bool Open(string path){
        file = new LinearFile(context, path, Endian.Little);
        context.AddFile(file);

        BinaryDeserializer s = new BinaryDeserializer(context);

        s.Goto(new Pointer(0, file));

        header = s.SerializeObject<ROMHeader>(header, name: nameof(header));
        switch (header.GameCode)
        {
            case "AMKE":
                region = Region.USA;
                break;
            case "AMKJ":
                region = Region.JPN;
                break;
            case "AMKP":
                region = Region.PAL;
                break;
            default:
                return false;
        }

       

        return true;
    }
    /// <summary>
    /// Decompress entire ROM
    /// </summary>
    public void Deserialize(){
        BinaryDeserializer s = new BinaryDeserializer(context);

        s.Goto(new Pointer(0, file));

        trackManager = s.SerializeObject<TrackManager>(trackManager, name: nameof(trackManager));
    }
    /// <summary>
    /// Reserialize entire ROM
    /// </summary>
    public void Reserialize(){
        BinarySerializer.BinarySerializer s = new BinarySerializer.BinarySerializer(context);

        s.Goto(new Pointer(0, file));

        trackManager = s.SerializeObject<TrackManager>(trackManager, name: nameof(trackManager));
    }
}

public enum Region
{
    USA,
    JPN,
    PAL,
}
