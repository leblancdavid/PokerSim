using PokerSim.Bots;
using PokerSim.Engine.Game;
using System;
using System.IO;
using System.Reflection;

namespace PokerSim.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var players = PlayerPluginLoader.LoadPluginPlayers(args[0]);

            var engine = new TexasHoldemGameEngine(new ConsoleGameEventLogger(true));
            engine.AddPlayer(new SimpleRandomPlayer("Bob", 0.25, 0.25));
            engine.AddPlayer(new SimpleRandomPlayer("Joe", 0.25, 0.25));
            //engine.AddPlayer(new BasicRandomPlayer("Jane", 0.25, 0.5, 0.25));
            //engine.AddPlayer(new BasicRandomPlayer("Frank", 0.25, 0.5, 0.25));

            engine.Play();
        }

        
    }
}
