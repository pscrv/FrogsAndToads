using System.Collections.Generic;
using System.Linq;

using GameCore;
using Monads;

namespace FrogsAndToadsCore
{
    public class MiniMaxEvaluator : MiniMaxEvaluator<FrogsAndToadsPosition>
    {
        #region MiniMaxEvaluator overrides
        public override int EvaluateEndPositionForLeft(FrogsAndToadsPosition position)
        {
            return 
                position
                .GetSubPositions()
                .Sum(x => _evaluateEndPositionForToads(x));
        }


        public override int EvaluateEndPositionForRight(FrogsAndToadsPosition position)
        {
            return -EvaluateEndPositionForLeft(position.Reverse());
        }
        #endregion


        #region internal methods
        internal List<(FrogsAndToadsMove move, int value)> EvaluateToadMoves(FrogsAndToadsPosition position)
        {
            return
                (from move in position.GetPossibleToadMoves()
                 select (move, RightEvaluation(position.PlayMove(move))))
                 .ToList();
        }

        #endregion


        #region private methods
        private int _evaluateEndPositionForToads(FrogsAndToadsPosition position)
        {            
            int sum = 0;
            int count = 0;
            for (int i = 0; i < position.Length; i++)
            {
                if (position[i] is Toad)
                {
                    sum += position.Length - i - 1;
                    count++;
                }
            }

            for (int i = 1; i < count; i++)
            {
                sum -= i;
            }

            return sum;
        }
        #endregion

    }
}
