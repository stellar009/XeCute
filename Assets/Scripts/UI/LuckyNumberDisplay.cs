using UnityEngine;
using UnityEngine.UI;
using TMPro;
using XeCute.Events;

namespace XeCute.UI
{
    /// <summary>
    /// Big lucky number display in the central play area.
    /// Shows "LUCKY NUMBER: 12" prominently above the cards.
    /// </summary>
    public class LuckyNumberDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text label;   // "LUCKY NUMBER"
        [SerializeField] private TMP_Text value;   // the number (big)
        [SerializeField] private Color normalColor = new Color(0.1f, 0.43f, 0.49f);
        [SerializeField] private Color matchColor = new Color(0.18f, 0.55f, 0.34f);

        private void OnEnable()
        {
            GameEvents.OnLuckyNumberChanged += UpdateNumber;
            GameEvents.OnOutcomeResolved += HandleOutcome;
            GameEvents.OnCardsReset += ResetColor;
        }

        private void OnDisable()
        {
            GameEvents.OnLuckyNumberChanged -= UpdateNumber;
            GameEvents.OnOutcomeResolved -= HandleOutcome;
            GameEvents.OnCardsReset -= ResetColor;
        }

        private void Start()
        {
            if (label != null) label.text = "LUCKY NUMBER";
            if (value != null) value.color = normalColor;
        }

        private void UpdateNumber(int num)
        {
            if (value != null) value.text = num.ToString();
        }

        private void HandleOutcome(XeCute.Core.RoundOutcome outcome)
        {
            if (value != null && outcome == XeCute.Core.RoundOutcome.Win)
            {
                value.color = matchColor;
            }
        }

        private void ResetColor()
        {
            if (value != null) value.color = normalColor;
        }
    }
}
