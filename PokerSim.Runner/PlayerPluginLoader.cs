using PokerSim.Engine.Players;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace PokerSim.Runner
{
    public static class PlayerPluginLoader
    {
        public static IEnumerable<IPlayer> LoadPluginPlayers(string pluginPath)
        {
            var players = new List<IPlayer>();
            var plugins = Directory.GetDirectories(pluginPath);
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
            var assembly = LoadPlugin(path);

            foreach (Type type in assembly.GetTypes())
            {
                if (typeof(IPlayer).IsAssignableFrom(type))
                {
                    IPlayer result = Activator.CreateInstance(type) as IPlayer;
                    if (result != null)
                    {
                        return result;
                    }
                }
            }

            return null;
        }

        private static Assembly LoadPlugin(string relativePath)
        {
            // Navigate up to the solution root
            string root = Path.GetFullPath(Path.Combine(
                Path.GetDirectoryName(
                    Path.GetDirectoryName(
                        Path.GetDirectoryName(
                            Path.GetDirectoryName(
                                Path.GetDirectoryName(typeof(Program).Assembly.Location)))))));

            string pluginLocation = Path.GetFullPath(Path.Combine(root, relativePath.Replace('\\', Path.DirectorySeparatorChar)));
            Console.WriteLine($"Loading commands from: {pluginLocation}");
            PlayerPluginLoadContext loadContext = new PlayerPluginLoadContext(pluginLocation);
            return loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(pluginLocation)));
        }
    }
}
