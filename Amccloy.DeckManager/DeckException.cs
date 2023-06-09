using System;

namespace Amccloy.DeckManager
{
    public class DeckException : Exception {
        public DeckException(string message)
            : base(message)
        {
        }

        public DeckException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}