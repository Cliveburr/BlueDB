using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Host.Helpers
{
    public class ConcurrentDoubleChecked<T>
    {
        private volatile Dictionary<string, T> _dic;
        private readonly object _dicLock = new object();

        public ConcurrentDoubleChecked()
        {
            _dic = new Dictionary<string, T>();
        }

        public T GetOrAdd(string key, Func<string, T> generate)
        {
            if (_dic.ContainsKey(key))
            {
                return _dic[key];
            }
            else
            {
                lock (_dicLock)
                {
                    if (_dic.ContainsKey(key))
                    {
                        return _dic[key];
                    }
                    else
                    {
                        var newItem = generate(key);
                        _dic[key] = newItem;
                        return _dic[key];
                    }
                }
            }
        }
    }
}