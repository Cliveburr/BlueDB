using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Communication.Messages.Commands
{
    public class WithDatabaseCommand : ICommand
    {
        public CommandType Type => CommandType.WithDatabase;

        public string DatabaseName { get; set; }
    }
}