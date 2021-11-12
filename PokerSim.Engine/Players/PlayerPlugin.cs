using PokerSim.Engine.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSim.Engine.Players
{
    public abstract class PlayerPlugin : IPlayer
    {
        public PlayerPlugin(string name)
        {
            Name = name;
            Id = Guid.NewGuid();
        }
        public string Name { get; private set; }

        public Guid Id { get; private set; }

        public abstract TurnResult TakeTurn(PlayerTurnState state);
    }
}
