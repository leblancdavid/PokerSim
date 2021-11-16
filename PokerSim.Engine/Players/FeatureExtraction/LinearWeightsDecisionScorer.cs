using PokerSim.Engine.Game;
using System;

namespace PokerSim.Engine.Players.FeatureExtraction
{
    public class LinearWeightsDecisionScorer : IDecisionScorer
    {
        private IGameStateFeatureExtractor _featureExtractor;
        private double[,] _weights;
        public LinearWeightsDecisionScorer(IGameStateFeatureExtractor featureExtractor, double[,] weights)
        {
            _featureExtractor = featureExtractor;
            _weights = weights;
            if(_weights.GetLength(0) != 4)
            {
                throw new ArgumentException($"The weights provided for the LinearWeightsDecisionScorer must have 4 rows (Fold, CheckCall, Raise, AllIn)");
            }
            if(_weights.GetLength(1) != _featureExtractor.FeatureLength)
            {
                throw new ArgumentException($"The weights column provided for the LinearWeightsDecisionScorer must match the FeatureLength of the feature extractor.");
            }
        }

        public DecisionScores Score(IGameState gameState)
        {
            var feature = _featureExtractor.Extract(gameState);
            double foldScore = 0.0, checkScore = 0.0, raiseScore = 0.0, allinScore = 0.0;
            for(int i = 0; i < feature.Length; ++i)
            {
                foldScore += _weights[0, i] * feature[i];
                checkScore += _weights[1, i] * feature[i];
                raiseScore += _weights[2, i] * feature[i];
                allinScore += _weights[3, i] * feature[i];
            }

            return new DecisionScores(foldScore, checkScore, raiseScore, allinScore);
        }

        public static LinearWeightsDecisionScorer InitWithRandomWeights(IGameStateFeatureExtractor featureExtractor)
        {
            var rng = new Random();
            var weights = new double[4, featureExtractor.FeatureLength];
            for(int i = 0; i < featureExtractor.FeatureLength; ++i)
            {
                weights[0, i] = rng.NextDouble() * 2.0 - 1.0;
                weights[1, i] = rng.NextDouble() * 2.0 - 1.0;
                weights[2, i] = rng.NextDouble() * 2.0 - 1.0;
                weights[3, i] = rng.NextDouble() * 2.0 - 1.0;
            }

            return new LinearWeightsDecisionScorer(featureExtractor, weights);
        }
    }
}
