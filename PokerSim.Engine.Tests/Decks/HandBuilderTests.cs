using FluentAssertions;
using PokerSim.Engine.Decks;
using Xunit;

namespace PokerSim.Engine.Tests.Decks
{
    public class HandBuilderTests
    {
        private HandBuilder _handBuilder = new HandBuilder();
        public HandBuilderTests()
        {

        }

        [Fact]
        public void WhenProvidedWithAStraight_ShouldBuildAValidStraightHand()
        {
            var cards = Card.StringToCards($"A{(char)CardSuit.Club}K{(char)CardSuit.Diamond}Q{(char)CardSuit.Heart}J{(char)CardSuit.Spade}T{(char)CardSuit.Spade}");

            var hand = _handBuilder.BuildHand(cards);

            hand.HandType.Should().Be(HandType.Straight);
        }

        [Fact]
        public void WhenProvidedWithAStraightFlush_ShouldBuildAValidStraightFlushHand()
        {
            var cards = Card.StringToCards($"A{(char)CardSuit.Club}K{(char)CardSuit.Club}Q{(char)CardSuit.Club}J{(char)CardSuit.Club}T{(char)CardSuit.Club}");

            var hand = _handBuilder.BuildHand(cards);

            hand.HandType.Should().Be(HandType.RoyalFlush);
        }
    }
}
