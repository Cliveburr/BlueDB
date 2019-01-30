using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Serialize.Tests.Interface
{
#pragma warning disable
    [Serializable]
    public class InterfaceEntity
    {
        public ISomeInterface ImplementationOne { get; set; }
        public ISomeInterface ImplementationTwo { get; set; }
        public ISomeInterface SomeNullValue { get; set; }
        public IList<ISomeInterface> ListOfInterface { get; set; }
        public ISomeInterface[] ArrayOfInterface { get; set; }

        public void Populate()
        {
            ImplementationOne = new ImplmentOne
            {
                Id = 123,
                Name = "essa é uma implmentação da interface!"
            };
            ImplementationTwo = new ImplmentTwo
            {
                Id = 444,
                Name = "essa é outra implmentação da interface!"
            };
            ListOfInterface = new List<ISomeInterface>
            {
                new ImplmentOne { Id = 1, Name = "primeiro colocado! List" },
                new ImplmentTwo { Id = 2, Name = "segundo colocado! List" },
                new ImplmentOne { Id = 3, Name = "terceiro colocado! List" }
            };
            ArrayOfInterface = new ISomeInterface[]
            {
                new ImplmentOne { Id = 1, Name = "primeiro colocado!" },
                new ImplmentTwo { Id = 2, Name = "segundo colocado!" },
                new ImplmentOne { Id = 3, Name = "terceiro colocado!" }
            };
        }
    }

    public interface ISomeInterface
    {
        int Id { get; set; }
        string Name { get; set; }
    }

    [Serializable]
    public class ImplmentOne : ISomeInterface
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            var from = obj as ImplmentOne;
            return Name == from.Name &&
                Id == from.Id;
        }
    }

    [Serializable]
    public class ImplmentTwo : ISomeInterface
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public System.DateTime ExtraData { get; set; }

        public override bool Equals(object obj)
        {
            var from = obj as ImplmentTwo;
            return Name == from.Name &&
                Id == from.Id;
        }
    }

#pragma warning restore
}