namespace PokerSim.Engine.Players
{
    public class PlayerTurn
    {
        public int Bet { get; private set; }
        public PlayerTurn(int bet)
        {
            Bet = bet;
        }
    }
}