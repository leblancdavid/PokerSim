using System.Collections.Generic;
using PokerSim.Engine.Decks;

namespace PokerSim.Engine.Players
{
    public class TexasHoldemPlayerTurnState
    {
        public int CurrentBet { get; private set; }
        public int CurrentPot { get; private set; }
        public int PlayerChips { get; private set; }
        private List<Card> _playerCards = new List<Card>();
        public IEnumerable<Card> PlayerCards => _playerCards;
        private List<Card> _communityCards = new List<Card>();
        public IEnumerable<Card> CommunityCards => _communityCards;

        public TexasHoldemPlayerTurnState(List<Card> playerCards,
            List<Card> communityCards,
            int currentBet,
            int currentPot,
            int playerChips)
        {
            _playerCards = playerCards;
            _communityCards = communityCards;
            CurrentBet = currentBet;
            CurrentPot = currentPot;
            PlayerChips = playerChips;
        }
    }
}