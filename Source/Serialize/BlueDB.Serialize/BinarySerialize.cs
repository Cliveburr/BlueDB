using BlueDB.Serialize.Attributes;
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
        private static IDictionary<string, ISerializeType> _knowTypes;
        private static IProvider[] _providers;

        static BinarySerialize()
        {
            _knowTypes = new Dictionary<string, ISerializeType>();

            SetProviders();
        }

        private static void SetProviders()
        {
            _providers = new IProvider[]
            {
                new AttributeProvider(),
                new ByteProvider(),
                new Int16Provider(),
                new UInt16Provider(),
                new Int32Provider(),
                new UInt32Provider(),
                new Int64Provider(),
                new UInt64Provider(),
                new StringProvider(),
                new DateTimeProvider(),
                new ArrayProvider(),
                new DictionaryProvider(),
                new IEnumerableProvider(),
                new InterfaceProvider(),
                new EnumProvider(),
                new ObjectProvider(),
                new ClassProvider()
            };
        }

        public static ISerializeType From(Type type)
        {
            var fullName = type.FullName;

            if (_knowTypes.ContainsKey(fullName))
            {
                return _knowTypes[fullName];
            }
            else
            {
                var provider = _providers
                    .First(p => p.Test(type));

                var newKnowType = provider.GetSerializeType(type);
                _knowTypes[fullName] = newKnowType;

                newKnowType.Initialize();

                return newKnowType;
            }
        }

        public static SerializeType<T> From<T>()
        {
            var type = typeof(T);
            return (SerializeType<T>)From(type);
        }
    }
}