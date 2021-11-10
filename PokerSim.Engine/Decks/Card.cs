namespace PokerSim.Engine.Decks
{
    public class Card
    {
        public int Id { get; private set; }
        public CardSuit Suit { get; private set; }
        public int Value { get; private set; }
        public string FullName 
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

        public string ShortName
        {
            get
            {
                switch (Value)
                {
                    case 11:
                        return $"J{SuitToCharacter(Suit)}";
                    case 12:
                        return $"Q{SuitToCharacter(Suit)}";
                    case 13:
                        return $"K{SuitToCharacter(Suit)}";
                    case 14:
                        return $"A{SuitToCharacter(Suit)}";
                    default:
                        return $"{Value}{SuitToCharacter(Suit)}";
                }
            }
        }

        public static string SuitToCharacter(CardSuit suit)
        {
            switch (suit)
            {
                case CardSuit.Heart:
                    return "♡";
                case CardSuit.Spade:
                    return "♠";
                case CardSuit.Club:
                    return "♣";
                case CardSuit.Diamond:
                    return "♢";
                default:
                    return "";
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