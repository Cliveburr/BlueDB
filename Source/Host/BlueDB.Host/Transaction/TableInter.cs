using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlueDB.Communication.Entity;
using BlueDB.Host.Data;
using BlueDB.Host.Data.Interface;

namespace BlueDB.Host.Transaction
{
    public class TableInter : IDisposable, IEnumerable<IData>, IEnumerator<IData>, IEnumerator
    {
        public string Name { get; private set; }

        private IDatabase _database;
        private ITable _data;
        private Dictionary<uint, IData> _tempData;
        private uint _lastId;

        public TableInter(IDatabase database, string name)
        {
            _database = database;
            Name = name;
        }

        public void Open(Action<TableInter> callBack)
        {
            _tempData = new Dictionary<uint, IData>();

            _database.OpenTable(Name, (table) =>
            {
                _data = table;
                _lastId = _data.LastId;

                callBack(this);
            });
        }

        public void ClearSelection()
        {
        }

        public uint ReserveId()
        {
            return ++_lastId;
        }

        public void Create(List<Property> properties)
        {
            var datasets = properties
                .Where(p => ((byte)p.Type) > 2)
                .ToDictionary(
                    p => p.Name,
                    p => new DataProperty((DataType)p.Type, p.Value)
                );

            var newData = new Data.MemoryData.Data
            {
                Id = ReserveId(),
                Datasets = datasets,
                State = DataState.Create
            };

            _tempData.Add(newData.Id, newData);
        }

        public void Rollback()
        {
            Dispose();
        }

        public void Commit()
        {
            _data.LastId = _lastId;

            foreach (var data in _tempData)
            {
                switch (data.Value.State)
                {
                    case DataState.Create: _data.Insert(data.Value); break;
                    case DataState.Update: _data.Update(data.Value); break;
                    case DataState.Remove: _data.Remove(data.Value); break;
                }
            }

            _data.Save();

            Dispose();
        }

        public void Dispose()
        {
            _database = null;
            _data = null;
            _tempData = null;
        }

        public IData ReadById(uint id)
        {
            if (_tempData.ContainsKey(id))
            {
                return _tempData[id];
            }
            else
            {
                var data = _data.ReadById(id);
                _tempData.Add(id, data);
                return data;
            }
        }

        public void Update(uint id, List<Property> properties)
        {
            var data = ReadById(id);
            if (data == null)
            {
                throw new Exception(string.Format("Data Id \"{0}\" on table \"{1}\" not found!", id.ToString(), Name));
            }

            foreach (var property in properties)
            {
                switch (property.Type)
                {
                    case PropertyType.Invalid:
                    case PropertyType.Id:
                        continue;
                    case PropertyType.Delete:
                        {
                            if (data.Datasets.ContainsKey(property.Name))
                            {
                                data.Datasets.Remove(property.Name);
                            }
                            break;
                        }
                    default:
                        {
                            var newSet = new DataProperty((DataType)property.Type, property.Value);

                            if (data.Datasets.ContainsKey(property.Name))
                            {
                                data.Datasets[property.Name] = newSet;
                            }
                            else
                            {
                                data.Datasets.Add(property.Name, newSet);
                            }
                            break;
                        }
                }
            }

            data.State = DataState.Update;
        }

        public IData Current => throw new NotImplementedException();

        object IEnumerator.Current => throw new NotImplementedException();

        public IEnumerator<IData> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        public bool MoveNext()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}