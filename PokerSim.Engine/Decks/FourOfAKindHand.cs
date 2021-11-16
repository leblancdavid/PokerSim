using System.Collections.Generic;
using System.Linq;

namespace PokerSim.Engine.Decks
{
    public class FourOfAKindHandBuilder : BaseHandBuilder
    {
        public override IHand BuildHand(IEnumerable<Card> cards)
        {
            var tempList = cards.ToList();
            //Todo how to finger this one out!
            var quadGroup = tempList.GroupBy(x => x.Value)
                .Where(g => g.Count() == 4)
                .OrderByDescending(x => x.Key).FirstOrDefault();
            if (quadGroup == null)
            {
                return new FourOfAKindHand(new List<Card>(), null);
            }

            tempList.RemoveAll(x => x.Value == quadGroup.Key);
            return new FourOfAKindHand(cards.Where(x => x.Value == quadGroup.Key),
                tempList.OrderByDescending(x => x.Value).FirstOrDefault());
        }

        public override bool ContainsHand(IEnumerable<Card> cards)
        {
            return cards.GroupBy(x => x.Value).Where(g => g.Count() == 4).Count() == 1;
        }
    }

    public class FourOfAKindHand : BaseHand
    {
        public FourOfAKindHand(IEnumerable<Card> quad, Card kicker)
            : base(HandType.FourOfAKind, quad.ToList().Concat(new List<Card>() { kicker }))
        {
            RawScore = 0;
            //Max possible score would be: A,A,A,A,K
            MaxPossibleScore = 14 * 100 * 4 + 13 * 10;

            long scoreFactor = 100;
            foreach(var card in quad)
            {
                RawScore += scoreFactor * card.Value;
            }
            scoreFactor /= 10;

            RawScore += scoreFactor * kicker.Value;
        }

        public override bool IsValid => IsFourOfAKindHand(Cards) && Cards.Count() == 5;

        private static bool IsFourOfAKindHand(IEnumerable<Card> cards)
        {
            return cards.GroupBy(x => x.Value).Where(g => g.Count() == 4).Count() == 1;
        }
    }
}
