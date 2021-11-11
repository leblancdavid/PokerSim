using System.Collections.Generic;
using System.Linq;

namespace PokerSim.Engine.Decks
{
    public class FlushHandBuilder : IHandBuilder
    {
        public IHand BuildHand(IEnumerable<Card> cards)
        {
            var groups = cards.GroupBy(x => x.Suit).FirstOrDefault(x => x.Count() >= 5);
            if(groups == null)
            {
                return new FlushHand(new List<Card>());
            }

            return new FlushHand(cards.Where(x => x.Suit == groups.Key)
                .OrderByDescending(x => x.Value)
                .Take(5));
        }

        public bool ContainsHand(IEnumerable<Card> cards)
        {
            return cards.GroupBy(x => x.Suit).FirstOrDefault(x => x.Count() >= 5) != null;
        }
    }

    public class FlushHand : BaseHand
    {
        public FlushHand(IEnumerable<Card> cards)
            : base(HandType.Flush, cards)
        {
            RawScore = 0;
            //Max possible score would be: A,K,Q,J,9
            MaxPossibleScore = 14 * 100000 + 13 * 10000 + 12 * 1000 + 11 * 100 + 9 * 10;
            long scoreFactor = 100000;
            foreach (var card in cards)
            {
                RawScore += scoreFactor * card.Value;
                scoreFactor /= 10;
            }
        }

        public override bool IsValid => IsFlushHand(Cards) && Cards.Count() == 5;

        private static bool IsFlushHand(IEnumerable<Card> cards)
        {
            return cards.GroupBy(x => x.Suit).FirstOrDefault(x => x.Count() >= 5) != null;
        }
    }
}
