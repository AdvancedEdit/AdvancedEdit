using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdvancedLib.Types;
using BinarySerializer;
using BinarySerializer.GBA;

namespace AdvancedLib.Serialize;

public class Track : BinarySerializable
{
    #pragma warning disable
    uint magic;
    public byte trackWidth { get; set; }
    public byte trackHeight { get; set; }
    public uint tilesetLookback { get; set; }
    public Pointer layoutPointers { get; set; }
    public Pointer tilesetPartsPointer { get; set; }
    public Pointer[] tilesetPartPointers { get; set; }
    public Pointer palettePointer { get; set; }
    public Palette palette { get; set; }
    public Pointer tileBehaviorsPointer { get; set; }
    public byte[] tileBehaviors { get; set; } // TODO implement behaviors type
    public Pointer objectsPointer { get; set; }
    // TODO implement objects
    public Pointer overlayPointer { get; set; }
    // TODO implement overlay
    public Pointer itemBoxPointer { get; set; }
    // TODO implement item boxes
    public Pointer finishLinePointer { get; set; }
    // TODO implement finish line

    public uint data0;
    public uint trackRoutine { get; set; }
    public Pointer minimapPointer { get; set; }
    // TODO implement minimap
    public Pointer aiZonesPointer { get; set; }
    // TODO implement AI zones
    public Pointer objectGfxPointer { get; set; }
    // TODO implement object graphics
    public Pointer objectPalettePointer { get; set; }
    // TODO implement objectPalette
    public Tileset tileset { get; set; }
    public Layout layout { get; set; }


#pragma warning enable
    public override void SerializeImpl(SerializerObject s)
    {
        Pointer basePointer = s.CurrentPointer;
        magic = s.Serialize<uint>(magic, nameof(magic));
        trackWidth = s.Serialize<byte>(trackWidth, nameof(trackWidth));
        trackHeight = s.Serialize<byte>(trackHeight, nameof(trackHeight));

        s.SerializePadding(42);
        
        tilesetLookback = s.Serialize<uint>(tilesetLookback,nameof(tilesetLookback));

        s.SerializePadding(12);

        layoutPointers = s.SerializePointer(layoutPointers, PointerSize.Pointer32, basePointer, name: nameof(layoutPointers));
        
        s.SerializePadding(60);

        tilesetPartsPointer = s.SerializePointer(tilesetPartsPointer, PointerSize.Pointer32, basePointer, name: nameof(tilesetPartPointers));
        palettePointer = s.SerializePointer(palettePointer, PointerSize.Pointer32, basePointer, name: nameof(palettePointer));
        tileBehaviorsPointer = s.SerializePointer(tileBehaviorsPointer, PointerSize.Pointer32, basePointer, name: nameof(tileBehaviorsPointer));
        objectsPointer = s.SerializePointer(objectsPointer, PointerSize.Pointer32, basePointer, name: nameof(objectsPointer));
        overlayPointer = s.SerializePointer(overlayPointer, PointerSize.Pointer32, basePointer, name: nameof(overlayPointer));
        overlayPointer = s.SerializePointer(overlayPointer, PointerSize.Pointer32, basePointer, name: nameof(itemBoxPointer));
        finishLinePointer = s.SerializePointer(finishLinePointer, PointerSize.Pointer32, basePointer, name: nameof(finishLinePointer));
        data0 = s.Serialize<uint>(data0, name: nameof(data0));
        
        s.SerializePadding(32);

        trackRoutine = s.Serialize<uint>(trackRoutine, nameof(trackRoutine));
        minimapPointer = s.SerializePointer(minimapPointer, PointerSize.Pointer32, basePointer, name: nameof(minimapPointer));
        
        s.SerializePadding(4);

        aiZonesPointer = s.SerializePointer(aiZonesPointer, PointerSize.Pointer32, basePointer, name: nameof(aiZonesPointer));
        
        s.SerializePadding(20);

        objectGfxPointer = s.SerializePointer(objectGfxPointer, PointerSize.Pointer32, basePointer, name: nameof(objectGfxPointer));
        objectPalettePointer = s.SerializePointer(objectPalettePointer, PointerSize.Pointer32, basePointer, name: nameof(objectPalettePointer));
        
        s.SerializePadding(20);

        palette = s.DoAt(palettePointer, () => 
            s.SerializeObject(palette, name: nameof(palette))
        );

        layout = s.DoAt(layoutPointers, () => 
            s.SerializeObject<Layout>(layout,onPreSerialize: x => x.size = new(trackWidth,trackHeight), name: nameof(layout))
        );

        // TODO: Implement tileset lookback
        if (tilesetPartsPointer.AbsoluteOffset != palettePointer.AbsoluteOffset)
        {
            tileset = s.DoAt(tilesetPartsPointer, () => 
                s.SerializeObject<Tileset>(tileset, name: nameof(tileset))
            );
        }
        else { }
    }
}
