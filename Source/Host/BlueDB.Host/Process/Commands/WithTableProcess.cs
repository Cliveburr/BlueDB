using BlueDB.Communication.Messages.Commands;
using BlueDB.Host.Enums;
using BlueDB.Host.Transaction;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Host.Process.Commands
{
    public static class WithTableProcess
    {
        public static void Execute(Executor executor, WithTableCommand withTableCommand)
        {
            if (executor.DatabaseSelected == null)
            {
                throw new Exception("Invalid WithTable command without selected database!");
            }

            executor.DatabaseSelected.OpenTable(withTableCommand.TableName, (table) =>
            {
                executor.DatabaseSelected.TableSelected = table;

                if (withTableCommand.ClearSelection)
                {
                    table.ClearSelection();
                }

                executor.ExecuteNextCommand();
            });
        }
    }
}