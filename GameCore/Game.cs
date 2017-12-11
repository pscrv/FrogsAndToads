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
        public abstract IEnumerable<GamePosition> GetRightMoves(GamePosition position);
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
        public GamePosition Position => _position;
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
            while (true)
            {
                _play(_leftPlayer, _rightPlayer, GetLeftMoves(_position));
                if (_gameIsOver)
                    return;

                _play(_rightPlayer, _leftPlayer, GetRightMoves(_position));
                if (_gameIsOver)
                    return;
            }
        }
        #endregion


        #region private methods
        private void _play(
            GamePlayer activePlayer, 
            GamePlayer inactivePlayer, 
            IEnumerable<GamePosition> options)
        {
            Try<GamePosition> result = activePlayer.Play(options);
            if (result == Try<GamePosition>.Failure)
            {
                _gameIsOver = true;
                _winner = inactivePlayer;
                return;
            }
            _position = result.Value;
            _positionHistory.Add(_position);
        }
        #endregion
    }    
}
