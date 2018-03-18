using System;
using System.Collections.Generic;
using System.Linq;
using PokerSim.Engine.Decks;

namespace PokerSim.Engine.Players
{
    public class TexasHoldemDealer
    {
        private Deck _deck;
        private List<TexasHoldemPlayer> _players;
        private List<TexasHoldemPlayerState> _playerStates;
        private List<Card> _communityCards;
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
            int currentPlayer = _startingPlayerIndex;
            var bets = new int[_players.Count];
            bets[_bigBlindIndex] = _bigBlindValue;
            bets[_smallBlindIndex] = _smallBlindValue;
            while(!CheckBettingDone(bets))
            {
                int currentBet = bets.Max();
                int currentPot = bets.Sum();
                var turnResult = _players[currentPlayer].TakeTurn(
                    new TexasHoldemPlayerTurnState(
                        _playerStates[currentPlayer].Cards.ToList(),
                        _communityCards,
                        currentBet,
                        currentPot,
                        _playerStates[currentPlayer].ChipCount
                    ));
                
                if(turnResult.Bet == 0 || 
                    (turnResult.Bet + bets[currentPlayer] < _currentBet &&
                     turnResult.Bet < _playerStates[currentPlayer].ChipCount))
                {
                    _playerStates[currentPlayer].Fold();
                }
                else if(turnResult.Bet + bets[currentPlayer] < _currentBet &&
                     turnResult.Bet == _playerStates[currentPlayer].ChipCount)
                {
                    //the player is all in, which will result in a split pot
                }
                else
                {
                    _playerStates[currentPlayer].RemoveChips(turnResult.Bet);
                    bets[currentPlayer] += turnResult.Bet;
                }

                currentPlayer++;
                if(currentPlayer >= _playerStates.Count)
                {
                    currentPlayer = 0;
                }
            }
        }

        private bool CheckBettingDone(int[] bets)
        {
            int currentMaxBet = bets.Max();
            for(int i = 0; i < _playerStates.Count; ++i)
            {
                if(!_playerStates[i].HasFolded && bets[i] != currentMaxBet)
                {
                    return false;
                }
            }
            return true;
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