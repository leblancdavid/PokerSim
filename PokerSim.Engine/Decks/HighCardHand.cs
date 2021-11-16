using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSim.Engine.Decks
{
    public class HighCardHandBuilder : BaseHandBuilder
    {
        public override IHand BuildHand(IEnumerable<Card> cards)
        {
            return new HighCardHand(cards.OrderByDescending(x => x.Value).Take(5));
        }

        public override bool ContainsHand(IEnumerable<Card> cards)
        {
            return cards.Distinct().Count() == cards.Count();
        }
    }

    public class HighCardHand : BaseHand
    {
        public HighCardHand(IEnumerable<Card> cards) 
            : base(HandType.HighCard, cards)
        {
            RawScore = 0;
            //Max possible score would be: A,K,Q,J,9
            MaxPossibleScore = 14 * 100000 + 13 * 10000 + 12 * 1000 + 11 * 100 + 9 * 10;
            long scoreFactor = 100000;
            foreach(var card in Cards)
            {
                RawScore += scoreFactor * card.Value;
                scoreFactor /= 10;
            }
        }

        public override bool IsValid => IsHighCardHand(Cards) && Cards.Count() == 5;

        private static bool IsHighCardHand(IEnumerable<Card> cards)
        {
            return cards.Distinct().Count() == cards.Count();
        }
    }
}
