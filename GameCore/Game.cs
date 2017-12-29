using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Utilities;

namespace GameCore
{
    public abstract class Game<T> where T : GamePosition
    {
        #region absract
        public abstract IEnumerable<T> GetLeftMoves(T position);
        #endregion


        #region protected and private members
        protected T _position;
        protected GamePlayer<T> _leftPlayer;
        protected GamePlayer<T> _rightPlayer;
        protected List<T> _positionHistory;

        private bool _gameIsOver;
        private GamePlayer<T> _winner;
        #endregion


        #region public properties
        public T Position
        {
            get => _position;
            private set
            {
                _position = value;
                _positionHistory.Add(value);
            }
        }

        public ReadOnlyCollection<T> History => _positionHistory.AsReadOnly();
        public GamePlayer<T> Winner => _winner;
        public bool GameIsOver => _gameIsOver;
        #endregion


        #region construction
        public Game(
            GamePlayer<T> leftPlayer, 
            GamePlayer<T> rightPlayer, 
            T initialPosition)
        {
            _leftPlayer = leftPlayer;
            _rightPlayer = rightPlayer;
            _gameIsOver = false;
            _position = initialPosition;
            _positionHistory = new List<T> { _position };
        }
        #endregion


        #region public methods
        public void Play()
        {
            while (! _gameIsOver)
            {
                PlayRound();
            }
        }

        public void PlayRound()
        {
            PlayLeftMove();
            if (GameIsOver)
                return;

            PlayRightMove();
        }

        public void PlayLeftMove()
        {
            _play(_leftPlayer, _rightPlayer, true);
        }

        public void PlayRightMove()
        {
            _play(_rightPlayer, _leftPlayer, false);
        }
        #endregion


        #region private methods
        private void _play(
            GamePlayer<T> activePlayer, 
            GamePlayer<T> inactivePlayer, 
            bool isLeftPlay)
        {
            IEnumerable<T> options =
                GetLeftMoves(_reverseIfRightPlay(Position, isLeftPlay));

            AttemptPlay<T> result = activePlayer.Play(options);
            if (result == AttemptPlay<T>.Failure)
            {
                _gameIsOver = true;
                _winner = inactivePlayer;
                return;
            }

            Position = _reverseIfRightPlay(result.Value, isLeftPlay);
        }

        T _reverseIfRightPlay(T position, bool isleftplay)
        {
            return
                isleftplay
                ? position
                : (T)position.Reverse;
        }
        #endregion
    }    
}
