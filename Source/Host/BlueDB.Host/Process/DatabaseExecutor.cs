using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Host.Process
{
    public class DatabaseExecutor
    {
        public Executor Executor { get; set; }
        public string Name { get; set; }

        public DatabaseExecutor(Executor executor, string name)
        {
            Executor = executor;
            Name = name;
        }
    }
}