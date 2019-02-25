using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Communication.Messages.Commands
{
    public enum CommandType : ushort
    {
        Invalid = 0,
        ReleaseConnection = 1,
        OpenTransaction = 2,
        CommitTransaction = 3,
        RollbackTransaction = 4,
        WithDatabase = 5,
        WithTable = 6,
        Set = 7,
        Select = 8
    }
}