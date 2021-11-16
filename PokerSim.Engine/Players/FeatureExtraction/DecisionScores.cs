namespace PokerSim.Engine.Players.FeatureExtraction
{
    public class DecisionScores
    {
        public double FoldScore { get; private set; }
        public double CheckCallScore { get; private set; }
        public double RaiseScore { get; private set; }
        public double AllInScore { get; private set; }

        public DecisionScores(double foldScore, double checkCallScore, double raiseScore, double allInScore)
        {
            FoldScore = foldScore;
            CheckCallScore = checkCallScore;
            RaiseScore = raiseScore;
            AllInScore = allInScore;
        }
    }
}
