using BlueDB.Communication.Entity;
using BlueDB.Communication.Messages.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueDB.Host.Process.Commands
{
    public static class SetProcess
    {
        public static void Execute(Executor executor, SetCommand setCommand)
        {
            if (executor.DatabaseSelected == null)
            {
                throw new Exception("Invalid Set command without database selected!");
            }

            if (executor.DatabaseSelected.TableSelected == null)
            {
                throw new Exception("Invalid Set command without table selected!");
            }

            var table = executor.DatabaseSelected.TableSelected;
            var properties = setCommand.Properties
                .ToList();

            var idProperty = properties
                .FirstOrDefault(p => p.Type == PropertyType.Id || p.Name == "_id");
            if (idProperty == null)
            {
                table.Create(properties);
            }
            else
            {
                properties.Remove(idProperty);

                if (idProperty.ValueUInt == 0)
                {
                    table.Create(properties);
                }
                else
                {
                    table.Update(idProperty.ValueUInt, properties);
                }
            }

            executor.ExecuteNextCommand();
        }
    }
}