using System;

namespace FrogsAndToadsCore
{
    public class Player
    {
        #region private
        private PlayChooser _chooser;
        private GamePiece _piece;
        #endregion


        #region construction
        internal Player(PlayChooser chooser, GamePiece piece)
        {
            _chooser = chooser;
            _piece = piece;
        }
        #endregion

        #region internal methods
        internal PlayChoice ChoosePlay(GamePosition position)
        {
            if (_piece == Toad.Instance)
                return _chooseToadPlay(position);

            if (_piece == Frog.Instance)
                return _chooseFrogPlay(position);

            throw new InvalidOperationException("_piece is not a Toad or Frog.");
        }
        #endregion


        #region private methods
        private PlayChoice _chooseToadPlay(GamePosition position)
        {
            return _chooser.ChoosePlay(position);
        }

        private PlayChoice _chooseFrogPlay(GamePosition position)
        {
            GamePosition reversePosition = position.Reverse();
            PlayChoice choice = _chooseToadPlay(reversePosition);

            return choice.NoChoiceMade
                ? choice
                : PlayChoice.ChoiceMade(position.Length - 1 - choice.Choice);
        }
        #endregion


        #region overrides
        public override string ToString()
        {
            return $"{_chooser} :: {_piece}";
        }
        #endregion
    }
}
