using System.Collections.Generic;
using System.Linq;
using Monads;

namespace FrogsAndToadsCore
{
    internal class MoveRecord
    {
        internal List<FrogsAndToadsMove> PossibleMoves { get; private set; }
        internal List<FrogsAndToadsMove> MovesToEvaluate { get; private set; }
        internal List<FrogsAndToadsMove> EvaluatedMoves { get; private set; }
        internal int BestValueSoFar { get; private set; }

        internal bool NoPossibleMoves
            => PossibleMoves.Count == 0;
        

        internal MoveRecord(
            IEnumerable<FrogsAndToadsMove> possibleMoves, 
            int bestsofar,
            Maybe<EvaluationRecord> record
            )
        {
            PossibleMoves = possibleMoves.ToList();
            MovesToEvaluate = PossibleMoves;
            EvaluatedMoves = new List<FrogsAndToadsMove>();
            BestValueSoFar = bestsofar;

            if (record.HasValue)
            {
                EvaluatedMoves = record.Value.EvaluatedMoves.ToList();
                BestValueSoFar = record.Value.Value;
            }
        }        

    }
}
