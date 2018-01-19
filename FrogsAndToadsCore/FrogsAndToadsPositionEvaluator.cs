using System;
using System.Collections.Generic;
using System.Linq;
using GameCore;
using Monads;

namespace FrogsAndToadsCore
{
    public abstract class FrogsAndToadsPositionEvaluator : GamePositionEvaluator<FrogsAndToadsPosition>
    { }



    public class MiniMaxEvaluator : FrogsAndToadsPositionEvaluator
    {
        //private EvalutationCache _cache = new EvalutationCache();
        private PositionEvaluationCache _cache = new PositionEvaluationCache();



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
        private enum ToadFrog { Toad, Frog }

        private delegate int _evaluator(FrogsAndToadsPosition position, int depth, int bestToad, int bestFrog);
        private delegate int _bestValueUpdater(int oldBestValue, int newValue);
        private delegate (int bestToad, int bestFrog) _bestPairUpdater(int bestValue);



        private int _evaluatePositionForToad(
            FrogsAndToadsPosition position, 
            int depth, 
            int bestToad, 
            int bestFrog)
        {

            (Maybe<EvaluationRecord> toad, Maybe<EvaluationRecord> frog) cached = _cache.Lookup(position);
            if (cached.toad.HasValue && cached.toad.Value.IsComplete)
                    return (cached.toad.Value.Value);
            
            MoveRecord moveRecord = new MoveRecord(
                position.GetPossibleToadMoves(),
                int.MinValue,
                cached.toad);

            if (moveRecord.NoPossibleMoves)
                return EvaluateEndPositionForFrogs(position);

            EvaluationRecord record = _updateEvaluation(
                position,
                moveRecord,
                depth,
                bestToad,
                bestFrog,
                _evaluatePositionForFrog,
                (x, y) => Math.Max(x, y),
                x => (Math.Min(bestToad, x), bestFrog));


            _cache.Store(position, (record.ToMaybe(), cached.frog));
            return record.Value;
        }


        private int _evaluatePositionForFrog(
            FrogsAndToadsPosition position, 
            int depth, 
            int bestToad, 
            int bestFrog)
        {
            (Maybe<EvaluationRecord> toad, Maybe<EvaluationRecord> frog) cached = _cache.Lookup(position);
            if (cached.frog.HasValue && cached.frog.Value.IsComplete)
                    return (cached.frog.Value.Value);
            
            MoveRecord moveRecord = new MoveRecord(
                position.GetPossibleFrogMoves(), 
                int.MaxValue,
                cached.frog);

            if (moveRecord.NoPossibleMoves)
                return EvaluateEndPositionForToads(position);


            EvaluationRecord record = _updateEvaluation(
                position,
                moveRecord,
                depth,
                bestToad,
                bestFrog,
                _evaluatePositionForToad,
                (x, y) => Math.Min(x, y),
                x => (bestToad, Math.Min(x, bestFrog)));
            
            _cache.Store(position, (cached.toad, record.ToMaybe()));
            return record.Value;
        }

        private EvaluationRecord _updateEvaluation(
            FrogsAndToadsPosition position,
            MoveRecord moveRecord,
            int depth,
            int bestToad,
            int bestFrog,
            _evaluator nextEvaluator,
            _bestValueUpdater bestValueUpdater,
            _bestPairUpdater bestPairUpdater)
        {
            int moveEvaluationcount = 0;
            int bestValue = moveRecord.BestValueSoFar;
            FrogsAndToadsPosition resultingPosition;
            List<FrogsAndToadsMove> evaluatedMoves = 
                new List<FrogsAndToadsMove>(moveRecord.EvaluatedMoves);

            foreach (FrogsAndToadsMove move in moveRecord.MovesToEvaluate)
            {
                moveEvaluationcount++;
                evaluatedMoves.Add(move);
                resultingPosition = position.PlayMove(move);
                
                bestValue =
                    bestValueUpdater(
                        bestValue,
                        nextEvaluator(resultingPosition, depth + 1, bestToad, bestFrog)
                        );

                (bestToad, bestFrog) = bestPairUpdater(bestValue);                               

                if (bestToad > bestFrog)
                    break;
            }

            return
                (moveEvaluationcount == moveRecord.MovesToEvaluate.Count
                ? new EvaluationRecord(bestValue)
                : new EvaluationRecord(bestValue, evaluatedMoves));
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
