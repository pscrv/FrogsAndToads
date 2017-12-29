using System.Collections.Generic;

namespace GameCore
{
    public abstract class GamePositionEvaluator<T> where T : GamePosition
    {
        #region abstract
        public abstract int LeftEvaluation(T position);
        #endregion


        public int RightEvaluation(T position)
        {
            return -LeftEvaluation(position.Reverse as T);
        }
    }
}
