using System;
using System.Collections.Generic;
using System.Linq;

namespace PokerSim.Engine.Game
{

    internal class PotState
    {

        public int TotalPotSize => _playerStates.Sum(x => x.PlayerPotSize);
        public int MaxPlayerPotSize => _playerStates.Max(x => x.PlayerPotSize);

        private IEnumerable<InternalPlayerState> _playerStates;
        public PotState(IEnumerable<InternalPlayerState> playerStates)
        {
            _playerStates = playerStates;
        }

        public void AddToPot(Guid playerId, int amount)
        {
            var player = _playerStates.FirstOrDefault(x => x.Player.Id == playerId);
            if(amount >= player.ChipCount)
            {
                player.IsAllIn = true;
                player.ChipCount = 0;
                player.PlayerPotSize += player.ChipCount;
            }
            else
            {
                player.IsAllIn = false;
                player.ChipCount -= amount;
                player.PlayerPotSize += amount;
            }
        }

        public int ToCallAmount(Guid playerId)
        {
            var player = _playerStates.FirstOrDefault(x => x.Player.Id == playerId);
            if (player == null)
            {
                return MaxPlayerPotSize;
            }

            return MaxPlayerPotSize - player.PlayerPotSize;
        }

        public void PlayerCallOrCheck(Guid playerId)
        {
            var toCall = ToCallAmount(playerId);
            if(toCall > 0)
            {
                AddToPot(playerId, toCall);
            }
        }

        public void PlayerRaise(Guid playerId, int amount)
        {
            var raise = ToCallAmount(playerId) + amount;
            if (raise > 0)
            {
                AddToPot(playerId, raise);
            }
        }

        public int PayoutPlayer(Guid playerId, double splitRatio = 1.0)
        {

            var player = _playerStates.FirstOrDefault(x => x.Player.Id == playerId);
            if (player == null)
                return 0;

            var playerPotSize = player.PlayerPotSize;
            int totalGains = 0;
            foreach (var pot in _playerStates)
            {
                var gains = (int)Math.Ceiling(Math.Min(playerPotSize, pot.PlayerPotSize) * splitRatio - 0.5);
                pot.PlayerPotSize -= gains;
                player.ChipCount += gains;
                totalGains += gains;
            }

            return totalGains;
        }

        public void PayoutPlayers(IEnumerable<PlayerHandResult> results)
        {
            var groupedResults = results.OrderByDescending(x => x.Hand).GroupBy(x => x.Hand.RawScore).ToList();
            foreach(var group in groupedResults)
            {
                foreach (var result in group)
                {
                    result.Winnings = PayoutPlayer(result.Player.Id, 1.0 / group.Count());
                }
            }
        }

    }
}
