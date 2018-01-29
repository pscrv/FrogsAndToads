using System.Collections.Generic;

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
        protected PositionEvaluationCache<GP> _cache = new PositionEvaluationCache<GP>();




        protected abstract int _evaluatePositionForLeft(EvaluationData<GP> ed);
        public override int LeftEvaluation(GP position)
        {
            return _evaluatePositionForLeft(
                new EvaluationData<GP>(
                    position,
                    0,
                    int.MinValue,
                    int.MaxValue));
        }
    }
}
