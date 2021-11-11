using PokerSim.Engine.Players;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace PokerSim.Runner
{
    public static class PlayerPluginLoader
    {
        public static IEnumerable<IPlayer> LoadPluginPlayers(string pluginPath)
        {
            var players = new List<IPlayer>();
            var plugins = Directory.GetFiles(pluginPath).Where(x => Path.GetExtension(x) == ".dll");
            foreach(var p in plugins)
            {
                var player = LoadPlayer(p);
                if(player != null)
                {
                    players.Add(player);
                }
            }

            return players;
        }

        public static IPlayer LoadPlayer(string path)
        {
            try
            {
                var assembly = LoadPlugin(path);

                foreach (Type type in assembly.GetTypes())
                {
                    if (typeof(PlayerPlugin).IsAssignableFrom(type))
                    {

                        var result = Activator.CreateInstance(type) as IPlayer;
                        if (result != null)
                        {
                            return result;
                        }
                    }

                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static Assembly LoadPlugin(string pluginLocation)
        {
            Console.WriteLine($"Loading commands from: {pluginLocation}");
            PlayerPluginLoadContext loadContext = new PlayerPluginLoadContext(pluginLocation);
            return loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(pluginLocation)));
        }
    }
}
