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
            FirstDeadKnot = Maybe<(int, int)>.Nothing();
            int lastIndex = _track.Length - 1;
            
            Maybe<(int start, int end)> pseudoKnot =
                from x in _getFirstToadIndex(offset)
                from y in _getEndOfKnot(x)
                select (x, y);

            if (pseudoKnot.HasValue)
            {
                FirstDeadKnot =
                    from x in _findDeadStart(pseudoKnot)
                    from y in _findDeadEnd(pseudoKnot, x)
                    select (x, y);

                if (FirstDeadKnot.HasValue 
                    || pseudoKnot.Value.end >= lastIndex)
                    return;

                _findFirstDeadKnot(pseudoKnot.Value.end + 1);
            }
        }



        Maybe<int> _getFirstToadIndex(int offset)
        {
            for (int start = offset; start < _track.Length; start++)
            {
                if (_track[start] is Toad)
                    return start.ToMaybe();
            }

            return Maybe<int>.Nothing();
        }

        Maybe<int> _getEndOfKnot(int start)
        { 
            if (! (_track[start] is Toad))
                return Maybe<int>.Nothing();
                       
            for (int end = start + 1; end < _track.Length; end++)
            {
                if (_track[end] is Space)
                {
                    return (end - 1).ToMaybe();
                }
            }

            return (_track.Length - 1).ToMaybe();
        }

        Maybe<int> _findDeadStart(Maybe<(int start, int end)> knot)
        {
            if (knot.HasValue)
            {
                for (int deadStart = knot.Value.start; deadStart <= knot.Value.end; deadStart++)
                {
                    if (_isPossibleDeadStart(deadStart))
                        return deadStart.ToMaybe();
                }
            }

            return Maybe<int>.Nothing();
        }

        Maybe<int> _findDeadEnd(Maybe<(int start, int end)> knot, int deadStart)
        {
            if (knot.HasValue)
            {
                for (int deadEnd = knot.Value.end; deadEnd > deadStart; deadEnd--)
                {
                    if (_isPossibleDeadEnd(deadEnd))
                        return deadEnd.ToMaybe();
                }
            }

            return Maybe<int>.Nothing();
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
