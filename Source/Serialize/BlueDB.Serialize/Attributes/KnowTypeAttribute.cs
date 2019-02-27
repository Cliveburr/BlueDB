using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BlueDB.Serialize.Attributes
{
    public class KnowTypeAttribute : SerializeTypeAttribute
    {
        private Type[] _types;

        public KnowTypeAttribute(params Type[] types)
        {
            _types = types;
        }

        public override ISerializeType GetSerializeType(Type type)
        {
            return new KnowType(_types);
        }
    }

    public class KnowType : SerializeType<string>
    {
        private Type[] _types;
        private Tuple<ushort, string, ISerializeType>[] _knowTypes;

        public KnowType(Type[] types)
        {
            _types = types;
        }

        public override void Initialize()
        {
            var index = 0;
            _knowTypes = _types
                .Select(t => new Tuple<ushort, string, ISerializeType>((ushort)index++, t.FullName, BinarySerialize.From(t)))
                .ToArray();
        }

        public override void Serialize(BinaryWriter writer, object value)
        {
            var fullName = value.GetType().FullName;
            var knowType = _knowTypes
                .FirstOrDefault(k => k.Item2 == fullName);
            if (knowType == null)
            {
                throw new Exception(string.Format("Unknow type! {0}", fullName));
            }

            writer.Write(knowType.Item1);
            knowType.Item3.Serialize(writer, value);
        }

        public override object Deserialize(BinaryReader reader, Type type)
        {
            var knowTypeIndex = reader.ReadUInt16();
            var knowType = _knowTypes[knowTypeIndex];

            return knowType.Item3.Deserialize(reader, type);
        }
    }
}