using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSim.Engine.Decks
{
    public class PairHand : BaseHand
    {
        public PairHand(IEnumerable<Card> pair, IEnumerable<Card> cards)
            : base(HandType.Pair, pair.Concat(cards))
        {
            Score = 0;
            long scoreFactor = 100000;
            foreach(var pairCard in pair)
            {
                Score += scoreFactor * pairCard.Value;
            }
            scoreFactor /= 10;

            foreach (var card in cards)
            {
                Score += scoreFactor * card.Value;
                scoreFactor /= 10;
            }
        }

        public static PairHand GetHandFromCards(IEnumerable<Card> cards)
        {
            var tempList = cards.ToList();
            //Todo how to finger this one out!
            var pairGroup = tempList.GroupBy(x => x.Value).Where(g => g.Count() == 2).FirstOrDefault();
            if(pairGroup == null)
            {
                //Invalid pair...
                return new PairHand(new List<Card>(), new List<Card>());
            }

            var pair = cards.Where(x => x.Value == pairGroup.Key);
            tempList.RemoveAll(x => x.Value == pairGroup.Key);
            
            return new PairHand(pair, tempList.OrderByDescending(x => x.Value).Take(3));
        }

        public override bool IsValid => IsPairHand(Cards) && Cards.Count() == 5;

        public static bool IsPairHand(IEnumerable<Card> cards)
        {
            return cards.GroupBy(x => x.Value).Where(g => g.Count() == 2).Count() == 1;
        }
    }
}
