using PokerSim.Engine.Decks;
using System.Collections.Generic;

namespace PokerSim.Engine.Game
{
    public interface IGameState
    {
        IEnumerable<Card> CommunityCards { get; }
        IEnumerable<IPlayerState> Players { get; }
        int SmallBlindIndex { get; }
        int SmallBlindValue { get; }
        int BigBlindIndex { get; }
        int BigBlindValue { get; }
        int TotalPotSize { get; }
        int CurrentBetToCall { get; }
        TexasHoldemStages CurrentStage { get; }
        int CurrentPlayerIndex { get; }
        int CurrentPlayerChipCount { get; }
        IEnumerable<Card> CurrentPlayerCards { get; }
        IHand BuildCurrentPlayerHand();
    }
}
