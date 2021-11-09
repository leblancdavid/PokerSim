using PokerSim.Engine.Decks;
using System.Collections.Generic;

namespace PokerSim.Engine.Game
{
    public interface IPlayerTurnState
    {
        int CurrentBet { get; }
        int CurrentPot { get; }
        int NumRemainingPlayersInHand { get; }

        IEnumerable<Card> CommunityCards { get; }
        IEnumerable<Card> PlayerCards { get; }
    }
}
