﻿using System;
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
        

        internal FrogsAndToadsPosition(FrogsAndToadsPosition position)
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

        internal bool CanMovePiece(GameMove move)
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

        internal bool CanJumpPiece(GameMove move)
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


        internal FrogsAndToadsPosition MovePiece(int index)
        {
            if (CanMovePiece(index))
            {
                int moveTarget = index + _track[index].Move;
                FrogsAndToadsPosition result = new FrogsAndToadsPosition(this);
                result._track[moveTarget] = result._track[index];
                result._track[index] = new Space();
                return result;
            }

            if (CanJumpPiece(index))
            {
                int jumpTarget = index + _track[index].Move + _track[index].Move;
                FrogsAndToadsPosition result = new FrogsAndToadsPosition(this);
                result._track[jumpTarget] = result._track[index];
                result._track[index] = new Space();
                return result;
            }

            throw new InvalidOperationException("Immovable piece.");    
        }

        //internal FrogsAndToadsPosition MovePiece(GameMove move)
        //{
        //    if (CanMovePiece(move))
        //    {
        //        int moveTarget = move.Source + _track[move.Source].Move;
        //        FrogsAndToadsPosition result = new FrogsAndToadsPosition(this);
        //        result._track[moveTarget] = result._track[move.Source];
        //        result._track[move.Source] = new Space();
        //        return result;
        //    }

        //    if (CanJumpPiece(move))
        //    {
        //        int jumpTarget = move.Source + _track[move.Source].Move + _track[move.Source].Move;
        //        FrogsAndToadsPosition result = new FrogsAndToadsPosition(this);
        //        result._track[jumpTarget] = result._track[move.Source];
        //        result._track[move.Source] = new Space();
        //        return result;
        //    }

        //    throw new InvalidOperationException("Immovable piece.");
        //}



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
                if (pieceChooser(_track[i])
                    && (CanMovePiece(i) || CanJumpPiece(i)))
                {
                    possibleMoves.Add(i);
                }               

            }
            return possibleMoves;
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