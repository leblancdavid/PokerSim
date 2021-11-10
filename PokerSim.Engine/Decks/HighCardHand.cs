using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSim.Engine.Decks
{
    public class HighCardHandBuilder : IHandBuilder<HighCardHand>
    {
        public HighCardHand BuildHand(IEnumerable<Card> cards)
        {
            return new HighCardHand(cards.OrderByDescending(x => x.Value).Take(5));
        }

        public bool ContainsHand(IEnumerable<Card> cards)
        {
            return cards.Distinct().Count() == cards.Count();
        }
    }

    public class HighCardHand : BaseHand
    {
        public HighCardHand(IEnumerable<Card> cards) 
            : base(HandType.HighCard, cards)
        {
            Score = 0;
            long scoreFactor = 100000;
            foreach(var card in Cards)
            {
                Score += scoreFactor * card.Value;
                scoreFactor /= 10;
            }
        }

        public override bool IsValid => IsHighCardHand(Cards) && Cards.Count() == 5;

        public static bool IsHighCardHand(IEnumerable<Card> cards)
        {
            return cards.Distinct().Count() == cards.Count();
        }
    }
}
