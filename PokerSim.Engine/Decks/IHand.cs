using System;
using System.Collections.Generic;

namespace PokerSim.Engine.Decks
{
    public enum HandType
    {
        HighCard = 1,
        Pair = 2,
        TwoPair = 3,
        ThreeOfAKind = 4,
        Straight = 5,
        Flush = 6,
        FullHouse = 7,
        FourOfAKind = 8,
        StraightFlush = 9,
        RoyalFlush = 10
    }
    public interface IHand : IComparable<IHand>
    {
        long RawScore { get; }
        long MaxPossibleScore { get; }
        double RelativeScore { get; }
        double NormalizedScore { get; }
        HandType HandType { get; }
        IEnumerable<Card> Cards { get; }
        bool IsValid { get; }
    }
}
