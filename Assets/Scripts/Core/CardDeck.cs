using UnityEngine;

namespace XeCute.Core
{
    /// <summary>
    /// Draws single cards (random ints in [cardMin, cardMax]) with replacement.
    /// v2: one card at a time, called by RoundManager on each DRAW press.
    /// </summary>
    public class CardDeck
    {
        private readonly GameConfig config;

        public CardDeck(GameConfig config)
        {
            this.config = config;
        }

        /// <summary>
        /// Draw one card. Values are in [cardMin, cardMax], with replacement.
        /// </summary>
        public int DrawOne()
        {
            return Random.Range(config.cardMin, config.cardMax + 1);
        }
    }
}
