using PokerSim.Engine.Decks;
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

        private int _startingPlayerIndex = 1;
        private int _currentPlayerIndex = 1;

        private int _initialChips = 1000;
        public TexasHoldemGameEngine()
        {
            SmallBlindIndex = -1;
            SmallBlindValue = 1;
            BigBlindIndex = 0;
            BigBlindValue = 2;
            _startingPlayerIndex = 1;
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

            //Pre-flop betting
            if(!DoBettingRound())
            {
                ResolveHand();
                return;
            }    

            //Flop bets
            for(int i = 0; i < 3; ++i)
            {
                _communityCards.Add(Deck.Draw());
            }
            if (!DoBettingRound())
            {
                ResolveHand();
                return;
            }

            //Turn bets
            _communityCards.Add(Deck.Draw());
            if (!DoBettingRound())
            {
                ResolveHand();
                return;
            }

            //River bets
            _communityCards.Add(Deck.Draw());
            DoBettingRound();
            ResolveHand();
        }

        private void ResolveHand()
        {
            var remainingPlayers = _players.Where(x => !x.HasFolded);
            if(remainingPlayers.Count() == 1)
            {
                CurrentPot.PayoutPlayer(remainingPlayers.FirstOrDefault());
                return;
            }

            var hands = new List<(IPlayerState Player, IHand Hand)>();
            foreach(var player in remainingPlayers)
            {
                hands.Add((player, HandBuilder.BuildHand(player.Cards.Concat(CommunityCards))));
            }

            hands = hands.OrderByDescending(x => x.Hand.Score).ToList();
            foreach (var hand in hands)
            {
                CurrentPot.PayoutPlayer(hand.Player);
            }
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
                    if (_currentPlayerIndex == _startingPlayerIndex)
                    {
                        everyoneBet = true;
                    }
                    continue;
                }

                var state = GetCurrentPlayerTurnState(player);
                var result = player.Player.TakeTurn(state);
                if(result.Decision == TurnDecisionType.Fold)
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
                    CurrentPot.PlayerRaise(player, result.RaiseAmount);
                }

                NextPlayer(); 
                if (_currentPlayerIndex == _startingPlayerIndex)
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
            _startingPlayerIndex = BigBlindIndex + 1;
            if (_startingPlayerIndex >= _players.Count)
            {
                _startingPlayerIndex = 0;
            }
            _currentPlayerIndex = _startingPlayerIndex;
        }
    }
}
