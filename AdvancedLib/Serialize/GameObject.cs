using BinarySerializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AdvancedLib.Serialize.GameObjectGroup;

namespace AdvancedLib.Serialize
{
    public class GameObjectGroup : BinarySerializable
    {
        public GameObject[] gameObject;
        public override void SerializeImpl(SerializerObject s)
        {
            gameObject = s.SerializeObjectArrayUntil(gameObject, x => (x.id | x.xPosition | x.yPosition | x.zone) == 0, name: nameof(gameObject));
        }
    }
    public class GameObject : BinarySerializable
    {
        public byte id;
        public byte xPosition;
        public byte yPosition;
        public byte zone;

        public override void SerializeImpl(SerializerObject s)
        {
            id = s.Serialize(id, nameof(id));
            xPosition = s.Serialize(xPosition, nameof(id));
            yPosition = s.Serialize(yPosition, nameof(id));
            zone = s.Serialize(zone, nameof(id));
        }
    }
}
