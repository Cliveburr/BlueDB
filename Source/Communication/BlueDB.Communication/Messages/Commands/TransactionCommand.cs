using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Communication.Messages.Commands
{
    public class OpenTransactionCommand : ICommand
    {
        public CommandType Type => CommandType.OpenTransaction;

        public bool IsVirtual { get; set; }

        public OpenTransactionCommand()
        {
        }
    }

    public class CommitTransactionCommand : ICommand
    {
        public CommandType Type => CommandType.CommitTransaction;

        public bool IsVirtual { get; set; }

        public CommitTransactionCommand()
        {
        }
    }

    public class RollbackTransactionCommand : ICommand
    {
        public CommandType Type => CommandType.RollbackTransaction;

        public RollbackTransactionCommand()
        {
        }
    }
}