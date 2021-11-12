using PokerSim.Engine.Decks;
using System.Collections.Generic;

namespace PokerSim.Engine.Game
{
    public class PlayerTurnState
    {
        private IHandBuilder _handBuilder = new HandBuilder();

        public int CurrentBet { get; private set; }

        public int CurrentPot { get; private set; }
        public int ChipCount { get; private set; }
        public int Blinds { get; private set; }

        public int NumRemainingPlayersInHand { get; private set; }

        public IEnumerable<Card> CommunityCards { get; private set; }

        public IEnumerable<Card> PlayerCards { get; private set; }
        public TexasHoldemStages CurrentStage { get; private set; }
        public PlayerTurnState(IEnumerable<Card> communityCards, 
            IEnumerable<Card> playerCards,
            TexasHoldemStages stage,
            int currentBet,
            int currentPot,
            int blinds,
            int chipCount,
            int numberPlayersLeft)
        {
            CommunityCards = communityCards;
            PlayerCards = playerCards;
            CurrentStage = stage;
            CurrentBet = currentBet;
            CurrentPot = currentPot;
            Blinds = blinds;
            ChipCount = chipCount;
            NumRemainingPlayersInHand = numberPlayersLeft;
        }

        public IHand GetHand()
        {
            return _handBuilder.BuildHand(PlayerCards, CommunityCards);
        }
    }
}
