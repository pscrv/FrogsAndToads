using Utilities;

namespace GameCore
{
    public class AttemptPlay : Try<GamePosition>
    {
        private static AttemptPlay _failureInstance
            = new AttemptPlay();

        public static new AttemptPlay Failure
            => _failureInstance;

        public static new AttemptPlay Success(GamePosition position)
            => new AttemptPlay(position);


        private AttemptPlay(GamePosition position)
            : base(position)
        { }

        private AttemptPlay()
            : base ()
        { }
    }
}
