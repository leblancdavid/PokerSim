using PokerSim.Engine.Decks;
using PokerSim.Engine.Decks.Statistics;
using PokerSim.Engine.Game;
using System.Collections.Generic;
using System.Linq;

namespace PokerSim.Engine.Players.FeatureExtraction
{
    public class GameStateFeatureExtractor : IGameStateFeatureExtractor
    {
        private IHandBuilder _handBuilder;
        private IHandProbabilityCalculator _handProbabilityCalculator;
        public GameStateFeatureExtractor()
        {
            _handProbabilityCalculator = new HandProbabilityCalculator(7);
            _handBuilder = new HandBuilder();
        }

        public GameStateFeatureExtractor(IHandBuilder handBuilder, IHandProbabilityCalculator handProbabilityCalculator)
        {
            _handBuilder = handBuilder;
            _handProbabilityCalculator = handProbabilityCalculator;
        }

        public int FeatureLength => 69;

        public double[] Extract(IGameState state)
        {
            var feature = new List<double>();
            feature.Add(state.GetPlayersInHand().Count());
            feature.Add((double)state.CurrentStage / 7.0);
            feature.Add((double)state.TotalPotSize / state.CurrentPlayerChipCount);
            feature.Add((double)state.CurrentBetToCall / state.CurrentPlayerChipCount);
            feature.Add((double)state.CurrentBetToCall / state.BigBlindValue);
            feature.Add(state.TotalNumberRaises);

            var allCards = state.CommunityCards.Concat(state.CurrentPlayerCards);
            var hand = _handBuilder.BuildHand(allCards);
            feature.Add((double)hand.HandType / 10.0);
            feature.Add(hand.NormalizedScore);
            feature.Add(hand.RelativeScore);

            var probabilities = _handProbabilityCalculator.Calculate(allCards);
            foreach(var p in probabilities)
            {
                feature.Add(p.CompletionPercentage);
                feature.Add(p.ProbabilityToComplete);
                feature.Add(p.CardsNeededToComplete / 5.0);
            }

            var communityProbabilities = _handProbabilityCalculator.Calculate(state.CommunityCards);
            foreach (var p in communityProbabilities)
            {
                feature.Add(p.CompletionPercentage);
                feature.Add(p.ProbabilityToComplete);
                feature.Add(p.CardsNeededToComplete / 5.0);
            }

            return feature.ToArray();
        }
    }
}
