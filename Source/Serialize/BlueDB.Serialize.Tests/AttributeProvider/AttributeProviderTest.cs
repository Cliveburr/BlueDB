using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Serialize.Tests.AttributeProvider
{
    [TestClass]
    public class AttributeProviderTest
    {
        [TestMethod]
        public void CustomSerialize()
        {
            var values = new AttributeProviderEntity
            {
                SomeAnyValue = 666
            };

            var serialize = BinarySerialize.From<AttributeProviderEntity>();

            var bytes = serialize.Serialize(values);

            var returnedValues = serialize.Deserialize(bytes);

            Assert.AreEqual(values.SomeAnyValue, returnedValues.SomeAnyValue);
        }

        [TestMethod]
        public void CustomPropertySerialize()
        {
            var values = new AttributeProviderPropertyEntity
            {
                TestSomeString = "uma string qualquer!!!"
            };

            var serialize = BinarySerialize.From<AttributeProviderPropertyEntity>();

            var bytes = serialize.Serialize(values);

            var returnedValues = serialize.Deserialize(bytes);

            Assert.AreEqual(values.TestSomeString, returnedValues.TestSomeString.TrimEnd('\0'));
        }

        [TestMethod]
        public void KnowTypeSerialize()
        {
            var values = new KnowTypeEntity
            {
                ObjectProp = "essa vai de string",
                InterfaceProp = new KnowTypeTestClass0
                {
                    Class0String = "outra string aqui"
                }
            };

            var serialize = BinarySerialize.From<KnowTypeEntity>();

            var bytes = serialize.Serialize(values);

            var returnedValues = serialize.Deserialize(bytes);

            Assert.AreEqual(values.ObjectProp, returnedValues.ObjectProp);
            Assert.AreEqual(((KnowTypeTestClass0)values.InterfaceProp).Class0String, ((KnowTypeTestClass0)returnedValues.InterfaceProp).Class0String);
        }
    }
}