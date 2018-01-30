using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GameCore;
using Monads;

namespace FrogsAndToadsCore
{
    public sealed class FrogsAndToadsPosition : GamePosition, IEquatable<FrogsAndToadsPosition>
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
        private FrogsAndToadsPiece[] _track;
        #endregion


        #region internal properties       
        internal int Length => _track.Length;


        internal int ToadCount => _track.Count(x => x is Toad);
        internal int FrogCount => _track.Count(x => x is Frog);

        internal FrogsAndToadsPiece this[int index]
            => _track[index];


        internal FrogsAndToadsPosition Reverse()
            => new FrogsAndToadsPosition(_reverseTrack());
        #endregion


        #region construction
        internal FrogsAndToadsPosition(int toadCount, int spaceCount, int frogCount)
        {
            _track = new FrogsAndToadsPiece[toadCount + spaceCount + frogCount];

            var toads = Enumerable.Repeat(Toad.Instance, toadCount).ToArray();
            var spaces = Enumerable.Repeat(Space.Instance, spaceCount).ToArray();
            var frogs = Enumerable.Repeat(Frog.Instance, frogCount).ToArray();

            Array.Copy(toads, 0, _track, 0, toadCount);
            Array.Copy(spaces, 0, _track, toadCount, spaceCount);
            Array.Copy(frogs, 0, _track, toadCount + spaceCount, frogCount);
        }


        internal FrogsAndToadsPosition(string positionString)
        {

            _track = new FrogsAndToadsPiece[positionString.Length];
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

        private FrogsAndToadsPosition(FrogsAndToadsPiece[] track)
        {
            _track = new FrogsAndToadsPiece[track.Length];
            Array.Copy(track, _track, track.Length);
        }

        private FrogsAndToadsPosition(FrogsAndToadsPiece[] track, FrogsAndToadsMove move)
            : this(track)
        {
            _track[move.Target] = _track[move.Source];
            _track[move.Source] = new Space();
        }
        #endregion


        #region internal methods
        internal bool CanMovePiece(int index)
        {
            if (_locationIsInvalid(index))
                throw new IndexOutOfRangeException();
            
            return 
                _isMovablePiece(_track[index]) 
                &&_targetIsFree(index + _track[index].Move);
        }
        
        internal FrogsAndToadsPosition PlayMove(FrogsAndToadsMove move)
        {
            return new FrogsAndToadsPosition(_track, move);
        }
        
        internal List<FrogsAndToadsMove> GetAllPossibleMoves()
        {
            return _getPossibleMoves(x => true);
        }

        internal List<FrogsAndToadsMove> GetPossibleToadMoves()
        {
            return _getPossibleMoves(x => x is Toad);
        }

        internal List<FrogsAndToadsMove> GetPossibleFrogMoves()
        {
            return _getPossibleMoves(x => x is Frog);
        }



        internal FrogsAndToadsPosition SubPosition(int leftIndex, int rightIndex)
        {
            if (_locationIsInvalid(leftIndex))
                throw new IndexOutOfRangeException("leftIndex");


            if (_locationIsInvalid(rightIndex))
                throw new IndexOutOfRangeException("rightIndex");

            if (leftIndex > rightIndex)
                throw new InvalidOperationException("leftIndex bigger than rightIndex");

            int length = rightIndex - leftIndex + 1;
            FrogsAndToadsPiece[] result = new FrogsAndToadsPiece[length];
            Array.Copy(_track, leftIndex, result, 0, length);
            return new FrogsAndToadsPosition(result);
        }

        internal List<FrogsAndToadsPosition> GetSubPositions()
        {
            List <FrogsAndToadsPosition> result = new List<FrogsAndToadsPosition>();

            KnotFinder finder = new KnotFinder(_track);
            Maybe<(int start, int end)> maybeDeadKnot = finder.FirstDeadKnot;
            
            if (!maybeDeadKnot.HasValue)
            {
                result.Add(this);
                return result;
            }

            int knotStart = maybeDeadKnot.Value.start;
            int knotEnd = maybeDeadKnot.Value.end;

            if (knotStart > 0)
            {
                result.Add(SubPosition(0, maybeDeadKnot.Value.start - 1));
            }

            if (knotEnd < Length - 1)
            {
                result.AddRange(
                        SubPosition(maybeDeadKnot.Value.end + 1, Length - 1)
                        .GetSubPositions());
            }


            return result;
        }
        #endregion


        #region private methods
        private FrogsAndToadsPiece[] _reverseTrack()
        {
            return 
                (from x in _track.Reverse()
                 select x.Converse)
                 .ToArray();
        }



        private List<FrogsAndToadsMove> _getPossibleMoves(Predicate<FrogsAndToadsPiece> pieceChooser)
        {
            FrogsAndToadsPiece currentPiece;
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

                  
                    possibleMoves.Add(new FrogsAndToadsMove(source, target));
                }
            }

            return possibleMoves;
        }
        

        private bool _locationIsInvalid(int location)
            => location < 0 || location >= Length;

        private bool _locationIsOccupied(int target)
            => ! (_track[target] is Space);

        private bool _targetIsJumpable(int target, FrogsAndToadsPiece piece)
            => _track[target] == piece.Converse;

        private bool _isMovablePiece(FrogsAndToadsPiece gamePiece)
            => gamePiece.Move != 0;

        private bool _targetIsFree(int target)
            => target >= 0
               && target < _track.Length
               && _track[target] is Space;
        #endregion


        #region GamePosition
        public override IEnumerable<GamePosition> GetLeftOptions()
        {
            return GetPossibleToadMoves()
                .Select(x => PlayMove(x));
        }

        public override IEnumerable<GamePosition> GetRightOptions()
        {
            return GetPossibleFrogMoves()
                .Select(x => PlayMove(x));
        }


        #endregion


        #region IEquatable
        public bool Equals(FrogsAndToadsPosition other)
        {
            if (other == null)
                return false;

            return this.ToString() == other.ToString();
        }

        public override bool Equals(object other)
        {
            if (ReferenceEquals(null, other))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return this.Equals(other as FrogsAndToadsPosition);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
        #endregion


        #region overrides
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("< ");
            foreach (FrogsAndToadsPiece gp in _track)
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