using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Host.Data.Interface
{
    public interface IData
    {
        uint Id { get; set; }
        Dictionary<string, DataProperty> Datasets { get; set; }
        DataState State { get; set; }
    }
}