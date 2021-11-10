using System.Collections.Generic;
using System.Linq;

namespace PokerSim.Engine.Decks
{
    public class FullHouseHandBuilder : IHandBuilder
    {
        public IHand BuildHand(IEnumerable<Card> cards)
        {
            var tempList = cards.ToList();
            var tripletGroup = cards.GroupBy(x => x.Value)
                .Where(x => x.Count() == 3)
                .OrderByDescending(x => x.Key).FirstOrDefault();

            if (tripletGroup == null)
            {
                return new FullHouseHand(new List<Card>(), new List<Card>());
            }

            var triplet = tempList.Where(x => x.Value == tripletGroup.Key);
            tempList.RemoveAll(x => x.Value == tripletGroup.Key);

            var pairGroup = tempList.GroupBy(x => x.Value)
                .Where(x => x.Count() >= 2)
                .OrderByDescending(x => x.Key)
                .FirstOrDefault();

            if (pairGroup == null)
            {
                return new FullHouseHand(new List<Card>(), new List<Card>());
            }

            var pair = tempList.Where(x => x.Value == pairGroup.Key).Take(2);

            return new FullHouseHand(triplet, pair);
        }

        public bool ContainsHand(IEnumerable<Card> cards)
        {
            var tempList = cards.ToList();
            var tripletGroup = cards.GroupBy(x => x.Value)
                .Where(x => x.Count() == 3)
                .OrderByDescending(x => x.Key).FirstOrDefault();

            if (tripletGroup == null)
            {
                return false;
            }

            tempList.RemoveAll(x => x.Value == tripletGroup.Key);
            var pairGroup = tempList.GroupBy(x => x.Value)
                .Where(x => x.Count() >= 2)
                .OrderByDescending(x => x.Key)
                .FirstOrDefault();

            if (pairGroup == null)
            {
                return false;
            }

            return true;
        }
    }

    public class FullHouseHand : BaseHand
    {
        public FullHouseHand(IEnumerable<Card> triplet, IEnumerable<Card> pair)
            : base(HandType.FullHouse, triplet.Concat(pair))
        {
            Score = 0;
            long scoreFactor = 100000;
            foreach (var card in triplet)
            {
                Score += scoreFactor * card.Value;
            }
            scoreFactor /= 10;
            foreach (var card in pair)
            {
                Score += scoreFactor * card.Value;
            }
        }

        public override bool IsValid => IsFullHouseHand(Cards) && Cards.Count() == 5;

        private static bool IsFullHouseHand(IEnumerable<Card> cards)
        {
            var tempList = cards.ToList();
            var tripletGroup = cards.GroupBy(x => x.Value)
                .Where(x => x.Count() == 3)
                .OrderByDescending(x => x.Key).FirstOrDefault();

            if (tripletGroup == null)
            {
                return false;
            }

            tempList.RemoveAll(x => x.Value == tripletGroup.Key);
            var pairGroup = tempList.GroupBy(x => x.Value)
                .Where(x => x.Count() >= 2)
                .OrderByDescending(x => x.Key)
                .FirstOrDefault();

            if(pairGroup == null)
            {
                return false;
            }

            return true;

        }
    }
}
