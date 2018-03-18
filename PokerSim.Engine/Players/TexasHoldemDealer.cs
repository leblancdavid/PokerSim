using System;
using System.Collections.Generic;
using PokerSim.Engine.Decks;

namespace PokerSim.Engine.Players
{
    public class TexasHoldemDealer
    {
        private Deck _deck;
        private List<TexasHoldemPlayer> _players;
        private List<TexasHoldemPlayerState> _playerStates;
        private int _smallBlindIndex = -1;
        private int _smallBlindValue = 1;
        private int _bigBlindIndex = 0;
        private int _bigBlindValue = 2;
        private int _currentPlayerIndex = 1;
        private int _startingPlayerIndex = 1;
        private int _currentBet;


        public TexasHoldemDealer()
        {
            _players = new List<TexasHoldemPlayer>();
            _playerStates = new List<TexasHoldemPlayerState>();
        }

        public void AddPlayer(TexasHoldemPlayer player, int initialChips)
        {
            _players.Add(player);
            _playerStates.Add(new TexasHoldemPlayerState(initialChips));
        }

        public void DealNewHand()
        {
            _deck = new Deck();
            _deck.Shuffle();
            MoveBlinds();
            // foreach(var playerState in _playerStates)
            // {
            //     playerState.DiscardHand();
            // }
            _playerStates[_smallBlindIndex].RemoveChips(_smallBlindValue);
            _playerStates[_bigBlindIndex].RemoveChips(_bigBlindValue);
            for(int i = 0; i < 2; ++i)
            {
                foreach(var playerState in _playerStates)
                {
                    playerState.Deal(_deck.Draw());
                }
            }

            _currentBet = _bigBlindValue;
        }

        public void PlayGame()
        {
            bool winner = false;
            while(!winner)
            {

            }
        }

        private void DoBettingRound()
        {
            bool bettingDone = false;
            int currentPlayer = _startingPlayerIndex;
            while(!bettingDone)
            {

            }
        }

        public void UpdateBlinds(int smallBlind, int bigBlind)
        {
            _smallBlindValue = smallBlind;
            _bigBlindValue = bigBlind;
        }

        private void MoveBlinds()
        {
            _smallBlindIndex++;
            if(_smallBlindIndex >= _players.Count)
            {
                _smallBlindIndex = 0;
            }
            _bigBlindIndex = _smallBlindIndex + 1;
            if(_bigBlindIndex >= _players.Count)
            {
                _bigBlindIndex = 0;
            }
            _startingPlayerIndex = _bigBlindIndex + 1;
            if(_startingPlayerIndex >= _players.Count)
            {
                _startingPlayerIndex = 0;
            }
            _currentPlayerIndex = _startingPlayerIndex;
        }


    }
}