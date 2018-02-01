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
        #endregion
    }



    internal class Cross : NoughtsAndCrossesPiece
    {
        #region static
        private static Cross _instance = new Cross();
        public static Cross Instance => _instance;
        #endregion


        private Cross()
        { }


        #region GamePiece overrides
        internal override NoughtsAndCrossesPiece Converse => Nought.Instance;
        

        protected override string _asString()
        {
            return "X";
        }
        #endregion
    }



    internal class Nought : NoughtsAndCrossesPiece
    {
        #region static
        private static Nought _instance = new Nought();
        public static Nought Instance => _instance;
        #endregion

        #region GamePiece overrides
        internal override NoughtsAndCrossesPiece Converse => Cross.Instance;
        

        protected override string _asString()
        {
            return "0";
        }
        #endregion
    }



    internal class Space : NoughtsAndCrossesPiece
    {
        #region static
        private static Space _instance = new Space();
        public static Space Instance => _instance;
        #endregion

        #region GamePiece overrides
        internal override NoughtsAndCrossesPiece Converse => _instance;
        
        protected override string _asString()
        {
            return "_";
        }
        #endregion
    }
}
