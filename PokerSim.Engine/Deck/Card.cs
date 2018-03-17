namespace PokerSim.Engine.Deck
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
                    case 1:
                        return $"Ace of {Suit.ToString()}";
                    case 10:
                        return $"Jack of {Suit.ToString()}";
                    case 11:
                        return $"Queen of {Suit.ToString()}";
                    case 12:
                        return $"King of {Suit.ToString()}";
                    default:
                        return $"{Value} of {Suit.ToString()}";
                }
            }
        }
    }
}