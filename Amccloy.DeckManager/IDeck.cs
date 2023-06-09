using System;
using System.Collections.Generic;
using System.Linq;

namespace Amccloy.DeckManager
{
    public interface IDeck<TCard> where TCard : ICard
    {
        IList<TCard> DrawPile { get; }
        IList<TCard> DiscardPile { get; }
        IList<TCard> BanishedPile { get; }

        /// <summary>
        /// Take the top card from the <see cref="DrawPile"/> and move it to the discard pile. That card is then returned
        /// </summary>
        TCard Draw();
        
        /// <summary>
        /// Remove an exact card from either the <see cref="DrawPile"/> or <see cref="DiscardPile"/> and move it to the
        /// <see cref="BanishedPile"/>
        /// </summary>
        void Banish(int cardId);
        void Banish(TCard card);
        
        /// <summary>
        /// Move all cards from <see cref="DiscardPile"/> into <see cref="DrawPile"/> and randomise the order
        /// </summary>
        void Shuffle();
    }

    public class StandardDeck<TCard> : IDeck<TCard> where TCard : ICard
    {
        public IList<TCard> DrawPile { get; private set; }
        public IList<TCard> DiscardPile { get; private set;  }
        public IList<TCard> BanishedPile { get; private set;  }

        // Reference to the original cards that were passed in
        private IList<TCard> _allCards;
        
        // For shuffling cards
        private static Random rng = new Random();

        public StandardDeck(IList<TCard> cards)
        {
            _allCards = cards;

            DrawPile = new List<TCard>(_allCards);
            DiscardPile = new List<TCard>();
            BanishedPile = new List<TCard>();
            
            // All the cards are now in the draw pile so shuffle it
            Shuffle();
        }

        public TCard Draw()
        {
            if (DrawPile.Count == 0)
            {
                throw new DeckException("Cannot draw from deck with no cards in it");
            }
            
            // Take the top card from the Draw pile and move it to the discard pile
            var card = DrawPile[0];
            DrawPile.RemoveAt(0);
            DiscardPile.Insert(0, card);
            return card;
        }

        public void Banish(int cardId)
        {
            // Find the card we need to banish
            var card = _allCards.SingleOrDefault(c => c.Id == cardId);

            if (card == null) throw new DeckException($"Cannot find card with id {cardId}");
            
            Banish(card);
        }

        public void Banish(TCard card)
        {
            // Try to banish from Draw deck
            if (!DrawPile.Remove(card))
            {
                // Try to banish from discard deck
                if (!DiscardPile.Remove(card))
                {
                    // We cant find the card
                    throw new DeckException($"Cannot find card to banish with id {card.Id} and name {card.DisplayName}");
                }
            }
            
            BanishedPile.Insert(0, card);
        }

        public void Shuffle()
        {
            // Add discard pile to draw pile and randomise order
            foreach (var card in DiscardPile)
            {
                DrawPile.Add(card);
            }
            DiscardPile.Clear();
            
            DrawPile = DrawPile.OrderBy(a => rng.Next()).ToList();
        }

    }
}