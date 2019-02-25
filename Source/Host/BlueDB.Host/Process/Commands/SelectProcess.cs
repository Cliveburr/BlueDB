using BlueDB.Communication.Entity;
using BlueDB.Communication.Messages.Commands;
using BlueDB.Communication.Messages.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueDB.Host.Process.Commands
{
    public static class SelectProcess
    {
        public static void Execute(Executor executor, SelectCommand setCommand)
        {
            var table = executor.DatabaseSelected.TableSelected;

            var newResult = new SelectResult
            {
                DatabaseName = executor.DatabaseSelected.Name,
                TableName = table.Name,
                Data = new Dictionary<uint, Property[]>()
            };

            foreach (var data in table)
            {
                var properties = data.Datasets
                    .Select(d => new Property
                    {
                        Name = d.Key,
                        Type = (PropertyType)d.Value.Type,
                        Value = d.Value.Value
                    })
                    .ToArray();

                newResult.Data.Add(data.Id, properties);
            }

            executor.Results.Add(newResult);

            executor.ExecuteNextCommand();
        }
    }
}