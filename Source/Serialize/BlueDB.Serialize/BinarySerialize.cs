using BlueDB.Serialize.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BlueDB.Serialize
{
    public static class BinarySerialize
    {
        public static IList<SerializeType> KnowTypes { get; set; }

        static BinarySerialize()
        {
            SetDefaultKnowTypes();
        }

        private static void SetDefaultKnowTypes()
        {
            KnowTypes = new List<SerializeType>
            {
                new ByteType(),
                new Int16Type(),
                new UInt16Type(),
                new Int32Type(),
                new UInt32Type(),
                new Int64Type(),
                new UInt64Type(),
                new StringType(),
                new DateTimeType(),
                new ArrayType(),
                new IEnumerableType(),
                new InterfaceType()
            };
        }

        public static SerializeType From(Type type)
        {
            var knowType = KnowTypes
                .Where(kp => kp.Test(type))
                .FirstOrDefault();

            if (knowType == null)
            {
                var genericNowType = typeof(ClassType<>).MakeGenericType(type);
                knowType = (SerializeType)Activator.CreateInstance(genericNowType);
            }

            if (knowType == null)
            {
                throw new NotSupportedException($"Type \"{type.FullName}\" not supported for this serialize!");
            }

            return knowType;
        }

        public static SerializeType<T> From<T>()
        {
            var type = typeof(T);
            return (SerializeType<T>)From(type);
        }
    }
}