using UnityEngine;
using UnityEngine.UI;
using TMPro;
using XeCute.Core;
using XeCute.Events;

namespace XeCute.UI
{
    /// <summary>
    /// Manages the bottom Controls bar buttons (Pause, Reset, DRAW).
    /// v2: DRAW is pressed once per card (up to 5 times per round).
    /// </summary>
    public class ControlsBar : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button drawButton;
        [SerializeField] private Button resetButton;

        [Header("References")]
        [SerializeField] private RoundManager roundManager;

        [Header("Optional visual hint on DRAW")]
        [SerializeField] private TMP_Text drawButtonText;

        private void OnEnable()
        {
            GameEvents.OnStateChanged += HandleStateChanged;
        }

        private void OnDisable()
        {
            GameEvents.OnStateChanged -= HandleStateChanged;
        }

        private void Start()
        {
            if (drawButton != null) drawButton.onClick.AddListener(OnDrawClicked);
            if (resetButton != null) resetButton.onClick.AddListener(OnResetClicked);
        }

        private void HandleStateChanged(RoundState state)
        {
            if (drawButton != null)
            {
                // DRAW is only interactable in Idle state AND when cards remain
                bool cardsLeft = roundManager == null || roundManager.CardsRemaining > 0;
                drawButton.interactable = (state == RoundState.Idle) && cardsLeft;
            }
            if (drawButtonText != null)
            {
                if (state == RoundState.GameOver)
                {
                    drawButtonText.text = "RETRY";
                }
                else
                {
                    drawButtonText.text = "DRAW";
                }
            }
        }

        public void OnDrawClicked()
        {
            if (roundManager == null) return;
            if (roundManager.State == RoundState.GameOver)
            {
                roundManager.StartNewRun();
            }
            else
            {
                roundManager.DrawCard();
            }
        }

        public void OnResetClicked() => roundManager?.ResetRun();

        public void OnResetConfirmYes() => roundManager?.ResetRun();
    }
}
