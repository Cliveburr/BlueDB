using BlueDB.Communication.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Communication.Messages.Results
{
    public class SelectResult : IResults
    {
        public string DatabaseName { get; set; }
        public string TableName { get; set; }
        public Dictionary<uint, Property[]> Data { get; set; }
    }
}