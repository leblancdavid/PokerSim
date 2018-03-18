using System.Collections.Generic;
using PokerSim.Engine.Decks;

namespace PokerSim.Engine.Game
{
    public class TexasHoldemGameState
    {
        public int CurrentBet { get; private set; }
        public int CurrentPot { get; private set; }
        public int PlayerChips { get; private set; }
        public List<Card> PlayerCards;
        public List<Card> CommunityCards;
    }
}