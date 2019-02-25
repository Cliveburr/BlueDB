using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Communication.Messages.Commands
{
    public class SelectCommand : ICommand
    {
        public CommandType Type => CommandType.Select;

    }
}