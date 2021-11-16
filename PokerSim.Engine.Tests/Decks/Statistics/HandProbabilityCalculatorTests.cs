using PokerSim.Engine.Decks;
using PokerSim.Engine.Decks.Statistics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace PokerSim.Engine.Tests.Decks.Statistics
{
    public class HandProbabilityCalculatorTests
    {
        private HandProbabilityCalculator _handCalculator = new HandProbabilityCalculator(7);
        private HandBuilder _handBuilder = new HandBuilder();
        public HandProbabilityCalculatorTests()
        {

        }

        //[Fact]
        public void TestRandomHands()
        {
            var fileMap = new Dictionary<HandType, StreamWriter>();
            var handTypes = Enum.GetValues(typeof(HandType)).Cast<HandType>();
            foreach(var hand in handTypes)
            {
                fileMap.Add(hand, new StreamWriter($"RandomHandProbability{hand.ToString()}.txt"));
            }

            for(int i = 0; i < 00; ++i)
            {
                var deck = new Deck();
                deck.Shuffle();

                var cards = new List<Card>();
                for(int j = 0; j < 5; ++j)
                {
                    cards.Add(deck.Draw());
                }

                var hand = _handBuilder.BuildHand(cards);
                foreach (var file in fileMap)
                {
                    var p = _handCalculator.Calculate(file.Key, cards);

                    file.Value.WriteLine($"{hand.ToString()} {p.CardsNeededToComplete}, {p.CompletionPercentage}, {p.ProbabilityToComplete}");
                }

            }
        }
    }
}
