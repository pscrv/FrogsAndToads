using System;
using System.Collections.Generic;
using Utilities;

namespace GameCore
{
    public abstract class GamePlayer<T> where T : GamePosition
    {
        public abstract AttemptPlay<T> Play(IEnumerable<T> playOptions);


        public readonly string Label;
        
        public GamePlayer(string label)
        {
            Label = label;
        }
    }
}