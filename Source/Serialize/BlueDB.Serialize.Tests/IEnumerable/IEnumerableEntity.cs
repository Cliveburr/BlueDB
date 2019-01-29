using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Serialize.Tests.IEnumerable
{
#pragma warning disable
    [Serializable]
    public class IEnumerableEntity
    {
        public IEnumerable<int> IEnumerableInt { get; set; }

        public void Populate()
        {
            IEnumerableInt = new List<int>
            {
                5, 6, 7, 10, 11
            };
        }
    }
#pragma warning restore
}