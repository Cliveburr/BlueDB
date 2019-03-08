using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Data
{
    public class FixedDataFileEnumerator<T> : IEnumerator, IEnumerator<T> where T : class
    {
        private FixedDataFile<T> _file;
        private int _position;
        private T _current;

        public object Current => GetCurrent();

        T IEnumerator<T>.Current => GetCurrent();

        public FixedDataFileEnumerator(FixedDataFile<T> file)
        {
            _position = -1;
            _file = file;
        }

        public bool MoveNext()
        {
            if (_position < (_file.Count - 1))
            {
                _position += 1;
                _current = null;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Reset()
        {
            _position = -1;
            _current = null;
        }

        private T GetCurrent()
        {
            if (_current == null)
            {
                _current = _file.Read(_position);
            }
            return _current;
        }

        public void Dispose()
        {
            _file = null;
            _position = -1;
            _current = null;
        }
    }
}