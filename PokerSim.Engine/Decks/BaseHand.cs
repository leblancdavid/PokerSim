using System;
using System.Collections.Generic;

namespace PokerSim.Engine.Decks
{
    public class BaseHand : IHand
    {
        public long Score { get; protected set; }
        public HandType HandType { get; protected set; }
        public IEnumerable<Card> Cards { get; protected set; }

        public virtual bool IsValid => true;

        public BaseHand(HandType handType, IEnumerable<Card> cards)
        {
            Score = 0;
            Cards = cards;
            HandType = handType;
        }


        public int CompareTo(IHand other)
        {
            throw new NotImplementedException();
        }
    }
}
