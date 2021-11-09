namespace PokerSim.Engine.Players
{
    public enum TurnDecisionType
    {
        Fold,
        CheckOrCall,
        Raise
    }

    public sealed class TurnResult
    {
        public TurnDecisionType Decision { get; private set; }
        public int RaiseAmount { get; private set; }
        public TurnResult()
        {
            Decision = TurnDecisionType.Fold;
            RaiseAmount = 0;
        }

        public static TurnResult Fold()
        {
            return new TurnResult();
        }

        public static TurnResult CheckOrCall()
        {
            return new TurnResult()
            {
                Decision = TurnDecisionType.CheckOrCall,
                RaiseAmount = 0
            };
        }

        public static TurnResult Raise(int amount)
        {
            return new TurnResult()
            {
                Decision = TurnDecisionType.Raise,
                RaiseAmount = amount
            };
        }
    }
}
