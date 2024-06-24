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