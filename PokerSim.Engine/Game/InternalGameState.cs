using PokerSim.Engine.Decks;
using PokerSim.Engine.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSim.Engine.Game
{
    internal class InternalGameState : IGameState
    {
        internal IHandBuilder HandBuilder { get; private set; }
        internal Deck Deck { get; private set; }
        internal PotState CurrentPot { get; private set; }
        private List<Card> _communityCards = new List<Card>();
        public IEnumerable<Card> CommunityCards => _communityCards;

        private List<InternalPlayerState> _players = new List<InternalPlayerState>();
        public IEnumerable<IPlayerState> Players => _players;
        internal IEnumerable<InternalPlayerState> InternalPlayers => _players;

        public int SmallBlindIndex { get; private set; }

        public int SmallBlindValue { get; private set; }

        public int BigBlindIndex { get; private set; }

        public int BigBlindValue { get; private set; }

        public int LastBettingPlayerIndex { get; private set; }
        public int CurrentPlayerIndex { get; private set; }
        public IEnumerable<Card> CurrentPlayerCards => _players[CurrentPlayerIndex].Cards;
        public int CurrentPlayerChipCount => _players[CurrentPlayerIndex].ChipCount;

        public int TotalPotSize => CurrentPot.TotalPotSize;
        public int CurrentBetToCall => CurrentPot.ToCallAmount(_players[CurrentPlayerIndex].Player.Id);
        public int TotalNumberRaises => _players.Sum(x => x.NumberRaises);
        public TexasHoldemStages CurrentStage { get; private set; }

        public IPlayerState CurrentPlayerState => _players[CurrentPlayerIndex];

        public IHand BuildCurrentPlayerHand() => HandBuilder.BuildHand(CurrentPlayerCards.ToList().Concat(CommunityCards.ToList()));
        public IEnumerable<IPlayerState> GetPlayersInHand() => _players.Where(x => !x.IsEliminated && !x.HasFolded);

        private IGameEventLogger _logger;
        internal InternalGameState(IGameEventLogger logger)
        {
            _logger = logger;
            HandBuilder = new HandBuilder();
            SmallBlindIndex = -1;
            SmallBlindValue = 1;
            BigBlindIndex = 0;
            BigBlindValue = 2;
            LastBettingPlayerIndex = BigBlindIndex;
            CurrentPlayerIndex = 1;
            Deck = new Deck();
        }

        public void AddPlayer(InternalPlayerState player)
        {
            _players.Add(player);
        }

        public HandResult PlayHand()
        {
            CurrentPot = new PotState(_players);
            _communityCards.Clear();
            Deck = new Deck();
            Deck.Shuffle();
            UpdateBlinds();

            //Each player gets 2 cards
            for (int i = 0; i < 2; ++i)
            {
                foreach (var player in _players)
                {
                    if (!player.IsEliminated)
                    {
                        player.Deal(Deck.Draw());
                    }
                }
            }

            _players[SmallBlindIndex].AddToPot(SmallBlindValue);
            _players[BigBlindIndex].AddToPot(BigBlindValue);

            CurrentStage = TexasHoldemStages.PreFlop;
            _logger?.Log(CurrentStage, CommunityCards);
            //Pre-flop betting
            if (!DoBettingRound())
            {
                return ResolveHand();
            }

            //Flop bets
            for (int i = 0; i < 3; ++i)
            {
                _communityCards.Add(Deck.Draw());
            }

            CurrentStage = TexasHoldemStages.Flop;
            _logger?.Log(CurrentStage, CommunityCards);
            if (!DoBettingRound())
            {
                return ResolveHand();
            }

            //Turn bets
            _communityCards.Add(Deck.Draw());
            CurrentStage = TexasHoldemStages.Turn;
            _logger?.Log(CurrentStage, CommunityCards);
            if (!DoBettingRound())
            {
                return ResolveHand();
            }

            //River bets
            _communityCards.Add(Deck.Draw());
            CurrentStage = TexasHoldemStages.River;
            _logger?.Log(CurrentStage, CommunityCards);
            DoBettingRound();

            return ResolveHand();
        }

        private HandResult ResolveHand()
        {
            var remainingPlayers = _players.Where(x => !x.HasFolded);
            if (remainingPlayers.Count() == 1)
            {
                var player = remainingPlayers.FirstOrDefault();
                var handResult = new HandResult(new List<PlayerHandResult>()
                {
                    new PlayerHandResult(
                        player.Player,
                        HandBuilder.BuildHand(player.Cards.ToList().Concat(CommunityCards)),
                        CurrentPot.PayoutPlayer(player.Player.Id))
                });

                player.Fold();
                player.IsAllIn = false;
                return handResult;
            }

            var playerResults = remainingPlayers.Select(x =>
                new PlayerHandResult(x.Player,
                    HandBuilder.BuildHand(x.Cards.ToList().Concat(CommunityCards))))
                .OrderByDescending(x => x.Hand)
                .ToList();

            CurrentPot.PayoutPlayers(playerResults);

            //Update player status at the end of the hand
            foreach (var player in remainingPlayers)
            {
                player.Fold();
                player.IsAllIn = false;
            }

            return new HandResult(playerResults);
        }

        private bool DoBettingRound()
        {
            bool everyoneBet = false;
            int remainingPlayers = _players.Count(x => !x.HasFolded);
            if (remainingPlayers == 1)
                return false;

            //If everyone remaining in the hand is all in, no reason to do any betting.
            int allInPlayers = _players.Count(x => x.IsAllIn);
            if (remainingPlayers - allInPlayers <= 1)
                return true;

            int turnCount = 0;
            while (!everyoneBet || turnCount < remainingPlayers)
            {
                var player = _players[CurrentPlayerIndex];
                if (player.IsEliminated || player.HasFolded)
                {
                    NextPlayer();
                    if (CurrentPlayerIndex == LastBettingPlayerIndex)
                    {
                        everyoneBet = true;
                    }
                    continue;
                }

                var result = player.Player.TakeTurn(this);

                _logger?.Log(result);

                if (result.Decision == TurnDecisionType.Fold)
                {
                    player.Fold();
                    if (AllPlayersFolded())
                        return false;

                }
                else if (result.Decision == TurnDecisionType.CheckOrCall)
                {
                    player.CallOrCheck(CurrentPot);
                }
                else
                {
                    if (result.RaiseAmount + CurrentBetToCall > CurrentPlayerChipCount)
                    {
                        player.CallOrCheck(CurrentPot);
                    }
                    else
                    {
                        LastBettingPlayerIndex = CurrentPlayerIndex;
                        everyoneBet = false;
                        player.Raise(CurrentPot, result.RaiseAmount);
                    }

                }

                NextPlayer();
                if (CurrentPlayerIndex == LastBettingPlayerIndex)
                {
                    everyoneBet = true;
                }
                turnCount++;
            }

            return true;
        }

        internal void Reset(int initialChips)
        {
            ShufflePlayers();
            foreach (var player in _players)
            {
                player.ChipCount = initialChips;
            }

        }

        private void NextPlayer()
        {
            CurrentPlayerIndex++;
            if (CurrentPlayerIndex >= _players.Count)
            {
                CurrentPlayerIndex = 0;
            }
        }

        private bool AllPlayersFolded()
        {
            return _players.Count(x => !x.HasFolded) <= 1;
        }

        private void UpdateBlinds()
        {
            do
            {
                SmallBlindIndex++;
                if (SmallBlindIndex >= _players.Count)
                {
                    SmallBlindIndex = 0;
                }
            }
            while (_players[SmallBlindIndex].IsEliminated);

            BigBlindIndex = SmallBlindIndex;
            do
            {
                BigBlindIndex++;
                if (BigBlindIndex >= _players.Count)
                {
                    BigBlindIndex = 0;
                }
            }
            while (_players[BigBlindIndex].IsEliminated);

            LastBettingPlayerIndex = BigBlindIndex;

            do
            {
                CurrentPlayerIndex++;
                if (CurrentPlayerIndex >= _players.Count)
                {
                    CurrentPlayerIndex = 0;
                }
            }
            while (_players[CurrentPlayerIndex].IsEliminated);
        }

        public void ShufflePlayers()
        {
            Random rng = new Random();
            int n = _players.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                var value = _players[k];
                _players[k] = _players[n];
                _players[n] = value;
            }
        }

    }
}
