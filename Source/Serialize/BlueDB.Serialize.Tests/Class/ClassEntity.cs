using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Serialize.Tests.Class
{
    [Serializable]
    public class ClassEntity
    {
        public string Name { get; set; }
        public int Index { get; set; }
        public System.DateTime Created { get; set; }
        public ClassEntityDeeperOne Deeper { get; set; }
        public ClassEntityDeeperOne DeeperNull { get; set; }
        public ClassEntity Recursive { get; set; }

        public void Populate(bool stopRecursive = false)
        {
            Name = "ClassEntity";
            Index = 22;
            Created = new System.DateTime(2019, 1, 28);
            Deeper = new ClassEntityDeeperOne
            {
                Name = "ClassEntityDeeperOne",
                Index = 33,
                Created = new System.DateTime(2018, 6, 18),
                MoreDeeper = new ClasEntityDeeperTwo
                {
                    Name = "ClasEntityDeeperTwo",
                    Index = 44
                }
            };
            if (!stopRecursive)
            {
                Recursive = new ClassEntity();
                Recursive.Populate(true);
            }
        }
    }

    [Serializable]
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class ClassEntityDeeperOne
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public string Name { get; set; }
        public int Index { get; set; }
        public System.DateTime Created { get; set; }
        public ClasEntityDeeperTwo MoreDeeper { get; set; }

        public override bool Equals(object obj)
        {
            var from = obj as ClassEntityDeeperOne;
            return Name == from.Name &&
                Index == from.Index &&
                Created.Equals(from.Created) &&
                MoreDeeper.Equals(from.MoreDeeper);
        }
    }

    [Serializable]
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class ClasEntityDeeperTwo
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public string Name { get; set; }
        public int Index { get; set; }
        public System.DateTime Created { get; set; }

        public override bool Equals(object obj)
        {
            var from = obj as ClasEntityDeeperTwo;
            return Name == from.Name &&
                Index == from.Index &&
                Created.Equals(from.Created);
        }
    }
}