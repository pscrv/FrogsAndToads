using System.Linq;
using Monads;

namespace FrogsAndToadsCore
{
    internal class KnotFinder
    {
        #region private attributes
        private FrogsAndToadsPiece[] _track;
        #endregion


        #region internal properties
        internal Maybe<(int, int)> FirstDeadKnot { get; private set; }
        #endregion


        #region construction
        internal KnotFinder(FrogsAndToadsPiece[] track)
        { 
            _track = track;
            _findFirstDeadKnot(0);
        }
        #endregion


        #region private methods
        private void _findFirstDeadKnot(int offset)
        {
            int lastIndex = _track.Length - 1;
            
            int start = _getFirstToadIndex(offset);
            if (start < 0)
            {
                FirstDeadKnot = Maybe<(int, int)>.Nothing();
                return;
            }

            int end =_getEndOfKnot(start);

            int deadStart = _findDeadStart(start, end);
            if (deadStart < 0)
            {
                if (end == lastIndex)
                {
                    FirstDeadKnot = Maybe<(int, int)>.Nothing();
                    return;
                }

                _findFirstDeadKnot(end + 1);
                return;
            }

            int deadEnd =_findDeadEnd(deadStart, end);
            if (deadEnd < 0)
            {
                if (end == lastIndex)
                {
                    FirstDeadKnot = Maybe<(int, int)>.Nothing();
                    return;
                }

                _findFirstDeadKnot(end + 1);
                return;
            }

            FirstDeadKnot = (deadStart, deadEnd).ToMaybe();
        }



        int _getFirstToadIndex(int offset)
        {
            for (int start = offset; start < _track.Length; start++)
            {
                if (_track[start] is Toad)
                    return start;
            }

            return -1;
        }

        int _getEndOfKnot(int start)
        {
            for (int end = start + 1; end < _track.Length; end++)
            {
                if (_track[end] is Space)
                {
                    return end - 1;
                }
            }

            return _track.Length - 1;
        }

        int _findDeadStart(int start, int end)
        {
            for (int deadStart = start; deadStart < end; deadStart++)
            {
                if (_isPossibleDeadStart(deadStart))
                    return deadStart;
            }

            return -1;
        }

        int _findDeadEnd(int deadStart, int end)
        {
            for (int deadEnd = end; deadEnd > deadStart; deadEnd--)
            {
                if (_isPossibleDeadEnd(deadEnd))
                    return deadEnd;
            }

            return -1;
        }

        bool _isPossibleDeadStart(int deadStart)
        {
            if (!(_track[deadStart] is Toad))
                return false;

            return (
                (deadStart == 0
                    && _track[deadStart] is Toad)
                ||
                (deadStart > 0
                    && deadStart < _track.Length - 1
                    && _track[deadStart] is Toad
                    && _track[deadStart + 1] is Toad));
        }

        bool _isPossibleDeadEnd(int deadEnd)
        {
            if (!(_track[deadEnd] is Frog))
                return false;
                 
            return (
                (deadEnd == _track.Length - 1
                    && _track[deadEnd] is Frog)
                ||
                (deadEnd < _track.Length -1
                    && deadEnd > 0
                    && _track[deadEnd] is Frog
                    && _track[deadEnd - 1] is Frog));
        }
        #endregion



    }
}
