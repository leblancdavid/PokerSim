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
        public bool AreAllBetsIn
        {
            get
            {
                return _playerPot.Values.Where(x => !x.IsAllIn && !x.HasFolded).Select(x => x.PotSize).Distinct().Count() == 1;
            }
        }

        public void AddToPot(IPlayerState player, int amount)
        {
            if(!_playerPot.ContainsKey(player.Player.Id))
            {
                _playerPot.Add(player.Player.Id, new PlayerPotState());
            }

            if(amount >= player.ChipCount)
            {
                _playerPot[player.Player.Id].PotSize += player.ChipCount;
                _playerPot[player.Player.Id].IsAllIn = true;
                player.ChipCount = 0;
            }
            else
            {
                _playerPot[player.Player.Id].PotSize += amount;
                _playerPot[player.Player.Id].IsAllIn = false;
                player.ChipCount -= amount;
            }
        }

        public int ToCallAmount(IPlayerState player)
        {
            if (!_playerPot.ContainsKey(player.Player.Id))
            {
                return MaxPlayerPotSize;
            }

            return MaxPlayerPotSize - _playerPot[player.Player.Id].PotSize;
        }

        public void PlayerFold(IPlayerState player)
        {
            if (!_playerPot.ContainsKey(player.Player.Id))
            {
                return;
            }

            _playerPot[player.Player.Id].HasFolded = true;
        }

        public void PlayerCallOrCheck(IPlayerState player)
        {
            var toCall = ToCallAmount(player);
            if(toCall > 0)
            {
                AddToPot(player, toCall);
            }
        }

        public void PlayerRaise(IPlayerState player, int amount)
        {
            var raise = ToCallAmount(player) + amount;
            if (raise > 0)
            {
                AddToPot(player, raise);
            }
        }

        public int PayoutPlayer(IPlayerState player)
        {
            if (!_playerPot.ContainsKey(player.Player.Id))
                return 0;

            var playerPotSize = _playerPot[player.Player.Id].PotSize;
            int totalGains = 0;
            foreach(var pot in _playerPot)
            {
                var gains = Math.Min(playerPotSize, pot.Value.PotSize);
                pot.Value.PotSize -= gains;
                player.ChipCount += gains;
                totalGains += gains;
            }

            return totalGains;
        }
    }
}
