using Utilities;

namespace GameCore
{
    public class AttemptPlay<T> : Try<T>
    {
        private static AttemptPlay<T> _failureInstance
            = new AttemptPlay<T>();

        public static new AttemptPlay<T> Failure
            => _failureInstance;

        public static new AttemptPlay<T> Success(T position)
            => new AttemptPlay<T>(position);


        private AttemptPlay(T position)
            : base(position)
        { }

        private AttemptPlay()
            : base ()
        { }
    }
}
