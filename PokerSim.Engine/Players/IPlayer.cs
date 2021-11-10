using PokerSim.Engine.Game;

namespace PokerSim.Engine.Players
{
    public interface IPlayer
    {
        string Name { get; }
        TurnResult TakeTurn(IPlayerTurnState state);
    }
}
