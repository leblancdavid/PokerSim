namespace PokerSim.Engine.Players
{
    public class PlayerTurn
    {
        public static PlayerTurn Call()
        {
            return new PlayerTurn();
        }

        public static PlayerTurn Fold()
        {
            return new PlayerTurn();
        }

        public static PlayerTurn Raise(int chips)
        {
            return new PlayerTurn();
        }
    }
}