using PokerSim.Engine.Decks;
using PokerSim.Engine.Players;
using System.Collections.Generic;

namespace PokerSim.Engine.Game
{
    public interface IGameEventLogger
    {
        void Log(TexasHoldemStages stage, IEnumerable<Card> communityCards);
        void Log(HandResult handResult);
        void Log(TurnResult turnResult);

    }
}
