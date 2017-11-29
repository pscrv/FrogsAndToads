using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrogsAndToadsCore
{
    public class GamePosition
    {
        #region static
        internal static GamePosition MakeInitialPosition()
        {
            return __simpleDefault();
        }

        private static GamePosition __simpleDefault()
        {
            return new GamePosition(1, 1, 1);
        }
        #endregion


        #region private attributes
        GamePiece[] _track;
        #endregion



        #region properties       
        public int Length => _track.Length;
        #endregion



        #region construction
        internal GamePosition(int toadCount, int spaceCount, int frogCount)
        {
            _track = new GamePiece[toadCount + spaceCount + frogCount];

            // all these Toads are the same instance
            // is that is no problem, make Toad etc singletons
            var toads = Enumerable.Repeat(Toad.Instance, toadCount).ToArray();
            var spaces = Enumerable.Repeat(Space.Instance, spaceCount).ToArray();
            var frogs = Enumerable.Repeat(Frog.Instance, frogCount).ToArray();

            Array.Copy(toads, 0, _track, 0, toadCount);
            Array.Copy(spaces, 0, _track, toadCount, spaceCount);
            Array.Copy(frogs, 0, _track, toadCount + spaceCount, frogCount);
        }

        internal GamePosition(string positionString)
        {
            _track = new GamePiece[positionString.Length];
            for (int i = 0; i < positionString.Length; i++)
            {
                switch (positionString[i])
                {
                    case 'F':
                        _track[i] = Frog.Instance;
                        break;

                    case 'T':
                        _track[i] = Toad.Instance;
                        break;

                    case '_':
                        _track[i] = Space.Instance;
                        break;

                    default:
                        throw new ArgumentException("positionString should only contain F T or _ characters.");
                }
            }
        }

        private GamePosition(GamePiece[] track)
        {
            _track = new GamePiece[track.Length];
            Array.Copy(track, _track, track.Length);
        }

        internal GamePosition Reverse()
        {
            int length = _track.Length;
            GamePiece[] _reverseTrack = new GamePiece[_track.Length];
            for (int i = 0, j = length - 1; i < length; i++, j--)
            {
                _reverseTrack[j] = _track[i].Converse;
            }

            return new GamePosition(_reverseTrack);
        }

        internal GamePosition(GamePosition position)
        {
            _track = new GamePiece[position._track.Length];
            Array.Copy(position._track, _track, _track.Length);
        }
        #endregion


        #region internal methods
        internal bool CanMovePiece(int index)
        {
            if (index < 0 || index >= _track.Length)
                throw new IndexOutOfRangeException();


            if (_track[index].Move == 0)
                return false;
            
            int moveTarget = index + _track[index].Move;
            return _targetIsFree(moveTarget);

        }

        internal bool CanJumpPiece(int index)
        {
            if (index < 0 || index >= _track.Length)
                throw new IndexOutOfRangeException();
            
            if (_track[index].Move == 0)
                return false;

            int moveTarget = index + _track[index].Move;
            int jumpTarget = moveTarget + _track[index].Move;
            return
                _targetIsFree(jumpTarget)
                && _track[index].CanJump(_track[moveTarget]);
        }

        internal GamePosition MovePiece(int index)
        {
            if (CanMovePiece(index))
            {
                int moveTarget = index + _track[index].Move;
                GamePosition result = new GamePosition(this);
                result._track[moveTarget] = result._track[index];
                result._track[index] = new Space();
                return result;
            }

            if (CanJumpPiece(index))
            {
                int jumpTarget = index + _track[index].Move + _track[index].Move;
                GamePosition result = new GamePosition(this);
                result._track[jumpTarget] = result._track[index];
                result._track[index] = new Space();
                return result;
            }

            throw new InvalidOperationException("Immovable piece.");    
        }

        internal List<int> GetAllPossibleMoves()
        {
            return _getPossibleMoves(x => true);
        }

        internal List<int> GetPossibleToadMoves()
        {
            return _getPossibleMoves(x => x is Toad);
        }

        internal List<int> GetPossibleFrogMoves()
        {
            return _getPossibleMoves(x => x is Frog);
        }
        #endregion
        

        #region private methods
        private List<int> _getPossibleMoves(Predicate<GamePiece> pieceChooser)
        {
            List<int> possibleMoves = new List<int>();
            for (int i = 0; i < _track.Length; i++)
            {
                if (pieceChooser(_track[i]) && (CanMovePiece(i) || CanJumpPiece(i)))
                    possibleMoves.Add(i);
            }
            return possibleMoves;
        }

        private bool _targetIsFree(int target)
        {
            return target >= 0
                && target < _track.Length
                && _track[target] is Space;
        }
        #endregion




        #region overrides
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("< ");
            foreach (GamePiece gp in _track)
            {
                sb.Append(gp.ToString());
                sb.Append(' ');               
            }
            sb.Append(">");
            return sb.ToString();
        }
        #endregion
    }
}