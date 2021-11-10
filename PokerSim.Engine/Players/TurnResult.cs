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
        public IPlayer Player { get; private set; }
        public TurnDecisionType Decision { get; private set; }
        public int RaiseAmount { get; private set; }
        public TurnResult(IPlayer player)
        {
            Decision = TurnDecisionType.Fold;
            RaiseAmount = 0;
            Player = player;
        }

        public static TurnResult Fold(IPlayer player)
        {
            return new TurnResult(player);
        }

        public static TurnResult CheckOrCall(IPlayer player)
        {
            return new TurnResult(player)
            {
                Decision = TurnDecisionType.CheckOrCall,
                RaiseAmount = 0
            };
        }

        public static TurnResult Raise(IPlayer player, int amount)
        {
            return new TurnResult(player)
            {
                Decision = TurnDecisionType.Raise,
                RaiseAmount = amount
            };
        }
    }
}
