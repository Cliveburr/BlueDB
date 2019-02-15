using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Communication.Messages.Commands
{
    public class ReleaseConnectionCommand : ICommand
    {
        public CommandType Type => CommandType.ReleaseConnection;

        public ReleaseConnectionCommand()
        {
        }
    }
}