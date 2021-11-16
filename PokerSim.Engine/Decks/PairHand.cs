using System.Collections.Generic;
using System.Linq;

namespace PokerSim.Engine.Decks
{
    public class PairHandBuilder : BaseHandBuilder
    {
        public override IHand BuildHand(IEnumerable<Card> cards)
        {
            var tempList = cards.ToList();
            //Todo how to finger this one out!
            var pairGroup = tempList.GroupBy(x => x.Value)
                .Where(g => g.Count() == 2)
                .OrderByDescending(x => x.Key)
                .FirstOrDefault();
            if (pairGroup == null)
            {
                //Invalid pair...
                return new PairHand(new List<Card>(), new List<Card>());
            }

            var pair = cards.Where(x => x.Value == pairGroup.Key);
            tempList.RemoveAll(x => x.Value == pairGroup.Key);

            return new PairHand(pair, tempList.OrderByDescending(x => x.Value).Take(3));
        }

        public override bool ContainsHand(IEnumerable<Card> cards)
        {
            return cards.GroupBy(x => x.Value).Where(g => g.Count() == 2).Count() == 1;
        }
    }

    public class PairHand : BaseHand
    {
        public PairHand(IEnumerable<Card> pair, IEnumerable<Card> cards)
            : base(HandType.Pair, pair.Concat(cards))
        {
            RawScore = 0;
            //Max possible score would be: A,A,K,Q,J
            MaxPossibleScore = 14 * 100000 * 2 + 13 * 10000 + 12 * 1000 + 11 * 100;
            long scoreFactor = 100000;
            foreach(var pairCard in pair)
            {
                RawScore += scoreFactor * pairCard.Value;
            }
            scoreFactor /= 10;

            foreach (var card in cards)
            {
                RawScore += scoreFactor * card.Value;
                scoreFactor /= 10;
            }
        }

        public override bool IsValid => IsPairHand(Cards) && Cards.Count() == 5;

        private static bool IsPairHand(IEnumerable<Card> cards)
        {
            return cards.GroupBy(x => x.Value).Where(g => g.Count() == 2).Count() == 1;
        }
    }
}
