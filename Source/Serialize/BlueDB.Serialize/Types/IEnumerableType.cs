using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BlueDB.Serialize.Types
{
    public class IEnumerableType : SerializeType<IEnumerable>
    {
        public override bool Test(Type type)
        {
            return typeof(IEnumerable).IsAssignableFrom(type); ;
        }

        public override void Serialize(BinaryWriter writer, object value)
        {
            if (value == null)
            {
                writer.Write((uint)0);
            }
            else
            {
                var elementType = value.GetType().GenericTypeArguments[0];

                var ienum = value as IEnumerable;
                var serialize = BinarySerialize.From(elementType);

                var count = 0;

                using (var memoryStream = new MemoryStream())
                using (var writeStream = new BinaryWriter(memoryStream))
                {
                    foreach (var item in ienum)
                    {
                        count++;
                        serialize.Serialize(writeStream, item);
                    }

                    writer.Write((uint)(count + 1));
                    writer.Write(memoryStream.ToArray());
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
                var elementType = type.GenericTypeArguments[0];

                var serialize = BinarySerialize.From(elementType);
                length--;
                var obj = Array.CreateInstance(elementType, length) as Array;

                for (var i = 0; i < length; i++)
                {
                    obj.SetValue(serialize.Deserialize(reader, elementType), i);
                }

                return obj;
            }
        }
    }
}