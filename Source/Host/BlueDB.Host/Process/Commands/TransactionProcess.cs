using BlueDB.Communication.Messages.Commands;
using BlueDB.Host.Enums;
using BlueDB.Host.Transaction;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Host.Process.Commands
{
    public static class OpenTransactionProcess
    {
        public static void Execute(Executor executor, OpenTransactionCommand openTransactionCommand)
        {
            var containsTransaction = executor.ProcessContext.Connection.ContainsItemInBag(BagNames.TRANSACTION);
            if (containsTransaction)
            {
                executor.HandleExecuteError(new Exception("Invalid open transaction!"));
            }

            var transaction = new TransactionOperation
            {
            };

            executor.ProcessContext.Connection.SetItemInBag(BagNames.TRANSACTION, transaction);

            executor.ExecuteNextCommand();
        }
    }

    public static class CommitTransactionProcess
    {
        public static void Execute(Executor executor, CommitTransactionCommand commitTransactionCommand)
        {

            executor.ProcessContext.Connection.RemoteItemInBag(BagNames.TRANSACTION);

            ExecutorController.Instance.ReleaseTables(executor.ProcessContext.TablesToLock);

            executor.ExecuteNextCommand();
        }
    }

    public static class RollbackTransactionProcess
    {
        public static void Execute(Executor executor, RollbackTransactionCommand rollbackTransactionCommand)
        {

            executor.ProcessContext.Connection.RemoteItemInBag(BagNames.TRANSACTION);

            ExecutorController.Instance.ReleaseTables(executor.ProcessContext.TablesToLock);

            executor.ExecuteNextCommand();
        }
    }
}