using System.Collections.Generic;
using System.Linq;

namespace PokerSim.Engine.Decks
{
    public class StraightHandBuilder : IHandBuilder
    {
        public IHand BuildHand(IEnumerable<Card> cards)
        {
            var straight = new List<Card>();
            var distinct = cards.Select(x => x.Value).Distinct().OrderBy(x => x).ToList();
            for (int i = 0; i < distinct.Count - 5; ++i)
            {
                int val = distinct[i];
                straight = new List<Card>();
                straight.Add(cards.FirstOrDefault(x => x.Value == val));
                for (int j = i + 1; j < distinct.Count; ++j)
                {
                    val++;
                    if (distinct[j] != val)
                    {
                        break;
                    }
                    straight.Add(cards.FirstOrDefault(x => x.Value == val));
                }
            }

            if(straight.Count < 5)
            {
                return new StraightHand(new List<Card>());
            }

            return new StraightHand(straight.OrderByDescending(x => x.Value).Take(5));
        }

        public bool ContainsHand(IEnumerable<Card> cards)
        {
            var distinct = cards.Select(x => x.Value).Distinct().OrderByDescending(x => x).ToList();
            bool containsStraight = false;
            for(int i = 0; i < distinct.Count - 5; ++i)
            {
                int val = distinct[i];
                containsStraight = true;
                for (int j = i + 1; j < distinct.Count; ++j)
                {
                    val--;
                    if(distinct[j] != val)
                    {
                        containsStraight = false;
                        break;
                    }
                }
            }
            return containsStraight;
        }
    }

    public class StraightHand : BaseHand
    {
        public StraightHand(IEnumerable<Card> cards)
            : base(HandType.Straight, cards)
        {
            Score = 0;
            long scoreFactor = 100000;
            foreach (var card in cards)
            {
                Score += scoreFactor * card.Value;
                scoreFactor /= 10;
            }
        }

        public override bool IsValid => IsStraightHand(Cards) && Cards.Count() == 5;

        private static bool IsStraightHand(IEnumerable<Card> cards)
        {
            var distinct = cards.Select(x => x.Value).Distinct().OrderByDescending(x => x).ToList();
            bool containsStraight = false;
            for (int i = 0; i < distinct.Count - 5; ++i)
            {
                int val = distinct[i];
                containsStraight = true;
                for (int j = i + 1; j < distinct.Count; ++j)
                {
                    val--;
                    if (distinct[j] != val)
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
