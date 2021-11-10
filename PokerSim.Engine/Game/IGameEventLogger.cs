using PokerSim.Engine.Players;

namespace PokerSim.Engine.Game
{
    public interface IGameEventLogger
    {
        void Log(HandResult handResult);
        void Log(TurnResult turnResult);

    }
}
