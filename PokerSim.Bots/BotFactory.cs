using PokerSim.Engine.Players;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PokerSim.Bots
{
    public class BotFactory : IBotFactory
    {
        private Random _random = new Random();
        private List<IBotFactory> _botTypeFactories = new List<IBotFactory>()
        {
            new SimpleRandomPlayerFactory()
        };

        public IPlayer GetRandomPlayer()
        {
            var index = _random.Next(0, _botTypeFactories.Count() - 1);
            return _botTypeFactories[index].GetRandomPlayer();
        }
    }
}
