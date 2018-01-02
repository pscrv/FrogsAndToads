using System.Collections.Generic;

namespace GameCore
{
    public abstract class GamePositionEvaluator<T> where T : GamePosition
    {
        #region abstract
        public abstract int LeftEvaluation(T position);
        public abstract int RightEvaluation(T position);
        #endregion


    }
}
