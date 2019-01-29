using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace BlueDB.Serialize.Types
{
    public class DateTimeType : SerializeType<DateTime>
    {
        public override bool Test(Type type)
        {
            return type.FullName == "System.DateTime";
        }

        public override void Serialize(BinaryWriter writer, object value)
        {
            var valueLong = ((DateTime)value).ToBinary();
            writer.Write(valueLong);
        }

        public override object Deserialize(BinaryReader reader, Type type)
        {
            var value = reader.ReadInt64();
            var dateTime =  DateTime.FromBinary(value);
            return dateTime;
        }
    }
}