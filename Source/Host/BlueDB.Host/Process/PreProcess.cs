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

            foreach (var command in context.Request.Commands)
            {

                if (command.Type == CommandType.OpenTransaction ||
                    command.Type == CommandType.CommitTransaction ||
                    command.Type == CommandType.RollbackTransaction)
                {
                    hasTransactionCommand = true;
                }
            }

            context.TablesToRead = new string[0];
            context.TablesToLock = new string[0];

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