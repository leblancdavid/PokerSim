using PokerSim.Engine.Players;

namespace PokerSim.Engine.Game
{
    public interface IGameEventLog
    {
        void Log(HandResult handResult);
        void PlayerEliminated(IPlayer player);

    }
}
