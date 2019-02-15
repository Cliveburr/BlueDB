using BlueDB.Communication.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Communication.Messages.Commands
{
    public class SetCommand : ICommand
    {
        public CommandType Type => CommandType.Set;

        public Property[] Properties { get; set; }
    }
}