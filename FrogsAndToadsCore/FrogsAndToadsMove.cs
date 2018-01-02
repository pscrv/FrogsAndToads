using System;

namespace FrogsAndToadsCore
{
    internal class FrogsAndToadsMove
    {
        internal int Source { get; }
        internal int Target { get; }

        
        internal FrogsAndToadsMove(int source, int target)
        {
            Source = source;
            Target = target;
        }   
    }
}
