using UnityEngine;
using UnityEngine.UI;
using TMPro;
using XeCute.Core;
using XeCute.Events;

namespace XeCute.UI
{
    /// <summary>
    /// Controls a single card slot in the Card View area.
    /// v2: listens to OnCardDrawn (one card at a time), shows face value + score value.
    /// Light juice: simple scale pop on reveal.
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class CardSlot : MonoBehaviour
    {
        [Header("Slot Identity")]
        [Tooltip("Index 0-4, matches the order cards are dealt")]
        public int slotIndex;

        [Header("Visuals")]
        [SerializeField] private TMP_Text faceValueText;    // big number (1-5)
        [SerializeField] private TMP_Text scoreValueText;   // small score (10, 40, 90, 160, 250)
        [SerializeField] private Image cardImage;
        [SerializeField] private Color backColor = new Color(0.94f, 0.94f, 0.94f);
        [SerializeField] private Color faceColor = Color.white;
        [SerializeField] private Color winColor = new Color(0.4f, 0.8f, 0.4f);
        [SerializeField] private Color loseColor = new Color(0.8f, 0.4f, 0.4f);

        [Header("Light Juice")]
        [SerializeField] private float popScale = 1.15f;
        [SerializeField] private float popDuration = 0.18f;

        private Vector3 baseScale;

        private void Awake()
        {
            if (cardImage == null) cardImage = GetComponent<Image>();
            baseScale = transform.localScale;
            Clear();
        }

        private void OnEnable()
        {
            GameEvents.OnCardDrawn += HandleCardDrawn;
            GameEvents.OnCardsReset += Clear;
            GameEvents.OnOutcomeResolved += HandleOutcome;
        }

        private void OnDisable()
        {
            GameEvents.OnCardDrawn -= HandleCardDrawn;
            GameEvents.OnCardsReset -= Clear;
            GameEvents.OnOutcomeResolved -= HandleOutcome;
        }

        private void HandleCardDrawn(int index, int faceValue, int scoreValue)
        {
            if (index != slotIndex) return;
            StopAllCoroutines();
            StartCoroutine(RevealCard(faceValue, scoreValue));
        }

        private void HandleOutcome(RoundOutcome outcome)
        {
            Color c = cardImage.color;
            switch (outcome)
            {
                case RoundOutcome.Win:
                    c = winColor;
                    break;
                case RoundOutcome.Lose:
                    c = loseColor;
                    break;
            }
            cardImage.color = c;
        }

        private System.Collections.IEnumerator RevealCard(int faceValue, int scoreValue)
        {
            // Pop up
            float t = 0f;
            while (t < popDuration)
            {
                t += Time.deltaTime;
                float k = t / popDuration;
                transform.localScale = Vector3.Lerp(baseScale, baseScale * popScale, k);
                yield return null;
            }
            // Flip: show values
            if (faceValueText != null) faceValueText.text = faceValue.ToString();
            if (scoreValueText != null) scoreValueText.text = "+" + scoreValue;
            cardImage.color = faceColor;
            // Pop back
            t = 0f;
            while (t < popDuration)
            {
                t += Time.deltaTime;
                float k = t / popDuration;
                transform.localScale = Vector3.Lerp(baseScale * popScale, baseScale, k);
                yield return null;
            }
            transform.localScale = baseScale;
        }

        public void Clear()
        {
            if (faceValueText != null) faceValueText.text = "";
            if (scoreValueText != null) scoreValueText.text = "";
            if (cardImage != null) cardImage.color = backColor;
            transform.localScale = baseScale;
        }
    }
}
