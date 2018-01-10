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


        #region SymmetricPlayer overrides
        protected override int _getOptionValue(FrogsAndToadsPosition position)
        {
            return
                position
                .GetPossibleFrogMoves() 
                .Min(x => position.PlayMove(x).GetPossibleToadMoves().Count);
        }
        #endregion
    }
}