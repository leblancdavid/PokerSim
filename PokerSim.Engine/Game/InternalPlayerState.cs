using PokerSim.Engine.Decks;
using PokerSim.Engine.Players;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PokerSim.Engine.Game
{
    internal sealed class InternalPlayerState : IPlayerState
    {
        public InternalPlayerState(IPlayer player, int initialChipCount)
        {
            Player = player;
            ChipCount = initialChipCount;
        }

        public IPlayer Player { get; private set; }

        public int ChipCount { get; set; }
        public int PlayerPotSize { get; set; }

        private List<Card> _cards = new List<Card>();
        public IEnumerable<Card> Cards => _cards;

        public bool HasFolded => !_cards.Any();

        public bool IsEliminated => ChipCount <= 0 && !IsAllIn;

        public bool IsAllIn { get; set; }

        public int NumberRaises { get; set; }
        public int LastAmountRaised { get; set; }

        public void Deal(Card card)
        {
            _cards.Add(card);
        }

        public void Fold()
        {
            _cards.Clear();
            IsAllIn = false;
            NumberRaises = 0;
            LastAmountRaised = 0;
        }

        public void AddToPot(int amount)
        {
            if (amount >= ChipCount)
            {
                IsAllIn = true;
                ChipCount = 0;
                PlayerPotSize += ChipCount;
            }
            else
            {
                IsAllIn = false;
                ChipCount -= amount;
                PlayerPotSize += amount;
            }
        }

        public void CallOrCheck(PotState currentPot)
        {
            var toCall = currentPot.ToCallAmount(Player.Id);
            if (toCall > 0)
            {
                AddToPot(toCall);
            }
        }

        public void Raise(PotState currentPot, int amount)
        {
            var raise = currentPot.ToCallAmount(Player.Id) + amount;
            if (raise > 0)
            {
                NumberRaises++;
                LastAmountRaised = amount;
                AddToPot(raise);
            }
        }
    }
}
