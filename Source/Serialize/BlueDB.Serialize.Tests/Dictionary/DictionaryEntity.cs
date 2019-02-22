using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Serialize.Tests.Dictionary
{
#pragma warning disable
    [Serializable]
    public class DictionaryEntity
    {
        public Dictionary<string, int> Dic1 { get; set; }
        public Dictionary<string, object> Dic2 { get; set; }

        public void Populate()
        {
            Dic1 = new Dictionary<string, int>
            {
                { "test0", 0 },
                { "test1", 1 },
                { "test2", 2 },
                { "test3", 3 },
                { "test4", 4 },
                { "test5", 5 }
            };
        }
    }
#pragma warning restore
}