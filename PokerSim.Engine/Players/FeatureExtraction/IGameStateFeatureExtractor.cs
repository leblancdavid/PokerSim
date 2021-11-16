using PokerSim.Engine.Game;

namespace PokerSim.Engine.Players.FeatureExtraction
{
    public interface IGameStateFeatureExtractor
    {
        int FeatureLength { get; }
        double[] Extract(IGameState gameState);
    }
}
