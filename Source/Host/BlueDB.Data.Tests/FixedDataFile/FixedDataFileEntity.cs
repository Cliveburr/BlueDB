using BlueDB.Serialize.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Data.Tests.FixedDataFile
{
    public class FixedDataFileEntity
    {
        public uint Id { get; set; }
        [SerializeSize(100)]
        public string Name { get; set; }
        public DateTime CreatedDatetime { get; set; }

        public override bool Equals(object obj)
        {
            var t = obj as FixedDataFileEntity;
            return Id == t.Id
                && Name == t.Name
                && CreatedDatetime == t.CreatedDatetime;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, CreatedDatetime);
        }
    }
}