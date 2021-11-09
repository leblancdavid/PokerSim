using System.Collections.Generic;

namespace PokerSim.Engine.Decks
{
    public interface IHandScorer
    {
        IHand GetHand(IEnumerable<Card> cards);
    }
}
