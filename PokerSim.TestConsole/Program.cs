using PokerSim.Engine.Game;
using PokerSim.Engine.Players;
using System;

namespace PokerSim.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var engine = new TexasHoldemGameEngine();
            engine.AddPlayer(new BasicRandomPlayer(0.25, 0.5, 0.25));
            engine.AddPlayer(new BasicRandomPlayer(0.25, 0.5, 0.25));
            engine.AddPlayer(new BasicRandomPlayer(0.25, 0.5, 0.25));
            engine.AddPlayer(new BasicRandomPlayer(0.25, 0.5, 0.25));

            engine.Play();
        }
    }
}
