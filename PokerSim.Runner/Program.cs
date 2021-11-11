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
            var engine = new TexasHoldemGameEngine(new ConsoleGameEventLogger(true));

            /*
            var pluginPlayers = PlayerPluginLoader.LoadPluginPlayers(args[0]);
            foreach (var player in pluginPlayers)
            {
                engine.AddPlayer(player);
            }

            int tableSize = 2;
            int numBotsToAdd = tableSize - (pluginPlayers.Count() % tableSize);
            var botFactory = new BotFactory();
            for(int i = 0; i < numBotsToAdd; ++i)
            {
                engine.AddPlayer(botFactory.GetRandomPlayer());
            }
            */

            engine.AddPlayer(new DeterministicPlayerV1("Bot1"));
            engine.AddPlayer(new DeterministicPlayerV1("Bot2"));
            engine.AddPlayer(new SimpleRandomPlayer("Bot3"));
            engine.AddPlayer(new SimpleRandomPlayer("Bot4"));

            for (int i = 0; i < 10; ++i)
            {
                Console.WriteLine("***** NEW GAME STARTED *****");
                engine.Play();
                Console.WriteLine("***** GAME ENDED *****");
            }

        }

        
    }
}
