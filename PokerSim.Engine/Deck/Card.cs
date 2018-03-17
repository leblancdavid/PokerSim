namespace PokerSim.Engine.Deck
{
    public class Card
    {
        public int Id { get; private set; }
        public CardSuit Suit { get; private set; }
        public int Value { get; private set; }
    }
}