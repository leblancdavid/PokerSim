using PokerSim.Engine.Game;
using PokerSim.Engine.Players;
using System;

namespace PokerSim.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var engine = new TexasHoldemGameEngine(new ConsoleGameEventLogger(true));
            engine.AddPlayer(new BasicRandomPlayer("Bob", 0.25, 0.5, 0.25));
            engine.AddPlayer(new BasicRandomPlayer("Joe", 0.25, 0.5, 0.25));
            //engine.AddPlayer(new BasicRandomPlayer("Jane", 0.25, 0.5, 0.25));
            //engine.AddPlayer(new BasicRandomPlayer("Frank", 0.25, 0.5, 0.25));

            engine.Play();
        }
    }
}
