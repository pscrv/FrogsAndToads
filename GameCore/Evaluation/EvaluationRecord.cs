using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GameCore
{
    public class EvaluationRecord<GP> where GP : GamePosition
    {
        public readonly int Value;
        public readonly bool IsComplete;
        public readonly ReadOnlyCollection<GP> EvaluatedPositions;


        public EvaluationRecord(int value, IList<GP> evaluatedPositions)
        {
            Value = value;
            IsComplete = false;
            EvaluatedPositions = new ReadOnlyCollection<GP>(evaluatedPositions);
        }

        public EvaluationRecord(int value)
        {
            Value = value;
            IsComplete = true;
            EvaluatedPositions = null;
        }
    }
}
