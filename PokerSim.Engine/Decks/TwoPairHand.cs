using System.Collections.Generic;
using System.Linq;

namespace PokerSim.Engine.Decks
{
    public class TwoPairHandBuilder : IHandBuilder
    {
        public IHand BuildHand(IEnumerable<Card> cards)
        {
            var tempList = cards.ToList();
            //Todo how to finger this one out!
            var pairGroup = tempList.GroupBy(x => x.Value)
                .Where(g => g.Count() == 2)
                .OrderByDescending(x => x.Key)
                .Take(2)
                .ToList();

            if (pairGroup.Count() != 2)
            {
                //Invalid pair...
                return new TwoPairHand(new List<Card>(), new List<Card>(), null);
            }

            pairGroup = pairGroup.OrderByDescending(x => x.Key).ToList();
            tempList.RemoveAll(x => x.Value == pairGroup[0].Key);
            tempList.RemoveAll(x => x.Value == pairGroup[1].Key);
            return new TwoPairHand(cards.Where(x => x.Value == pairGroup[0].Key),
                cards.Where(x => x.Value == pairGroup[1].Key),
                tempList.OrderByDescending(x => x.Value).FirstOrDefault());
        }

        public bool ContainsHand(IEnumerable<Card> cards)
        {
            return cards.GroupBy(x => x.Value).Where(g => g.Count() == 2).Count() >= 2;
        }
    }

    public class TwoPairHand : BaseHand
    {
        public TwoPairHand(IEnumerable<Card> highPair, IEnumerable<Card> lowPair, Card card)
            : base(HandType.TwoPair, highPair.ToList().Concat(lowPair.ToList()).Concat(new List<Card>() { card }))
        {
            RawScore = 0;
            //Max possible score would be: A,A,K,K,Q
            MaxPossibleScore = 14 * 100000 * 2 + 13 * 10000 * 2 + 12 * 1000;

            long scoreFactor = 100000;
            foreach(var pairCard in highPair)
            {
                RawScore += scoreFactor * pairCard.Value;
            }
            scoreFactor /= 10;

            foreach (var pairCard in lowPair)
            {
                RawScore += scoreFactor * pairCard.Value;
            }
            scoreFactor /= 10;

            if(card != null)
            {
                RawScore += scoreFactor * card.Value;
            }
        }

        public override bool IsValid => IsTwoPairHand(Cards) && Cards.Count() == 5;

        private static bool IsTwoPairHand(IEnumerable<Card> cards)
        {
            return cards.GroupBy(x => x.Value).Where(g => g.Count() == 2).Count() >= 2;
        }
    }
}
