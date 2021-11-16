using System.Collections.Generic;

namespace PokerSim.Engine.Decks
{
    public class Card
    {
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
                    case 10:
                        return $"T{SuitToCharacter(Suit)}";
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
            return ((char)suit).ToString();
        }

        public Card(CardSuit suit, int value)
        {
            Suit = suit;
            Value = value;
        }

        public static Card StringToCard(string cardString)
        {
            if (string.IsNullOrEmpty(cardString) || cardString.Length != 2)
            {
                return null;
            }

            int value; 
            if (cardString[0] == 'T')
                value = 10;
            else if (cardString[0] == 'J')
                value = 11;
            else if (cardString[0] == 'Q')
                value = 12;
            else if (cardString[0] == 'K')
                value = 13;
            else if (cardString[0] == 'A')
                value = 14;
            else
            {
                value = int.Parse(cardString[0].ToString());
            }

            return new Card((CardSuit)cardString[1], value);
        }

        public static IEnumerable<Card> StringToCards(string cardsString)
        {
            var cards = new List<Card>();
            if (string.IsNullOrEmpty(cardsString) || cardsString.Length % 2 != 0)
            {
                return cards;
            }

            for (int i = 0; i < cardsString.Length; i += 2)
            {
                cards.Add(StringToCard(cardsString.Substring(i, 2)));
            }
            return cards;
        }
    }
}