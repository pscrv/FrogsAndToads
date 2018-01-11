using System;
using System.Collections.Generic;
using System.Linq;
using GameCore;

namespace FrogsAndToadsCore
{
    public abstract class FrogsAndToadsPositionEvaluator : GamePositionEvaluator<FrogsAndToadsPosition>
    { }



    public class MiniMaxEvaluator : FrogsAndToadsPositionEvaluator
    {
        #region FrogAndToadsPositionEvaluator overrides
        public override int LeftEvaluation(FrogsAndToadsPosition position)
        {
            return _evaluatePositionForToad(
                position,
                0,
                int.MinValue,
                int.MaxValue);
        }


        public override int RightEvaluation(FrogsAndToadsPosition position)
        {
            return _evaluatePositionForFrog(
                position,
                0,
                int.MinValue,
                int.MaxValue);
        }
        #endregion


        #region internal
        internal int EvaluateEndPositionForToads(FrogsAndToadsPosition position)
        {
            return 
                position
                .GetSubPositions()
                .Sum(x => _evaluateEndPositionForToads(x));
        }

        internal int EvaluateEndPositionForFrogs(FrogsAndToadsPosition position)
        {
            return -EvaluateEndPositionForToads(position.Reverse());
        }

        internal List<(FrogsAndToadsMove move, int value)> EvaluateToadMoves(FrogsAndToadsPosition position)
        {
            return
                (from move in position.GetPossibleToadMoves()
                 select (move, RightEvaluation(position.PlayMove(move))))
                 .ToList();
        }

        #endregion


        #region private
        private int _evaluatePositionForToad(
            FrogsAndToadsPosition position, 
            int depth, 
            int bestToad, 
            int bestFrog)
        {
            FrogsAndToadsPosition resultingPosition;
            List<FrogsAndToadsMove> possibleMoves = position.GetPossibleToadMoves();

            if (possibleMoves.Count == 0)
            {
                return EvaluateEndPositionForFrogs(position);
            }


            int bestvalue = int.MinValue;
            foreach(FrogsAndToadsMove move in possibleMoves)
            {
                resultingPosition = position.PlayMove(move);
                bestvalue =
                    Math.Max(
                        bestvalue,
                        _evaluatePositionForFrog(resultingPosition, depth + 1, bestToad, bestFrog)
                        );
                bestToad = Math.Max(bestToad, bestvalue);
                if (bestToad > bestFrog)
                    break;
            }

            return bestvalue;
        }


        private int _evaluatePositionForFrog(
            FrogsAndToadsPosition position, 
            int depth, 
            int bestToad, 
            int bestFrog)
        {
            FrogsAndToadsPosition resultingPosition;
            List<FrogsAndToadsMove> possibleMoves = position.GetPossibleFrogMoves();

            if (possibleMoves.Count == 0)
            {
                return EvaluateEndPositionForToads(position);
            }

            int bestvalue = int.MaxValue;
            foreach(FrogsAndToadsMove move in possibleMoves)
            {
                resultingPosition = position.PlayMove(move);
                bestvalue =
                    Math.Min(
                        bestvalue,
                        _evaluatePositionForToad(resultingPosition, depth + 1, bestToad, bestFrog)
                        );
                bestFrog = Math.Min(bestFrog, bestvalue);
                if (bestToad > bestFrog)
                    break;
            }

            return bestvalue;
        }

        

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
