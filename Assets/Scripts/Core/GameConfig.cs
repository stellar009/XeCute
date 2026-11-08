using UnityEngine;

namespace XeCute.Core
{
    /// <summary>
    /// Configuration constants for XeCute (v2 — card-by-card draw + bonus scoring).
    /// </summary>
    [CreateAssetMenu(fileName = "GameConfig", menuName = "XeCute/Game Config")]
    public class GameConfig : ScriptableObject
    {
        [Header("Cards")]
        [Tooltip("Minimum card face value (inclusive)")]
        public int cardMin = 1;
        [Tooltip("Maximum card face value (inclusive)")]
        public int cardMax = 5;
        [Tooltip("Max cards drawn per round before forced lose")]
        public int maxCardsPerRound = 5;

        [Header("Card Score Values (index 0 = card 1, index 4 = card 5)")]
        public int[] cardScores = { 10, 40, 90, 160, 250 };

        [Header("Lucky Number")]
        [Tooltip("Minimum lucky number (inclusive)")]
        public int luckyMin = 5;
        [Tooltip("Maximum lucky number (inclusive)")]
        public int luckyMax = 20;

        [Header("Bonus Multipliers")]
        [Tooltip("Dual Hand: 2+ cards of value 2 → multiply round score by this")]
        public float dualHandMultiplier = 1.25f;
        [Tooltip("Triplets: 3+ cards of value 3 → multiply round score by this")]
        public float tripletsMultiplier = 1.75f;
        [Tooltip("Quadrants: 2+ cards of value 4 → multiply by random in [min, max]")]
        public float quadrantsMin = 1.5f;
        public float quadrantsMax = 2.0f;
        [Tooltip("Packs of 5: 2+ cards of value 5 → multiply round score by this")]
        public float packsOf5Multiplier = 2.5f;
        [Tooltip("Unique Ones: all 5 cards unique (1,2,3,4,5) → add this flat bonus")]
        public int uniqueOnesFlatBonus = 5000;

        [Header("Timing (seconds)")]
        [Tooltip("Card flip animation duration")]
        public float cardFlipDuration = 0.25f;
        [Tooltip("Pause after each card reveal before player can draw again")]
        public float afterCardDelay = 0.3f;
        [Tooltip("Pause after round ends before showing outcome")]
        public float resolveDelay = 0.5f;
        [Tooltip("Pause after outcome before next round")]
        public float nextRoundDelay = 1.5f;

        /// <summary>
        /// Get the score value for a card face value (1-5).
        /// </summary>
        public int GetCardScore(int faceValue)
        {
            int index = faceValue - cardMin;
            if (index < 0 || index >= cardScores.Length) return 0;
            return cardScores[index];
        }
    }
}
