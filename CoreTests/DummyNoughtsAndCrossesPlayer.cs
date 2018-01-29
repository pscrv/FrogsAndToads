using System;
using System.Collections.Generic;
using GameCore;
using Monads;
using NoughtsAndCrossesCore;

namespace CoreTests
{
    internal class DummyNoughtsAndCrossesPlayer : GamePlayer<NoughtsAndCrossesPosition>
    {
        public DummyNoughtsAndCrossesPlayer()
            : base("Dummy player")
        { }

        public override Maybe<NoughtsAndCrossesPosition> PlayLeft(IEnumerable<NoughtsAndCrossesPosition> playOptions)
        {
            throw new NotImplementedException();
        }

        public override Maybe<NoughtsAndCrossesPosition> PlayRight(IEnumerable<NoughtsAndCrossesPosition> playOptions)
        {
            throw new NotImplementedException();
        }
    }
}