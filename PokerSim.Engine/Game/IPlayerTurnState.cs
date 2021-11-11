using PokerSim.Engine.Decks;
using System.Collections.Generic;

namespace PokerSim.Engine.Game
{
    public interface IPlayerTurnState
    {
        int CurrentBet { get; }
        int CurrentPot { get; }
        int ChipCount { get; }
        int Blinds { get; }
        int NumRemainingPlayersInHand { get; }
        TexasHoldemStages CurrentStage { get; }

        IEnumerable<Card> CommunityCards { get; }
        IEnumerable<Card> PlayerCards { get; }

        IHand GetHand();
    }
}
