using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FrogsAndToadsCore
{
    public class Game
    {
        #region private
        private GamePosition _position;
        private List<GamePosition> _positionHistory;
        private Player _playerA;
        private Player _playerB;
        private PlayerQueue _playerQueue;
        private Player _winner;


        private IEnumerator<Player> _playerEnumerator;
        #endregion

        #region public properties
        public string CurrentPosition => _position.ToString();
        public List<string> History => _positionHistory.Select(x => x.ToString()).ToList();
        public Player Winner => _winner;
        public bool GameIsRunning => _winner == null;
        public Player ActivePlayer => _playerEnumerator.Current;
        #endregion


        #region construction
        public Game()
            : this("", new TrivialPlayer(), new TrivialPlayer())
        { }


        public Game(string position, Player toadPlayer, Player frogPlayer)
        {
            _position = position == "" 
                ? GamePosition.MakeInitialPosition() 
                : new GamePosition(position);
            _positionHistory = new List<GamePosition> { _position };
            _playerA = toadPlayer ?? new TrivialPlayer();
            _playerB = frogPlayer ?? new TrivialPlayer();
            _playerQueue = new PlayerQueue(new List<Player> { _playerA, _playerB });
            _playerEnumerator = _playerQueue.GetEnumerator();
            _winner = null;
        }        
        #endregion
        

        public void Play()
        {
            PlayChoice choice;
            while (true)
            {
                choice = _playerA.ChooseToadPlay(_position);
                if (choice.NoChoiceMade)
                {
                    _winner = _playerB;
                    return;
                }
                _position = _position.MovePiece(choice.Choice);
                _positionHistory.Add(_position);

                choice = _playerB.ChooseFrogPlay(_position);
                if (choice.NoChoiceMade)
                {
                    _winner = _playerA;
                    return;
                }
                _position = _position.MovePiece(choice.Choice);
                _positionHistory.Add(_position);


                //_play(_playerA, _playerB);
                //_play(_playerB, _playerA);                
            }
        }




        public void PlayOneMove()
        {
            _playerEnumerator.MoveNext();

            _play(
                _playerEnumerator.Current,
                _playerEnumerator.Current == _playerA ? _playerB : _playerA);            
        }


        private void _play(Player movingPlayer, Player otherPlayer)
        {
            if (!GameIsRunning)
                return;

            PlayChoice chosenPlay = movingPlayer == _playerA 
                ? movingPlayer.ChooseToadPlay(_position)
                : movingPlayer.ChooseFrogPlay(_position);
            if (chosenPlay.NoChoiceMade)
            {
                _winner = otherPlayer;
                return;
            }

            _position = _position.MovePiece(chosenPlay.Choice);
            _positionHistory.Add(_position);
        }

    }
}
