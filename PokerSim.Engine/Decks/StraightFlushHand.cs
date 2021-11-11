using System.Collections.Generic;
using System.Linq;

namespace PokerSim.Engine.Decks
{
    public class StraightFlushHandBuilder : BaseHandBuilder
    {
        public override IHand BuildHand(IEnumerable<Card> cards)
        {
            var suitGroup = cards.GroupBy(x => x.Suit).FirstOrDefault(x => x.Count() >= 5);
            if(suitGroup == null)
            {
                return new StraightFlushHand(new List<Card>());
            }

            var output = new List<Card>();
            var straight = cards.Where(x => x.Suit == suitGroup.Key).OrderByDescending(x => x.Value).ToList();
            for (int i = 0; i < straight.Count - 5; ++i)
            {
                int val = straight[i].Value;
                straight = new List<Card>();
                straight.Add(cards.FirstOrDefault(x => x.Value == val));
                for (int j = i + 1; j < straight.Count; ++j)
                {
                    val--;
                    if (straight[j].Value != val)
                    {
                        break;
                    }
                    straight.Add(cards.FirstOrDefault(x => x.Value == val));
                }
                if (straight.Count >= 5)
                    break;
            }

            if (straight.Count < 5)
            {
                return new StraightFlushHand(new List<Card>());
            }

            return new StraightFlushHand(straight.OrderByDescending(x => x.Value)
                .Take(5));
        }

        public override bool ContainsHand(IEnumerable<Card> cards)
        {
            var suitGroup = cards.GroupBy(x => x.Suit).FirstOrDefault(x => x.Count() >= 5);
            if (suitGroup == null)
            {
                return false;
            }

            var straight = cards.Where(x => x.Suit == suitGroup.Key).OrderByDescending(x => x.Value).ToList();
            bool containsStraight = false;
            for (int i = 0; i < straight.Count - 5; ++i)
            {
                int val = straight[i].Value;
                containsStraight = true;
                for (int j = i + 1; j < straight.Count; ++j)
                {
                    val--;
                    if (straight[j].Value != val)
                    {
                        containsStraight = false;
                        break;
                    }
                }
            }
            return containsStraight;
        }
    }

    public class StraightFlushHand : BaseHand
    {
        public StraightFlushHand(IEnumerable<Card> cards)
            : base(HandType.StraightFlush, cards)
        {
            RawScore = 0;

            //Max possible score would be: A,K,Q,J,10
            MaxPossibleScore = 14 * 100000 + 13 * 10000 + 12 * 1000 + 11 * 100 + 10 * 10;

            long scoreFactor = 100000;
            foreach (var card in cards)
            {
                RawScore += scoreFactor * card.Value;
                scoreFactor /= 10;
            }

            if(cards.Any() && cards.FirstOrDefault().Value == 14)
            {
                HandType = HandType.RoyalFlush;
            }
        }

        public override bool IsValid => IsStraightFlushHand(Cards) && Cards.Count() == 5;

        private static bool IsStraightFlushHand(IEnumerable<Card> cards)
        {
            var suitGroup = cards.GroupBy(x => x.Suit).FirstOrDefault(x => x.Count() >= 5);
            if(suitGroup == null)
            {
                return false;
            }

            var straight = cards.Where(x => x.Suit == suitGroup.Key).OrderByDescending(x => x.Value).ToList();
            bool containsStraight = false;
            for (int i = 0; i < straight.Count - 5; ++i)
            {
                int val = straight[i].Value;
                containsStraight = true;
                for (int j = i + 1; j < straight.Count; ++j)
                {
                    val--;
                    if (straight[j].Value != val)
                    {
                        containsStraight = false;
                        break;
                    }
                }
            }
            return containsStraight;
        }
    }
}
