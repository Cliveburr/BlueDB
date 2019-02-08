using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Communication.Messages.Commands
{
    public enum CommandType : ushort
    {
        Invalid = 0,
        WithDatabase = 1,
        WithTable = 2
    }
}