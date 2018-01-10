using System.Collections.Generic;
using System.Linq;
using Monads;

namespace FrogsAndToadsCore
{
    public class MiniMiniMaxPlayer : SymmetricPlayer
    {
        public MiniMiniMaxPlayer(string label) 
            : base(label)
        { }

        public override Maybe<FrogsAndToadsPosition> PlayLeft(IEnumerable<FrogsAndToadsPosition> playOptions)
        {
            if (playOptions.Count() == 0)
                return Maybe<FrogsAndToadsPosition>.Nothing();

            if (playOptions.Count() == 1)
                return playOptions.First().ToMaybe();

            FrogsAndToadsPosition bestOption = playOptions.First();
            FrogsAndToadsPosition currentOption;
            int maximum = int.MinValue;
            foreach (FrogsAndToadsPosition option in playOptions)
            {
                currentOption = option;
                
                List<FrogsAndToadsMove> possibleResponses = currentOption.GetPossibleFrogMoves();

                int minimum = int.MaxValue;
                foreach (FrogsAndToadsMove response in possibleResponses)
                {
                    int reResponseCount = 
                        currentOption
                        .PlayMove(response)
                        .GetPossibleToadMoves()
                        .Count;

                    if (reResponseCount < minimum)
                        minimum = reResponseCount;
                }

                if (minimum > maximum)
                {
                    maximum = minimum;
                    bestOption = option;
                }

            }

            return bestOption.ToMaybe();
        }

        
    }
}