using System.Collections.Generic;
using System.Linq;

using GameCore;

namespace NoughtsAndCrossesCore
{
    public class NoughtsAndCrossesGame : Game<NoughtsAndCrossesPosition>
    {

        #region public properties
        public string PositionString => _position.ToString();
        public List<string> StringHistory => 
            _positionHistory
            .Select(x => x.ToString())
            .ToList();
        #endregion


        #region construction
        public NoughtsAndCrossesGame(
            GamePlayer<NoughtsAndCrossesPosition> leftPlayer, 
            GamePlayer<NoughtsAndCrossesPosition> rightPlayer, 
            int size)
            : base(leftPlayer, rightPlayer, new NoughtsAndCrossesPosition(size))
        { }


        public NoughtsAndCrossesGame(
            GamePlayer<NoughtsAndCrossesPosition> leftPlayer,
            GamePlayer<NoughtsAndCrossesPosition> rightPlayer)
            : this(leftPlayer, rightPlayer, 3)
        { }
        #endregion



        #region Game overrides
        public override IEnumerable<NoughtsAndCrossesPosition> GetLeftOptions(NoughtsAndCrossesPosition position)
            => position.GetCrossOptions();
        
        public override IEnumerable<NoughtsAndCrossesPosition> GetRightOptions(NoughtsAndCrossesPosition position)
            => position.GetNoughtOptions();        
        #endregion
    }
}
