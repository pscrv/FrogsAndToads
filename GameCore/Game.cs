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
            while (! _gameIsOver)
            {
                PlayRound();
            }
        }

        public void PlayRound()
        {
            _play(_leftPlayer);
            if (GameIsOver)
                return;

            _play(_rightPlayer);
        }
        #endregion



        #region internal methods
        internal void PlayLeftMove()
        {
            _play(_leftPlayer);
        }

        internal void PlayRightMove()
        {
            _play(_rightPlayer);
        }
        #endregion


        #region private methods
        private void _play(
            GamePlayer<T> activePlayer)
        {
            bool isLeftPlay = activePlayer == _leftPlayer;

            IEnumerable<T> options =
                isLeftPlay ?
                GetLeftOptions(Position) :
                GetRightOptions(Position);

            Maybe<T> result = activePlayer.Play(options);
            if (!result.HasValue)
            {
                _gameIsOver = true;
                _winner = isLeftPlay ? _rightPlayer : _leftPlayer;
                return;
            }
            
            Position = result.Value;
        }
        #endregion
    }    
}
