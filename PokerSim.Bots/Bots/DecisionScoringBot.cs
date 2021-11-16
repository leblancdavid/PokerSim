using PokerSim.Engine.Game;
using PokerSim.Engine.Players;
using PokerSim.Engine.Players.FeatureExtraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSim.Bots
{
    public class DecisionScoringBotFactory : IBotFactory
    {
        public IPlayer GetRandomPlayer()
        {
            var random = new Random();
            return new DecisionScoringBot(BotNameGenerator.GenerateName(random.Next(3, 10)),
                LinearWeightsDecisionScorer.InitWithRandomWeights(new GameStateFeatureExtractor()));
        }
    }

    public class DecisionScoringBot : PlayerPlugin
    {
        private IDecisionScorer _decisionScorer;
        public DecisionScoringBot(string name, IDecisionScorer decisionScorer) 
            : base("(Bot-DecisionScoring) " + name)
        {
            _decisionScorer = decisionScorer;
        }
        public override TurnResult TakeTurn(IGameState state)
        {
            var score = _decisionScorer.Score(state);

            var decision = score.GetDecision();
            switch (decision)
            {
                case TurnDecisionType.Fold:
                    return TurnResult.Fold(this);
                case TurnDecisionType.CheckOrCall:
                    return TurnResult.CheckOrCallAny(this);
                case TurnDecisionType.Raise:
                    return TurnResult.Raise(this, state.BigBlindValue * 2);
                case TurnDecisionType.AllIn:
                    return TurnResult.Raise(this, state.CurrentPlayerChipCount);
            }

            return TurnResult.Fold(this);
        }
    }
}
