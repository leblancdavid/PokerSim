using System;
using System.Collections.Generic;

namespace PokerSim.Engine.Decks
{
    public class BaseHand : IHand
    {
        public long RawScore { get; protected set; }
        public long MaxPossibleScore { get; protected set; }
        public HandType HandType { get; protected set; }
        public IEnumerable<Card> Cards { get; protected set; }

        public virtual bool IsValid => true;

        public double NormalizedScore => (double)RawScore / (double)MaxPossibleScore;
        public double RelativeScore => (double)NormalizedScore * (double)HandType / 10.0; //there are 10 possible hands

        public BaseHand(HandType handType, IEnumerable<Card> cards)
        {
            RawScore = 0;
            MaxPossibleScore = 1;
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

            if (RawScore > other.RawScore)
                return 1;

            if (RawScore == other.RawScore)
                return 0;

            return -1;
        }

        public override string ToString()
        {
            string output = "";
            foreach(var card in Cards)
            {
                output += card?.ShortName;
            }
            return output;
        }
    }
}
