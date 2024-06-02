using BinarySerializer;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdvancedLib.Types{
    /// <summary>
    /// Palette of BGR555 colors
    /// </summary>
    public class Palette64 : BinarySerializable
    {
        public BgrColor[] palette;
        public byte[] rawPalette {
            get {
                byte[] raw = new byte[128];
                byte[] data = new byte[2];
                for (int i = 0; i < palette.Length; i+=2)
                {
                    data = BitConverter.GetBytes(palette[i].rawValue);
                    raw[i] = data[0];
                    raw[i+1] = data[1];
                }
                return raw;
            }
            set {
                BgrColor[] pal = new BgrColor[rawPalette.Length / 2];
                for (int i = 0; i < rawPalette.Length; i += 2)
                {
                    ushort color = (ushort)((rawPalette[i + 1] << 8) | rawPalette[i]);
                    pal[i / 2] = new BgrColor(color);
                }
                this.palette = pal;
            }
        }
        public Palette64(BgrColor[] pal)
        {
            palette = pal;
        }
        public Palette64()
        {
            palette = new BgrColor[64];
            for (int i = 0; i < palette.Length; i++) { BgrColor? p = palette[i]; p = new BgrColor(); }
        }
        public Palette64(byte[] rawPalette)
        {
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
            palette = s.SerializeObjectArray<BgrColor>(palette, 64, name: nameof(palette));
        }
    }
}