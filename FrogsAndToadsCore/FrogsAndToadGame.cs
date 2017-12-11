using System.Collections.Generic;
using System.Linq;

namespace FrogsAndToadsCore
{
    public class FrogsAndToadGame
    {
        #region private
        private GamePosition _position;
        private List<GamePosition> _positionHistory;
        private Player _toadPlayer;
        private Player _frogPlayer;
        private Player _activePlayer;
        private Player _winner;
        #endregion



        #region public properties
        public string CurrentPosition => _position.ToString();
        public List<string> History => _positionHistory.Select(x => x.ToString()).ToList();
        public Player Winner => _winner;
        public bool GameIsRunning => _winner == null;

        public Player ActivePlayer => _activePlayer;

        public Player InactivePlayer => 
            ActivePlayer == _toadPlayer
            ? _frogPlayer
            : _toadPlayer;
        #endregion


        #region construction
        public FrogsAndToadGame()
            : this("", new TrivialChooser(), new TrivialChooser())
        { }


        public FrogsAndToadGame(string position, PlayChooser toadChooser, PlayChooser frogChooser)
        {
            _position = position == "" 
                ? GamePosition.MakeInitialPosition() 
                : new GamePosition(position);
            _positionHistory = new List<GamePosition> { _position };
            
            
            //_toadPlayer = toadPlayer ?? new TrivialChooser();
            //_frogPlayer = frogPlayer ?? new TrivialChooser();
            _toadPlayer = new Player(toadChooser, Toad.Instance);
            _frogPlayer = new Player(frogChooser, Frog.Instance);


            _activePlayer = _toadPlayer;
            _winner = null;
        }        
        #endregion
        

        public void Play()
        {
            PlayChoice choice;
            while (true)
            {
                choice = _toadPlayer.ChoosePlay(_position);
                if (choice.NoChoiceMade)
                {
                    _winner = _frogPlayer;
                    return;
                }
                _position = _position.MovePiece(choice.Choice);
                _positionHistory.Add(_position);

                choice = _frogPlayer.ChoosePlay(_position);
                if (choice.NoChoiceMade)
                {
                    _winner = _toadPlayer;
                    return;
                }
                _position = _position.MovePiece(choice.Choice);
                _positionHistory.Add(_position);           
            }
        }




        public void PlayOneMove()
        {
            if (!GameIsRunning)
                return;

            PlayChoice chosenPlay = ActivePlayer.ChoosePlay(_position);

            if (chosenPlay.NoChoiceMade)
            {
                _winner = InactivePlayer;
                return;
            }

            _position = _position.MovePiece(chosenPlay.Choice);
            _positionHistory.Add(_position);
            _activePlayer = InactivePlayer;
            return;
        }


        public IEnumerable<GamePosition> GameMoves()
        {
            Cycle<Player> players = new Cycle<Player> { _toadPlayer, _frogPlayer };
            
            foreach (Player player in players)
            {
                _activePlayer = player;
                if (_play().NoChoiceMade)
                    yield break;
                yield return _position;
            }            
        }

        private PlayChoice _play()
        {
            PlayChoice choice = _activePlayer.ChoosePlay(_position);

            if (choice.NoChoiceMade)
            {
                _winner = InactivePlayer;
            }
            else
            {
                _position = _position.MovePiece(choice.Choice);
                _positionHistory.Add(_position);
            }

            return choice;
        }

    }
}
