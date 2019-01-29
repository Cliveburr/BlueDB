using BlueDB.Serialize.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BlueDB.Serialize.Types
{
    public class StringType : SerializeType<string>
    {
        public override bool Test(Type type)
        {
            return type.FullName == "System.String";
        }

        public override void Serialize(BinaryWriter writer, object value)
        {
            var stringValue = value as string;

            if (stringValue == null)
            {
                writer.Write((uint)0);
            }
            else if (stringValue == string.Empty)
            {
                writer.Write((uint)1);
            }
            else
            {
                var bytes = Encoding.UTF8.GetBytes(stringValue);

                writer.Write((uint)(bytes.Length + 1));
                writer.Write(bytes);
            }
        }

        public override object Deserialize(BinaryReader reader, Type type)
        {
            var length = reader.ReadUInt32();

            if (length == 0)
            {
                return null;
            }
            else if (length == 1)
            {
                return string.Empty;
            }
            else
            {
                var bytes = reader.ReadBytes((int)(length - 1));
                return Encoding.UTF8.GetString(bytes);
            }
        }
    }
}