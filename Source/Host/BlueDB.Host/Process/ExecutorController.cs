using BlueDB.Host.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueDB.Host.Process
{
    public class ExecutorController
    {
        private static ExecutorController _instance;

        public static ExecutorController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ExecutorController();
                }
                return _instance;
            }
        }

        private List<ProcessContext> _processQueue;
        private object _processLock;
        private int _maxExecutor;
        private List<Executor> _executors;
        private List<string> _tablesLocked;

        public ExecutorController()
        {
            _processQueue = new List<ProcessContext>();
            _processLock = new object();
            _maxExecutor = 5;
            _executors = new List<Executor>();
        }

        public void ScheduleToProcess(ProcessContext request)
        {
            lock (_processLock)
            {
                _processQueue.Add(request);
                RunNextRequest();
            }
        }

        private void RunNextRequest()
        {
            if (!_processQueue.Any())
            {
                return;
            }

            if (_executors.Count >= _maxExecutor)
            {
                return;
            }

            var request = GetNextPossibleRequest();
            if (request != null)
            {
                _tablesLocked.AddRange(request.TablesToLock);
                var executor = new Executor(request);
                _executors.Add(executor);
            }

            RunNextRequest();
        }

        private ProcessContext GetNextPossibleRequest()
        {
            for (var i = 0; i < _processQueue.Count; i++)
            {
                var request = _processQueue[i];

                var useTableLocked = _tablesLocked
                    .Intersect(request.TablesToRead);

                if (!useTableLocked.Any())
                {
                    _processQueue.RemoveAt(i);
                    return request;
                }
            }
            return null;
        }

        public void ReleaseExecutor(Executor executor)
        {
            lock (_processLock)
            {
                _executors.Remove(executor);

                foreach (var table in executor.ProcessContext.TablesToLock)
                {
                    _tablesLocked.Remove(table);
                }

                RunNextRequest();
            }
        }
    }
}