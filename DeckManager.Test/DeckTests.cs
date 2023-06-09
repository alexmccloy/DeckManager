using System.Collections.Generic;
using Amccloy.DeckManager;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DeckManager.Test;

[TestClass]
public class DeckTests
{
    [TestMethod]
    public void TestDrawPileContainsAllCards()
    {
        var deck = new StandardDeck<StandardCard>(GenerateTestCards(5));

        for (int i = 0; i < 5; i++)
        {
            deck.DrawPile.Should().ContainSingle(card => card.Id == i);
        }
    }
    
    [TestMethod]
    public void TestDrawingMovesCardToDiscard()
    {
        var deck = new StandardDeck<StandardCard>(GenerateTestCards(5));

        for (int i = 0; i < 5; i++)
        {
            var drawnCard = deck.Draw();
            deck.DrawPile.Should().NotContain(card => card.Id == drawnCard.Id);
            deck.DiscardPile.Should().Contain(card => card.Id == drawnCard.Id);
        }
    }
    
    [TestMethod]
    public void TestThrowExceptionWhenDrawingFromEmptyDrawPile()
    {
        var deck = new StandardDeck<StandardCard>(GenerateTestCards(5));

        for (int i = 0; i < 5; i++)
        {
            deck.Draw();
        }

        deck.Invoking(d => d.Draw())
            .Should().Throw<DeckException>()
            .WithMessage("Cannot draw from deck with no cards in it");
    } 

    public IList<StandardCard> GenerateTestCards(int cardCount)
    {
        var cards = new List<StandardCard>();
        for (int i = 0; i < cardCount; i++)
        {
            cards.Add(new StandardCard(i, $"Card {i}"));
        }

        return cards;
    }
}