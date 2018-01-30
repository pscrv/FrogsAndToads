using System;
using System.Collections.Generic;
using System.Linq;
using Monads;

namespace GameCore
{
    public abstract class GamePositionEvaluator<GP> where GP : GamePosition
    {
        #region abstract
        public abstract int LeftEvaluation(GP position);
        public abstract int RightEvaluation(GP position);
        public abstract int EvaluateEndPositionForLeft(GP position);
        public abstract int EvaluateEndPositionForRight(GP position);
        #endregion
    }



    public abstract class MiniMaxEvaluator<GP> : GamePositionEvaluator<GP> where GP : GamePosition
    {
        protected PositionEvaluationCache _cache = new PositionEvaluationCache();




        public override int LeftEvaluation(GP position)
        {
            return _evaluateForLeft(
                new EvaluationData<GP>(
                    position,
                    0,
                    int.MinValue,
                    int.MaxValue));
        }
    

        public override int RightEvaluation(GP position)
        {
            return _evaluateForRight(
                new EvaluationData<GP>(
                    position,
                    0,
                    int.MinValue,
                    int.MaxValue));
        }






        private delegate int _evaluator(EvaluationData<GP> evaluationData);
        private delegate int _bestValueUpdater(int oldBestValue, int newValue);
        private delegate (int bestToad, int bestFrog) _bestPairUpdater(int bestValue);


        private int _evaluateForLeft(
            EvaluationData<GP> evaluationData)
        {
            (Maybe<EvaluationRecord> toad, Maybe<EvaluationRecord> frog) cached
                = _cache.Lookup(evaluationData.Position);
            if (cached.toad.HasValue && cached.toad.Value.IsComplete)
                return (cached.toad.Value.Value);

            OptionRecord optionRecord =
                new OptionRecord(
                    evaluationData.Position.GetLeftOptions().Select(x => x as GP),
                    int.MinValue,
                    cached.toad
                    );

            if (optionRecord.NoPossibleMoves)
                return EvaluateEndPositionForRight(evaluationData.Position);

            EvaluationRecord record = _updateEvaluation(
                evaluationData,
                optionRecord,
                _evaluateForRight,
                (x, y) => Math.Max(x, y),
                x => (Math.Min(evaluationData.BestLeft, x), evaluationData.BestRight));


            _cache.Store(evaluationData.Position, (record.ToMaybe(), cached.frog));
            return record.Value;
        }



        private int _evaluateForRight(EvaluationData<GP> evaluationData)
        {
            (Maybe<EvaluationRecord> toad, Maybe<EvaluationRecord> frog) cached =
                _cache.Lookup(evaluationData.Position);
            if (cached.frog.HasValue && cached.frog.Value.IsComplete)
                return (cached.frog.Value.Value);


            OptionRecord optionRecord =
                new OptionRecord(
                    evaluationData.Position.GetRightOptions().Select(x => x as GP),
                    int.MaxValue,
                    cached.frog
                    );

            if (optionRecord.NoPossibleMoves)
                return EvaluateEndPositionForLeft(evaluationData.Position);

            EvaluationRecord record = _updateEvaluation(
                evaluationData,
                optionRecord,
                _evaluateForLeft,
                (x, y) => Math.Min(x, y),
                x => (evaluationData.BestLeft, Math.Min(x, evaluationData.BestRight)));

            _cache.Store(evaluationData.Position, (cached.toad, record.ToMaybe()));
            return record.Value;
        }



        private EvaluationRecord _updateEvaluation(
            EvaluationData<GP> evaluationData,
            OptionRecord optionRecord,
            _evaluator nextEvaluator,
            _bestValueUpdater bestValueUpdater,
            _bestPairUpdater bestPairUpdater)
        {
            EvaluationData<GP> resultingEvaluationData;

            int bestToad = evaluationData.BestLeft;
            int bestFrog = evaluationData.BestRight;
            int optionEvaluationcount = 0;
            int bestValue = optionRecord.BestValueSoFar;
            List<GamePosition> evaluatedOptions =
                optionRecord.EvaluatedOptions;

            foreach (GP option in optionRecord.OptionsToEvaluate)
            {
                optionEvaluationcount++;
                evaluatedOptions.Add(option);
                resultingEvaluationData = new EvaluationData<GP>(
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
                ? new EvaluationRecord(bestValue)
                : new EvaluationRecord(bestValue, evaluatedOptions));
        }




    }
}
