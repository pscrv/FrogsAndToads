using System;
using System.Collections.Generic;
using Monads;

namespace FrogsAndToadsCore
{
    internal class EvalutationCache
    {
        private Dictionary<string, (Maybe<int> toad, Maybe<int> frog)> _cache;



        internal EvalutationCache()
        {
            _cache = new Dictionary<string, (Maybe<int>, Maybe<int>)>();
        }


        internal (Maybe<int>, Maybe<int>) Lookup(FrogsAndToadsPosition position)
        {
            string key = position.ToString();
            if (_cache.ContainsKey(key))
            {
                return _cache[key];
            }

            return (Maybe<int>.Nothing(), Maybe<int>.Nothing());
        }

        internal void StoreToad(FrogsAndToadsPosition position, int value)
        {
            string key = position.ToString();
            if (_cache.ContainsKey(key) && _cache[key].toad.HasValue)
                throw new InvalidOperationException($"Key {key} is alreay present.");

            Maybe<int> frog =
                _cache.ContainsKey(key) && _cache[key].frog.HasValue
                ? _cache[key].frog
                : Maybe<int>.Nothing();

            _cache[key] = (value.ToMaybe(), frog);
        }

        internal void StoreFrog(FrogsAndToadsPosition position, int value)
        {
            string key = position.ToString();
            if (_cache.ContainsKey(key) && _cache[key].frog.HasValue)
                throw new InvalidOperationException($"Key {key} is alreay present.");

            Maybe<int> toad =
                _cache.ContainsKey(key) && _cache[key].toad.HasValue
                ? _cache[key].toad
                : Maybe<int>.Nothing();

            _cache[key] = (toad, value.ToMaybe());
        }
    }
}