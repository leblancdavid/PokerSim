using PokerSim.Engine.Game;

namespace PokerSim.Engine.Players.FeatureExtraction
{
    public interface IDecisionScorer
    {
        DecisionScores Score(IGameState gameState);
    }
}
