using System;
using System.Collections.Generic;
using Monads;

namespace FrogsAndToadsCore
{
    internal abstract class Cache<S, T>
    {
        protected abstract T _defaultReturnValue { get; }

        private Dictionary<S, T> _cache;
        

        internal Cache()
        {
            _cache = new Dictionary<S, T>();
        }


        internal T Lookup(S s)
        {
            return
                _cache.ContainsKey(s)
                ? _cache[s]
                : _defaultReturnValue;
        }

        internal void Store(S s, T t)
        {
            _cache[s] = t;
        }        
    }



    internal class PositionEvaluationCache
        : Cache<FrogsAndToadsPosition, (Maybe<EvaluationRecord> toad, Maybe<EvaluationRecord> frog)>
    {
        protected override (Maybe<EvaluationRecord> toad, Maybe<EvaluationRecord> frog) _defaultReturnValue 
            => (Maybe<EvaluationRecord>.Nothing(), Maybe<EvaluationRecord>.Nothing());
    }
}