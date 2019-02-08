using BlueDB.Host.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Host.Process
{
    public class PreProcess
    {
        private static PreProcess _instance;

        public static PreProcess Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PreProcess();
                }
                return _instance;
            }
        }

        public ProcessContext Compute(ConnectionContext context)
        {
            foreach (var command in context.Request.Commands)
            {

            }

            return new ProcessContext
            {
                ConnectionContext = context
            };
        }
    }
}