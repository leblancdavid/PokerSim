using System.Collections.Generic;

namespace PokerSim.Engine.Decks.Statistics
{
    public interface IHandProbabilityCalculator
    {
        HandProbability Calculate(HandType handType, IEnumerable<Card> cards);
        IEnumerable<HandProbability> Calculate(IEnumerable<Card> cards);
    }
}
