using PokerSim.Bots;
using PokerSim.Engine.Game;

namespace PokerSim.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var pluginPlayer = new SamplePlayerPlugin.SamplePlayerPlugin();
            var engine = new TexasHoldemGameEngine(new ConsoleGameEventLogger(true));
            engine.AddPlayer(new SimpleRandomBot("Bob", 0.25, 0.25));
            engine.AddPlayer(new SimpleRandomBot("Joe", 0.25, 0.25));
            //engine.AddPlayer(new BasicRandomPlayer("Jane", 0.25, 0.5, 0.25));
            //engine.AddPlayer(new BasicRandomPlayer("Frank", 0.25, 0.5, 0.25));

            engine.Play();
        }
    }
}
