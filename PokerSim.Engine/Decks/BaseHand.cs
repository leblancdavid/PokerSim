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
            if (other == null)
                return 1;

            if(HandType != other.HandType)
            {
                return (int)HandType > (int)other.HandType ? 1 : -1;
            }

            if (Score > other.Score)
                return 1;

            if (Score == other.Score)
                return 0;

            return -1;
        }

        public override string ToString()
        {
            string output = "";
            foreach(var card in Cards)
            {
                output += card.ShortName;
            }
            return output;
        }
    }
}
