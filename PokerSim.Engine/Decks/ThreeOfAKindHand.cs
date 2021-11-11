using System.Collections.Generic;
using System.Linq;

namespace PokerSim.Engine.Decks
{
    public class ThreeOfAKindHandBuilder : IHandBuilder
    {
        public IHand BuildHand(IEnumerable<Card> cards)
        {
            var tempList = cards.ToList();
            //Todo how to finger this one out!
            var tripleGroup = tempList.GroupBy(x => x.Value)
                .Where(g => g.Count() == 3)
                .OrderByDescending(x => x.Key).FirstOrDefault();
            if (tripleGroup == null)
            {
                //Invalid pair...
                return new ThreeOfAKindHand(new List<Card>(), new List<Card>());
            }

            tempList.RemoveAll(x => x.Value == tripleGroup.Key);
            return new ThreeOfAKindHand(cards.Where(x => x.Value == tripleGroup.Key),
                tempList.OrderByDescending(x => x.Value).Take(2));
        }

        public bool ContainsHand(IEnumerable<Card> cards)
        {
            return cards.GroupBy(x => x.Value).Where(g => g.Count() == 3).Count() >= 1;
        }
    }

    public class ThreeOfAKindHand : BaseHand
    {
        public ThreeOfAKindHand(IEnumerable<Card> triplet, IEnumerable<Card> cards)
            : base(HandType.ThreeOfAKind, triplet.ToList().Concat(cards.ToList()))
        {
            Score = 0;
            long scoreFactor = 100000;
            foreach(var card in triplet)
            {
                Score += scoreFactor * card.Value;
            }
            scoreFactor /= 10;

            foreach (var card in cards)
            {
                Score += scoreFactor * card.Value;
                scoreFactor /= 10;
            }
        }

        public override bool IsValid => IsThreeOfAKindHand(Cards) && Cards.Count() == 5;

        private static bool IsThreeOfAKindHand(IEnumerable<Card> cards)
        {
            return cards.GroupBy(x => x.Value).Where(g => g.Count() == 3).Count() >= 1;
        }
    }
}
