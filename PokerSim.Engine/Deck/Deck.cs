namespace PokerSim.Engine.Deck
{
    public class Deck
    {
        private List<Card> _cards;
        public IEnumerable<Card> Cards => _cards;
        public bool IsEmpty
        {
            get
            {
                if(_cards.Count == 0)
                {
                    return true;
                }
                return false;
            }
        }

        private void Initialize()
        {
            
        }
    }
}