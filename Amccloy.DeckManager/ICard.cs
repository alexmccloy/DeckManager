namespace Amccloy.DeckManager
{
    public interface ICard
    {
        /// <summary>
        /// A unique ID for the card. This ID must be unique within a <see cref="IDeck{TCard}"/>
        /// </summary>
        int Id { get; }
        
        /// <summary>
        /// The name of the card. This can be repeated within a deck.
        /// </summary>
        string DisplayName { get; }
        
        string ImageUrl { get; }
    }

    public class StandardCard : ICard
    {
        public int Id { get; }
        public string DisplayName { get; }
        public string ImageUrl { get; }

        public StandardCard(int id, string displayName)
        {
            Id = id;
            DisplayName = displayName;
        }
    }
}