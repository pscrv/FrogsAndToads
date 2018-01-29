using System;
using System.Collections.Generic;
using System.Linq;
using GameCore;

namespace NoughtsAndCrossesCore
{
    public sealed class NoughtsAndCrossesPosition : GamePosition, IEquatable<NoughtsAndCrossesPosition>
    {
        #region private attributes
        private NoughtsAndCrossesPiece[][] _board;
        #endregion


        #region internal properties 
        internal int Size => _board.Length;

        internal NoughtsAndCrossesPiece this[int row, int column]
            => _board[row][column];

        internal NoughtsAndCrossesPosition Reverse
            => new NoughtsAndCrossesPosition(_reverseBoard());

        #endregion



        #region construction
        internal NoughtsAndCrossesPosition(int size)
        {
            _board = new NoughtsAndCrossesPiece[size][];
            for (int i = 0; i < size; i++)
            {
                _board[i] = new NoughtsAndCrossesPiece[size];

                for (int j = 0; j < size; j++)
                {
                    _board[i][j] = new Space();
                }
            }
        }

        private NoughtsAndCrossesPosition(NoughtsAndCrossesPiece[][] board)
        {
            if (board?.Length == 0)
                throw new ArgumentException($"{nameof(board)} is empty or null.");
            
            int size = board.Length;
            _board = new NoughtsAndCrossesPiece[size][];
            for (int i = 0; i < size; i++)
            {
                if (board[i].Length != size)
                    throw new ArgumentException($"{nameof(board)} is not square.");

                _board[i] = new NoughtsAndCrossesPiece[size];

                for (int j = 0; j < size; j++)
                {
                    _board[i][j] = board[i][j];
                }
            }
        }
        #endregion



        #region private methods
        private NoughtsAndCrossesPiece[][] _reverseBoard()
        {
            return _board.Select(x => _reverseRow(x)).ToArray();            
        }

        private NoughtsAndCrossesPiece[] _reverseRow(NoughtsAndCrossesPiece[] row)
        {
            return row.Select(x => x.Converse).ToArray();
        }
        #endregion



        #region IEquatable
        public bool Equals(NoughtsAndCrossesPosition other)
        {
            if (other == null)
                return false;

            for (int i = 0; i < _board.Length; i++)
            {
                for (int j = 0; j < _board[i].Length; j++)
                {
                    if (other[i, j] != this[i, j])
                        return false;
                }
            }

            return true;
        }

        public override bool Equals(object other)
        {
            if (ReferenceEquals(null, other))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return this.Equals(other as NoughtsAndCrossesPosition);
        }

        public override int GetHashCode()
        {
            int value = 113;
            for (int i = 0; i < _board.Length; i++)
            {
                for (int j = 0; j < _board[i].Length; j++)
                {
                    unchecked
                    {
                        value *= _board[i][j].GetHashCode();
                    }                     
                }
            }

            return value;
        }

        public override List<GameMove> GetLeftMoves()
        {
            throw new NotImplementedException();
        }

        public override List<GameMove> GetRightMoves()
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}