using PokerSim.Engine.Decks;
using PokerSim.Engine.Players;
using System;
using System.Collections.Generic;

namespace PokerSim.Engine.Game
{
    internal interface IPlayerState
    {
        Guid PlayerId { get; }
        IPlayer Player { get; }
        int ChipCount { get; set; }
        IEnumerable<Card> Cards { get; }
        bool HasFolded { get; }
        bool IsEliminated { get; }
        void Deal(Card card);
        void Fold();
    }
}
