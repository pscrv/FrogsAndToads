using System;
using System.Collections.Generic;
using System.Linq;
using GameCore;

namespace FrogsAndToadsCore
{
    public abstract class FrogsAndToadsPositionEvaluator : PartisanGamePositionEvaluator<FrogsAndToadsPosition>
    {
        internal abstract int ToadEvaluation(FrogsAndToadsPosition position);
        internal abstract int FrogEvaluation(FrogsAndToadsPosition position);


        public override int LeftEvaluation(FrogsAndToadsPosition position)
        {
            return ToadEvaluation(position);
        }

        public override int RightEvaluation(FrogsAndToadsPosition position)
        {
            return FrogEvaluation(position);
        }
    }



    public class MiniMaxEvaluator : FrogsAndToadsPositionEvaluator
    {
        #region abstract overrides
        internal override int ToadEvaluation(FrogsAndToadsPosition position)
        {
            return _evaluatePositionForToad(
                position,
                0,
                int.MinValue,
                int.MaxValue);
        }


        internal override int FrogEvaluation(FrogsAndToadsPosition position)
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
            List<FrogsAndToadsPosition> subPositions = position.GetSubPositions();
            return subPositions.Sum(
                x => _evaluateEndPositionForToads(x));
        }

        internal int EvaluateEndPositionForFrogs(FrogsAndToadsPosition position)
        {
            return -EvaluateEndPositionForToads(position.Reverse());
        }

        internal List<(FrogsAndToadsMove move, int value)> EvaluateToadMoves(FrogsAndToadsPosition position)
        {
            List<(FrogsAndToadsMove, int)> result = new List<(FrogsAndToadsMove, int)>();

            foreach (FrogsAndToadsMove move in position.GetPossibleToadMoves())
            {
                FrogsAndToadsPosition resultingPosition = position.PlayMove(move);
                result.Add((move, FrogEvaluation(resultingPosition)));
            }

            return result;
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

        

        private int _evaluateSimpleEndPosition(FrogsAndToadsPosition position)
        {
            return
                EvaluateEndPositionForToads(position)
                + EvaluateEndPositionForFrogs(position);
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
