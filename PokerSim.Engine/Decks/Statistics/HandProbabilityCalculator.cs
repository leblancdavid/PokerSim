using System;
using System.Collections.Generic;
using System.Linq;

namespace PokerSim.Engine.Decks.Statistics
{
    public class HandProbabilityCalculator : IHandProbabilityCalculator
    {
        private int _totalCardsDrawn;
        public HandProbabilityCalculator(int totalCardsDrawn)
        {
            _totalCardsDrawn = totalCardsDrawn;
        }

        public HandProbability Calculate(HandType handType, IEnumerable<Card> cards)
        {
            switch (handType)
            {
                case HandType.HighCard:
                    return CalculateHighCardHand(cards);
                case HandType.Pair:
                    return CalculatePairHand(cards);
                case HandType.TwoPair:
                    return CalculateTwoPairHand(cards);
                case HandType.ThreeOfAKind:
                    return CalculateThreeOfAKindHand(cards);
                case HandType.Straight:
                    return CalculateStraightHand(cards);
                case HandType.Flush:
                    return CalculateFlushHand(cards);
                case HandType.FullHouse:
                    return CalculateFullHouseHand(cards);
                case HandType.FourOfAKind:
                    return CalculateFourOfAKindHand(cards);
                case HandType.StraightFlush:
                    return CalculateStraightFlushHand(cards);
                case HandType.RoyalFlush:
                    return CalculateRoyalFlushHand(cards);
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

            return new HandProbability(HandType.Pair, 1, 0.5, GetOutProbability(cards.Count() * 3, cards.Count()));
        }

        private HandProbability CalculateTwoPairHand(IEnumerable<Card> cards)
        {
            int numberOfPairs = cards.GroupBy(x => x.Value).Where(g => g.Count() >= 2).Count();
            if(numberOfPairs == 2)
            {
                return new HandProbability(HandType.TwoPair, 0, 1.0, 1.0);
            }
            else if (numberOfPairs == 1)
            {
                return new HandProbability(HandType.TwoPair, 1, 0.75, GetOutProbability((cards.Count() - 2) * 3, cards.Count()));
            }


            return new HandProbability(HandType.TwoPair, 2, 0.5, GetOutProbability((cards.Count() - 2) * 3, cards.Count()));
        }

        private HandProbability CalculateThreeOfAKindHand(IEnumerable<Card> cards)
        {
            if (cards.GroupBy(x => x.Value).Where(g => g.Count() >= 3).Count() == 1)
            {
                return new HandProbability(HandType.ThreeOfAKind, 0, 1.0, 1.0);
            }

            //If there's a pair
            if(cards.GroupBy(x => x.Value).Where(g => g.Count() == 2).Count() == 1)
            {
                return new HandProbability(HandType.ThreeOfAKind, 1, 0.66667, GetOutProbability(2, cards.Count()));
            }


            if(_totalCardsDrawn - cards.Count() < 2)
            {
                return new HandProbability(HandType.ThreeOfAKind, 2, 0.33333, 0.0);
            }

            //This logic is probably wrong but should be an OK estimate?!?
            double pairProb = GetOutProbability(cards.Count() * 3, cards.Count());
            return new HandProbability(HandType.ThreeOfAKind, 2, 0.33333, pairProb * pairProb);
        }

        private HandProbability CalculateStraightHand(IEnumerable<Card> cards)
        {
            if(cards.Count() < 5)
            {
                //4.62% chance to get a straight so just gonna hard-code that estimate for now
                return new HandProbability(HandType.Straight, 5 - cards.Count(), cards.Count() / 5.0, 0.0462);
            }

            int bestMatch = 0;
            bool containsInsideStraight = false;
            for (int val = 2; val <= 14; ++val)
            {
                int numMatches = 0;
                int connected = 0;
                for (int i = 0; i < 5; ++i)
                {
                    if(cards.FirstOrDefault(x => x.Value == val + i) != null)
                    {
                        numMatches++;
                        connected++;
                        if(connected == 4)
                        {
                            containsInsideStraight = true;
                        }
                    }
                    else
                    {
                        connected = 0;
                    }
                }

                if(numMatches > bestMatch)
                {
                    bestMatch = numMatches;
                }
            }

            if(bestMatch == 5)
            {
                return new HandProbability(HandType.Straight, 0, 1.0, 1.0);
            }
            else if(bestMatch == 4)
            {
                if(containsInsideStraight)
                {
                    return new HandProbability(HandType.Straight, 1, 0.8, GetOutProbability(8, cards.Count()));
                }
                else
                {
                    return new HandProbability(HandType.Straight, 1, 0.8, GetOutProbability(4, cards.Count()));
                }
            }

            //This logic is probably wrong but should be an OK estimate?!?
            double p = GetOutProbability(4, cards.Count());
            return new HandProbability(HandType.Straight, 5 - bestMatch, (double)bestMatch / 5.0, Math.Pow(p, 5 - bestMatch));
        }

        private HandProbability CalculateFlushHand(IEnumerable<Card> cards)
        {
            var suitGroup = cards.GroupBy(x => x.Suit).OrderByDescending(x => x.Count()).FirstOrDefault();
            if(suitGroup == null)
            {
                return new HandProbability(HandType.Flush, 5, 0.0, 0.0);
            }

            int cardsNeededToComplete = 5 - suitGroup.Count();
            if (_totalCardsDrawn - cards.Count() < cardsNeededToComplete)
            {
                return new HandProbability(HandType.Flush, cardsNeededToComplete, suitGroup.Count() / 5.0, 0.0);
            }

            //This logic is probably wrong but should be an OK estimate?!?
            double p = GetOutProbability(13 - suitGroup.Count(), cards.Count());
            return new HandProbability(HandType.Flush, cardsNeededToComplete, suitGroup.Count() / 5.0, Math.Pow(p, cardsNeededToComplete));

        }

        private HandProbability CalculateFullHouseHand(IEnumerable<Card> cards)
        {
            var valueGroups = cards.GroupBy(x => x.Value).OrderByDescending(x => x.Count());
            var triplets = valueGroups.Where(x => x.Count() >= 3);
            var pairs = valueGroups.Where(x => x.Count() == 2);

            //You have a full house when you have at least 2 triplets, or a triplet and at least one pair
            if(triplets.Count() > 1 || (triplets.Count() == 1 && pairs.Count() >= 1))
            {
                return new HandProbability(HandType.FullHouse, 0, 1.0, 1.0);
            }
            //If you just have a triplet, other cards are singles
            if(triplets.Count() == 1)
            {
                //if you have a triplet, you should have 80% of the cards you need for a full house,
                //for each other card you have, there should be a chance to draw a pair 
                return new HandProbability(HandType.FullHouse, 1, 0.8, GetOutProbability((cards.Count() - 2)* 3, cards.Count()));
            }
            
            //If you have no triplets but 2 or more pairs, you just need 1 card
            if(pairs.Count() >= 2)
            {
                return new HandProbability(HandType.FullHouse, 1, 0.8, GetOutProbability(pairs.Count() * 2, cards.Count()));
            }

            //If you have only a single pair you need at least 2 cards
            if(pairs.Count() == 1)
            {
                double probToDrawTriplet = GetOutProbability(2, cards.Count());
                double probToDrawAPair = GetOutProbability((cards.Count() - 1) * 3, cards.Count());
                return new HandProbability(HandType.FullHouse, 2, 0.6, probToDrawTriplet * probToDrawAPair);
            }


            //2.80% chance to get a full house so just gonna hard-code that estimate for now
            return new HandProbability(HandType.FullHouse, 3, 0.4, 0.0280);

        }

        private HandProbability CalculateFourOfAKindHand(IEnumerable<Card> cards)
        {
            var valueGroup = cards.GroupBy(x => x.Value).OrderByDescending(x => x.Count()).FirstOrDefault();
            if (valueGroup == null)
            {
                return new HandProbability(HandType.FourOfAKind, 5, 0.0, 0.0);
            }
            if(valueGroup.Count() == 4)
            {
                return new HandProbability(HandType.FourOfAKind, 0, 1.0, 1.0);
            }

            int cardsNeededToComplete = 4 - valueGroup.Count();
            if (_totalCardsDrawn - cards.Count() < cardsNeededToComplete)
            {
                return new HandProbability(HandType.FourOfAKind, cardsNeededToComplete, valueGroup.Count() / 4.0, 0.0);
            }

            double p = GetOutProbability(1, cards.Count());
            return new HandProbability(HandType.FourOfAKind, cardsNeededToComplete, valueGroup.Count() / 4.0, Math.Pow(p, cardsNeededToComplete));

        }

        private HandProbability CalculateStraightFlushHand(IEnumerable<Card> cards)
        {
            var suitGroup = cards.GroupBy(x => x.Suit).OrderByDescending(x => x.Count()).FirstOrDefault();
            if (suitGroup == null)
            {
                return new HandProbability(HandType.StraightFlush, 5, 0.0, 0.0);
            }

            int cardsNeededToComplete = 5 - suitGroup.Count();
            if (_totalCardsDrawn - cards.Count() < cardsNeededToComplete)
            {
                return new HandProbability(HandType.StraightFlush, cardsNeededToComplete, suitGroup.Count() / 5.0, 0.0);
            }

            int bestMatch = 0;
            bool containsInsideStraight = false;
            for (int val = 2; val <= 14; ++val)
            {
                int numMatches = 0;
                int connected = 0;
                for (int i = 0; i < 5; ++i)
                {
                    if (suitGroup.FirstOrDefault(x => x.Value == val + i) != null)
                    {
                        numMatches++;
                        connected++;
                        if (connected == 4)
                        {
                            containsInsideStraight = true;
                        }
                    }
                    else
                    {
                        connected = 0;
                    }
                }

                if (numMatches > bestMatch)
                {
                    bestMatch = numMatches;
                }
            }

            if (bestMatch == 5)
            {
                return new HandProbability(HandType.StraightFlush, 0, 1.0, 1.0);
            }
            else if (bestMatch == 4)
            {
                if (containsInsideStraight)
                {
                    return new HandProbability(HandType.StraightFlush, 1, 0.8, GetOutProbability(2, cards.Count()));
                }
                else
                {
                    return new HandProbability(HandType.StraightFlush, 1, 0.8, GetOutProbability(1, cards.Count()));
                }
            }

            //This logic is probably wrong but should be an OK estimate?!?
            double p = GetOutProbability(1, cards.Count());
            return new HandProbability(HandType.Straight, 5 - bestMatch, (double)bestMatch / 5.0, Math.Pow(p, 5 - bestMatch));

        }

        private HandProbability CalculateRoyalFlushHand(IEnumerable<Card> cards)
        {
            var royalCards = cards.Where(x => x.Value >= 10);
            var suitGroup = royalCards.GroupBy(x => x.Suit).OrderByDescending(x => x.Count()).FirstOrDefault();
            if (suitGroup == null)
            {
                return new HandProbability(HandType.RoyalFlush, 5, 0.0, 0.0);
            }

            int cardsNeededToComplete = 5 - suitGroup.Count();
            if (_totalCardsDrawn - cards.Count() < cardsNeededToComplete)
            {
                return new HandProbability(HandType.RoyalFlush, cardsNeededToComplete, suitGroup.Count() / 5.0, 0.0);
            }

            int bestMatch = 0;
            for (int val = 10; val <= 14; ++val)
            {
                int numMatches = 0;
                for (int i = 0; i < 5; ++i)
                {
                    if (suitGroup.FirstOrDefault(x => x.Value == val + i) != null)
                    {
                        numMatches++;
                    }
                }

                if (numMatches > bestMatch)
                {
                    bestMatch = numMatches;
                }
            }

            if (bestMatch == 5)
            {
                return new HandProbability(HandType.StraightFlush, 0, 1.0, 1.0);
            }
            else if (bestMatch == 4)
            {
                return new HandProbability(HandType.StraightFlush, 1, 0.8, GetOutProbability(1, cards.Count()));
          
            }

            //This logic is probably wrong but should be an OK estimate?!?
            double p = GetOutProbability(1, cards.Count());
            return new HandProbability(HandType.Straight, 5 - bestMatch, (double)bestMatch / 5.0, Math.Pow(p, 5 - bestMatch));

        }

        private double GetOutProbability(int numberOfOuts, int numberOfPlayerCards)
        {
            int numberOfCardsToDraw = _totalCardsDrawn - numberOfPlayerCards;
            double cummulative = 1.0;
            for(int i = 0; i < numberOfCardsToDraw; ++i)
            {
                double probabilityOfNotDrawing = 1.0 - ((double)numberOfOuts / (52 - numberOfPlayerCards - i));
                cummulative *= probabilityOfNotDrawing;
            }
            return 1.0 - cummulative;
        }
    }
}
