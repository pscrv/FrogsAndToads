using System.Collections.Generic;
using System.Linq;
using Monads;

namespace GameCore
{
    public class OptionRecord 
    {
        internal List<GamePosition> Options { get; private set; }
        public List<GamePosition> OptionsToEvaluate { get; private set; }
        public List<GamePosition> EvaluatedOptions { get; private set; }
        public int BestValueSoFar { get; private set; }

        public bool NoPossibleMoves
            => Options.Count == 0;


        public OptionRecord(
            IEnumerable<GamePosition> possibleMoves,
            int bestsofar,
            Maybe<EvaluationRecord> record
            )
        {
            Options = possibleMoves.ToList();
            OptionsToEvaluate = Options;
            EvaluatedOptions = new List<GamePosition>();
            BestValueSoFar = bestsofar;

            if (record.HasValue)
            {
                EvaluatedOptions = record.Value.EvaluatedPositions.ToList();
                BestValueSoFar = record.Value.Value;
            }
        }

    }
    
}
