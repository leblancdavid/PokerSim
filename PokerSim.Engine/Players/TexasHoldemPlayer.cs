using System;
using System.Collections.Generic;
using PokerSim.Engine.Decks;
using PokerSim.Engine.Game;

namespace PokerSim.Engine.Players
{
    public class TexasHoldemPlayer
    {
        public Guid PlayerId { get; private set; }
        private List<Card> _cards = new List<Card>();
        public IEnumerable<Card> Cards => _cards;

        private int _chipCount;
        public int ChipCount
        {
            get
            {
                return _chipCount;
            }
        }

        public bool HasFolded
        {
            get
            {
                return _cards.Count == 0;
            }

        }

        public TexasHoldemPlayer(int initialChipCount)
        {
            _chipCount = initialChipCount;
            PlayerId = Guid.NewGuid();
        }

        public void Deal(Card card)
        {
            _cards.Add(card);
        }

        public void Fold()
        {
            _cards.Clear();
        }

        public void AddChips(int chips)
        {
            _chipCount += chips;
        }

        public void RemoveChips(int chips)
        {
            _chipCount -= chips;
        }

        public PlayerTurn TakeTurn(TexasHoldemPlayerTurnState state)
        {
            return new PlayerTurn(0);
        }
    }
}