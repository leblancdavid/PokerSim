using System.Collections.Generic;

namespace PokerSim.Engine.Decks
{
    public interface IHandBuilder
    {
        IHand BuildHand(IEnumerable<Card> cards);
    }

    public interface IHandBuilder<THandType> where THandType : IHand
    {
        THandType BuildHand(IEnumerable<Card> cards);
        bool ContainsHand(IEnumerable<Card> cards);
    }
}
