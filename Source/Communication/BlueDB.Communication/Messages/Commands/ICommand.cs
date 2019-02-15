using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Communication.Messages.Commands
{
    //TODO: criar um mecanismo para se enumerar as possiveis implementações para melhorar a serialização
    public interface ICommand
    {
        CommandType Type { get; }
    }
}