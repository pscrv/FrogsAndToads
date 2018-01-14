using System;

namespace Monads
{
    public struct Maybe<T>
    {
        public static Maybe<T> Nothing() => 
            Nothing("Created from Nothing");

        public static Maybe<T> Nothing(string explanation) =>
            new Maybe<T>(default(T), false, explanation);

        public static Maybe<T> Some(T value) =>
            Some(value, "ok");

        public static Maybe<T> Some(T value, string explanation) =>
            value == null
            ? Nothing($"{nameof(value)} was null")
            : new Maybe<T>(value, true, explanation);



        public T Value { get; }
        public bool HasValue { get; }
        public string Explanation { get; }


        private Maybe(T value, bool hasValue, string explanation)
        {
            HasValue = hasValue;
            Value = value;
            Explanation = explanation;
        }
        

        public override string ToString()
        {
            return
                HasValue
                ? $"Some < { Value } >"
                : "Nothing";
        }
    }
    


    public static class MaybeExtensions
    {
        public static Maybe<T> ToMaybe<T>(this T value)
        {
            return Maybe<T>.Some(value);
        }


        public static Maybe<T> Bind<S, T>(this Maybe<S> source, Func<S, Maybe<T>> lift)
        {
            if (lift == null)
                return Maybe<T>.Nothing($"{nameof(lift)} was null");

            if (source.HasValue)
                return lift(source.Value);

            return Maybe<T>.Nothing();
        }


        public static Maybe<U> SelectMany<S, T, U>(
            this Maybe<S> source, 
            Func<S, Maybe<T>> lift, 
            Func<S, T, U> select)
        {
            if (select == null)
                return Maybe<U>.Nothing($"{nameof(select)} was null");

            if (lift == null)
                return Maybe<U>.Nothing($"{nameof(lift)} was null");
            
            return
                    source
                    .Bind(
                        sourceValue => lift(sourceValue)
                        .Bind(
                            liftValue => select(sourceValue, liftValue).ToMaybe()));
            
        }
   

        public static Maybe<T> Select<S, T>(
              this Maybe<S> source,
              Func<S, T> select)
        {
            if (select == null)
                return Maybe<T>.Nothing($"{nameof(select)} was null");

            if(source.HasValue)
                return
                    select(source.Value)
                    .ToMaybe();

            return Maybe<T>.Nothing();
        }
        
        
        //public static Maybe<(S, T)> Pair<S, T> (this Maybe<S> first, Maybe<T> second)
        //{
        //    if (first.HasValue && second.HasValue)
        //        return (first.Value, second.Value).ToMaybe();

        //    return Maybe<(S, T)>.Nothing();
        //}


    }
}
    