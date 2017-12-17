using System;
using System.Collections.Generic;
using GameCore;

namespace FrogsAndToadsCore
{
    public abstract class FrogsAndToadsPositionEvaluator : GamePositionEvaluator
    {
        public abstract int ToadEvaluation(FrogsAndToadsPosition position);
        //public abstract Dictionary<int, int> ToadMoveEvaluations(FrogsAndToadsPosition position);



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
     
        //public override Dictionary<int, int> ToadMoveEvaluations(FrogsAndToadsPosition position)
        //{
        //    Dictionary<int, int> values = new Dictionary<int, int>();

        //    foreach (int move in position.GetPossibleToadMoves())
        //    {
        //        values[move] = _evaluatePositionForFrog(
        //            position.MovePiece(move),
        //            0,
        //            int.MinValue,
        //            int.MaxValue);
        //    }

        //    return values;
        //}
        #endregion


        #region private
        private int _evaluatePositionForToad(
            FrogsAndToadsPosition position, 
            int depth, 
            int bestToad, 
            int bestFrog)
        {
            FrogsAndToadsPosition resultingPosition;
            //List<int> possibleMoves = position.GetPossibleToadMoves();
            List<FrogsAndToadsMove> possibleMoves = position.GetPossibleToadMoves();

            if (possibleMoves.Count == 0)
            {
                return _evaluateEndPositionForFrogs(position);
            }


            int bestvalue = int.MinValue;
            //foreach (int move in possibleMoves)
            foreach(FrogsAndToadsMove move in possibleMoves)
            {
                //resultingPosition = position.MovePiece(move);
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
            //List<int> possibleMoves = position.GetPossibleFrogMoves();
            List<FrogsAndToadsMove> possibleMoves = position.GetPossibleFrogMoves();

            if (possibleMoves.Count == 0)
            {
                return _evaluateEndPositionForToads(position);
            }

            int bestvalue = int.MaxValue;
            //foreach (int move in possibleMoves)
            foreach(FrogsAndToadsMove move in possibleMoves)
            {
                //resultingPosition = position.MovePiece(move);
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
            return position.GetPossibleToadMoves().Count;
        }

        private int _evaluateEndPositionForFrogs(FrogsAndToadsPosition position)
        {
            return -position.GetPossibleFrogMoves().Count;
        }
        #endregion

    }


}
