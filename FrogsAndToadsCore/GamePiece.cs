namespace FrogsAndToadsCore
{

    public abstract class GamePiece
    {
        #region abstract
        internal abstract int Move { get; }
        internal abstract GamePiece Converse { get; }
        internal abstract bool CanJump(GamePiece otherPiece);
        protected abstract string _asString();
        #endregion



        #region overrides
        public override string ToString()
        {
            return _asString();
        }
        #endregion
    }



    public class Frog : GamePiece
    {
        #region static
        private static Frog _instance = new Frog();
        public static Frog Instance => _instance;
        #endregion


        private Frog()
        { }


        #region GamePiece overrides
        internal override int Move => -1;
        internal override GamePiece Converse => Toad.Instance;

        internal override bool CanJump(GamePiece otherPiece)
        {
            return otherPiece is Toad;
        }

        protected override string _asString()
        {
            return "F";
        }
        #endregion
    }



    public class Toad : GamePiece
    {
        #region static
        private static Toad _instance = new Toad();
        public static Toad Instance => _instance;
        #endregion

        #region GamePiece overrides
        internal override int Move => 1;

        internal override GamePiece Converse => Frog.Instance;

        internal override bool CanJump(GamePiece otherPiece)
        {
            return otherPiece is Frog;
        }

        protected override string _asString()
        {
            return "T";
        }
        #endregion
    }



    public class Space : GamePiece
    {
        #region static
        private static Space _instance = new Space();
        public static Space Instance => _instance;
        #endregion

        #region GamePiece overrides
        internal override int Move => 0;

        internal override GamePiece Converse => _instance;

        internal override bool CanJump(GamePiece otherPiece)
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