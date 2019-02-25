using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlueDB.Host.Helpers
{
    public class ConcurrentOpen
    {
        private volatile ConcurrentOpenState _state;
        private readonly object _lock = new object();
        private List<Action> _callBacks;

        public ConcurrentOpenState State
        {
            get
            {
                return _state;
            }
        }

        public void Open(Action callBack, Action executeOpen)
        {
            if (_state == ConcurrentOpenState.Opened)
            {
                callBack();
            }
            else
            {
                lock (_lock)
                {
                    if (_state == ConcurrentOpenState.Opened)
                    {
                        callBack();
                    }
                    else
                    {
                        if (_state == ConcurrentOpenState.Closed)
                        {
                            _callBacks = new List<Action>
                            {
                                callBack
                            };

                            _state = ConcurrentOpenState.Openning;
                            executeOpen();
                        }
                        else
                        {
                            _callBacks.Add(callBack);
                        }
                    }
                }
            }
        }

        public void SetOpened()
        {
            _state = ConcurrentOpenState.Opened;
            Parallel.ForEach(_callBacks, (callback) => callback());
            _callBacks.Clear();
            _callBacks = null;
        }
    }

    public enum ConcurrentOpenState : byte
    {
        Closed = 0,
        Openning = 1,
        Opened = 2
    }
}