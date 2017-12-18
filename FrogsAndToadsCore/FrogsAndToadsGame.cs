using System;
using System.Collections.Generic;
using System.Linq;
using GameCore;

namespace FrogsAndToadsCore
{
    public class FrogsAndToadsGame : Game
    {

        #region public properties
        public string PositionString => _position.ToString();
        public List<string> StringHistory => _positionHistory.Select(x => x.ToString()).ToList();
        #endregion


        #region construction
        public FrogsAndToadsGame(
            GamePlayer leftPlayer, 
            GamePlayer rightPlayer, 
            FrogsAndToadsPosition initialPosition)
            : base(leftPlayer, rightPlayer, initialPosition)
        { }


        public FrogsAndToadsGame(
            GamePlayer leftPlayer,
            GamePlayer rightPlayer,
            string positionString)
            : base(leftPlayer, rightPlayer, new FrogsAndToadsPosition(positionString))
        { }


        #endregion



        #region Game overrides
        public override IEnumerable<GamePosition> GetLeftMoves(GamePosition position)
        {
            FrogsAndToadsPosition ftp = (position as FrogsAndToadsPosition);
            return ftp
                .GetPossibleToadMoves()
                .Select(x => ftp.PlayMove(x));
        }
        #endregion
    }
}
