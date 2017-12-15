using Utilities;

namespace GameCore
{
    public class AttemptPlay : Try<GamePosition>
    {
        public static new AttemptPlay Failure 
            => Try<GamePosition>.Failure as AttemptPlay;

        public static new AttemptPlay Success(GamePosition position)
            => new AttemptPlay(position);


        private AttemptPlay(GamePosition position)
            : base(position)
        { }
    }
}
