using PokerSim.Engine.Decks;
using System.Collections.Generic;

namespace PokerSim.Engine.Game
{
    public class PlayerTurnState : IPlayerTurnState
    {
        public int CurrentBet { get; private set; }

        public int CurrentPot { get; private set; }
        public int ChipCount { get; private set; }

        public int NumRemainingPlayersInHand { get; private set; }

        public IEnumerable<Card> CommunityCards { get; private set; }

        public IEnumerable<Card> PlayerCards { get; private set; }

        public PlayerTurnState(IEnumerable<Card> communityCards, 
            IEnumerable<Card> playerCards,
            int currentBet,
            int currentPot,
            int chipCount,
            int numberPlayersLeft)
        {
            CommunityCards = communityCards;
            PlayerCards = playerCards;
            CurrentBet = currentBet;
            CurrentPot = currentPot;
            ChipCount = chipCount;
            NumRemainingPlayersInHand = numberPlayersLeft;
        }
    }
}
