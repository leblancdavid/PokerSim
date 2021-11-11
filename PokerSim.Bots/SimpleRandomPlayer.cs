using PokerSim.Engine.Game;
using PokerSim.Engine.Players;
using System;

namespace PokerSim.Bots
{
    public class SimpleRandomPlayerFactory : IBotFactory
    {
        public IPlayer GetRandomPlayer()
        {
            var random = new Random();

            double checkProb = random.NextDouble() * 0.3 + 0.1;
            double raiseProb = random.NextDouble() * 0.3 + 0.1;

            return new SimpleRandomPlayer(BotNameGenerator.GenerateName(random.Next(3, 10)),
                checkProb, raiseProb);
        }
    }

    public class SimpleRandomPlayer : IPlayer
    {
        private double _checkCallProb;
        private double _raiseProb;
        private Random _random = new Random();

        public SimpleRandomPlayer(string name, double checkCallProb, double raiseProb)
        {
            _checkCallProb = checkCallProb;
            _raiseProb = raiseProb;
            Name = name;
            Id = Guid.NewGuid();
        }

        public string Name { get; private set; }

        public Guid Id { get; private set; }

        public TurnResult TakeTurn(IPlayerTurnState state)
        {
            double p = _random.NextDouble();
            if (p < _raiseProb)
            {
                return TurnResult.Raise(this, state.Blinds * 3);
            }
            if (p < _raiseProb + _checkCallProb)
            {
                return TurnResult.CheckOrCall(this);
            }

            if (state.CurrentBet == 0)
            {
                return TurnResult.CheckOrCall(this);
            }
            return TurnResult.Fold(this);

        }
    }
}
