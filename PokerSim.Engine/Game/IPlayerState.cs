using PokerSim.Engine.Players;

namespace PokerSim.Engine.Game
{
    public interface IPlayerState
    {
        public IPlayer Player { get; }
        public int ChipCount { get; }
        public int PlayerPotSize { get; }
        public bool HasFolded { get; }
        public bool IsEliminated { get; }
        public bool IsAllIn { get; }
        public int NumberRaises { get; }
        public int LastAmountRaised { get; }
    }
}
