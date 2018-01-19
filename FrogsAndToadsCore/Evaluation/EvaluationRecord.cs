using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FrogsAndToadsCore
{
    internal class EvaluationRecord
    {
        internal readonly int Value;
        internal readonly bool IsComplete;
        internal readonly ReadOnlyCollection<FrogsAndToadsMove> EvaluatedMoves;


        internal EvaluationRecord(int value, IList<FrogsAndToadsMove> evaluatedMoves)
        {
            Value = value;
            IsComplete = false;
            EvaluatedMoves = new ReadOnlyCollection<FrogsAndToadsMove>(evaluatedMoves);
        }

        internal EvaluationRecord(int value)
        {
            Value = value;
            IsComplete = true;
            EvaluatedMoves = null;
        }
    }
}
