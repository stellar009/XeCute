using UnityEngine;
using UnityEngine.UI;
using TMPro;
using XeCute.Events;

namespace XeCute.UI
{
    /// <summary>
    /// Displays the current round's accumulated score (sum of card score values,
    /// before bonus multiplier). Shows bonus breakdown when bonus is calculated.
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class RoundScoreDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text bonusText;       // optional: shows triggered bonuses
        [SerializeField] private TMP_Text finalScoreText;  // optional: shows final score after bonus

        private int currentRoundScore = 0;

        private void Awake()
        {
            if (scoreText == null) scoreText = GetComponent<TMP_Text>();
            Clear();
        }

        private void OnEnable()
        {
            GameEvents.OnRoundScoreUpdated += UpdateScore;
            GameEvents.OnCardsReset += Clear;
            GameEvents.OnBonusCalculated += ShowBonus;
            GameEvents.OnFinalRoundScoreAwarded += ShowFinal;
        }

        private void OnDisable()
        {
            GameEvents.OnRoundScoreUpdated -= UpdateScore;
            GameEvents.OnCardsReset -= Clear;
            GameEvents.OnBonusCalculated -= ShowBonus;
            GameEvents.OnFinalRoundScoreAwarded -= ShowFinal;
        }

        private void UpdateScore(int score)
        {
            currentRoundScore = score;
            if (scoreText != null) scoreText.text = "Round Score: " + score;
        }

        private void ShowBonus(XeCute.Core.BonusCalculator.BonusResult bonus)
        {
            if (bonusText != null)
            {
                if (bonus.triggeredBonuses.Count == 0)
                {
                    bonusText.text = "No bonus";
                }
                else
                {
                    bonusText.text = "Bonuses:\n" + string.Join("\n", bonus.triggeredBonuses);
                    bonusText.text += $"\nMultiplier: ×{bonus.totalMultiplier:F2}";
                    if (bonus.flatBonus > 0)
                        bonusText.text += $"\nFlat: +{bonus.flatBonus}";
                }
            }
        }

        private void ShowFinal(int finalScore)
        {
            if (finalScoreText != null)
            {
                finalScoreText.text = "Final Round Score: " + finalScore;
            }
        }

        public void Clear()
        {
            currentRoundScore = 0;
            if (scoreText != null) scoreText.text = "Round Score: 0";
            if (bonusText != null) bonusText.text = "";
            if (finalScoreText != null) finalScoreText.text = "";
        }
    }
}
