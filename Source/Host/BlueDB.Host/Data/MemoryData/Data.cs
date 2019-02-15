using BlueDB.Host.Data.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Host.Data.MemoryData
{
    public class Data : IData
    {
        public uint Id { get; set; }
        public Dictionary<string, DataProperty> Datasets { get; set; }
        public DataState State { get; set; }
    }
}