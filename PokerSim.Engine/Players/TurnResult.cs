using System;

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
        public Guid PlayerId { get; private set; }
        public TurnDecisionType Decision { get; private set; }
        public int RaiseAmount { get; private set; }
        public TurnResult(Guid playerId)
        {
            Decision = TurnDecisionType.Fold;
            RaiseAmount = 0;
            PlayerId = playerId;
        }

        public static TurnResult Fold(Guid playerId)
        {
            return new TurnResult(playerId);
        }

        public static TurnResult CheckOrCall(Guid playerId)
        {
            return new TurnResult(playerId)
            {
                Decision = TurnDecisionType.CheckOrCall,
                RaiseAmount = 0
            };
        }

        public static TurnResult Raise(Guid playerId, int amount)
        {
            return new TurnResult(playerId)
            {
                Decision = TurnDecisionType.Raise,
                RaiseAmount = amount
            };
        }
    }
}
