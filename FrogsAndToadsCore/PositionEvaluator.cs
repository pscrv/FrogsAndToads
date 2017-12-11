using System;
using System.Collections.Generic;

namespace FrogsAndToadsCore
{
    public abstract class PositionEvaluator
    {
        #region abstract
        internal abstract int ToadEvaluation(GamePosition position);
        internal abstract Dictionary<int, int> ToadMoveEvaluations(GamePosition position);
        #endregion
    }


    public class MiniMaxEvaluator : PositionEvaluator
    {



        #region abstract overrides
        internal override int ToadEvaluation(GamePosition position)
        {
            return _evaluatePositionForToad(
                position, 
                0, 
                int.MinValue, 
                int.MaxValue);
        }

     
        internal override Dictionary<int, int> ToadMoveEvaluations(GamePosition position)
        {
            Dictionary<int, int> values = new Dictionary<int, int>();

            foreach (int move in position.GetPossibleToadMoves())
            {
                values[move] = _evaluatePositionForFrog(
                    position.MovePiece(move),
                    0,
                    int.MinValue,
                    int.MaxValue);
            }

            return values;
        }
        #endregion


        #region private
        private int _evaluatePositionForToad(
            GamePosition position, 
            int depth, 
            int bestToad, 
            int bestFrog)
        {
            GamePosition resultingPosition;
            List<int> possibleMoves = position.GetPossibleToadMoves();

            if (possibleMoves.Count == 0)
            {
                return _evaluateEndPositionForFrogs(position);
            }


            int bestvalue = int.MinValue;
            foreach (int move in possibleMoves)
            {
                resultingPosition = position.MovePiece(move);
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
            GamePosition position, 
            int depth, 
            int bestToad, 
            int bestFrog)
        {
            GamePosition resultingPosition;
            List<int> possibleMoves = position.GetPossibleFrogMoves();

            if (possibleMoves.Count == 0)
            {
                return _evaluateEndPositionForToads(position);
            }

            int bestvalue = int.MaxValue;
            foreach (int move in possibleMoves)
            {
                resultingPosition = position.MovePiece(move);
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



        private int _evaluateEndPositionForToads(GamePosition position)
        {
            return position.GetPossibleToadMoves().Count;
        }

        private int _evaluateEndPositionForFrogs(GamePosition position)
        {
            return -position.GetPossibleFrogMoves().Count;
        }
        #endregion

    }


}
