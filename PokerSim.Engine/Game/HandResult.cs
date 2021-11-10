using PokerSim.Engine.Decks;
using PokerSim.Engine.Players;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PokerSim.Engine.Game
{
    public class HandResult
    {
        public IEnumerable<PlayerHandResult> PlayerResults { get; private set; }
        public IEnumerable<PlayerHandResult> Winners => PlayerResults.Where(x => x.Winnings > 0);
        public IEnumerable<PlayerHandResult> Losers => PlayerResults.Where(x => x.Winnings == 0);

        public HandResult(IEnumerable<PlayerHandResult> playerResults)
        {
            PlayerResults = playerResults;
        }
    }

    public class PlayerHandResult
    {
        public IPlayer Player { get; private set; }
        public IHand Hand { get; private set; }
        public int Winnings { get; set; }

        public PlayerHandResult(IPlayer player, IHand hand, int winnings = 0)
        {
            Player = player;
            Hand = hand;
            Winnings = winnings;
        }

    }
}
