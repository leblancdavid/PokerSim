namespace PokerSim.Engine.Decks
{
    public class Card
    {
        public int Id { get; private set; }
        public CardSuit Suit { get; private set; }
        public int Value { get; private set; }
        public string Name 
        {
            get
            {
                switch(Value)
                {
                    case 11:
                        return $"Jack of {Suit.ToString()}";
                    case 12:
                        return $"Queen of {Suit.ToString()}";
                    case 13:
                        return $"King of {Suit.ToString()}";
                    case 14:
                        return $"Ace of {Suit.ToString()}";
                    default:
                        return $"{Value} of {Suit.ToString()}";
                }
            }
        }

        public Card(int id, CardSuit suit, int value)
        {
            Id = id;
            Suit = suit;
            Value = value;
        }
    }
}