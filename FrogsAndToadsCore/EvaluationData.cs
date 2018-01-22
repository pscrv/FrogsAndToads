namespace FrogsAndToadsCore
{
    internal class EvaluationData
    {
        internal readonly FrogsAndToadsPosition Position;
        internal readonly int Depth;
        internal readonly int BestToad;
        internal readonly int BestFrog;


        internal EvaluationData(
            FrogsAndToadsPosition position,
            int depth,
            int bestToad,
            int bestFrog)
        {
            Position = position;
            Depth = depth;
            BestToad = bestToad;
            BestFrog = bestFrog;
        }
    }
}
