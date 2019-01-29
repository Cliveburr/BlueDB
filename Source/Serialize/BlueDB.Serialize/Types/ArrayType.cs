using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace BlueDB.Serialize.Types
{
    public class ArrayType : SerializeType<Array>
    {
        public override bool Test(Type type)
        {
            return type.IsArray;
        }

        public override void Serialize(BinaryWriter writer, object value)
        {
            if (value == null)
            {
                writer.Write((uint)0);
            }
            else
            {
                var array = value as Array;
                var serialize = BinarySerialize.From(array.GetType().GetElementType());
                
                writer.Write((uint)(array.Length + 1));

                for (var i = 0; i < array.Length; i++)
                {
                    serialize.Serialize(writer, array.GetValue(i));
                }
            }
        }

        public override object Deserialize(BinaryReader reader, Type type)
        {
            var length = reader.ReadUInt32();

            if (length == 0)
            {
                return null;
            }
            else
            {
                var arrayElementType = type.GetElementType();
                var serialize = BinarySerialize.From(arrayElementType);
                length--;
                var obj = Array.CreateInstance(arrayElementType, length) as Array;

                for (var i = 0; i < length; i++)
                {
                    obj.SetValue(serialize.Deserialize(reader, arrayElementType), i);
                }

                return obj;
            }
        }
    }
}