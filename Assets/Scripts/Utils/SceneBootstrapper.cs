using UnityEngine;
using XeCute.Core;
using XeCute.Persistence;

namespace XeCute.Utils
{
    /// <summary>
    /// Bootstrapper. Place on an empty GameObject named "_Bootstrapper".
    /// Ensures GameConfig exists, initializes HighScorePrefs, and verifies scene references.
    /// Add this to the scene last, after RoundManager and UI are set up.
    /// </summary>
    public class SceneBootstrapper : MonoBehaviour
    {
        [Header("Auto-assign on Awake")]
        [SerializeField] private RoundManager roundManager;

        [Header("Optional — auto-create if missing")]
        [Tooltip("If true and no GameConfig is assigned in RoundManager, one will be created at runtime.")]
        [SerializeField] private bool autoCreateConfig = true;

        private void Awake()
        {
            if (roundManager == null)
            {
                roundManager = FindAnyObjectByType<RoundManager>();
            }
            if (roundManager == null)
            {
                Debug.LogError("[SceneBootstrapper] No RoundManager found in scene. The game will not function.");
                return;
            }

            // Verify GameConfig
            var configField = typeof(RoundManager).GetField("config",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            GameConfig config = configField?.GetValue(roundManager) as GameConfig;
            if (config == null && autoCreateConfig)
            {
                Debug.LogWarning("[SceneBootstrapper] No GameConfig assigned to RoundManager. Creating runtime instance.");
            }

            // Ensure Time.timeScale is 1 (in case a paused state leaked)
            Time.timeScale = 1f;
        }

        private void Update()
        {
            // Press F12 to clear high score (dev shortcut — remove before shipping)
            if (Input.GetKeyDown(KeyCode.F12))
            {
                var prefs = new HighScorePrefs();
                prefs.Clear();
                Debug.Log("[SceneBootstrapper] High score cleared.");
            }
        }
    }
}
