using System;
using System.Collections.Generic;
using GameCore;

namespace NoughtsAndCrossesCore
{
    public class NoughtsAndCrossesGame : Game<NoughtsAndCrossesPosition>
    {
        public NoughtsAndCrossesGame(
            GamePlayer<NoughtsAndCrossesPosition> leftPlayer, 
            GamePlayer<NoughtsAndCrossesPosition> rightPlayer, 
            NoughtsAndCrossesPosition initialPosition) 
            : base(leftPlayer, rightPlayer, initialPosition)
        { }



        #region Game
        public override IEnumerable<NoughtsAndCrossesPosition> GetLeftOptions(NoughtsAndCrossesPosition position)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<NoughtsAndCrossesPosition> GetRightOptions(NoughtsAndCrossesPosition position)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
