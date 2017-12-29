using System;
using System.Collections.Generic;
using System.Linq;
using GameCore;
using Utilities;

namespace FrogsAndToadsCore
{
    public abstract class FrogsAndToadsPositionEvaluator : GamePositionEvaluator
    {
        public abstract int ToadEvaluation(FrogsAndToadsPosition position);


        public override int ToadEvaluation(GamePosition position)
        {
            if (position is FrogsAndToadsPosition ftp)
                return ToadEvaluation(ftp);

            throw new ArgumentException("Can only evalute FrogsAndToadsPosition.");
        }
    }



    public class MiniMaxEvaluator : FrogsAndToadsPositionEvaluator
    {
        #region abstract overrides
        public override int ToadEvaluation(FrogsAndToadsPosition position)
        {
            return _evaluatePositionForToad(
                position,
                0,
                int.MinValue,
                int.MaxValue);
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
                return _evaluateEndPositionForFrogs(position);
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
                return _evaluateEndPositionForToads(position);
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
            return _toadValue(position);
        }

        private int _evaluateEndPositionForFrogs(FrogsAndToadsPosition position)
        {
            return _frogValue(position);
        }


        public int EvaluateEndPosition(FrogsAndToadsPosition position)
        {
            List<FrogsAndToadsPosition> subPositions = position.GetSubPositions();
            return subPositions.Sum(x => _toadValue(x) + _frogValue(x));
        }

        private int _toadValue(FrogsAndToadsPosition position)
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

        private int _frogValue(FrogsAndToadsPosition position)
        {
            return -_toadValue(position.Reverse as FrogsAndToadsPosition);
        }

        #endregion

    }


}
