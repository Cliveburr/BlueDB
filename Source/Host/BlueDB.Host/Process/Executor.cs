using BlueDB.Communication.Messages;
using BlueDB.Communication.Messages.Commands;
using BlueDB.Communication.Messages.Results;
using BlueDB.Host.Process.Commands;
using BlueDB.Host.Transaction;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlueDB.Host.Process
{
    public class Executor
    {
        public ProcessContext ProcessContext { get; private set; }
        public List<IResults> Results { get; set; }
        public DatabaseInter DatabaseSelected { get; set; }


        private int _commandIndex;
        private Exception _error;

        public Executor(ProcessContext processContext)
        {
            ProcessContext = processContext;
            Results = new List<IResults>();

            Task.Run(new Action(Execute));
        }

        public void ExecuteNextCommand()
        {
            _commandIndex++;
            Task.Run(new Action(Execute));
        }

        private void Execute()
        {
            if (_commandIndex >= ProcessContext.Request.Commands.Length)
            {
                FinishExecute();
                return;
            }

            var command = ProcessContext.Request.Commands[_commandIndex];

            try
            {
                switch (command.Type)
                {
                    case CommandType.ReleaseConnection: ReleaseConnectionProcess.Execute(this, command as ReleaseConnectionCommand); break;
                    case CommandType.OpenTransaction: OpenTransactionProcess.Execute(this, command as OpenTransactionCommand); break;
                    case CommandType.CommitTransaction: CommitTransactionProcess.Execute(this, command as CommitTransactionCommand); break;
                    case CommandType.RollbackTransaction: RollbackTransactionProcess.Execute(this, command as RollbackTransactionCommand); break;
                    case CommandType.WithDatabase: WithDatabaseProcess.Execute(this, command as WithDatabaseCommand); break;
                    case CommandType.WithTable: WithTableProcess.Execute(this, command as WithTableCommand); break;
                    case CommandType.Set: SetProcess.Execute(this, command as SetCommand); break;
                    case CommandType.Select: SelectProcess.Execute(this, command as SelectCommand); break;
                }
            }
            catch (Exception err)
            {
                HandleExecuteError(err);
            }
        }

        private void FinishExecute()
        {
            var response = new MessageReponse
            {
                Id = ProcessContext.Request.Id,
                Results = Results.ToArray(),
                Error =  _error?.Message
            };

            ProcessContext.Callback(response);

            ExecutorController.Instance.ReleaseExecutor(this);
        }

        public void HandleExecuteError(Exception err)
        {
            _error = err;
            FinishExecute();
        }
    }
}