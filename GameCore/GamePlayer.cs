using System;
using System.Collections.Generic;

using Monads;

namespace GameCore
{
    public abstract class GamePlayer<T> where T : GamePosition
    {
        public abstract Maybe<T> PlayLeft(IEnumerable<T> playOptions);
        public abstract Maybe<T> PlayRight(IEnumerable<T> playOptions);


        public readonly string Label;
        
        public GamePlayer(string label)
        {
            Label = label;
        }
    }
}