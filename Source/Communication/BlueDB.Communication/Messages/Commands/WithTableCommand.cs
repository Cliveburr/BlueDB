using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Communication.Messages.Commands
{
    public class WithTableCommand : ICommand
    {
        public CommandType Type => CommandType.WithTable;

        public string TableName { get; set; }
        public bool ClearSelection { get; set; }
    }
}