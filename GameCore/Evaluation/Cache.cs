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



    public class PositionEvaluationCache<GP> 
        : Cache<GamePosition, (Maybe<EvaluationRecord<GP>> left, Maybe<EvaluationRecord<GP>> right)>
    where GP : GamePosition
    {
        protected override (Maybe<EvaluationRecord<GP>> left, Maybe<EvaluationRecord<GP>> right) _defaultReturnValue 
            => (Maybe<EvaluationRecord<GP>>.Nothing(), Maybe<EvaluationRecord<GP>>.Nothing());
    }
}