using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSim.Engine.Decks
{
    public class TwoPairHand : BaseHand
    {
        public TwoPairHand(IEnumerable<Card> highPair, IEnumerable<Card> lowPair, Card card)
            : base(HandType.TwoPair, highPair.Concat(lowPair).Concat(new List<Card>() { card }))
        {
            Score = 0;
            long scoreFactor = 100000;
            foreach(var pairCard in highPair)
            {
                Score += scoreFactor * pairCard.Value;
            }
            scoreFactor /= 10;

            foreach (var pairCard in lowPair)
            {
                Score += scoreFactor * pairCard.Value;
            }
            scoreFactor /= 10;

            Score += scoreFactor * card.Value;
        }

        public static TwoPairHand GetHandFromCards(IEnumerable<Card> cards)
        {
            var tempList = cards.ToList();
            //Todo how to finger this one out!
            var pairGroup = tempList.GroupBy(x => x.Value).Where(g => g.Count() == 2).Take(2).ToList();
            if(pairGroup.Count() != 2)
            {
                //Invalid pair...
                return new TwoPairHand(new List<Card>(), new List<Card>(), null);
            }

            pairGroup = pairGroup.OrderByDescending(x => x.Key).ToList();
            tempList.RemoveAll(x => x.Value == pairGroup[0].Key);
            tempList.RemoveAll(x => x.Value == pairGroup[1].Key);
            return new TwoPairHand(cards.Where(x => x.Value == pairGroup[0].Key),
                cards.Where(x => x.Value == pairGroup[1].Key),
                tempList.OrderByDescending(x => x.Value).FirstOrDefault());
        }

        public override bool IsValid => IsTwoPairHand(Cards) && Cards.Count() == 5;

        public static bool IsTwoPairHand(IEnumerable<Card> cards)
        {
            return cards.GroupBy(x => x.Value).Where(g => g.Count() == 2).Count() == 2;
        }
    }
}
