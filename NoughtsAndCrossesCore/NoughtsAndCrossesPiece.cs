using System;

namespace NoughtsAndCrossesCore
{
    internal abstract class NoughtsAndCrossesPiece
    {
        #region abstract
        internal abstract NoughtsAndCrossesPiece Converse { get; }
        protected abstract string _asString();
        #endregion


        #region overrides
        public override string ToString()
        {
            return _asString();
        }
        #endregion}
    }



    internal class Cross : NoughtsAndCrossesPiece
    {
        internal override NoughtsAndCrossesPiece Converse 
            => new Nought();

        protected override string _asString()
        {
            return "X";
        }

        public override int GetHashCode()
        {
            return 7;
        }
    }

    internal class Nought : NoughtsAndCrossesPiece
    {
        internal override NoughtsAndCrossesPiece Converse
            => new Cross();

        protected override string _asString()
        {
            return "O";
        }

        public override int GetHashCode()
        {
            return 11;
        }
    }


    internal class Space : NoughtsAndCrossesPiece
    {
        internal override NoughtsAndCrossesPiece Converse 
            => this;

        protected override string _asString()
        {
            return " ";
        }

        public override int GetHashCode()
        {
            return 13;
        }
    }


}