using System;
using System.Collections.Generic;

using GameCore;

namespace NoughtsAndCrossesCore
{
    public class NoughtsAndCrossesPosition : GamePosition
    {
        public override IEnumerable<GamePosition> GetLeftOptions()
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<GamePosition> GetRightOptions()
        {
            throw new NotImplementedException();
        }
    }
}
