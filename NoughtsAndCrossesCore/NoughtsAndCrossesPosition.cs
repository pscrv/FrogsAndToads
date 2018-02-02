using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GameCore;

namespace NoughtsAndCrossesCore
{
    public class NoughtsAndCrossesPosition : GamePosition, IEquatable<NoughtsAndCrossesPosition>
    {
        #region private data
        private NoughtsAndCrossesPiece[,] _board;
        private int _size;
        #endregion


        #region construction
        internal NoughtsAndCrossesPosition(int size)
        {
            if (size < 1)
                throw new ArgumentException($"{nameof(size)} must be >= 1.");

            _size = size;
            _board = new NoughtsAndCrossesPiece[_size, _size];
            for (int row = 0; row < _size; row++)
                for (int column = 0; column < _size; column++)
                    _board[row, column] = Space.Instance;            
        }

        private NoughtsAndCrossesPosition(NoughtsAndCrossesPiece[,] board)
        {
            if (board.GetLength(0) != board.GetLength(1))
                throw new ArgumentException($"{nameof(board)} is not square.");

            _size = board.GetLength(0);
            _board = board.Clone() as NoughtsAndCrossesPiece[,];
            for (int row = 0; row < _size; row++)
                for (int column = 0; column < _size; column++)
                    _board[row, column] = board[row, column];
        }
        #endregion


        #region internal properties
        internal bool IsEndPosition
            => _isFinished();

        internal int FreeSpaceCount
            => _countFreeSpaces();

        private int _countFreeSpaces()
        {
            return _board.OfType<Space>().Count();
        }
        #endregion


        #region public methods
        public IEnumerable<NoughtsAndCrossesPosition> GetCrossOptions()
            => _getEmptyCells().Select(cell => Play(cell.row, cell.column, Cross.Instance));

        public IEnumerable<NoughtsAndCrossesPosition> GetNoughtOptions()
            => _getEmptyCells().Select(cell => Play(cell.row, cell.column, Nought.Instance));
        #endregion


        #region internal methods
        internal NoughtsAndCrossesPiece this [int row, int column]
            => _board[row, column];


        internal NoughtsAndCrossesPosition Play(int row, int column, NoughtsAndCrossesPiece piece)
        {
            _exceptionIfUnplayablePiece(piece);
            _exceptionIfInvalidIndex(row);
            _exceptionIfInvalidIndex(column);

            if (_board[row, column] is Space)
            {
                NoughtsAndCrossesPiece[,] newBoard = _board.Clone() as NoughtsAndCrossesPiece[,];
                newBoard[row, column] = piece;
                return new NoughtsAndCrossesPosition(newBoard);
            }

            throw new InvalidOperationException($"There is no Space at [{row}, {column}]");
        }
        #endregion


        #region private methods
        private IEnumerable<NoughtsAndCrossesPiece> _enumerate()
        {
            foreach (NoughtsAndCrossesPiece piece in _board)
            {
                yield return piece;
            }
        }


        private IEnumerable<(int row, int column)> _getEmptyCells()
        {
            for (int r = 0; r < _size; r++)
                for (int c = 0; c < _size; c++)
                {
                    if (_board[r, c] is Space)
                        yield return (r, c);
                }
        }


        private IEnumerable<IEnumerable<NoughtsAndCrossesPiece>> _getWinningLines()
        {
            foreach (IEnumerable<NoughtsAndCrossesPiece> row in _getRows())
            {
                yield return row;
            }

            foreach (IEnumerable<NoughtsAndCrossesPiece> column in _getColumns())
            {
                yield return column;
            }

            foreach (IEnumerable<NoughtsAndCrossesPiece> diagonal in _getDiagonals())
            {
                yield return diagonal;
            }
        }

        private IEnumerable<IEnumerable<NoughtsAndCrossesPiece>> _getRows()
        {
            for (int row = 0; row < _size; row++)
            {
                yield return _getRow(row);
            }
        }

        private IEnumerable<IEnumerable<NoughtsAndCrossesPiece>> _getColumns()
        {
            for (int column = 0; column < _size; column++)
            {
                yield return _getColumn(column);
            }
        }


        private IEnumerable<IEnumerable<NoughtsAndCrossesPiece>> _getDiagonals()
        {
            yield return _getTopLeftDiagonal();
            yield return _getTopRightDiagonal();
        }

        private IEnumerable<NoughtsAndCrossesPiece> _getTopLeftDiagonal()
        {
            for (int i = 0; i < _size; i++)
            {
                yield return _board[i, i];
            }
        }

        private IEnumerable<NoughtsAndCrossesPiece> _getTopRightDiagonal()
        {
            for (int i = 0; i < _size; i++)
            {
                yield return _board[i, _size - i - 1];
            }
        }



        private IEnumerable<NoughtsAndCrossesPiece> _getRow(int row)
        {
            _exceptionIfInvalidIndex(row);
            for (int column = 0; column < _size; column++)
            {
                yield return _board[row, column];
            }
        }

        private IEnumerable<NoughtsAndCrossesPiece> _getColumn(int column)
        {
            _exceptionIfInvalidIndex(column);
            for (int row = 0; row < _size; row++)
            {
                yield return _board[row, column];   
            }
        }


        private void _exceptionIfUnplayablePiece(NoughtsAndCrossesPiece piece)
        {
            if (piece == null)
                throw new ArgumentException($"{nameof(piece)} cannot be null");

            if (piece is Space)
                throw new InvalidOperationException("Cannot play a Space.");
        }

        private void _exceptionIfInvalidIndex(int index)
        {
            if (_isInvalidIndex(index))
                throw new IndexOutOfRangeException(nameof(index));
        }

        private bool _isInvalidIndex(int row)
        {
            return row < 0 || row >= _size;
        }

        private bool _isFinished()
        {
            foreach (IEnumerable<NoughtsAndCrossesPiece> line in _getWinningLines())
            {
                NoughtsAndCrossesPiece firstPiece = line.First();
                if ( !(firstPiece is Space) && line.All(x => x == firstPiece))
                    return true;
            }

            return false;
        }
        
        #endregion



        #region GamePosition

        public override IEnumerable<GamePosition> GetLeftOptions()
            => GetCrossOptions();

        public override IEnumerable<GamePosition> GetRightOptions()
            => GetNoughtOptions();
        #endregion



        #region IEquatable
        public bool Equals(NoughtsAndCrossesPosition other)
        {
            if (ReferenceEquals(other, null))
                return false;

            if (this._size != other._size)
                return false;
            
            for (int row = 0; row < _size; row++)
                for (int column = 0; column < _size; column++)
                    if (this[row, column] != other[row, column])
                        return false;

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
            int hash = 1;
            int itemhash = 1;
            foreach (var item in _board)
            {

                if (item is Cross)
                    hash *= itemhash * 13;

                if (item is Nought)
                    hash *= itemhash * 17;

                itemhash *= 3;
            }

            return hash;
        }
        #endregion



        #region overrides
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            foreach (IEnumerable<NoughtsAndCrossesPiece> row in _getRows())
            {
                foreach (NoughtsAndCrossesPiece piece in row)
                {
                    builder.Append(piece.ToString());
                    builder.Append(" ");
                }
                
                builder.Append(Environment.NewLine);
            }

            return builder.ToString();
        }
        #endregion
    }
}
