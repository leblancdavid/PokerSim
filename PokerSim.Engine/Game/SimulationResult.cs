using PokerSim.Engine.Players;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PokerSim.Engine.Game
{
    public class PlayerScore
    {
        public IPlayer Player { get; set; }
        public long Score { get; set; }


        public PlayerScore(IPlayer key, long val)
        {
            this.Player = key;
            this.Score = val;
        }
    }

    public class SimulationResult
    {
        private List<PlayerScore> _leaderboard = new List<PlayerScore>();
        public IEnumerable<PlayerScore> Leaderboard => _leaderboard;
        public void NotifyGameCompleted(GameResult gameResult)
        {
            int numPlayers = gameResult.Ranking.Count();
            int index = 1;
            foreach(var player in gameResult.Ranking)
            {
                var leader = _leaderboard.FirstOrDefault(x => x.Player.Id == player.Id);
                if (leader == null)
                {
                    leader = new PlayerScore(player, 0);
                    _leaderboard.Add(leader);
                }

                leader.Score += (long)Math.Floor((1.0 - Math.Pow((double)index / numPlayers, 2.0)) * 100.0);
            }
        }
    }
}
