using Utilities;

namespace FrogsAndToadsCore
{
    public class TestMove : Try<FrogsAndToadsMove>
    {
        public static new TestMove Failure
            => Try<FrogsAndToadsMove>.Failure as TestMove;

        public static new TestMove Success(FrogsAndToadsMove move)
            => new TestMove(move);


        private TestMove(FrogsAndToadsMove move)
            : base(move)
        { }
    }
}
