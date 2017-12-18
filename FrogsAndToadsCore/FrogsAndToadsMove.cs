using System;

namespace FrogsAndToadsCore
{
    public class FrogsAndToadsMove
    {
        public int Source { get; }
        public int Target { get; }

        
        public FrogsAndToadsMove(int source, int target)
        {
            Source = source;
            Target = target;
        }   
    }
}
