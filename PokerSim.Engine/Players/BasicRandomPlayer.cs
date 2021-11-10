using System;
using System.Collections.Generic;
using PokerSim.Engine.Decks;
using PokerSim.Engine.Game;

namespace PokerSim.Engine.Players
{
    public class BasicRandomPlayer : IPlayer
    {
        private double _foldProb;
        private double _checkCallProb;
        private double _raiseProb;
        private Random _random = new Random();

        public BasicRandomPlayer(string name, double foldProb, double checkCallProb, double raiseProb)
        {
            _foldProb = foldProb;
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
            if(p < _foldProb)
            {
                if (state.CurrentBet == 0)
                {
                    return TurnResult.CheckOrCall(Id);
                }
                return TurnResult.Fold(Id);
            }

            if(p < _foldProb + _checkCallProb)
            {
                return TurnResult.CheckOrCall(Id);
            }

            return TurnResult.Raise(Id, state.Blinds * 3);
        }
    }
}