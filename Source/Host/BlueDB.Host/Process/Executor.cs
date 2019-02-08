using BlueDB.Communication.Messages;
using BlueDB.Communication.Messages.Commands;
using BlueDB.Communication.Messages.Results;
using BlueDB.Host.Context;
using BlueDB.Host.Process.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlueDB.Host.Process
{
    public class Executor
    {
        public ProcessContext ProcessContext { get; private set; }
        public Dictionary<string, DatabaseExecutor> Databases { get; set; }
        public DatabaseExecutor DatabaseSelected { get; set; }
        public List<IResults> Results { get; set; }

        private int _commandIndex;
        private Exception _error;

        public Executor(ProcessContext processContext)
        {
            ProcessContext = processContext;
            Databases = new Dictionary<string, DatabaseExecutor>();
            Results = new List<IResults>();

            ProcessContext.Executor = this;

            Task.Run(new Action(Execute));
        }

        public void ExecuteNextCommand()
        {
            _commandIndex++;
            Execute();
        }

        private void Execute()
        {
            if (_commandIndex >= ProcessContext.ConnectionContext.Request.Commands.Length)
            {
                FinishExecute();
                return;
            }

            var command = ProcessContext.ConnectionContext.Request.Commands[_commandIndex];

            switch (command.Type)
            {
                case CommandType.WithDatabase:
                    {
                        WithDatabaseProcess.Execute(this, command as WithDatabaseCommand);
                        break;
                    }
                case CommandType.WithTable:
                    {
                        break;
                    }
            }
        }

        private void FinishExecute()
        {
            var response = new MessageReponse
            {
                Results = Results.ToArray(),
                Error =  _error.Message
            };

            ExecutorController.Instance.ReleaseExecutor(this);
        }
    }
}