using UnityEngine;
using XeCute.Events;
using XeCute.Persistence;

namespace XeCute.Core
{
    /// <summary>
    /// Tracks total score, high score, and player name.
    /// (Strike system removed — one lose = game over.)
    /// </summary>
    public class ScoreManager
    {
        private readonly GameConfig config;
        private readonly HighScorePrefs prefs;

        public int TotalScore { get; private set; }
        public int HighScore { get; private set; }

        public ScoreManager(GameConfig config, HighScorePrefs prefs)
        {
            this.config = config;
            this.prefs = prefs;
            HighScore = prefs.LoadScore();
        }

        public void Reset()
        {
            TotalScore = 0;
            GameEvents.RaiseTotalScoreChanged(TotalScore);
        }

        /// <summary>
        /// Add a round's final score to total.
        /// Returns true if game over (i.e., round was a loss).
        /// </summary>
        public bool AddRoundScore(int roundScore, bool isWin)
        {
            TotalScore += roundScore;
            GameEvents.RaiseTotalScoreChanged(TotalScore);
            TryUpdateHighScore();
            return !isWin; // game over on any loss
        }

        private void TryUpdateHighScore()
        {
            if (TotalScore > HighScore)
            {
                HighScore = TotalScore;
                prefs.Save(HighScore);
                GameEvents.RaiseHighScoreUpdated(HighScore);
            }
        }
    }
}
