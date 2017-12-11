using System;

namespace GameCore
{
    internal abstract class GamePiece
    {
        #region abstract
        protected abstract string _asString();
        #endregion


        #region static
        private static GamePiece __none = new None();
        private static GamePiece __dummy = new Dummy();

        internal static GamePiece None => __none;
        internal static GamePiece Dummy => __dummy;
        #endregion



        public override string ToString()
        {
            return _asString();
        }
    }







    internal class None : GamePiece
    {
        internal None()
        { }


        protected override string _asString()
        {
            return "_";
        }
    }


    internal class Dummy : GamePiece
    {
        protected override string _asString()
        {
            return "°";
        }
    }


}