using System.Collections.Generic;
using PokerSim.Engine.Decks;

namespace PokerSim.Engine.Players
{
    public class TexasHoldemPlayerState
    {
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

        public TexasHoldemPlayerState(int initialChipCount)
        {
            _chipCount = initialChipCount;
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
    }
}