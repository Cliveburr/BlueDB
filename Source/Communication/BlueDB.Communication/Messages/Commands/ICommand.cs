using BlueDB.Serialize.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Communication.Messages.Commands
{
    [KnowType(typeof(OpenTransactionCommand), typeof(CommitTransactionCommand), typeof(RollbackTransactionCommand), typeof(WithDatabaseCommand), typeof(WithTableCommand), typeof(SetCommand),
        typeof(ReleaseConnectionCommand), typeof(SelectCommand))]
    public interface ICommand
    {
        CommandType Type { get; }
    }
}