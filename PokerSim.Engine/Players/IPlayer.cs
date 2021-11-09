using PokerSim.Engine.Decks;
using PokerSim.Engine.Game;
using System;

namespace PokerSim.Engine.Players
{
    public interface IPlayer
    {
        TurnResult TakeTurn(IPlayerTurnState state);
    }
}
