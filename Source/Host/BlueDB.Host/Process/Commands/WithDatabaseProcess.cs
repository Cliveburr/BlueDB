using System;
using System.Collections.Generic;
using System.Text;
using BlueDB.Communication.Messages.Commands;
using BlueDB.Host.Enums;
using BlueDB.Host.Transaction;

namespace BlueDB.Host.Process.Commands
{
    public static class WithDatabaseProcess
    {
        public static void Execute(Executor executor, WithDatabaseCommand withDatabaseCommand)
        {
            var transaction = executor.ProcessContext.Connection.GetItemInBag<TransactionOperation>(BagNames.TRANSACTION);

            transaction.OpenDatabase(withDatabaseCommand.DatabaseName, (database) =>
            {
                executor.DatabaseSelected = database;

                executor.ExecuteNextCommand();
            });
        }
    }
}