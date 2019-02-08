using BlueDB.Host.Context;
using BlueDB.Host.Process;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Host.Context
{
    public class ProcessContext
    {
        public ConnectionContext ConnectionContext { get; set; }
        public string[] TablesToLock { get; set; }
        public string[] TablesToRead { get; set; }
        public Executor Executor { get; set; }
    }
}