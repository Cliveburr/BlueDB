using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BlueDB.Serialize.Types
{
    public class EnumType : SerializeType<Enum>
    {
        public override bool Test(Type type)
        {
            return type.IsEnum;
        }

        public override void Serialize(BinaryWriter writer, object value)
        {
            Enum.GetUnderlyingType(typeof(YourEnum));

            writer.Write((Enum)value);
        }

        public override object Deserialize(BinaryReader reader, Type type)
        {
            throw new NotImplementedException();
        }
    }
}