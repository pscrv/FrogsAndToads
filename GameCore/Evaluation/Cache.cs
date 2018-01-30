using System;
using System.Collections.Generic;
using Monads;

namespace GameCore
{
    public abstract class Cache<S, T>
    {
        protected abstract T _defaultReturnValue { get; }

        private Dictionary<S, T> _cache;
        

        internal Cache()
        {
            _cache = new Dictionary<S, T>();
        }


        public T Lookup(S s)
        {
            return
                _cache.ContainsKey(s)
                ? _cache[s]
                : _defaultReturnValue;
        }

        public void Store(S s, T t)
        {
            _cache[s] = t;
        }        
    }



    public class PositionEvaluationCache
        : Cache<GamePosition, (Maybe<EvaluationRecord> left, Maybe<EvaluationRecord> right)>
    {
        protected override (Maybe<EvaluationRecord> left, Maybe<EvaluationRecord> right) _defaultReturnValue
            => (Maybe<EvaluationRecord>.Nothing(), Maybe<EvaluationRecord>.Nothing());
    }
}