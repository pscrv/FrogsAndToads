using System.Collections;
using System.Collections.Generic;

namespace Utilities
{

    public class Cycle<T> : IEnumerable<T>
    {
        #region private
        private List<T> _elements;
        #endregion


        public Cycle()
        {
            _elements = new List<T>();
        }

        public Cycle(IEnumerable<T> elements)
        {
            _elements = new List<T>(elements);
        }

        public void Add(T element)
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
