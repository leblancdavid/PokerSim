using PokerSim.Engine.Players;
using System.Linq;

namespace PokerSim.Engine.Game
{
    public class TexasHoldemGameEngine : IGameEngine
    {
        private InternalGameState _state;
        private int _initialChips = 50;
        private IGameEventLogger _logger;
        public TexasHoldemGameEngine(IGameEventLogger logger)
        {
            _logger = logger;
            _state = new InternalGameState(logger);
        }

        public void AddPlayer(IPlayer player)
        {
            _state.AddPlayer(new InternalPlayerState(player, _initialChips));
        }

        public GameResult Play()
        {
            var gameResult = new GameResult();
            Reset();
            bool winner = false;
            while (!winner)
            {
                var handResult = _state.PlayHand();
                _logger?.Log(handResult);
                
                var eliminatedPlayers = _state.Players.Where(x => x.IsEliminated);
                foreach(var player in eliminatedPlayers)
                {
                    gameResult.NotifyPlayerEliminated(player.Player);
                }

                var remainingPlayers = _state.Players.Where(x => !x.IsEliminated);
                if (remainingPlayers.Count() == 1)
                {
                    winner = true;
                    gameResult.NotifyWinner(remainingPlayers.First().Player);
                    return gameResult;
                }    
            }
            return gameResult;
        }

        private void Reset()
        {
            _state.Reset(_initialChips);
        }

        
    }
}
