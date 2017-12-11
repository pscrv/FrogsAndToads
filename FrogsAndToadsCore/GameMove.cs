using System;

namespace FrogsAndToadsCore
{
    internal class GameMove
    {
        private GamePosition _position;

        internal int Source { get; }
        internal int Target { get; }

        internal GameMove(int location, GamePosition position)
        {
            _position = position;

            if (_locationIsInvalid(location))
                throw new InvalidOperationException("location is not valid.");
                
                
            if(_isMovablePiece(location))
            {
                Source = location;
                Target = location + position[location].Move;
                if (_targetIsJumpable(Target))
                {
                    Target += position[location].Move;
                }

                throw new InvalidOperationException("Piece cannot move or jump.");
            }

        }

        private bool _locationIsInvalid(int index)
        {
            return
                index < 0
                || index >= _position.Length;
        }

        private bool _isMovablePiece(int index)
        {
            return _position[index].Move != 0;
        }

        private bool _targetIsJumpable(int target)
        {
            return _position[target] == _position[Source].Converse;
        }

        private bool _targetIsFree(int target)
        {
            return _position[target] is Space;
        }
    }
}
