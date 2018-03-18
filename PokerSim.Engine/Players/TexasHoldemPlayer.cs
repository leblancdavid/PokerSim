using System;
using System.Collections.Generic;
using PokerSim.Engine.Decks;
using PokerSim.Engine.Game;

namespace PokerSim.Engine.Players
{
    public class TexasHoldemPlayer
    {
        public Guid PlayerId { get; private set; }
        
        public TexasHoldemPlayer()
        {
            PlayerId = Guid.NewGuid();
        }
        public PlayerTurn TakeTurn(TexasHoldemPlayerTurnState state)
        {
            return new PlayerTurn(0);
        }
    }
}