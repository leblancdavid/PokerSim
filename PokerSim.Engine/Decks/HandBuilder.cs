using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSim.Engine.Decks
{
    public class HandBuilder : IHandBuilder
    {
        protected List<IHandBuilder> _builders = new List<IHandBuilder>();

        public HandBuilder()
        {
            _builders.Add(new StraightFlushHandBuilder());
            _builders.Add(new FourOfAKindHandBuilder());
            _builders.Add(new FullHouseHandBuilder());
            _builders.Add(new FlushHandBuilder());
            _builders.Add(new StraightHandBuilder());
            _builders.Add(new ThreeOfAKindHandBuilder());
            _builders.Add(new TwoPairHandBuilder());
            _builders.Add(new PairHandBuilder());
            _builders.Add(new HighCardHandBuilder());
        }

        public IHand BuildHand(IEnumerable<Card> cards)
        {
            foreach(var builder in _builders)
            {
                if(builder.ContainsHand(cards))
                {
                    return builder.BuildHand(cards);
                }
            }

            return new HighCardHand(cards.OrderByDescending(x => x.Value));
        }

        public bool ContainsHand(IEnumerable<Card> cards)
        {
            return true;
        }
    }
}
