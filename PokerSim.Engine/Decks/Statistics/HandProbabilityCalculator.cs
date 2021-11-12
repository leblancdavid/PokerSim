using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSim.Engine.Decks.Statistics
{
    public class HandProbabilityCalculator : IHandProbabilityCalculator
    {
        public HandProbability Calculate(HandType handType, IEnumerable<Card> cards)
        {
            switch (handType)
            {
                case HandType.HighCard:
                    return CalculateHighCardHand(cards);
                case HandType.Pair:
                    return CalculatePairHand(cards);
                case HandType.TwoPair:
                    return CalculateHighCardHand(cards);
                case HandType.ThreeOfAKind:
                    return CalculateHighCardHand(cards);
                case HandType.Straight:
                    return CalculateHighCardHand(cards);
                case HandType.Flush:
                    return CalculateHighCardHand(cards);
                case HandType.FullHouse:
                    return CalculateHighCardHand(cards);
                case HandType.FourOfAKind:
                    return CalculateHighCardHand(cards);
                case HandType.StraightFlush:
                    return CalculateHighCardHand(cards);
                case HandType.RoyalFlush:
                    return CalculateHighCardHand(cards);
            }
            return CalculateHighCardHand(cards);
        }

        public IEnumerable<HandProbability> Calculate(IEnumerable<Card> cards) 
            => Enum.GetValues(typeof(HandType)).Cast<HandType>().Select(x => Calculate(x, cards));

        private HandProbability CalculateHighCardHand(IEnumerable<Card> cards)
        {
            return new HandProbability(HandType.HighCard, 0, 1.0, 1.0);
        }

        private HandProbability CalculatePairHand(IEnumerable<Card> cards)
        {
            if(cards.GroupBy(x => x.Value).Where(g => g.Count() >= 2).Count() > 0)
            {
                return new HandProbability(HandType.Pair, 0, 1.0, 1.0);
            }

            int cardsNeededForAPair = cards.Count() * 3;

        }
    }
}
