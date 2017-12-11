using System;
using Utilities;

namespace GameCore
{
    internal class GameTrack
    {
        #region attributes and properties
        private GamePiece[] _track;

        internal int Length => _track.Length;
        #endregion


        #region construction
        internal GameTrack(int length)
        {
            _track = new GamePiece[length].Initialize(x => GamePiece.None);            
        }


        internal GameTrack(string setupString)
            : this(setupString.Length)
        {
            if (setupString.Length == 0)
                throw new ArgumentException("setupString must have length >= 1.");
            
            int index = 0;
            foreach (char ch in setupString)
            {
                switch (ch)
                {
                    case 'T':
                        _track[index] = new Toad();
                        break;
                    case 'F':
                        _track[index] = new Frog();
                        break;
                    case '_':
                        break;
                    default:
                        throw new ArgumentException("Only F, T, and _ should appear in setupString.");
                }

                index++;
            }

        }


        internal class Toad : GamePiece
        {
            protected override string _asString()
            {
                return "T";
            }
        }

        internal class Frog : GamePiece
        {
            protected override string _asString()
            {
                return "F";
            }
        }

        #endregion


        #region internal methods
        internal GamePiece this[int location]
        {
            get { _checkValidity(location); return _track[location]; }
        }


        internal int this[GamePiece piece] => _findLocation(piece);


        internal void AddPiece(GamePiece piece, int location)
        {
            _checkValidity(location);
            _track[location] = piece;
        }

        internal void RemovePiece(GamePiece piece, int location)
        {
            _checkValidity(location);
            _track[location] = GamePiece.None;
        }

        internal void MovePiece(int source, int target)
        {
            _checkValidity(source);
            _checkValidity(target);
            _track[target] = _track[source];
            _track[source] = GamePiece.None;
        }
        #endregion


        #region private methods
        private void _checkValidity(int location)
        {
            if (location < 0 || location >= _track.Length)
                throw new IndexOutOfRangeException("");
        }

        private int _findLocation(GamePiece piece)
        {
            for (int i = 0; i < _track.Length; i++)
            {
                if (_track[i] == piece)
                    return i;
            }

            throw new InvalidOperationException("Piece not found.");
        }
        #endregion
    }
}