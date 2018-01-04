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

            if (source.HasValue)
                return
                    select(
                        source.Value,
                        source.Bind(lift).Value)
                        .ToMaybe();

            return Maybe<U>.Nothing();
        }
    }

}
    