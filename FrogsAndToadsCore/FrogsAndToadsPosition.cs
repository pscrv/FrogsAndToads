using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCore;
using Monads;

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
        private FrogsAndToadsPiece[] _track;
        #endregion



        #region internal properties       
        internal int Length => _track.Length;


        internal int ToadCount => _track.Count(x => x is Toad);
        internal int FrogCount => _track.Count(x => x is Frog);

        internal FrogsAndToadsPiece this[int index]
        {
            get => _track[index];
        }


        internal FrogsAndToadsPosition Reverse
            => new FrogsAndToadsPosition(
                _track.Reverse()
                .Select(x => x.Converse)
                .ToArray());

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
            if (index < 0 || index >= _track.Length)
                throw new IndexOutOfRangeException();


            if (_track[index].Move == 0)
                return false;

            int moveTarget = index + _track[index].Move;
            return _targetIsFree(moveTarget);
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
            List<FrogsAndToadsPosition> result = new List<FrogsAndToadsPosition>();

            Maybe<(int start, int end)> findDeadKnot = _getDeadKnot(this, 0);
            if (!findDeadKnot.HasValue)
            {
                result.Add(this);
                return result;
            }

            int knotStart = findDeadKnot.Value.start;
            int knotEnd = findDeadKnot.Value.end;

            if (knotStart > 0)
            {
                result.Add(SubPosition(0, findDeadKnot.Value.start - 1));
            }

            if (knotEnd < Length - 1)
            {
                result.AddRange(
                        SubPosition(findDeadKnot.Value.end + 1, Length - 1)
                        .GetSubPositions());
            }


            return result;
        }
        #endregion


        #region private methods
        private Maybe<(int start, int end)> _getDeadKnot(FrogsAndToadsPosition position, int offset)
        {
            int start;
            int deadStart;
            int end;
            int deadEnd;
            int lastIndex = Length - 1;
            

            _setStartAtFirstToad();
            if (start < 0)
                return Maybe<(int, int)>.Nothing();

            _setEndAtEndOfKnot();
            //if (end < 0)
            //    return Maybe<(int, int)>.Nothing();

            _findDeadStart();
            if (deadStart < 0)
            {
                if (end == lastIndex)
                    return Maybe<(int, int)>.Nothing();

                return _getDeadKnot(position, end + 1);
            }

            _findDeadEnd();
            if (deadEnd < 0)
            {
                if (end == lastIndex)
                    return Maybe<(int, int)>.Nothing();

                return _getDeadKnot(position, end + 1);
            }

            return (deadStart, deadEnd).ToMaybe();




            void _setStartAtFirstToad()
            {
                for (start = offset; start < Length; start++)
                {
                    if (position[start] is Toad)
                        return;
                }

                start = -1;
            }

            void _setEndAtEndOfKnot()
            {
                for (end = start + 1; end < Length; end++)
                {
                    if (position[end] is Space)
                    {
                        end--;
                        return;
                    }
                }
                
                end = Length - 1;
            }

            void _findDeadStart()
            {
                for (deadStart = start; deadStart < end; deadStart++)
                {
                    var i = deadStart;

                    if (_isPossibleDeadStart())
                        return;
                }

                deadStart = -1;
            }

            void _findDeadEnd()
            {
                for (deadEnd = end; deadEnd > deadStart; deadEnd--)
                {
                    var i = deadStart;
                    var j = deadEnd;

                    if (_isPossibleDeadEnd())
                        return;
                }

                deadEnd = -1;
            }
            
            bool _isPossibleDeadStart()
            {
                if (!(position[deadStart] is Toad))
                    return false;

                return (
                    (deadStart == 0 
                        && position[deadStart] is Toad)
                    || 
                    (deadStart > 0 
                        && deadStart < lastIndex 
                        && position[deadStart] is Toad 
                        && position[deadStart + 1] is Toad));
            }

            bool _isPossibleDeadEnd()
            {
                if (!(position[deadEnd] is Frog))
                    return false;

                return (
                    (deadEnd == lastIndex 
                        && position[deadEnd] is Frog)
                    ||
                    (deadEnd < lastIndex
                        && deadEnd > 0
                        && position[deadEnd] is Frog
                        && position[deadEnd - 1] is Frog));
            }        
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



        private bool _locationIsOccupied(int target)
        {
            return ! (_track[target] is Space);
        }

        private bool _targetIsJumpable(int target, FrogsAndToadsPiece piece)
        {
            return _track[target] == piece.Converse;
        }

        private bool _locationIsInvalid(int location)
        {
            return 
                location < 0
                || location >= Length;
        }

        private bool _isMovablePiece(FrogsAndToadsPiece gamePiece)
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
            foreach (FrogsAndToadsPiece gp in _track)
            {
                sb.Append(gp.ToString());
                sb.Append(' ');               
            }
            sb.Append(">");
            return sb.ToString();
        }
        #endregion



        #region private classes
        //private class FindKnot : Try<(int start, int end)>
        //{
        //    private static FindKnot _failureInstance = new FindKnot();
        //    internal static new FindKnot Failure => _failureInstance;
        //    internal static new FindKnot Success((int start, int end) x)
        //        => new FindKnot((x.start, x.end));

        //    private FindKnot((int start, int end) x)
        //        : base((x.start, x.end))
        //    { }

        //    private FindKnot()
        //        : base()
        //    { }
        //}
        
        #endregion
    }
}