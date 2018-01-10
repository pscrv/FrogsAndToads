using System.Collections.Generic;
using System.Linq;
using Monads;

namespace FrogsAndToadsCore
{
    public abstract class SymmetricPlayer : FrogsAndToadsPlayer
    {
        public SymmetricPlayer(string label) 
            : base(label)
        { }


        public override Maybe<FrogsAndToadsPosition> PlayRight(IEnumerable<FrogsAndToadsPosition> playOptions)
        {
            var reversedOptions =
                from x in playOptions
                select x.Reverse();

            var bestOption = PlayLeft(reversedOptions);

            return
                from x in bestOption
                select x.Reverse();
        }
    }
}