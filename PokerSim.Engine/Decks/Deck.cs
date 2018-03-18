using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace PokerSim.Engine.Decks
{
    public class Deck
    {
        private List<Card> _cards = new List<Card>();
        public IEnumerable<Card> Cards => _cards;
        public bool IsEmpty
        {
            get
            {
                if(_cards.Count == 0)
                {
                    return true;
                }
                return false;
            }
        }

        public Deck()
        {
            Reset();
        }

        public void Shuffle()
        {
            _cards.Shuffle();
        }

        public Card Draw()
        {
            return _cards.Draw();
        }

        public void Reset()
        {
            _cards.Clear();
            var suits = Enum.GetValues(typeof(CardSuit)).Cast<CardSuit>();
            int id = 1;
            foreach(var suit in suits)
            {
                for(int i = 1; i <= 13; ++i)
                {
                    _cards.Add(new Card(id, suit, i));
                    ++id;
                }
            }
        }
    }
}