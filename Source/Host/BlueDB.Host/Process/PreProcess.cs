using BlueDB.Communication.Messages.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueDB.Host.Process
{
    public class PreProcess
    {
        private static PreProcess _instance;

        public static PreProcess Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PreProcess();
                }
                return _instance;
            }
        }

        public void Compute(ProcessContext context)
        {
            var hasTransactionCommand = false;

            var tablesToRead = new List<string>();
            var tablesToLock = new List<string>();
            var withDatabase = string.Empty;
            var withTable = string.Empty;

            foreach (var command in context.Request.Commands)
            {
                switch (command.Type)
                {
                    case CommandType.WithDatabase:
                        withDatabase = ((WithDatabaseCommand)command).DatabaseName;
                        break;
                    case CommandType.WithTable:
                        withTable = ((WithTableCommand)command).TableName;
                        break;
                    case CommandType.Set:
                        tablesToLock.Add($"{withDatabase}.{withTable}");
                        break;
                    case CommandType.OpenTransaction:
                    case CommandType.CommitTransaction:
                    case CommandType.RollbackTransaction:
                        hasTransactionCommand = true;
                        break;
                }
            }

            context.TablesToRead = tablesToRead.ToArray();
            context.TablesToLock = tablesToLock.ToArray();

            var containsTransaction = context.Connection.ContainsItemInBag("Transaction");
            if (!hasTransactionCommand && !containsTransaction)
            {
                CreateVirtualTransaction(context);
            }
        }

        private void CreateVirtualTransaction(ProcessContext context)
        {
            var listCommands = context.Request.Commands
                .ToList();

            listCommands
                .Insert(0, new OpenTransactionCommand
                {
                    IsVirtual = true
                });

            listCommands
                .Add(new CommitTransactionCommand
                {
                    IsVirtual = true
                });

            context.Request.Commands = listCommands
                .ToArray();
        }
    }
}