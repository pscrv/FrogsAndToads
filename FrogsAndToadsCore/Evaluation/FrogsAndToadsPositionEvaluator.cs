using System;
using System.Collections.Generic;
using System.Linq;
using GameCore;
using Monads;

namespace FrogsAndToadsCore
{    
    public class MiniMaxEvaluator : MinimaxPositionEvaluator<FrogsAndToadsPosition>
    {
        private PositionEvaluationCache _cache = new PositionEvaluationCache();


        #region FrogAndToadsPositionEvaluator overrides
        //public override int LeftEvaluation(FrogsAndToadsPosition position)
        //{
        //    return _evaluatePositionForToad(
        //        new EvaluationData<FrogsAndToadsPosition>(
        //            position,
        //            0,
        //            int.MinValue,
        //            int.MaxValue));
        //}


        //public override int RightEvaluation(FrogsAndToadsPosition position)
        //{
        //    return _evaluatePositionForFrog(
        //        new EvaluationData<FrogsAndToadsPosition>(
        //            position,
        //            0,
        //            int.MinValue,
        //            int.MaxValue));
        //}
        #endregion


        #region internal
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

        internal List<(FrogsAndToadsMove move, int value)> EvaluateToadMoves(FrogsAndToadsPosition position)
        {
            return
                (from move in position.GetLeftMoves()
                 select ((move as FrogsAndToadsMove), RightEvaluation(position.PlayMove(move as FrogsAndToadsMove))))
                 .ToList();
        }

        #endregion


        #region private
        private delegate int _evaluator(EvaluationData<FrogsAndToadsPosition> evaluationData);
        private delegate int _bestValueUpdater(int oldBestValue, int newValue);
        private delegate (int bestToad, int bestFrog) _bestPairUpdater(int bestValue);


        private int _evaluatePositionForToad(EvaluationData<FrogsAndToadsPosition> evaluationData)
        {
            (Maybe<EvaluationRecord> toad, Maybe<EvaluationRecord> frog) cached 
                = _cache.Lookup(evaluationData.Position);
            if (cached.toad.HasValue && cached.toad.Value.IsComplete)
                    return (cached.toad.Value.Value);
            
            MoveRecord moveRecord = new MoveRecord(
                evaluationData.Position.GetLeftMoves().Select(x => x as FrogsAndToadsMove),
                int.MinValue,
                cached.toad);

            if (moveRecord.NoPossibleMoves)
                return EvaluateEndPositionForRight(evaluationData.Position);

            EvaluationRecord record = _updateEvaluation(
                evaluationData,
                moveRecord,
                _evaluatePositionForFrog,
                (x, y) => Math.Max(x, y),
                x => (Math.Min(evaluationData.BestToad, x), evaluationData.BestFrog));


            _cache.Store(evaluationData.Position, (record.ToMaybe(), cached.frog));
            return record.Value;
        }


        private int _evaluatePositionForFrog(EvaluationData<FrogsAndToadsPosition> evaluationData)
        {
            (Maybe<EvaluationRecord> toad, Maybe<EvaluationRecord> frog) cached = 
                _cache.Lookup(evaluationData.Position);
            if (cached.frog.HasValue && cached.frog.Value.IsComplete)
                    return (cached.frog.Value.Value);
            
            MoveRecord moveRecord = new MoveRecord(
                evaluationData.Position.GetRightMoves().Select(x => x as FrogsAndToadsMove), 
                int.MaxValue,
                cached.frog);

            if (moveRecord.NoPossibleMoves)
                return EvaluateEndPositionForLeft(evaluationData.Position);
            
            EvaluationRecord record = _updateEvaluation(
                evaluationData,
                moveRecord,
                _evaluatePositionForToad,
                (x, y) => Math.Min(x, y),
                x => (evaluationData.BestToad, Math.Min(x, evaluationData.BestFrog)));
            
            _cache.Store(evaluationData.Position, (cached.toad, record.ToMaybe()));
            return record.Value;
        }



        private EvaluationRecord _updateEvaluation(
            EvaluationData<FrogsAndToadsPosition> evaluationData,
            MoveRecord moveRecord,
            _evaluator nextEvaluator,
            _bestValueUpdater bestValueUpdater,
            _bestPairUpdater bestPairUpdater)
        {
            FrogsAndToadsPosition resultingPosition;
            EvaluationData<FrogsAndToadsPosition> resultingEvaluationData;

            int bestToad = evaluationData.BestToad;
            int bestFrog = evaluationData.BestFrog;
            int moveEvaluationcount = 0;
            int bestValue = moveRecord.BestValueSoFar;
            List<FrogsAndToadsMove> evaluatedMoves =
                moveRecord.EvaluatedMoves;

            foreach (FrogsAndToadsMove move in moveRecord.MovesToEvaluate)
            {
                moveEvaluationcount++;
                evaluatedMoves.Add(move);
                resultingPosition = evaluationData.Position.PlayMove(move);
                resultingEvaluationData = new EvaluationData<FrogsAndToadsPosition>(
                    resultingPosition,
                    evaluationData.Depth + 1,
                    evaluationData.BestToad,
                    evaluationData.BestFrog);
                
                bestValue =
                    bestValueUpdater(
                        bestValue,
                        nextEvaluator(resultingEvaluationData));

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
