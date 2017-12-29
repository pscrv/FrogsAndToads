namespace GameCore
{
    public abstract class GamePosition
    {
        public abstract GamePosition Reverse { get; }
        // move to Game<T>, to avoid casts?
    }
}