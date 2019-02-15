using BlueDB.Communication.Messages.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Host.Process.Commands
{
    public static class ReleaseConnectionProcess
    {
        public static void Execute(Executor executor, ReleaseConnectionCommand releaseConnectionCommand)
        {
            executor.ProcessContext.Connection.CleanBag();

            executor.ExecuteNextCommand();
        }
    }
}