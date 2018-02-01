using System.Linq;

using GameCore;

namespace FrogsAndToadsCore
{
    public class MiniMiniMaxPlayer : SymmetricEvaluatingPlayer<FrogsAndToadsPosition>
    {
        public MiniMiniMaxPlayer(string label) 
            : base(label)
        { }


        #region SymmetricPlayer overrides
        protected override int _getOptionValue(FrogsAndToadsPosition option)
        {
            return
                option
                .GetPossibleFrogMoves() 
                .Min(x => option.PlayMove(x).GetPossibleToadMoves().Count);
        }
        #endregion
    }
}