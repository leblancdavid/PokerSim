using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSim.Engine.Decks
{
    public abstract class BaseHandBuilder : IHandBuilder
    {
        public abstract IHand BuildHand(IEnumerable<Card> cards);
        public IHand BuildHand(IEnumerable<Card> cards, IEnumerable<Card> communityCards)
        {
            return BuildHand(cards.ToList().Concat(communityCards.ToList()));
        }

        public abstract bool ContainsHand(IEnumerable<Card> cards);

        public bool ContainsHand(IEnumerable<Card> cards, IEnumerable<Card> communityCards)
        {
            return ContainsHand(cards.ToList().Concat(communityCards.ToList()));
        }
    }
}
