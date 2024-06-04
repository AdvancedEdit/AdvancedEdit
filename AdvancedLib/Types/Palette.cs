using System.ComponentModel;
using BinarySerializer;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdvancedLib.Types{
    /// <summary>
    /// Palette of BGR555 colors
    /// </summary>
    public class Palette : BinarySerializable
    {
        public int paletteLength {get; set;}
        public BgrColor[] palette;
        public byte[] rawPalette {
            get {
                byte[] raw = new byte[paletteLength*2];
                byte[] data = new byte[2];
                for (int i = 0; i < paletteLength; i+=2)
                {
                    data = BitConverter.GetBytes(palette[i].rawValue);
                    raw[i] = data[0];
                    raw[i+1] = data[1];
                }
                return raw;
            }
        }
        public Palette(BgrColor[] pal)
        {
            paletteLength = pal.Length;
            palette = pal;
        }
        public Palette()
        {
            paletteLength = 64;
            palette = new BgrColor[paletteLength];
            for (int i = 0; i < paletteLength; i++) { BgrColor? p = palette[i]; p = new BgrColor(); }
        }
        public Palette(byte[] rawPalette)
        {
            paletteLength = rawPalette.Length/2;
            BgrColor[] pal = new BgrColor[rawPalette.Length / 2];
            for (int i = 0; i < rawPalette.Length; i += 2)
            {
                ushort color = (ushort)((rawPalette[i + 1] << 8) | rawPalette[i]);
                pal[i / 2] = new BgrColor(color);
            }
            this.palette = pal;
        }
        public BgrColor this[int i]
        {
            get { 
                if (i > palette.Length) i = i % palette.Length;
                return this.palette[i]; 
            }
            set { 
                this.palette[i] = value; 
            }
        }
        public override void SerializeImpl(SerializerObject s)
        {
            palette = s.SerializeObjectArray<BgrColor>(palette, paletteLength, name: nameof(palette));
        }
    }
}