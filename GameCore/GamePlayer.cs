using System.Collections.Generic;
using System.Linq;

using Monads;
namespace GameCore
{
    public abstract class GamePlayer<GP> where GP : GamePosition
    {
        public abstract Maybe<GP> PlayLeft(IEnumerable<GP> playOptions);
        public abstract Maybe<GP> PlayRight(IEnumerable<GP> playOptions);


        public readonly string Label;
        
        public GamePlayer(string label)
        {
            Label = label;
        }
    }



    public abstract class SymmetricEvaluatingPlayer<GP> : GamePlayer<GP> where GP : GamePosition, IReversibleGame<GP>
    {
        #region abstract
        protected abstract int _getOptionValue(GP option);
        #endregion



        #region construction
        public SymmetricEvaluatingPlayer(string label)
            : base(label)
        { }
        #endregion


        #region GamePlayer overrides
        public override Maybe<GP> PlayLeft(IEnumerable<GP> playOptions)
        {
            if (playOptions.Count() == 0)
                return Maybe<GP>.Nothing();

            if (playOptions.Count() == 1)
                return playOptions.First().ToMaybe();

            return _getMaximumOption(playOptions).ToMaybe();
        }


        public override Maybe<GP> PlayRight(IEnumerable<GP> playOptions)
        {
            IEnumerable<GP> reversedOptions =
                from x in playOptions
                select x.Reverse();

            Maybe<GP> bestOption = PlayLeft(reversedOptions);

            return
                from x in bestOption
                select x.Reverse();
        }
        #endregion



        #region private methods
        private GP _getMaximumOption(IEnumerable<GP> playOptions)
        {
            // No simple linq version of this

            GP bestOption = null;
            int bestValue = int.MinValue;
            int optionValue;

            foreach (GP option in playOptions)
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