using System.Collections.Generic;

namespace GameCore
{
    public abstract class GamePositionEvaluator
    {
        #region abstract
        public abstract int LeftEvaluation(GamePosition position);
        #endregion


        public int RightEvaluation(GamePosition position)
        {
            return -LeftEvaluation(position.Reverse);
        }
    }
}
