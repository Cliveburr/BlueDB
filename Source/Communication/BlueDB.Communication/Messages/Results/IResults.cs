using BlueDB.Serialize.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Communication.Messages.Results
{
    [KnowType(typeof(SelectResult))]
    public interface IResults
    {
    }
}