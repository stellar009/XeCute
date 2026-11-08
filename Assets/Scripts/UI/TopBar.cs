using UnityEngine;
using UnityEngine.UI;
using TMPro;
using XeCute.Events;
using XeCute.Core;

namespace XeCute.UI
{
    /// <summary>
    /// The top bar showing: Current Score label + 6-digit score wheel + ×N multiplier box.
    /// The × symbol is decorative; the small box shows the current multiplier value.
    /// </summary>
    public class TopBar : MonoBehaviour
    {
        [Header("Score")]
        [SerializeField] private TMP_Text scoreLabel;       // "Current Score"
        [SerializeField] private ScoreDigitWheel scoreWheel; // 6 circular digit wheels
        [SerializeField] private TMP_Text multiplierSymbol; // "×" (decorative)
        [SerializeField] private TMP_Text multiplierValue;  // "1.25" in the small box

        [Header("Colors")]
        [SerializeField] private Color normalColor = new Color(0.1f, 0.1f, 0.1f);
        [SerializeField] private Color bonusActiveColor = new Color(0.83f, 0.53f, 0.35f); // graphite orange

        private void OnEnable()
        {
            GameEvents.OnTotalScoreChanged += UpdateScoreText;
            GameEvents.OnBonusCalculated += UpdateMultiplier;
            GameEvents.OnCardsReset += ClearMultiplier;
            GameEvents.OnLiveBonusCalculated += UpdateLiveMultiplier;
        }

        private void OnDisable()
        {
            GameEvents.OnTotalScoreChanged -= UpdateScoreText;
            GameEvents.OnBonusCalculated -= UpdateMultiplier;
            GameEvents.OnCardsReset -= ClearMultiplier;
            GameEvents.OnLiveBonusCalculated -= UpdateLiveMultiplier;
        }

        private void Start()
        {
            if (scoreLabel != null) scoreLabel.text = "Current Score";
            if (multiplierSymbol != null) multiplierSymbol.text = "×";
            if (multiplierValue != null)
            {
                multiplierValue.text = "1.00";
                multiplierValue.color = normalColor;
            }
        }

        // Score is displayed by the ScoreDigitWheel component directly via its own event subscription.
        // This method is for any auxiliary score text if needed (currently unused but kept for extensibility).
        private void UpdateScoreText(int score)
        {
            // ScoreDigitWheel handles the actual digit display.
            // If you want a plain-text score somewhere too, update it here.
        }

        // Live bonus (updated after each card draw)
        private void UpdateLiveMultiplier(BonusCalculator.BonusResult bonus)
        {
            RenderMultiplier(bonus);
        }

        // Final bonus (at round end)
        private void UpdateMultiplier(BonusCalculator.BonusResult bonus)
        {
            RenderMultiplier(bonus);
        }

        private void RenderMultiplier(BonusCalculator.BonusResult bonus)
        {
            if (multiplierValue != null)
            {
                // Show the multiplier value (e.g. "1.25", "1.75", "2.50")
                // If flat bonus is also active (Unique Ones +5000), show both
                if (bonus.flatBonus > 0)
                {
                    multiplierValue.text = $"{bonus.totalMultiplier:F2}\n+{bonus.flatBonus}";
                }
                else
                {
                    multiplierValue.text = $"{bonus.totalMultiplier:F2}";
                }
                multiplierValue.color = bonus.triggeredBonuses.Count > 0 ? bonusActiveColor : normalColor;
            }
        }

        private void ClearMultiplier()
        {
            if (multiplierValue != null)
            {
                multiplierValue.text = "1.00";
                multiplierValue.color = normalColor;
            }
        }
    }
}
