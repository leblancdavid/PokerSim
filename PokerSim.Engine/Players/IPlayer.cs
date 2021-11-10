using PokerSim.Engine.Game;

namespace PokerSim.Engine.Players
{
    public interface IPlayer
    {
        TurnResult TakeTurn(IPlayerTurnState state);
    }
}
