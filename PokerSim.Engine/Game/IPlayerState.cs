﻿using PokerSim.Engine.Decks;
using PokerSim.Engine.Players;
using System.Collections.Generic;

namespace PokerSim.Engine.Game
{
    internal interface IPlayerState
    {
        IPlayer Player { get; }
        int ChipCount { get; set; }
        IEnumerable<Card> Cards { get; }
        bool IsFolded { get; }
        bool IsEliminated { get; }
        void Deal(Card card);
        void Fold();
    }
}