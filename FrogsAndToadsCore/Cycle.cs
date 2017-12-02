using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrogsAndToadsCore
{
    internal class Cycle<T> : IEnumerable<T>
    {
        #region private
        private List<T> _elements;
        #endregion


        internal Cycle()
        {
            _elements = new List<T>();
        }

        internal  Cycle(IEnumerable<T> elements)
        {
            _elements = new List<T>(elements);
        }

        internal void Add(T element)
        {
            _elements.Add(element);
        }


        public IEnumerator<T> GetEnumerator()
        {
            while (true)
            {
                foreach (T element in _elements)
                {
                    yield return element;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
