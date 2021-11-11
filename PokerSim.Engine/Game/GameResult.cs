using PokerSim.Engine.Players;
using System.Collections.Generic;
using System.Linq;

namespace PokerSim.Engine.Game
{
    public class GameResult
    {
        private Stack<IPlayer> _ranking = new Stack<IPlayer>();

        public IEnumerable<IPlayer> Ranking => _ranking.ToList();

        public void NotifyPlayerEliminated(IPlayer player)
        {
            if (_ranking.FirstOrDefault(x => x.Id == player.Id) == null)
            {
                _ranking.Push(player);
            }
        }

        public void NotifyWinner(IPlayer player)
        {
            if (_ranking.FirstOrDefault(x => x.Id == player.Id) == null)
            {
                _ranking.Push(player);
            }
        }
    }
}
