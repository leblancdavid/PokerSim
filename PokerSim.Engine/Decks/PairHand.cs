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
            foreach (var card in Cards)
            {
                Score += scoreFactor * card.Value;
                scoreFactor /= 10;
            }
        }

        public static PairHand GetHandFromCards(IEnumerable<Card> cards)
        {
            var tempList = cards.ToList();
            //Todo how to finger this one out!
            var pair = tempList.GroupBy(x => x.Value).Where(g => g.Count() == 2).Select(y => y.Key);

            return new PairHand(cards.OrderByDescending(x => x.Value).Take(5));
        }

        public override bool IsValid => IsPairHand(Cards) && Cards.Count() == 5;

        public static bool IsPairHand(IEnumerable<Card> cards)
        {
            return cards.Distinct().Count() == cards.Count() - 1;
        }
    }
}
