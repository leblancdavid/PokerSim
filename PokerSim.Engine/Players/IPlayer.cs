using PokerSim.Engine.Game;
using System;

namespace PokerSim.Engine.Players
{
    public interface IPlayer
    {
        Guid Id { get; }
        string Name { get; }
        TurnResult TakeTurn(IPlayerTurnState state);
    }
}
