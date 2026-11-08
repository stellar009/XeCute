using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using XeCute.Events;
using XeCute.Core;

namespace XeCute.UI
{
    /// <summary>
    /// Bottom-left panel showing live bonus detection.
    /// Updates after each card draw to show which bonuses are currently active
    /// (Dual Hand, Triplets, Quadrants, Packs of 5, Unique Ones).
    /// Matches the blueprint's "Pair of two / Dual Hand" panel.
    /// </summary>
    public class BonusPairPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text panelTitle;     // "BONUSES"
        [SerializeField] private TMP_Text bonusListText;  // multi-line list of active bonuses
        [SerializeField] private TMP_Text totalMultiplierText; // "×1.25" running total

        [Header("Colors")]
        [SerializeField] private Color activeColor = new Color(0.83f, 0.53f, 0.35f);   // graphite orange
        [SerializeField] private Color inactiveColor = new Color(0.66f, 0.66f, 0.66f);
        [SerializeField] private Color titleColor = new Color(0.1f, 0.1f, 0.1f);

        // Bonus display names
        private static readonly string[] BonusNames = {
            "Dual Hand (2×'2')",
            "Triplets (3×'3')",
            "Quadrants (2×'4')",
            "Packs of 5 (2×'5')",
            "Unique Ones (1-5)"
        };

        private void OnEnable()
        {
            GameEvents.OnLiveBonusCalculated += UpdateLiveBonus;
            GameEvents.OnBonusCalculated += UpdateFinalBonus;
            GameEvents.OnCardsReset += ClearDisplay;
        }

        private void OnDisable()
        {
            GameEvents.OnLiveBonusCalculated -= UpdateLiveBonus;
            GameEvents.OnBonusCalculated -= UpdateFinalBonus;
            GameEvents.OnCardsReset -= ClearDisplay;
        }

        private void Start()
        {
            if (panelTitle != null)
            {
                panelTitle.text = "BONUSES";
                panelTitle.color = titleColor;
            }
            ClearDisplay();
        }

        private void UpdateLiveBonus(BonusCalculator.BonusResult bonus)
        {
            RenderBonus(bonus, false);
        }

        private void UpdateFinalBonus(BonusCalculator.BonusResult bonus)
        {
            RenderBonus(bonus, true);
        }

        private void RenderBonus(BonusCalculator.BonusResult bonus, bool isFinal)
        {
            if (bonusListText != null)
            {
                if (bonus.triggeredBonuses == null || bonus.triggeredBonuses.Count == 0)
                {
                    bonusListText.text = "No active bonuses";
                    bonusListText.color = inactiveColor;
                }
                else
                {
                    bonusListText.text = string.Join("\n", bonus.triggeredBonuses);
                    bonusListText.color = activeColor;
                }
            }

            if (totalMultiplierText != null)
            {
                totalMultiplierText.text = $"x{bonus.totalMultiplier:F2}";
                if (bonus.flatBonus > 0)
                {
                    totalMultiplierText.text += $" +{bonus.flatBonus}";
                }
                totalMultiplierText.color = bonus.triggeredBonuses.Count > 0 ? activeColor : inactiveColor;
            }
        }

        private void ClearDisplay()
        {
            if (bonusListText != null)
            {
                bonusListText.text = "No active bonuses";
                bonusListText.color = inactiveColor;
            }
            if (totalMultiplierText != null)
            {
                totalMultiplierText.text = "×1.00";
                totalMultiplierText.color = inactiveColor;
            }
        }
    }
}
