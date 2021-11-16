using PokerSim.Engine.Game;
using PokerSim.Engine.Players;
using System;

namespace PokerSim.Bots
{
    public class SimpleRandomBotFactory : IBotFactory
    {
        public IPlayer GetRandomPlayer()
        {
            var random = new Random();

            double checkProb = random.NextDouble() / 2.0;
            double raiseProb = random.NextDouble() / 3.0 + 0.1;

            return new SimpleRandomBot(BotNameGenerator.GenerateName(random.Next(3, 10)),
                checkProb, raiseProb);
        }
    }

    public class SimpleRandomBot : IPlayer
    {
        private double _checkCallProb;
        private double _raiseProb;
        private Random _random = new Random();

        public SimpleRandomBot(string name, double checkCallProb = 0.4, double raiseProb = 0.3)
        {
            _checkCallProb = checkCallProb;
            _raiseProb = raiseProb;
            Name = "(Bot-SimpleRandom) " + name;
            Id = Guid.NewGuid();
        }

        public string Name { get; private set; }

        public Guid Id { get; private set; }

        public TurnResult TakeTurn(IGameState state)
        {
            double p = _random.NextDouble();
            if (p < _raiseProb)
            {
                return TurnResult.Raise(this, state.BigBlindValue * 3);
            }
            if (p < _raiseProb + _checkCallProb)
            {
                return TurnResult.CheckOrCallAny(this);
            }

            if (state.CurrentBetToCall == 0)
            {
                return TurnResult.CheckOrCallAny(this);
            }
            return TurnResult.Fold(this);

        }
    }
}
