using PokerSim.Engine.Decks;
using PokerSim.Engine.Players;
using System.Collections.Generic;

namespace PokerSim.Engine.Game
{
    internal class GameEngine : IGameEngine
    {
        public Deck Deck { get; private set; }
        public PotState CurrentPot { get; private set; }

        private List<IPlayerState> _players = new List<IPlayerState>();
        public IEnumerable<IPlayerState> Players => _players;

        private List<Card> _communityCards = new List<Card>();
        public IEnumerable<Card> CommunityCards => _communityCards;

        public int SmallBlindIndex { get; private set; }

        public int SmallBlindValue { get; private set; }

        public int BigBlindIndex { get; private set; }

        public int BigBlindValue { get; private set; }

        public int CurrentPlayerIndex { get; private set; }

        private int _startingPlayerIndex = 1;
        private int _currentPlayerIndex = 1;

        private int _initialChips = 1000;
        public GameEngine()
        {
            SmallBlindIndex = -1;
            SmallBlindValue = 1;
            BigBlindIndex = 0;
            BigBlindValue = 2;
            _startingPlayerIndex = 1;
            _currentPlayerIndex = 1;
            Deck = new Deck();
        }

        public void AddPlayer(IPlayer player)
        {
            _players.Add(new PlayerState(player, _initialChips));
        }

        public void Play()
        {
            bool winner = false;
            while (!winner)
            {

            }
        }

        public void PlayHand()
        {
            CurrentPot = new PotState();
            Deck = new Deck();
            Deck.Shuffle();
            UpdateBlinds();

            CurrentPot.AddToPot(_players[SmallBlindIndex], SmallBlindValue);
            CurrentPot.AddToPot(_players[BigBlindIndex], BigBlindValue);

            //Each player gets 2 cards
            for (int i = 0; i < 2; ++i)
            {
                foreach (var player in _players)
                {
                    player.Deal(Deck.Draw());
                }
            }


        }

        private void DoBettingRound()
        {
            int currentPlayerIndex = _startingPlayerIndex;

            while (!CurrentPot.AreAllBetsIn)
            {
                currentPlayerIndex++;
                if (currentPlayerIndex >= _players.Count)
                {
                    currentPlayerIndex = 0;
                }

                var player = _players[currentPlayerIndex];
                var amountToCall = CurrentPot.ToCallAmount(player);
            }

        }


        private void UpdateBlinds()
        {
            SmallBlindIndex++;
            if (SmallBlindIndex >= _players.Count)
            {
                SmallBlindIndex = 0;
            }
            BigBlindIndex = SmallBlindIndex + 1;
            if (BigBlindIndex >= _players.Count)
            {
                BigBlindIndex = 0;
            }
            _startingPlayerIndex = BigBlindIndex + 1;
            if (_startingPlayerIndex >= _players.Count)
            {
                _startingPlayerIndex = 0;
            }
            _currentPlayerIndex = _startingPlayerIndex;
        }
    }
}
