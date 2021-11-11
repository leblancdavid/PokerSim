using PokerSim.Bots;
using PokerSim.Engine.Game;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace PokerSim.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var pluginPlayers = PlayerPluginLoader.LoadPluginPlayers(args[0]);

            var engine = new TexasHoldemGameEngine(new ConsoleGameEventLogger(true));
            foreach(var player in pluginPlayers)
            {
                engine.AddPlayer(player);
            }

            int tableSize = 8;
            int numBotsToAdd = tableSize - (pluginPlayers.Count() % tableSize);
            var botFactory = new BotFactory();
            for(int i = 0; i < numBotsToAdd; ++i)
            {
                engine.AddPlayer(botFactory.GetRandomPlayer());
            }

            engine.Play();
        }

        
    }
}
