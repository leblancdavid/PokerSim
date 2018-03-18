using System.Collections.Generic;
using PokerSim.Engine.Decks;
using PokerSim.Engine.Game;

namespace PokerSim.Engine.Players
{
    public class TexasHoldemPlayer
    {
        public PlayerTurn TakeTurn(TexasHoldemGameState state)
        {
            return new PlayerTurn();
        }
    }
}