namespace PokerSim.Engine.Decks.Statistics
{
    public class HandProbability
    {
        public HandType HandType { get; private set; }

        public int CardsNeededToComplete { get; private set; }
        public double CompletionPercentage { get; private set; }
        public double ProbabilityToComplete { get; private set; }
        public bool IsComplete => CardsNeededToComplete == 0;

        public HandProbability(HandType handType, int cardsNeededToComplete, double completionPercentage, double probabilityToComplete)
        {
            HandType = handType;
            CardsNeededToComplete = cardsNeededToComplete;
            CompletionPercentage = completionPercentage;
            ProbabilityToComplete = probabilityToComplete;
        }

    }
}
