using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCore;

namespace FrogsAndToadsCore
{
    public class FrogsAndToadsPosition : GamePosition
    {
        #region static
        internal static FrogsAndToadsPosition MakeInitialPosition()
        {
            return __simpleDefault();
        }

        private static FrogsAndToadsPosition __simpleDefault()
        {
            return new FrogsAndToadsPosition(1, 1, 1);
        }
        #endregion


        #region private attributes
        GamePiece[] _track;
        #endregion



        #region properties       
        public int Length => _track.Length;

        public GamePiece this[int index]
        {
            get => _track[index];
        }

        public override GamePosition Reverse
            => new FrogsAndToadsPosition(
                _track.Reverse()
                .Select(x => x.Converse)
                .ToArray());

        #endregion



        #region construction
        internal FrogsAndToadsPosition(int toadCount, int spaceCount, int frogCount)
        {
            _track = new GamePiece[toadCount + spaceCount + frogCount];

            var toads = Enumerable.Repeat(Toad.Instance, toadCount).ToArray();
            var spaces = Enumerable.Repeat(Space.Instance, spaceCount).ToArray();
            var frogs = Enumerable.Repeat(Frog.Instance, frogCount).ToArray();

            Array.Copy(toads, 0, _track, 0, toadCount);
            Array.Copy(spaces, 0, _track, toadCount, spaceCount);
            Array.Copy(frogs, 0, _track, toadCount + spaceCount, frogCount);
        }

        internal FrogsAndToadsPosition(string positionString)
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

        private FrogsAndToadsPosition(GamePiece[] track)
        {
            _track = new GamePiece[track.Length];
            Array.Copy(track, _track, track.Length);
        }

        private FrogsAndToadsPosition(GamePiece[] track, FrogsAndToadsMove move)
            : this(track)
        {
            _track[move.Target] = _track[move.Source];
            _track[move.Source] = new Space();
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

        internal bool CanMovePiece(FrogsAndToadsMove move)
        {
            return _targetIsFree(move.Target);
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

        internal bool CanJumpPiece(FrogsAndToadsMove move)
        {
            if (move.Source < 0 || move.Source >= _track.Length)
                throw new IndexOutOfRangeException();

            if (_track[move.Source].Move == 0)
                return false;

            int moveTarget = move.Source + _track[move.Source].Move;
            int jumpTarget = moveTarget + _track[move.Source].Move;
            return
                _targetIsFree(jumpTarget)
                && _track[move.Source].CanJump(_track[moveTarget]);
        }


        internal FrogsAndToadsPosition PlayMove(FrogsAndToadsMove move)
        {
            return new FrogsAndToadsPosition(_track, move);
        }


        internal List<FrogsAndToadsMove> GetAllPossibleMoves()
        {
            return _getPossibleMoves1(x => true);
        }

        internal List<FrogsAndToadsMove> GetPossibleToadMoves()
        {
            return _getPossibleMoves1(x => x is Toad);
        }

        internal List<FrogsAndToadsMove> GetPossibleFrogMoves()
        {
            return _getPossibleMoves1(x => x is Frog);
        }
        #endregion


        #region private methods
        private List<int> _getPossibleMoves(Predicate<GamePiece> pieceChooser)
        {
            List<int> possibleMoves = new List<int>();
            for (int i = 0; i < _track.Length; i++)
            {
                if (pieceChooser(_track[i])
                    && (CanMovePiece(i) || CanJumpPiece(i)))
                {
                    possibleMoves.Add(i);
                }

            }
            return possibleMoves;
        }

        private List<FrogsAndToadsMove> _getPossibleMoves1(Predicate<GamePiece> pieceChooser)
        {
            GamePiece currentPiece;
            List<FrogsAndToadsMove> possibleMoves = new List<FrogsAndToadsMove>();
            for (int source = 0; source < _track.Length; source++)
            {
                currentPiece = _track[source];
                if (pieceChooser(currentPiece)
                    && _isMovablePiece(currentPiece))
                {
                    int target = source + currentPiece.Move;

                    if (_locationIsInvalid(target))
                        continue;

                    if (_targetIsJumpable(target, currentPiece))
                        target += currentPiece.Move;

                    if (_locationIsInvalid(target))
                        continue;

                    if (_locationIsOccupied(target))
                        continue;


                    FrogsAndToadsMove move = new FrogsAndToadsMove(source, target);                    
                    possibleMoves.Add(move);
                }
            }

            return possibleMoves;
        }

        private bool _locationIsOccupied(int target)
        {
            return ! (_track[target] is Space);
        }

        private bool _targetIsJumpable(int target, GamePiece piece)
        {
            return _track[target] == piece.Converse;
        }

        private bool _locationIsInvalid(int location)
        {
            return 
                location < 0
                || location >= Length;
        }

        private bool _isMovablePiece(GamePiece gamePiece)
        {
            return gamePiece.Move != 0;
        }

        private bool _targetIsFree(int target)
        {
            return 
                target >= 0
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