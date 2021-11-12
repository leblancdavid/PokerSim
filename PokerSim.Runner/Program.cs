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

            engine.AddPlayer(new DeterministicPlayerV1("Bot1", 0.9, 0.3, 0.25, 0.1));
            engine.AddPlayer(new DeterministicPlayerV1("Bot2", 0.9, 0.3, 0.25, 0.1));
            engine.AddPlayer(new SimpleRandomPlayer("Bot3"));
            engine.AddPlayer(new SimpleRandomPlayer("Bot4"));
            engine.AddPlayer(new SimpleRandomPlayer("Bot5"));
            engine.AddPlayer(new SimpleRandomPlayer("Bot6"));
            engine.AddPlayer(new SimpleRandomPlayer("Bot7"));
            engine.AddPlayer(new SimpleRandomPlayer("Bot8"));

            var simulationResults = new SimulationResult();
            for (int i = 0; i < 1000; ++i)
            {
                Console.WriteLine("***** NEW GAME STARTED *****");
                simulationResults.NotifyGameCompleted(engine.Play());
                Console.WriteLine("***** GAME ENDED *****");

                Console.WriteLine("***** LEADERBOARD *****");
                Console.WriteLine(simulationResults.ToString());
                Console.WriteLine("***** LEADERBOARD *****");
            }

        }

        
    }
}
