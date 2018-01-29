using System;
using System.Collections.Generic;
using System.Linq;
using GameCore;
using Monads;

namespace FrogsAndToadsCore
{
    public class MiniMaxEvaluator : MiniMaxEvaluator<FrogsAndToadsPosition>
    {
        #region MiniMaxEvaluator overrides
        public override int LeftEvaluation(FrogsAndToadsPosition position)
        {
            return _evaluatePositionForToads(
                new GameCore.EvaluationData<FrogsAndToadsPosition>(
                    position,
                    0,
                    int.MinValue,
                    int.MaxValue));
        }


        public override int RightEvaluation(FrogsAndToadsPosition position)
        {
            return _evaluatePositionForFrogs(
                new EvaluationData<FrogsAndToadsPosition>(
                    position,
                    0,
                    int.MinValue,
                    int.MaxValue));
        }


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


        #region private
        private delegate int _evaluator(EvaluationData<FrogsAndToadsPosition> evaluationData);
        private delegate int _bestValueUpdater(int oldBestValue, int newValue);
        private delegate (int bestToad, int bestFrog) _bestPairUpdater(int bestValue);



        protected override int _evaluatePositionForLeft(
            EvaluationData<FrogsAndToadsPosition> ed)
            => _evaluatePositionForToads(ed);

        private int _evaluatePositionForToads(
            EvaluationData<FrogsAndToadsPosition> evaluationData)
        {
            (Maybe<EvaluationRecord<FrogsAndToadsPosition>> toad, Maybe<EvaluationRecord<FrogsAndToadsPosition>> frog) cached 
                = _cache.Lookup(evaluationData.Position);
            if (cached.toad.HasValue && cached.toad.Value.IsComplete)
                    return (cached.toad.Value.Value);

            OptionRecord<FrogsAndToadsPosition> optionRecord = 
                new OptionRecord<FrogsAndToadsPosition>(
                    evaluationData.Position.GetLeftOptions(),
                    int.MinValue,
                    cached.toad
                    );

            if (optionRecord.NoPossibleMoves)
                return EvaluateEndPositionForRight(evaluationData.Position);

            EvaluationRecord<FrogsAndToadsPosition> record = _updateEvaluation(
                evaluationData,
                optionRecord,
                _evaluatePositionForFrogs,
                (x, y) => Math.Max(x, y),
                x => (Math.Min(evaluationData.BestLeft, x), evaluationData.BestRight));


            _cache.Store(evaluationData.Position, (record.ToMaybe(), cached.frog));
            return record.Value;
        }


        private int _evaluatePositionForFrogs(EvaluationData<FrogsAndToadsPosition> evaluationData)
        {
            (Maybe<EvaluationRecord<FrogsAndToadsPosition>> toad, Maybe<EvaluationRecord<FrogsAndToadsPosition>> frog) cached = 
                _cache.Lookup(evaluationData.Position);
            if (cached.frog.HasValue && cached.frog.Value.IsComplete)
                    return (cached.frog.Value.Value);


            OptionRecord<FrogsAndToadsPosition> optionRecord =
                new OptionRecord<FrogsAndToadsPosition>(
                    evaluationData.Position.GetRightOptions(),
                    int.MaxValue,
                    cached.frog
                    );

            if (optionRecord.NoPossibleMoves)
                return EvaluateEndPositionForLeft(evaluationData.Position);
            
            EvaluationRecord<FrogsAndToadsPosition> record = _updateEvaluation(
                evaluationData,
                optionRecord,
                _evaluatePositionForToads,
                (x, y) => Math.Min(x, y),
                x => (evaluationData.BestLeft, Math.Min(x, evaluationData.BestRight)));
            
            _cache.Store(evaluationData.Position, (cached.toad, record.ToMaybe()));
            return record.Value;
        }



        private EvaluationRecord<FrogsAndToadsPosition> _updateEvaluation(
            EvaluationData<FrogsAndToadsPosition> evaluationData,
            OptionRecord<FrogsAndToadsPosition> optionRecord,
            _evaluator nextEvaluator,
            _bestValueUpdater bestValueUpdater,
            _bestPairUpdater bestPairUpdater)
        {
            EvaluationData<FrogsAndToadsPosition> resultingEvaluationData;

            int bestToad = evaluationData.BestLeft;
            int bestFrog = evaluationData.BestRight;
            int optionEvaluationcount = 0;
            int bestValue = optionRecord.BestValueSoFar;
            List<FrogsAndToadsPosition> evaluatedOptions =
                optionRecord.EvaluatedOptions;

            foreach (FrogsAndToadsPosition option in optionRecord.OptionsToEvaluate)
            {
                optionEvaluationcount++;
                evaluatedOptions.Add(option);
                resultingEvaluationData = new EvaluationData<FrogsAndToadsPosition>(
                    option,
                    evaluationData.Depth + 1,
                    evaluationData.BestLeft,
                    evaluationData.BestRight);
                
                bestValue =
                    bestValueUpdater(
                        bestValue,
                        nextEvaluator(resultingEvaluationData));

                (bestToad, bestFrog) = bestPairUpdater(bestValue);
                if (bestToad > bestFrog)
                    break;
            }

            return
                (optionEvaluationcount == optionRecord.OptionsToEvaluate.Count
                ? new EvaluationRecord<FrogsAndToadsPosition>(bestValue)
                : new EvaluationRecord<FrogsAndToadsPosition>(bestValue, evaluatedOptions));
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
