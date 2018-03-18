using System.Collections.Generic;
using PokerSim.Engine.Decks;
using PokerSim.Engine.Players;

namespace PokerSim.Engine.Game
{
    public class TexasHoldemHand
    {
        private Deck _deck;
        private List<TexasHoldemPlayer> _players;

        public TexasHoldemHand(List<TexasHoldemPlayer> players)
        {
            _deck = new Deck();
            _deck.Shuffle();
            _players = players;
        }

    }
}