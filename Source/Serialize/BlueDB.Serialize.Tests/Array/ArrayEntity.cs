using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Serialize.Tests.Array
{
#pragma warning disable
    [Serializable]
    public class ArrayEntity
    {
        public int[] IntArray { get; set; }
        public System.String[] NullStringArray { get; set; }
        public ClassArrayEntity[] ClassArrayEntityArray { get; set; }

        public void Populate()
        {
            IntArray = new int[]
            {
                4, 5, 6, 8, 9, 11, 66
            };
            ClassArrayEntityArray = new ClassArrayEntity[]
            {
                new ClassArrayEntity { Id = 4, Name = "esse é o primeiro" },
                new ClassArrayEntity { Id = 6, Name = "minha id é 6" },
                new ClassArrayEntity { Id = 3, Name = "Só mais um!!!!" }
            };
        }
    }

    [Serializable]
    public class ClassArrayEntity
    {
        public byte Id { get; set; }
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            var from = obj as ClassArrayEntity;
            return Name == from.Name &&
                Id == from.Id;
        }
    }
#pragma warning restore
}