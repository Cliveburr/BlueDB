using System;
using System.Collections.Generic;
using System.Text;
using BlueDB.Communication.Messages.Commands;

namespace BlueDB.Host.Process.Commands
{
    public static class WithDatabaseProcess
    {
        public static void Execute(Executor executor, WithDatabaseCommand withDatabaseCommand)
        {
            // verifica se já abriu essa database nesse contexto

            // coloca essa database como selecionada no contexto

            if (!executor.Databases.ContainsKey(withDatabaseCommand.DatabaseName))
            {
                var newDatabase = new DatabaseExecutor(executor, withDatabaseCommand.DatabaseName);
                executor.Databases.Add(withDatabaseCommand.DatabaseName, newDatabase);
            }
            executor.DatabaseSelected = _databases[databaseName];

            // precisa ser async

            executor.ExecuteNextCommand();
        }
    }
}