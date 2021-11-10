using System.Collections.Generic;

namespace PokerSim.Engine.Decks
{
    public interface IHandBuilder
    {
        IHand BuildHand(IEnumerable<Card> cards);
        bool ContainsHand(IEnumerable<Card> cards);
    }
}
