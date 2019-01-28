using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BlueDB.Serialize
{
    public interface ISerilizeObjectKnowProperty
    {
        bool Test(Type propertyType);
        ISerializeObjectProperty Create(PropertyInfo propertyInfo);
    }
}