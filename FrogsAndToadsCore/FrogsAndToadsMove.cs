using System;

namespace FrogsAndToadsCore
{
    public class FrogsAndToadsMove
    {
        #region static
        public static TestMove CreateMove(int location, FrogsAndToadsPosition position)
        {
            if (_locationIsInvalid(location, position)
                || _isImmovablePiece(location, position))
            {
                return TestMove.Failure;
            }

            int source = location;
            int target = source + position[source].Move;

            if (_locationIsInvalid(target, position))
            {
                return TestMove.Failure;
            }

            if (_targetIsJumpable(source, target, position))
                target += position[source].Move;

            if (_locationIsInvalid(target, position))
            {
                return TestMove.Failure;
            }

            if (_targetIsFree(target, position))
            {
                return TestMove.Success(new FrogsAndToadsMove(source, target, position));
            }

            return TestMove.Failure;
        }


        private static bool _locationIsInvalid(int index, FrogsAndToadsPosition position)
        {
            return
                index < 0
                || index >= position.Length;
        }

        private static bool _isImmovablePiece(int index, FrogsAndToadsPosition position)
        {
            return position[index].Move == 0;
        }

        private static bool _targetIsJumpable(int source, int target, FrogsAndToadsPosition position)
        {
            return position[target] == position[source].Converse;
        }

        private static bool _targetIsFree(int target, FrogsAndToadsPosition position)
        {
            return position[target] is Space;
        }


        #endregion




        private FrogsAndToadsPosition _position;

        public int Source { get; }
        public int Target { get; }

        private FrogsAndToadsMove(int location, FrogsAndToadsPosition position)
        {
            _position = position;

            if (_locationIsInvalid(location))
                throw new IndexOutOfRangeException("location is not valid.");
                
                
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

        private FrogsAndToadsMove(int source, int target, FrogsAndToadsPosition position)
        {
            _position = position;
            Source = source;
            Target = target;
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
