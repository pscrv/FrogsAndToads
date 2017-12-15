using System;
using System.Collections.Generic;
using Utilities;

namespace GameCore
{
    public abstract class GamePlayer
    {
        public abstract AttemptPlay Play(IEnumerable<GamePosition> playOptions);


        public readonly string Label;
        
        public GamePlayer(string label)
        {
            Label = label;
        }
    }
}