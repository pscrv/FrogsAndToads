using System;

namespace FrogsAndToadsCore
{
    internal sealed class FrogsAndToadsMove : IEquatable<FrogsAndToadsMove>
    {
        internal int Source { get; }
        internal int Target { get; }

        
        internal FrogsAndToadsMove(int source, int target)
        {
            Source = source;
            Target = target;
        }

        #region IEquatable
        public bool Equals(FrogsAndToadsMove other)
        {
            return 
                this.Source == other.Source 
                && this.Target == other.Target;
        }

        public override bool Equals(object other)
        {
            if (ReferenceEquals(other, null))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return this.Equals(other as FrogsAndToadsMove);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 13;
                hashCode = (hashCode * 397) ^ Source;
                hashCode = (hashCode * 397) ^ Target;
                return hashCode;
            }
        }

        #endregion
    }
}
