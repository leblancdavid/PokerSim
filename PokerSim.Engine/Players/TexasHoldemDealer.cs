using System.Collections.Generic;
using PokerSim.Engine.Decks;

namespace PokerSim.Engine.Players
{
    public class TexasHoldemDealer
    {
        private Deck _deck;
        private List<TexasHoldemPlayer> _players;

        public TexasHoldemDealer(List<TexasHoldemPlayer> players)
        {
            _deck = new Deck();
            _deck.Shuffle();
            _players = players;
        }
    }
}