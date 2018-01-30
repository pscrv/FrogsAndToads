namespace GameCore
{
    public class EvaluationData
    {
        public readonly GamePosition Position;
        public readonly int Depth;
        public readonly int BestLeft;
        public readonly int BestRight;


        public EvaluationData(
            GamePosition position,
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
