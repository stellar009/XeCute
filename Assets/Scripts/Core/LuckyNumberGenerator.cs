using UnityEngine;

namespace XeCute.Core
{
    /// <summary>
    /// Generates a lucky number between luckyMin and luckyMax inclusive.
    /// v2: no bonus round — lucky number is always fresh each round.
    /// </summary>
    public class LuckyNumberGenerator
    {
        private readonly GameConfig config;
        private int currentLuckyNumber;

        public int Current => currentLuckyNumber;

        public LuckyNumberGenerator(GameConfig config)
        {
            this.config = config;
        }

        /// <summary>
        /// Roll a fresh lucky number for a new round.
        /// </summary>
        public int RollNew()
        {
            currentLuckyNumber = Random.Range(config.luckyMin, config.luckyMax + 1);
            Events.GameEvents.RaiseLuckyNumberChanged(currentLuckyNumber);
            return currentLuckyNumber;
        }
    }
}
