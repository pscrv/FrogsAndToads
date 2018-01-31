using System;
using System.Collections.Generic;

namespace GameCore
{
    public abstract class GamePosition
    {
        public abstract IEnumerable<GamePosition> GetLeftOptions();
        public abstract IEnumerable<GamePosition> GetRightOptions();
    }

}