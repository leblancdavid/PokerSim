using System;
using System.Collections.Generic;
using System.Linq;

namespace PokerSim.Engine.Game
{
    internal class PlayerPotState
    {
        public int PotSize { get; set; }
        public bool IsAllIn { get; set; }
        public bool HasFolded { get; set; }
    }

    internal class PotState
    {
        Dictionary<Guid, PlayerPotState> _playerPot = new Dictionary<Guid, PlayerPotState>();


        public int TotalPotSize => _playerPot.Values.Sum(x => x.PotSize);
        public int MaxPlayerPotSize => _playerPot.Values.Max(x => x.PotSize);

        private IEnumerable<IPlayerState> _playerStates;
        public PotState(IEnumerable<IPlayerState> playerStates)
        {
            _playerStates = playerStates;
            foreach(var player in _playerStates)
            {
                _playerPot.Add(player.Player.Id, new PlayerPotState());
            }
        }

        public void AddToPot(Guid playerId, int amount)
        {
            if(!_playerPot.ContainsKey(playerId))
            {
                _playerPot.Add(playerId, new PlayerPotState());
            }

            var player = _playerStates.FirstOrDefault(x => x.Player.Id == playerId);
            if(amount >= player.ChipCount)
            {
                _playerPot[playerId].PotSize += player.ChipCount;
                _playerPot[playerId].IsAllIn = true;
                player.IsAllIn = true;
                player.ChipCount = 0;
            }
            else
            {
                _playerPot[player.Player.Id].PotSize += amount;
                _playerPot[player.Player.Id].IsAllIn = false;
                player.IsAllIn = false;
                player.ChipCount -= amount;
            }
        }

        public int ToCallAmount(Guid playerId)
        {
            if (!_playerPot.ContainsKey(playerId))
            {
                return MaxPlayerPotSize;
            }

            return MaxPlayerPotSize - _playerPot[playerId].PotSize;
        }

        public void PlayerFold(Guid playerId)
        {
            if (!_playerPot.ContainsKey(playerId))
            {
                return;
            }

            _playerPot[playerId].HasFolded = true;
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
            if (!_playerPot.ContainsKey(playerId))
                return 0;

            var playerPotSize = _playerPot[playerId].PotSize;
            int totalGains = 0;
            var player = _playerStates.FirstOrDefault(x => x.Player.Id == playerId);
            foreach (var pot in _playerPot)
            {
                var gains = (int)Math.Ceiling(Math.Min(playerPotSize, pot.Value.PotSize) * splitRatio - 0.5);
                pot.Value.PotSize -= gains;
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
