namespace FrogsAndToadsCore
{

    internal abstract class FrogsAndToadsPiece
    {
        #region abstract
        internal abstract int Move { get; }
        internal abstract FrogsAndToadsPiece Converse { get; }
        internal abstract bool CanJump(FrogsAndToadsPiece otherPiece);
        protected abstract string _asString();
        #endregion


        #region overrides
        public override string ToString()
        {
            return _asString();
        }
        #endregion
    }



    internal class Frog : FrogsAndToadsPiece
    {
        #region static
        private static Frog _instance = new Frog();
        public static Frog Instance => _instance;
        #endregion


        private Frog()
        { }


        #region GamePiece overrides
        internal override int Move => -1;
        internal override FrogsAndToadsPiece Converse => Toad.Instance;

        internal override bool CanJump(FrogsAndToadsPiece otherPiece)
        {
            return otherPiece is Toad;
        }

        protected override string _asString()
        {
            return "F";
        }
        #endregion
    }



    internal class Toad : FrogsAndToadsPiece
    {
        #region static
        private static Toad _instance = new Toad();
        public static Toad Instance => _instance;
        #endregion

        #region GamePiece overrides
        internal override int Move => 1;

        internal override FrogsAndToadsPiece Converse => Frog.Instance;

        internal override bool CanJump(FrogsAndToadsPiece otherPiece)
        {
            return otherPiece is Frog;
        }

        protected override string _asString()
        {
            return "T";
        }
        #endregion
    }



    internal class Space : FrogsAndToadsPiece
    {
        #region static
        private static Space _instance = new Space();
        public static Space Instance => _instance;
        #endregion

        #region GamePiece overrides
        internal override int Move => 0;

        internal override FrogsAndToadsPiece Converse => _instance;

        internal override bool CanJump(FrogsAndToadsPiece otherPiece)
        {
            return false;
        }

        protected override string _asString()
        {
            return "_";
        }
        #endregion
    }
}