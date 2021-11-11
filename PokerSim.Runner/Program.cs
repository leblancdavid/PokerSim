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
        }

        
    }
}
