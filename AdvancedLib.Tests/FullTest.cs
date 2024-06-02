using AdvancedLib.Serialize;
using AdvancedLib.Types;

namespace AdvancedLib.Tests.Serialize;

public class FullTest
{
    [Fact]
    public void DoSomething(){
        Manager manager = new Manager();

        manager.Open(@"D:\Roms\GBA\mksc.gba");
        manager.Deserialize();
        manager.Reserialize();

    }
}