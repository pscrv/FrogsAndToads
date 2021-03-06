﻿using System.Linq;
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
            Maybe<(int start, int end)> pseudoKnot =
                from knotstart in _getFirstToadIndex(offset)
                from knotend in _getEndOfKnot(knotstart)
                select (knotstart, knotend);
            
            FirstDeadKnot =
                from knot in pseudoKnot
                from deadstart in _findDeadStart(knot)
                from deadend in _findDeadEnd(knot, deadstart)
                select (deadstart, deadend);
            
            if (FirstDeadKnot.HasValue)
                return;


            Maybe<int> searchFurther =
                from x in pseudoKnot
                where x.end < _track.Length - 1
                select x.end + 1;

            if (searchFurther.HasValue)
                _findFirstDeadKnot(searchFurther.Value );
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

        Maybe<int> _findDeadStart((int start, int end) knot)
        {
            for (int deadStart = knot.start; deadStart <= knot.end; deadStart++)
            {
                if (_isPossibleDeadStart(deadStart))
                    return deadStart.ToMaybe();
            }

            return Maybe<int>.Nothing();
        }

        Maybe<int> _findDeadEnd((int start, int end) knot, int deadStart)
        {
            for (int deadEnd = knot.end; deadEnd > deadStart; deadEnd--)
            {
                if (_isPossibleDeadEnd(deadEnd))
                    return deadEnd.ToMaybe();
            }

            return Maybe<int>.Nothing();
        }

        bool _isPossibleDeadStart(int deadStart)
        {
            return 
                (deadStart == 0
                    && _track[0] is Toad)
                ||
                (deadStart > 0
                    && deadStart < _track.Length - 1
                    && _track[deadStart] is Toad
                    && _track[deadStart + 1] is Toad);
        }

        bool _isPossibleDeadEnd(int deadEnd)
        {                 
            return
                (deadEnd == _track.Length - 1
                    && _track[deadEnd] is Frog)
                ||
                (deadEnd < _track.Length -1
                    && deadEnd > 0
                    && _track[deadEnd] is Frog
                    && _track[deadEnd - 1] is Frog);
        }
        #endregion
    }
}
