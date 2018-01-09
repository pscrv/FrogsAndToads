using System.Collections.Generic;

using Monads;

namespace GameCore
{
    public abstract class PartisanGamePlayer<T> where T : PartisanGamePosition
    {
        public abstract Maybe<T> PlayLeft(IEnumerable<T> playOptions);
        public abstract Maybe<T> PlayRight(IEnumerable<T> playOptions);


        public readonly string Label;

        public PartisanGamePlayer(string label)
        {
            Label = label;
        }
    }
}