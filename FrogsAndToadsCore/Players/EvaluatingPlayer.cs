using GameCore;

namespace FrogsAndToadsCore
{
    public class EvaluatingPlayer : SymmetricEvaluatingPlayer<FrogsAndToadsPosition>
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