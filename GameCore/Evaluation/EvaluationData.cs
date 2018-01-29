namespace GameCore
{
    public class EvaluationData<GP> where GP : GamePosition
    {
        public readonly int Depth;
        public readonly GP Position;
        public readonly int BestLeft;
        public readonly int BestRight;


        public EvaluationData(
            GP position,
            int depth,
            int bestLeft,
            int bestRight)
        {
            Position = position;
            Depth = depth;
            BestLeft = bestLeft;
            BestRight = bestRight;
        }
    }
}
