using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Utilities;

namespace GameCore
{
    public abstract class Game
    {
        #region absract
        public abstract IEnumerable<GamePosition> GetLeftMoves(GamePosition position);
        #endregion


        #region protected and private members
        protected GamePosition _position;
        protected GamePlayer _leftPlayer;
        protected GamePlayer _rightPlayer;
        protected List<GamePosition> _positionHistory;

        private bool _gameIsOver;
        private GamePlayer _winner;
        #endregion


        #region public properties
        public GamePosition Position
        {
            get => _position;
            private set
            {
                _position = value;
                _positionHistory.Add(value);
            }
        }

        public ReadOnlyCollection<GamePosition> History => _positionHistory.AsReadOnly();
        public GamePlayer Winner => _winner;
        public bool GameIsOver => _gameIsOver;
        #endregion


        #region construction
        public Game(
            GamePlayer leftPlayer, 
            GamePlayer rightPlayer, 
            GamePosition initialPosition)
        {
            _leftPlayer = leftPlayer;
            _rightPlayer = rightPlayer;
            _gameIsOver = false;
            _position = initialPosition;
            _positionHistory = new List<GamePosition> { _position };
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
            GamePlayer activePlayer, 
            GamePlayer inactivePlayer, 
            bool isLeftPlay)
        {
            IEnumerable<GamePosition> options =
                GetLeftMoves(_reverseIfRightPlay(Position, isLeftPlay));

            AttemptPlay result = activePlayer.Play(options);
            if (result == AttemptPlay.Failure)
            {
                _gameIsOver = true;
                _winner = inactivePlayer;
                return;
            }

            Position = _reverseIfRightPlay(result.Value, isLeftPlay);
        }

        GamePosition _reverseIfRightPlay(GamePosition position, bool isleftplay)
        {
            return
                isleftplay
                ? position
                : position.Reverse;
        }
        #endregion
    }    
}
