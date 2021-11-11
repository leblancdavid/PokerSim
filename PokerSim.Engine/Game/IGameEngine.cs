using PokerSim.Engine.Decks;
using PokerSim.Engine.Players;
using System.Collections.Generic;

namespace PokerSim.Engine.Game
{
    public interface IGameEngine
    {
        IHandBuilder HandBuilder { get; }
        Deck Deck { get; }
        //IEnumerable<IPlayerState> Players { get; }
        IEnumerable<Card> CommunityCards { get; }
        //PotState CurrentPot { get; }
        int SmallBlindIndex { get; }
        int SmallBlindValue { get; }
        int BigBlindIndex { get; }
        int BigBlindValue { get; }
        int CurrentPlayerIndex { get; }

        void AddPlayer(IPlayer player);

        HandResult PlayHand();

        void Play();
    }
}
