namespace GameCore
{
    public abstract class PartisanGamePositionEvaluator<T> where T : GamePosition
    {
        #region abstract
        public abstract int LeftEvaluation(T position);
        public abstract int RightEvaluation(T position);
        #endregion
    }
}
