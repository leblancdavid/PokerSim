using PokerSim.Engine.Decks;
using PokerSim.Engine.Game;
using PokerSim.Engine.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSim.Bots
{
    public class DeterministicPlayerV1PlayerFactory : IBotFactory
    {
        public IPlayer GetRandomPlayer()
        {
            var random = new Random();

            double pfRaise = random.NextDouble() / 5.0 + 0.8;
            double pfFold = random.NextDouble() / 5.0 + 0.05;
            double raise = random.NextDouble() / 5.0 + 0.1;
            double fold = random.NextDouble() / 5.0 + 0.05;

            return new DeterministicPlayerV1(BotNameGenerator.GenerateName(random.Next(3, 10)),
                pfRaise, pfFold, raise, fold);
        }
    }

    public class DeterministicPlayerV1 : PlayerPlugin
    {
        double _preFlopRaiseHandThreshold = 0.75;
        double _preFlopFoldHandThreshold = 0.25;
        double _flopRaiseHandThreshold = 0.15;
        double _flopFoldHandThreshold = 0.05;

        public DeterministicPlayerV1(string name,
            double preFlopRaiseThreshold = 0.75,
            double preFlopFoldThreshold = 0.25,
            double raiseThreshold = 0.15,
            double foldThreshold = 0.05)
            : base("(Bot-Det-v1) " + name)
        {
            _preFlopRaiseHandThreshold = preFlopRaiseThreshold;
            _preFlopFoldHandThreshold = preFlopFoldThreshold;
            _flopRaiseHandThreshold = raiseThreshold;
            _flopFoldHandThreshold = foldThreshold;
        }

        public override TurnResult TakeTurn(IGameState state)
        {
            switch (state.CurrentStage)
            {
                case TexasHoldemStages.PreFlop:
                    return PreFlopTurn(state);
                case TexasHoldemStages.Flop:
                case TexasHoldemStages.Turn:
                case TexasHoldemStages.River:
                default:
                    return PostPreFlopTurn(state);
            }
        }

        private TurnResult PreFlopTurn(IGameState state)
        {
            var hand = state.BuildCurrentPlayerHand();
            if (hand.NormalizedScore > _preFlopRaiseHandThreshold)
            {
                if(state.CurrentPlayerState.NumberRaises >= 2)
                {
                    return TurnResult.CheckOrCallAny(this);
                }

                return TurnResult.Raise(this, state.BigBlindValue * 3);
            }
            else if (hand.NormalizedScore < _preFlopFoldHandThreshold)
            {
                return TurnResult.CheckOrFoldAny(this, state.CurrentBetToCall);
            }
            else
            {
                return TurnResult.CheckOrCallAny(this);
            }
        }

        private TurnResult PostPreFlopTurn(IGameState state)
        {
            var hand = state.BuildCurrentPlayerHand();
            if (hand.RelativeScore > _flopRaiseHandThreshold)
            {
                return TurnResult.Raise(this, state.BigBlindValue * 3);
            }
            else if (hand.RelativeScore < _flopFoldHandThreshold)
            {
                if (state.CurrentBetToCall == 0)
                {
                    return TurnResult.CheckOrCallAny(this);
                }
                return TurnResult.Fold(this);
            }
            else
            {
                return TurnResult.CheckOrCallAny(this);
            }
        }
    }
}
