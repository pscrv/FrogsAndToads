using System.Collections.Generic;
using System.Linq;

using GameCore;

namespace FrogsAndToadsCore
{
    public class FrogsAndToadsGame : PartisanGame<FrogsAndToadsPosition>
    {

        #region public properties
        public string PositionString => _position.ToString();
        public List<string> StringHistory => 
            _positionHistory
            .Select(x => x.ToString())
            .ToList();
        #endregion


        #region construction
        public FrogsAndToadsGame(
            PartisanGamePlayer<FrogsAndToadsPosition> leftPlayer, 
            PartisanGamePlayer<FrogsAndToadsPosition> rightPlayer, 
            FrogsAndToadsPosition initialPosition)
            : base(leftPlayer, rightPlayer, initialPosition)
        { }


        public FrogsAndToadsGame(
            PartisanGamePlayer<FrogsAndToadsPosition> leftPlayer,
            PartisanGamePlayer<FrogsAndToadsPosition> rightPlayer,
            string positionString)
            : base(leftPlayer, rightPlayer, new FrogsAndToadsPosition(positionString))
        { }
        #endregion



        #region Game overrides
        public override IEnumerable<FrogsAndToadsPosition> GetLeftOptions(FrogsAndToadsPosition position)
        {
            return 
                position
                .GetPossibleToadMoves()
                .Select(x => Position.PlayMove(x));
        }

        public override IEnumerable<FrogsAndToadsPosition> GetRightOptions(FrogsAndToadsPosition position)
        {
            return 
                position
                .GetPossibleFrogMoves()
                .Select(x => Position.PlayMove(x));
        }
        #endregion
    }
}
