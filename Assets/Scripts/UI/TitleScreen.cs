using UnityEngine;
using UnityEngine.UI;
using TMPro;
using XeCute.Core;

namespace XeCute.UI
{
    /// <summary>
    /// The title screen. Shows game name, high score, player name input, and Play button.
    /// v2: includes player name InputField.
    /// </summary>
    public class TitleScreen : MonoBehaviour
    {
        [SerializeField] private GameObject overlayRoot;
        [SerializeField] private TMP_Text highScoreText;
        [SerializeField] private Button playButton;
        [SerializeField] private RoundManager roundManager;
        [SerializeField] private GameObject gameplayUI;
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private TMP_Text appVersionText;

        private void Start()
        {
            if (playButton != null) playButton.onClick.AddListener(OnPlayClicked);
            if (gameplayUI != null) gameplayUI.SetActive(false);

            if (appVersionText) appVersionText.text = $"{Application.version}";

            UpdateHighScoreDisplay();
        }

        private void OnPlayClicked()
        {
            if (overlayRoot != null) overlayRoot.SetActive(false);
            if (gameplayUI != null) gameplayUI.SetActive(true);
            audioManager.UnlockAudio();
            if (roundManager != null) roundManager.StartNewRound();
        }

        public void UpdateHighScoreDisplay()
        {
            if (roundManager != null)
            {
                if (highScoreText != null)
                    highScoreText.text = "High Score: " + roundManager.CurrentHighScore;
            }
        }
    }
}
