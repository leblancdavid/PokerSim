using PokerSim.Engine.Players;

namespace PokerSim.Bots
{
    public interface IBotFactory
    {
        IPlayer GetRandomPlayer();
    }
}
