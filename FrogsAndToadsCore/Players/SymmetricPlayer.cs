using System.Collections.Generic;
using System.Linq;
using Monads;

namespace FrogsAndToadsCore
{
    public abstract class SymmetricPlayer : FrogsAndToadsPlayer
    {
        #region abstract
        protected abstract int _getOptionValue(FrogsAndToadsPosition option);
        #endregion



        #region construction
        public SymmetricPlayer(string label) 
            : base(label)
        { }
        #endregion


        #region FrogsAndToadsPlayer overrides
        public override Maybe<FrogsAndToadsPosition> PlayLeft(IEnumerable<FrogsAndToadsPosition> playOptions)
        {
            if (playOptions.Count() == 0)
                return Maybe<FrogsAndToadsPosition>.Nothing();

            if (playOptions.Count() == 1)
                return playOptions.First().ToMaybe();

            return _getMaximumOption(playOptions).ToMaybe();
        }


        public override Maybe<FrogsAndToadsPosition> PlayRight(IEnumerable<FrogsAndToadsPosition> playOptions)
        {
            IEnumerable<FrogsAndToadsPosition> reversedOptions =
                from x in playOptions
                select x.Reverse();

            Maybe<FrogsAndToadsPosition> bestOption = PlayLeft(reversedOptions);

            return
                from x in bestOption
                select x.Reverse();
        }
        #endregion



        #region private methods
        private FrogsAndToadsPosition _getMaximumOption(IEnumerable<FrogsAndToadsPosition> playOptions)
        {
            // No simple linq version of this

            FrogsAndToadsPosition bestOption = null;
            int bestValue = int.MinValue;
            int optionValue;

            foreach (FrogsAndToadsPosition option in playOptions)
            {
                optionValue = _getOptionValue(option);

                if (optionValue > bestValue)
                {
                    bestValue = optionValue;
                    bestOption = option;
                }
            }

            return bestOption;
        }
        #endregion
    }
}