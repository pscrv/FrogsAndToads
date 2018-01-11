using System.Collections.Generic;
using System.Linq;

using GameCore;
using Monads;

namespace FrogsAndToadsCore
{
    public class EvaluatingPlayer : SymmetricPlayer
    {
        public EvaluatingPlayer(string label, GamePositionEvaluator<FrogsAndToadsPosition> evaluator)
            :base(label)
        {
            _evaluator = evaluator;        
        }

        #region private attributes
        private GamePositionEvaluator<FrogsAndToadsPosition> _evaluator;
        #endregion


        #region SymmetricChooser overrides
        protected override int _getOptionValue(FrogsAndToadsPosition option)
        {
            return _evaluator.RightEvaluation(option);
        }
        #endregion
    }
}