namespace Utilities
{
    public class Try<T> where T: class
    {
        private static Try<T> _failureInstance 
            = new Try<T>();

        public static Try<T> Failure
            => _failureInstance;


        public static Try<T> Success(T value) 
            => new Try<T>(value);




        public T Value { get; private set; }
        public bool IsFailure { get; private set; }



        protected Try(T value)
        {
            Value = value;
            IsFailure = false;
        }

        protected Try()
        {
            Value = default(T);
            IsFailure = true;
        }
        
    }
}
