using PokerSim.Engine.Decks;
using PokerSim.Engine.Players;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PokerSim.Engine.Game
{
    internal sealed class PlayerState : IPlayerState
    {
        public PlayerState(IPlayer player, int initialChipCount)
        {
            Player = player;
            ChipCount = initialChipCount;
        }

        public IPlayer Player { get; private set; }

        public int ChipCount { get; set; }

        private List<Card> _cards = new List<Card>();
        public IEnumerable<Card> Cards => _cards;

        public bool HasFolded => !_cards.Any();

        public bool IsEliminated => ChipCount <= 0;

        public void Deal(Card card)
        {
            _cards.Add(card);
        }

        public void Fold()
        {
            _cards.Clear();
        }
    }
}
