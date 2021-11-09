﻿using System;
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
            if(!_playerPot.ContainsKey(player.PlayerId))
            {
                _playerPot.Add(player.PlayerId, new PlayerPotState());
            }

            if(amount >= player.ChipCount)
            {
                _playerPot[player.PlayerId].PotSize += player.ChipCount;
                _playerPot[player.PlayerId].IsAllIn = true;
                player.ChipCount = 0;
            }
            else
            {
                _playerPot[player.PlayerId].PotSize += amount;
                _playerPot[player.PlayerId].IsAllIn = false;
                player.ChipCount -= amount;
            }
        }

        public int ToCallAmount(IPlayerState player)
        {
            if (!_playerPot.ContainsKey(player.PlayerId))
            {
                return MaxPlayerPotSize;
            }

            return MaxPlayerPotSize - _playerPot[player.PlayerId].PotSize;
        }

        public void FoldPlayer(IPlayerState player)
        {
            if (!_playerPot.ContainsKey(player.PlayerId))
            {
                return;
            }

            _playerPot[player.PlayerId].HasFolded = true;
        }

        public void PayoutPlayer(IPlayerState player)
        {
            if (!_playerPot.ContainsKey(player.PlayerId))
                return;

            var playerPotSize = _playerPot[player.PlayerId].PotSize;
            foreach(var pot in _playerPot)
            {
                var gains = Math.Min(playerPotSize, pot.Value.PotSize);
                pot.Value.PotSize -= gains;
                player.ChipCount += gains;
            }
        }
    }
}
