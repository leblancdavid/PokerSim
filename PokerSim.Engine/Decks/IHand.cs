using System;
using System.Collections.Generic;

namespace PokerSim.Engine.Decks
{
    public enum HandType
    {
        HighCard = 0,
        Pair = 1,
        TwoPair = 2,
        ThreeOfAKind = 3,
        Straight = 4,
        Flush = 5,
        FullHouse = 6,
        FourOfAKind = 7,
        StraightFlush = 8,
        RoyalFlush = 9
    }
    public interface IHand : IComparable<IHand>
    {
        public long Score { get; }
        public HandType HandType { get; }
        public IEnumerable<Card> Cards { get; }
        public bool IsValid { get; }
    }
}
