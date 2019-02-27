using BlueDB.Serialize.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Serialize.Tests.AttributeProvider
{
    public class KnowTypeEntity
    {
        [KnowType(typeof(int), typeof(string))]
        public object ObjectProp { get; set; }

        public IKnowTypeTestInterface InterfaceProp { get; set; }
    }

    [KnowType(typeof(KnowTypeTestClass0), typeof(KnowTypeTestClass1))]
    public interface IKnowTypeTestInterface
    {
    }

    public class KnowTypeTestClass0 : IKnowTypeTestInterface
    {
        public string Class0String { get; set; }
    }

    public class KnowTypeTestClass1 : IKnowTypeTestInterface
    {
        public int Class1Int { get; set; }
    }
}