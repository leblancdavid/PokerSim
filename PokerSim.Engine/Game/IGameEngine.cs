using PokerSim.Engine.Decks;
using PokerSim.Engine.Players;
using System.Collections.Generic;

namespace PokerSim.Engine.Game
{
    public interface IGameEngine
    {
        void AddPlayer(IPlayer player);

        GameResult Play();
    }
}
