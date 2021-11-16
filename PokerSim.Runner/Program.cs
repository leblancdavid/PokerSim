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
            //var engine = new TexasHoldemGameEngine(new ConsoleGameEventLogger(true));
            var engine = new TexasHoldemGameEngine(null);

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

            //engine.AddPlayer(new DeterministicPlayerV1("Bot1", 0.9, 0.2, 0.25, 0.1));
           // engine.AddPlayer(new DeterministicBotV1("Bot2", 0.95, 0.25, 0.3, 0.05));
            int numBotsToAdd = 8;
            var botFactory = new BotFactory();
            for (int i = 0; i < numBotsToAdd; ++i)
            {
                engine.AddPlayer(botFactory.GetRandomPlayer());
            }

            var simulationResults = new SimulationResult();
            for (int i = 0; i < 1000; ++i)
            {
                var result = engine.Play();
                //Console.WriteLine("***** NEW GAME STARTED *****");
                simulationResults.NotifyGameCompleted(result);
                //Console.WriteLine("***** GAME ENDED *****");
                if (i % 10 == 0)
                {
                    Console.Clear();
                    Console.WriteLine("***** LEADERBOARD *****");
                    Console.WriteLine(simulationResults.ToString());
                    Console.WriteLine("***** LEADERBOARD *****");
                }
               
            }

        }

        
    }
}
