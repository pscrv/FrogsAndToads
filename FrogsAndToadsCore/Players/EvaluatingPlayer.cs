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
        public override Maybe<FrogsAndToadsPosition> PlayLeft(IEnumerable<FrogsAndToadsPosition> playOptions)
        {              

            if (playOptions.Count() == 0)
                return Maybe<FrogsAndToadsPosition>.Nothing();

            if (playOptions.Count() == 1)
                return playOptions.First().ToMaybe();

            int optionValue;
            int bestValue = int.MinValue;
            FrogsAndToadsPosition bestOption = null;
            foreach (FrogsAndToadsPosition option in playOptions)
            {
                optionValue = _evaluator.RightEvaluation(option);
                if (optionValue > bestValue)
                {
                    bestValue = optionValue;
                    bestOption = option;
                }
            }            

            return bestOption.ToMaybe();
        }
        #endregion
    }
}