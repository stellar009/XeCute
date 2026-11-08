using UnityEngine;
using UnityEngine.UI;
using TMPro;
using XeCute.Core;
using XeCute.Events;

namespace XeCute.UI
{
    /// <summary>
    /// The game-over overlay. Shows final score, high score, player name, and Retry button.
    /// v2: shows player name with high score.
    /// </summary>
    public class GameOverScreen : MonoBehaviour
    {
        [SerializeField] private GameObject overlayRoot;
        [SerializeField] private TMP_Text finalScoreText;
        [SerializeField] private TMP_Text highScoreText;
        [SerializeField] private Button retryButton;
        [SerializeField] private RoundManager roundManager;

        private void OnEnable()
        {
            GameEvents.OnGameOver += Show;
        }

        private void OnDisable()
        {
            GameEvents.OnGameOver -= Show;
        }

        private void Start()
        {
            if (overlayRoot != null) overlayRoot.SetActive(false);
            if (retryButton != null) retryButton.onClick.AddListener(OnRetryClicked);
        }

        private void Show()
        {
            if (overlayRoot != null) overlayRoot.SetActive(true);
            if (finalScoreText != null)
                finalScoreText.text = "Final Score: " + roundManager.CurrentTotalScore;
            if (highScoreText != null)
                highScoreText.text = "High Score: " + roundManager.CurrentHighScore;
        }

        private void OnRetryClicked()
        {
            if (overlayRoot != null) overlayRoot.SetActive(false);
            roundManager?.StartNewRun();
        }
    }
}
