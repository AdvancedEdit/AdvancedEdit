using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdvancedLib.Types;
using BinarySerializer;
using BinarySerializer.Nintendo.GBA;

namespace AdvancedLib.Serialize;


/// <summary>
/// A class to serialize tracks from binary data
/// </summary>
public class Track : BinarySerializable
{
#pragma warning disable
    ushort magic;

    #region Variables
    public ushort trackType { get; set; }
    public byte trackWidth { get; set; }
    public byte trackHeight { get; set; }
    public uint tilesetLookback { get; set; }
    public Pointer layoutPointer { get; set; }
    public Layout layout { get; set; }
    public Pointer tilesetPartsPointer { get; set; }
    public Tileset tileset { get; set; }
    public Pointer palettePointer { get; set; }
    public AdvancedLib.Types.Palette palette { get; set; }
    public Pointer tileBehaviorsPointer { get; set; }
    public byte[] tileBehaviors { get; set; } // TODO implement behaviors type
    public Pointer gameObjectsPointer { get; set; }
    public GameObjectGroup gameObjects {get; set;}
    public Pointer overlayPointer { get; set; }
    public GameObjectGroup overlay { get; set; }
    public Pointer itemBoxesPointer { get; set; }
    public GameObjectGroup itemBoxes { get; set; }
    public Pointer finishLinePointer { get; set; }
    public GameObjectGroup finishLine { get; set; }
    public uint data0 { get; set; }
    public uint trackRoutine { get; set; }
    public Pointer minimapPointer { get; set; }
    public Minimap minimap { get; set; }
    public Pointer trackAIPointer { get; set; }
    public TrackAI trackAI { get; set; }
    public Pointer objectGfxPointer { get; set; }
    public ObjectGfx? objectGfx { get; set; }
    public MultiObjectGfx? multiObjectGfx { get; set; }
    public Pointer objectPalettePointer { get; set; }
    public AdvancedLib.Types.Palette objectPalette { get; set; }

    private byte[] unk0;
    private byte[] unk1;
    private byte[] unk2;
    private byte[] unk3;
    private byte[] unk4;
    private byte[] unk5;
    #endregion

    public override void SerializeImpl(SerializerObject s)
    {
        Pointer basePointer = s.CurrentPointer;

        #region Header
        magic = s.Serialize<ushort>(
            magic,
            nameof(magic)
        );
        trackType = s.Serialize<ushort>(
            trackType,
            nameof(trackType)
        );
        trackWidth = s.Serialize<byte>(
            trackWidth,
            nameof(trackWidth)
        );
        trackHeight = s.Serialize<byte>(
            trackHeight,
            nameof(trackHeight)
        );

        unk0 = s.SerializeArray(unk0, 42, "Padding - unk");
        //s.SerializePadding(42);
        
        tilesetLookback = s.Serialize<uint>(
            tilesetLookback,
            nameof(tilesetLookback)
        );

        unk1 = s.SerializeArray(unk1, 12, "Padding - unk");
        //s.SerializePadding(12);

        layoutPointer = s.SerializePointer(
            layoutPointer,
            PointerSize.Pointer32,
            basePointer,
            name: nameof(layoutPointer)
        );

        unk2 = s.SerializeArray(unk2, 60, "Padding - unk");
        //s.SerializePadding(60);

        tilesetPartsPointer = s.SerializePointer(
            tilesetPartsPointer,
            PointerSize.Pointer32,
            basePointer,
            name: nameof(tilesetPartsPointer)
        );
        palettePointer = s.SerializePointer(
            palettePointer,
            PointerSize.Pointer32,
            basePointer,
            name: nameof(palettePointer)
        );
        tileBehaviorsPointer = s.SerializePointer(
            tileBehaviorsPointer,
            PointerSize.Pointer32,
            basePointer,
            name: nameof(tileBehaviorsPointer)
        );
        gameObjectsPointer = s.SerializePointer(
            gameObjectsPointer,
            PointerSize.Pointer32,
            basePointer,
            name: nameof(gameObjectsPointer)
        );
        overlayPointer = s.SerializePointer(
            overlayPointer,
            PointerSize.Pointer32,
            basePointer,
            name: nameof(overlayPointer)
        );
        itemBoxesPointer = s.SerializePointer(
            itemBoxesPointer,
            PointerSize.Pointer32,
            basePointer,
            name: nameof(itemBoxesPointer)
        );
        finishLinePointer = s.SerializePointer(
            finishLinePointer,
            PointerSize.Pointer32,
            basePointer,
            name: nameof(finishLinePointer)
        );

        data0 = s.Serialize<uint>(
            data0,
            name: nameof(data0)
        );

        unk3 = s.SerializeArray(unk3, 32, "Padding - unk");
        //s.SerializePadding(32);

        trackRoutine = s.Serialize<uint>(
            trackRoutine,
            nameof(trackRoutine)
        );

        minimapPointer = s.SerializePointer(
            minimapPointer,
            PointerSize.Pointer32,
            basePointer,
            name: nameof(minimapPointer)
        );
        
        s.SerializePadding(4);

        trackAIPointer = s.SerializePointer(
            trackAIPointer,
            PointerSize.Pointer32,
            basePointer,
            name: nameof(trackAIPointer)
        );

        unk4 = s.SerializeArray(unk4, 20, "Padding - unk");
        //s.SerializePadding(20);

        objectGfxPointer = s.SerializePointer(
            objectGfxPointer,
            PointerSize.Pointer32,
            basePointer,
            name: nameof(objectGfxPointer)
        );

        objectPalettePointer = s.SerializePointer(
            objectPalettePointer,
            PointerSize.Pointer32,
            basePointer,
            name: nameof(objectPalettePointer)
        );

        unk5 = s.SerializeArray(unk5, 20, "Padding - unk");
        #endregion

        #region Serialize objects
        s.Goto(palettePointer);
        palette = s.SerializeObject(
            palette,
            onPreSerialize: x => x.paletteLength = 64,
            name: nameof(palette)
        );

        s.Goto(layoutPointer);
        layout = s.SerializeObject<Layout>(
            layout,
            onPreSerialize: x => x.size = new(trackWidth, trackHeight),
            name: nameof(layout)
        );

        // TODO: Implement tileset lookback
        if (tilesetLookback == 0)
        {
            s.Goto(tilesetPartsPointer);
            tileset = s.SerializeObject<Tileset>(
                tileset,
                name: nameof(tileset)
            );
        }
        else { }

        s.Goto(minimapPointer);
        minimap = s.SerializeObject<Minimap>(
            minimap,
            name: nameof(layout)
        );

        s.Goto(tileBehaviorsPointer);
        tileBehaviors = s.SerializeArray(
            tileBehaviors,
            256,
            name: nameof(layout)
        );
        if (objectGfxPointer.SerializedOffset != 0)
        {
            switch (trackType)
            {
                case 0x200:
                case 0x300:
                    {
                        s.Goto(objectGfxPointer);
                        objectGfx = s.SerializeObject<ObjectGfx>(
                            objectGfx,
                            name: nameof(layout)
                        );
                        multiObjectGfx = null;
                        break;
                    }
                case 0x700:
                    {
                        s.Goto(objectGfxPointer);
                        multiObjectGfx = s.SerializeObject<MultiObjectGfx>(
                            multiObjectGfx,
                            name: nameof(layout)
                        );
                        break;
                    }
                default:
                    {
                        throw new NotImplementedException("edge case");
                        break;
                    }
            }
        }
        if (objectPalettePointer.SerializedOffset != 0)
        {
            s.Goto(objectPalettePointer);
            objectPalette = s.SerializeObject<AdvancedLib.Types.Palette>(
                objectPalette,
                onPreSerialize: x => x.paletteLength = 16,
                name: nameof(layout)
            );
        }

        s.Goto(trackAIPointer);
        trackAI = s.SerializeObject<TrackAI>(
            trackAI,
            name: nameof(layout)
        );

        s.Goto(gameObjectsPointer);
        gameObjects = s.SerializeObject<GameObjectGroup>(
            gameObjects,
            name: nameof(gameObjects)
        );

        s.Goto(overlayPointer);
        overlay = s.SerializeObject<GameObjectGroup>(
            overlay,
            name: nameof(overlay)
        );

        s.Goto(itemBoxesPointer);
        itemBoxes = s.SerializeObject<GameObjectGroup>(
            itemBoxes,
            name: nameof(itemBoxes)
        );

        s.Goto(finishLinePointer);
        finishLine = s.SerializeObject<GameObjectGroup>(
            finishLine,
            name: nameof(finishLine)
        );
        #endregion
    }
    public override void RecalculateSize()
    {
        int currentSize = 0;

        #region Update Pointers
        // Header
        currentSize += 256;

        layoutPointer = new Pointer(currentSize, layoutPointer.File, layoutPointer.Anchor, layoutPointer.Size);
        layout.RecalculateSize();
        currentSize += (int)layout.SerializedSize;

        minimapPointer = new Pointer(currentSize, minimapPointer.File, minimapPointer.Anchor, minimapPointer.Size);
        minimap.RecalculateSize();
        currentSize += (int)minimap.SerializedSize;
        if (tilesetLookback == 0)
        {
            tilesetPartsPointer = new Pointer(currentSize, tilesetPartsPointer.File, tilesetPartsPointer.Anchor, tilesetPartsPointer.Size);
            tileset.RecalculateSize();
            currentSize += (int)tileset.SerializedSize;
        } else {
            // implement lookback
        }

        palettePointer = new Pointer(currentSize, palettePointer.File, palettePointer.Anchor, palettePointer.Size);
        palette.RecalculateSize();
        currentSize += (int)palette.SerializedSize;

        tileBehaviorsPointer = new Pointer(currentSize, tileBehaviorsPointer.File, tileBehaviorsPointer.Anchor, tileBehaviorsPointer.Size);
        currentSize += tileBehaviors.Length;

        trackAIPointer = new Pointer(currentSize, trackAIPointer.File, trackAIPointer.Anchor, trackAIPointer.Size);
        trackAI.RecalculateSize();
        currentSize += (int)trackAI.SerializedSize;

        if (objectGfxPointer.SerializedOffset != 0)
        {
            switch (trackType)
            {
                case 0x200:
                case 0x300:
                    objectGfxPointer = new Pointer(currentSize, objectGfxPointer.File, objectGfxPointer.Anchor, objectGfxPointer.Size);
                    objectGfx.RecalculateSize();
                    currentSize += (int)objectGfx.SerializedSize;
                    break;
                case 0x700:
                    objectGfxPointer = new Pointer(currentSize, objectGfxPointer.File, objectGfxPointer.Anchor, objectGfxPointer.Size);
                    multiObjectGfx.RecalculateSize();
                    currentSize += (int)multiObjectGfx.SerializedSize;
                    break;
            }
        }

        if (objectPalettePointer.SerializedOffset != 0)
        {
            objectPalettePointer = new Pointer(currentSize, objectPalettePointer.File, objectPalettePointer.Anchor, objectPalettePointer.Size);
            objectPalette.RecalculateSize();
            currentSize += (int)objectPalette.SerializedSize;
        }

        gameObjectsPointer = new Pointer(currentSize, gameObjectsPointer.File, gameObjectsPointer.Anchor, gameObjectsPointer.Size);
        gameObjects.RecalculateSize();
        currentSize += (int)gameObjects.SerializedSize;

        overlayPointer = new Pointer(currentSize, overlayPointer.File, overlayPointer.Anchor, overlayPointer.Size);
        overlay.RecalculateSize();
        currentSize += (int)overlay.SerializedSize;

        itemBoxesPointer = new Pointer(currentSize, itemBoxesPointer.File, itemBoxesPointer.Anchor, itemBoxesPointer.Size);
        itemBoxes.RecalculateSize();
        currentSize += (int)itemBoxes.SerializedSize;

        finishLinePointer = new Pointer(currentSize, finishLinePointer.File, finishLinePointer.Anchor, finishLinePointer.Size);
        finishLine.RecalculateSize();
        currentSize += (int)finishLine.SerializedSize;
        #endregion

        SerializedSize = currentSize;
    }
}