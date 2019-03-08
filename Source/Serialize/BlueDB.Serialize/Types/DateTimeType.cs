using System;
using System.IO;

namespace BlueDB.Serialize.Types
{
    public class DateTimeProvider : IProvider
    {
        public bool Test(Type type)
        {
            return type.FullName == "System.DateTime";
        }

        public ISerializeType GetSerializeType(Type type)
        {
            return new DateTimeType();
        }
    }

    public class DateTimeType : SerializeType<DateTime>
    {
        public override void Serialize(BinaryWriter writer, object value)
        {
            var valueLong = ((DateTime)value).ToBinary();
            writer.Write(valueLong);
        }

        public override object Deserialize(BinaryReader reader)
        {
            var value = reader.ReadInt64();
            var dateTime =  DateTime.FromBinary(value);
            return dateTime;
        }

        public override int CalculateSize()
        {
            return 8;
        }
    }
}