using BlueDB.Serialize.KnowProperty;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BlueDB.Serialize
{
    public static class BinarySerialize
    {
        public static IDictionary<string, SerializeObjectType> ObjectTypes { get; private set; }
        public static IList<ISerilizeObjectKnowProperty> KnowProperties { get; set; }

        static BinarySerialize()
        {
            ObjectTypes = new Dictionary<string, SerializeObjectType>();

            SetDefaultKnowProperties();
        }
        
        private static void SetDefaultKnowProperties()
        {
            KnowProperties = new List<ISerilizeObjectKnowProperty>
            {
                new ByteKnowProperty(),
                new Int16KnowProperty(),
                new UInt16KnowProperty(),
                new Int32KnowProperty(),
                new UInt32KnowProperty(),
                new Int64KnowProperty(),
                new UInt64KnowProperty(),
                new StringKnowProperty()
            };
        }

        public static SerializeObjectType From(Type type)
        {
            var fullName = type.FullName;
            if (!ObjectTypes.ContainsKey(fullName))
            {
                var newObjectType = new SerializeObjectType(type);
                ObjectTypes[fullName] = newObjectType;
            }
            return ObjectTypes[fullName];
        }

        public static SerializeObjectGenericType<T> From<T>()
        {
            var type = typeof(T);
            var fullName = type.FullName;
            if (!ObjectTypes.ContainsKey(fullName))
            {
                var newObjectType = new SerializeObjectGenericType<T>(type);
                ObjectTypes[fullName] = newObjectType;
            }
            return ObjectTypes[fullName] as SerializeObjectGenericType<T>;
        }
    }
}