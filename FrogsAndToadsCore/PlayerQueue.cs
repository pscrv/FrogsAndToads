using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FrogsAndToadsCore
{
    internal class PlayerQueue : IEnumerable
    {
        private List<Player> _players;

        internal PlayerQueue(IEnumerable<Player> players)
        {
            _players = players.ToList();
        }


        internal IEnumerable<Player> PlayerIterator
        {
            get
            {
                while (true)
                {
                    foreach (Player p in _players)
                    {
                        yield return p;
                    }
                }
            }
        }
        

        public IEnumerator<Player> GetEnumerator()
        {
            while (true)
            {
                foreach (Player p in _players)
                {
                    yield return p;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}