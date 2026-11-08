namespace XeCute.Core
{
    /// <summary>
    /// State machine for a single round (v2 — card-by-card draw).
    /// Idle → Drawing → Idle (next card) | GameOver
    /// </summary>
    public enum RoundState
    {
        Idle,       // Waiting for player to press DRAW for next card
        Drawing,    // Card flip animation playing
        Resolving,  // Round ended — win, lose, or checking outcome
        GameOver    // Lose reached (one lose = game over)
    }

    /// <summary>
    /// Possible round outcomes.
    /// </summary>
    public enum RoundOutcome
    {
        Win,    // face sum == lucky number (instant win, can happen on any draw 1-5)
        Lose    // 5 cards drawn, face sum never matched lucky
    }
}
