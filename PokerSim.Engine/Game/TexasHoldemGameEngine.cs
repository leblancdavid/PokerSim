﻿using PokerSim.Engine.Decks;
using PokerSim.Engine.Players;
using System.Collections.Generic;
using System.Linq;

namespace PokerSim.Engine.Game
{
    public class TexasHoldemGameEngine : IGameEngine
    {
        public IHandBuilder HandBuilder { get; private set; }
        public Deck Deck { get; private set; }
        internal PotState CurrentPot { get; private set; }

        private List<IPlayerState> _players = new List<IPlayerState>();
        internal IEnumerable<IPlayerState> Players => _players;

        private List<Card> _communityCards = new List<Card>();
        public IEnumerable<Card> CommunityCards => _communityCards;

        public int SmallBlindIndex { get; private set; }

        public int SmallBlindValue { get; private set; }

        public int BigBlindIndex { get; private set; }

        public int BigBlindValue { get; private set; }

        public int CurrentPlayerIndex { get; private set; }

        private int _lastBettingPlayerIndex = 1;
        private int _currentPlayerIndex = 1;

        private int _initialChips = 1000;
        private TexasHoldemStages _currentStage;
        private IGameEventLogger _logger;
        public TexasHoldemGameEngine(IGameEventLogger logger)
        {
            _logger = logger;

            SmallBlindIndex = -1;
            SmallBlindValue = 1;
            BigBlindIndex = 0;
            BigBlindValue = 2;
            _lastBettingPlayerIndex = BigBlindIndex;
            _currentPlayerIndex = 1;
            Deck = new Deck();
            HandBuilder = new HandBuilder();
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
                PlayHand();
                if(_players.Where(x => !x.IsEliminated).Count() == 1)
                {
                    winner = true;
                    return;
                }    
            }
        }

        public void PlayHand()
        {
            CurrentPot = new PotState();
            _communityCards.Clear();
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
                    if(!player.IsEliminated)
                    {
                        player.Deal(Deck.Draw());
                    }
                }
            }
            _currentStage = TexasHoldemStages.PreFlop;
            _logger.Log(_currentStage, CommunityCards);
            //Pre-flop betting
            if (!DoBettingRound())
            {
                _logger.Log(ResolveHand());
                return;
            }    

            //Flop bets
            for(int i = 0; i < 3; ++i)
            {
                _communityCards.Add(Deck.Draw());
            }

            _currentStage = TexasHoldemStages.Flop;
            _logger.Log(_currentStage, CommunityCards);
            if (!DoBettingRound())
            {
                _logger.Log(ResolveHand());
                return;
            }

            //Turn bets
            _communityCards.Add(Deck.Draw());
            _currentStage = TexasHoldemStages.Turn;
            _logger.Log(_currentStage, CommunityCards);
            if (!DoBettingRound())
            {
                _logger.Log(ResolveHand());
                return;
            }

            //River bets
            _communityCards.Add(Deck.Draw());
            _currentStage = TexasHoldemStages.River;
            _logger.Log(_currentStage, CommunityCards);
            DoBettingRound();
            _logger.Log(ResolveHand());
        }

        private HandResult ResolveHand()
        {
            var remainingPlayers = _players.Where(x => !x.HasFolded);
            if(remainingPlayers.Count() == 1)
            {
                var player = remainingPlayers.FirstOrDefault();
                return new HandResult(new List<PlayerHandResult>()
                {
                    new PlayerHandResult(
                        player.Player, 
                        HandBuilder.BuildHand(player.Cards.Concat(CommunityCards)),
                        CurrentPot.PayoutPlayer(player))
                });
            }

            var playerResults = new List<PlayerHandResult>();
            foreach(var player in remainingPlayers)
            {
                playerResults.Add(new PlayerHandResult(player.Player, HandBuilder.BuildHand(player.Cards.ToList().Concat(CommunityCards))));
                player.Fold();
            }

            playerResults = playerResults.OrderByDescending(x => x.Hand).ToList();
            foreach (var result in playerResults)
            {
                result.Winnings = CurrentPot.PayoutPlayer(_players.FirstOrDefault(x => x.Player.Id == result.Player.Id));
            }

            return new HandResult(playerResults);
        }

        private bool DoBettingRound()
        {
            bool everyoneBet = false;
            while (!CurrentPot.AreAllBetsIn || !everyoneBet)
            {
                var player = _players[_currentPlayerIndex];
                if (player.IsEliminated || player.HasFolded)
                {
                    NextPlayer();
                    if (_currentPlayerIndex == _lastBettingPlayerIndex)
                    {
                        everyoneBet = true;
                    }
                    continue;
                }

                var state = GetCurrentPlayerTurnState(player);
                var result = player.Player.TakeTurn(state);
                
                _logger.Log(result);
                
                if (result.Decision == TurnDecisionType.Fold)
                {
                    player.Fold();
                    CurrentPot.PlayerFold(player);
                    if (AllPlayersFolded())
                        return false;
                    
                }
                else if(result.Decision == TurnDecisionType.CheckOrCall)
                {
                    CurrentPot.PlayerCallOrCheck(player);
                }
                else
                {
                    _lastBettingPlayerIndex = _currentPlayerIndex;
                    CurrentPot.PlayerRaise(player, result.RaiseAmount);
                }

                NextPlayer();
                if (_currentPlayerIndex == _lastBettingPlayerIndex)
                {
                    everyoneBet = true;
                }

            }

            return true;
        }

        private void NextPlayer()
        {
            _currentPlayerIndex++;
            if (_currentPlayerIndex >= _players.Count)
            {
                _currentPlayerIndex = 0;
            }
        }

        private bool AllPlayersFolded()
        {
            return _players.Count(x => !x.HasFolded) <= 1;
        }

        private IPlayerTurnState GetCurrentPlayerTurnState(IPlayerState currentPlayer)
        {
            return new PlayerTurnState(CommunityCards,
                currentPlayer.Cards,
                _currentStage,
                CurrentPot.ToCallAmount(currentPlayer),
                CurrentPot.TotalPotSize,
                BigBlindValue,
                currentPlayer.ChipCount,
                _players.Count(x => !x.IsEliminated && !x.HasFolded));
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

            _lastBettingPlayerIndex = BigBlindIndex;

            _currentPlayerIndex = _lastBettingPlayerIndex + 1;
            if (_currentPlayerIndex >= _players.Count)
            {
                _currentPlayerIndex = 0;
            }
        }
    }
}
