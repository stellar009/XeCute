using UnityEngine;
using UnityEngine.UI;
using TMPro;
using XeCute.Events;
using XeCute.Core;

namespace XeCute.UI
{
    /// <summary>
    /// Shows the face-value sum (used for lucky-number matching) and cards remaining.
    /// v2: face sum is what's compared to lucky number, not card score sum.
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class SumDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text sumText;
        [SerializeField] private Color normalColor = Color.black;
        [SerializeField] private Color winColor = new Color(0.3f, 0.7f, 0.3f);
        [SerializeField] private Color loseColor = new Color(0.7f, 0.3f, 0.3f);

        private int currentSum = 0;
        private int currentCount = 0;

        private void Awake()
        {
            if (sumText == null) sumText = GetComponent<TMP_Text>();
            Clear();
        }

        private void OnEnable()
        {
            GameEvents.OnFaceSumUpdated += UpdateSum;
            GameEvents.OnCardsReset += Clear;
            GameEvents.OnOutcomeResolved += HandleOutcome;
        }

        private void OnDisable()
        {
            GameEvents.OnFaceSumUpdated -= UpdateSum;
            GameEvents.OnCardsReset -= Clear;
            GameEvents.OnOutcomeResolved -= HandleOutcome;
        }

        private void UpdateSum(int sum, int count)
        {
            currentSum = sum;
            currentCount = count;
            if (sumText != null)
            {
                sumText.text = $"Sum: {sum}  (Cards: {count}/5)";
                sumText.color = normalColor;
            }
        }

        private void HandleOutcome(RoundOutcome outcome)
        {
            if (sumText == null) return;
            switch (outcome)
            {
                case RoundOutcome.Win:
                    sumText.color = winColor;
                    sumText.text = $"WIN!";
                    break;
                case RoundOutcome.Lose:
                    sumText.color = loseColor;
                    sumText.text = $"LOSE";
                    break;
            }
        }

        public void Clear()
        {
            currentSum = 0;
            currentCount = 0;
            if (sumText != null)
            {
                sumText.text = "Sum: 0  (Cards: 0/5)";
                sumText.color = normalColor;
            }
        }
    }
}
