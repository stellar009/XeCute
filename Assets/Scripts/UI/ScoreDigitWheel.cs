using UnityEngine;
using UnityEngine.UI;
using TMPro;
using XeCute.Events;

namespace XeCute.UI
{
    /// <summary>
    /// 6 circular digit wheels showing the total score (like an odometer).
    /// Each circle displays one digit 0-9. All start at 0.
    /// Example: score 125 → circles show "0 0 0 1 2 5"
    ///          score 55500 → circles show "0 5 5 5 0 0"
    /// Max displayable score: 999,999.
    /// </summary>
    public class ScoreDigitWheel : MonoBehaviour
    {
        [Tooltip("6 TMP_Text objects, left to right (most significant to least significant digit)")]
        [SerializeField] private TMP_Text[] digits = new TMP_Text[6];

        [Header("Colors")]
        [SerializeField] private Color zeroColor = new Color(0.66f, 0.66f, 0.66f);   // dim for leading zeros
        [SerializeField] private Color valueColor = new Color(0.1f, 0.1f, 0.1f);     // dark for actual digits
        [SerializeField] private Color circleBackColor = new Color(0.85f, 0.85f, 0.85f);

        private void OnEnable()
        {
            GameEvents.OnTotalScoreChanged += UpdateScore;
        }

        private void OnDisable()
        {
            GameEvents.OnTotalScoreChanged -= UpdateScore;
        }

        private void Start()
        {
            UpdateScore(0);
        }

        private void UpdateScore(int score)
        {
            if (digits == null || digits.Length < 6) return;

            // Clamp to 6 digits (max 999,999)
            int clamped = Mathf.Clamp(score, 0, 999999);

            // Convert to 6-digit zero-padded string
            string scoreStr = clamped.ToString("D6");

            // Track whether we've hit the first non-zero digit (for color styling)
            bool hitFirstValue = false;
            for (int i = 0; i < 6 && i < digits.Length; i++)
            {
                char c = scoreStr[i];
                if (digits[i] != null)
                {
                    digits[i].text = c.ToString();
                    // Leading zeros are dim; actual value digits are dark
                    bool isLeadingZero = (c == '0' && !hitFirstValue);
                    digits[i].color = isLeadingZero ? zeroColor : valueColor;
                }
                if (c != '0') hitFirstValue = true;
            }
        }

        /// <summary>
        /// Manually set the score (for initialization or testing).
        /// </summary>
        public void SetScore(int score)
        {
            UpdateScore(score);
        }
    }
}
