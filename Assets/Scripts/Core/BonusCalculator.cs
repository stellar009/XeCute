using System.Collections.Generic;
using UnityEngine;
using XeCute.Events;

namespace XeCute.Core
{
    /// <summary>
    /// Calculates bonus multipliers and flat bonuses based on cards drawn in a round.
    ///
    /// Bonus System:
    ///   1. Dual Hand:  2+ cards of value 2 → ×1.25
    ///   2. Triplets:   3+ cards of value 3 → ×1.75
    ///   3. Quadrants:  2+ cards of value 4 → ×1.5 to ×2.0 (random)
    ///   4. Packs of 5: 2+ cards of value 5 → ×2.5
    ///   5. Unique Ones: all 5 cards unique (1,2,3,4,5) → +5000 flat
    ///
    /// Multipliers stack (multiply together). Flat bonus added after multipliers.
    /// </summary>
    public static class BonusCalculator
    {
        public struct BonusResult
        {
            public float totalMultiplier;   // product of all active multipliers (1.0 if none)
            public int flatBonus;           // flat bonus added after multiplier (0 if none)
            public List<string> triggeredBonuses; // human-readable list for UI display
        }

        public static BonusResult Calculate(List<int> cards, GameConfig config)
        {
            var result = new BonusResult
            {
                totalMultiplier = 1f,
                flatBonus = 0,
                triggeredBonuses = new List<string>()
            };

            if (cards == null || cards.Count == 0) return result;

            // Count occurrences of each face value
            var counts = new Dictionary<int, int>();
            foreach (int c in cards)
            {
                if (!counts.ContainsKey(c)) counts[c] = 0;
                counts[c]++;
            }

            // 1. Dual Hand: 2+ cards of value 2
            if (counts.ContainsKey(2) && counts[2] >= 2)
            {
                result.totalMultiplier *= config.dualHandMultiplier;
                result.triggeredBonuses.Add($"Dual Hand ×{config.dualHandMultiplier:F2}");
            }

            // 2. Triplets: 3+ cards of value 3
            if (counts.ContainsKey(3) && counts[3] >= 3)
            {
                result.totalMultiplier *= config.tripletsMultiplier;
                result.triggeredBonuses.Add($"Triplets ×{config.tripletsMultiplier:F2}");
            }

            // 3. Quadrants: 2+ cards of value 4 (random multiplier in [min, max])
            if (counts.ContainsKey(4) && counts[4] >= 2)
            {
                float quadMult = Random.Range(config.quadrantsMin, config.quadrantsMax);
                quadMult = Mathf.Round(quadMult * 100f) / 100f; // 2 decimal places
                result.totalMultiplier *= quadMult;
                result.triggeredBonuses.Add($"Quadrants ×{quadMult:F2}");
            }

            // 4. Packs of 5: 2+ cards of value 5
            if (counts.ContainsKey(5) && counts[5] >= 2)
            {
                result.totalMultiplier *= config.packsOf5Multiplier;
                result.triggeredBonuses.Add($"Packs of 5 ×{config.packsOf5Multiplier:F2}");
            }

            // 5. Unique Ones: all 5 cards unique (1,2,3,4,5)
            if (cards.Count == 5 && counts.Count == 5)
            {
                bool allPresent = counts.ContainsKey(1) && counts.ContainsKey(2) &&
                                  counts.ContainsKey(3) && counts.ContainsKey(4) &&
                                  counts.ContainsKey(5);
                if (allPresent)
                {
                    result.flatBonus += config.uniqueOnesFlatBonus;
                    result.triggeredBonuses.Add($"Unique Ones +{config.uniqueOnesFlatBonus}");
                }
            }

            return result;
        }

        /// <summary>
        /// Apply bonus to a base round score: (base × multiplier) + flat.
        /// </summary>
        public static int ApplyBonus(int baseScore, BonusResult bonus)
        {
            return Mathf.RoundToInt(baseScore * bonus.totalMultiplier) + bonus.flatBonus;
        }
    }
}
