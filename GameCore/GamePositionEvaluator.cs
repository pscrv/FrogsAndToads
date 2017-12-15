using System.Collections.Generic;

namespace GameCore
{
    public abstract class GamePositionEvaluator
    {
        #region abstract
        public abstract int ToadEvaluation(GamePosition position);
        #endregion


        public int FrogEvaluation(GamePosition position)
        {
            return -ToadEvaluation(position.Reverse);
        }
    }
}
