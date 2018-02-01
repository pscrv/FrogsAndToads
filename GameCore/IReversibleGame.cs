namespace GameCore
{
    public interface IReversibleGame<GP> where GP : GamePosition
    {
        GP Reverse();
    }
}
