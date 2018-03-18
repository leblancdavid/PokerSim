using System;
using System.Collections.Generic;
using PokerSim.Engine.Deck;

namespace PokerSim.Engine
{
    public static class CardListExtension
    {
        private static Random rng = new Random();

        public static void Shuffle(this IList<Card> list)  
        {  
            int n = list.Count;  
            while (n > 1) {  
                n--;  
                int k = rng.Next(n + 1);  
                Card value = list[k];  
                list[k] = list[n];  
                list[n] = value;  
            }  
        }

        public static Card Draw(this IList<Card> list)
        {
            if(list.Count == 0)
                return null;
            
            var card = list[0];
            list.RemoveAt(0);
            return card;
        }
    }
   
}