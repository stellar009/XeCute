using UnityEngine;

namespace XeCute.Persistence
{
    /// <summary>
    /// Wrapper around PlayerPrefs for high-score + player-name persistence.
    /// v2: stores both score (int) and name (string).
    /// </summary>
    public class HighScorePrefs
    {
        private const string ScoreKey = "XeCute_HighScore";

        public int LoadScore()
        {
            return PlayerPrefs.GetInt(ScoreKey, 0);
        }

        public void Save(int score)
        {
            PlayerPrefs.SetInt(ScoreKey, score);
            PlayerPrefs.Save();
        }

        public void Clear()
        {
            PlayerPrefs.DeleteKey(ScoreKey);
            PlayerPrefs.Save();
        }
    }
}
