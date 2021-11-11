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

        public static TurnResult CheckOrCallAny(IPlayer player)
        {
            return new TurnResult(player)
            {
                Decision = TurnDecisionType.CheckOrCall,
                RaiseAmount = 0
            };
        }

        public static TurnResult CheckOrFoldAny(IPlayer player, int currentBet)
        {
            if (currentBet > 0)
                return Fold(player);
            return CheckOrCallAny(player);
        }

        public static TurnResult Raise(IPlayer player, int amount)
        {
            return new TurnResult(player)
            {
                Decision = TurnDecisionType.Raise,
                RaiseAmount = amount
            };
        }

        public override string ToString()
        {
            switch (Decision)
            {
                case TurnDecisionType.Fold:
                    return $"{Player.Name} Folds";
                case TurnDecisionType.CheckOrCall:
                    return $"{Player.Name} Checks/Calls";
                case TurnDecisionType.Raise:
                    return $"{Player.Name} Raises ${RaiseAmount}";
            }
            return $"{Player.Name}";
        }
    }
}
