using System.Collections.Generic;
using System.Collections.ObjectModel;
using Monads;

namespace GameCore
{
    public abstract class Game<T> where T : GamePosition
    {
        #region absract
        public abstract IEnumerable<T> GetLeftOptions(T position);
        public abstract IEnumerable<T> GetRightOptions(T position);
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
            while (!_gameIsOver)
            {
                PlayRound();
            }
        }

        public void PlayRound()
        {
            PlayLeft();
            if (GameIsOver)
                return;

            PlayRight();
        }
        #endregion




        #region internal methods
        internal void PlayLeft() =>
            _play(_leftPlayer.PlayLeft(GetLeftOptions(Position)), _rightPlayer);

        internal void PlayRight() =>
            _play(_rightPlayer.PlayRight(GetRightOptions(Position)), _leftPlayer);
        #endregion


        #region private methods
        private void _play(Maybe<T> selection, GamePlayer<T> inactivePlayer)
        {
            if (selection is Maybe<T> newPosition
                && newPosition.HasValue)
            {
                Position = newPosition.Value;
                return;
            }

            _gameIsOver = true;
            _winner = inactivePlayer;
        }
        #endregion
    }
}
