using System;
using System.Collections.Generic;
using XeCute.Core;

namespace XeCute.Events
{
    /// <summary>
    /// Central event bus. Decouples UI from game logic.
    /// v2: events for card-by-card draw + bonus scoring.
    /// </summary>
    public static class GameEvents
    {
        // ── Round lifecycle ──
        public static event Action<RoundState> OnStateChanged;
        public static void RaiseStateChanged(RoundState state) => OnStateChanged?.Invoke(state);

        // ── Lucky number ──
        public static event Action<int> OnLuckyNumberChanged;
        public static void RaiseLuckyNumberChanged(int value) => OnLuckyNumberChanged?.Invoke(value);

        // ── Cards (one at a time) ──
        public static event Action<int, int, int> OnCardDrawn; // (slotIndex, faceValue, scoreValue)
        public static void RaiseCardDrawn(int slot, int face, int score) => OnCardDrawn?.Invoke(slot, face, score);

        public static event Action<int, int> OnFaceSumUpdated; // (currentFaceSum, cardsDrawnCount)
        public static void RaiseFaceSumUpdated(int sum, int count) => OnFaceSumUpdated?.Invoke(sum, count);

        public static event Action<int> OnRoundScoreUpdated; // current round score (sum of card scores)
        public static void RaiseRoundScoreUpdated(int score) => OnRoundScoreUpdated?.Invoke(score);

        public static event Action OnCardsReset;
        public static void RaiseCardsReset() => OnCardsReset?.Invoke();

        // ── Cards remaining ──
        public static event Action<int> OnCardsRemainingChanged; // cards left to draw this round
        public static void RaiseCardsRemainingChanged(int remaining) => OnCardsRemainingChanged?.Invoke(remaining);

        // ── Outcome ──
        public static event Action<RoundOutcome> OnOutcomeResolved;
        public static void RaiseOutcomeResolved(RoundOutcome outcome) => OnOutcomeResolved?.Invoke(outcome);

        // ── Bonus ──
        public static event Action<BonusCalculator.BonusResult> OnBonusCalculated;
        public static void RaiseBonusCalculated(BonusCalculator.BonusResult bonus) => OnBonusCalculated?.Invoke(bonus);

        // Live bonus (updated after each card draw, before round end)
        public static event Action<BonusCalculator.BonusResult> OnLiveBonusCalculated;
        public static void RaiseLiveBonusCalculated(BonusCalculator.BonusResult bonus) => OnLiveBonusCalculated?.Invoke(bonus);

        public static event Action<int> OnFinalRoundScoreAwarded; // final round score after bonus
        public static void RaiseFinalRoundScoreAwarded(int score) => OnFinalRoundScoreAwarded?.Invoke(score);

        // ── Total score ──
        public static event Action<int> OnTotalScoreChanged;
        public static void RaiseTotalScoreChanged(int score) => OnTotalScoreChanged?.Invoke(score);

        // ── Game flow ──
        public static event Action OnGameOver;
        public static void RaiseGameOver() => OnGameOver?.Invoke();

        public static event Action<int> OnHighScoreUpdated; // (newHighScore, playerName)
        public static void RaiseHighScoreUpdated(int value) => OnHighScoreUpdated?.Invoke(value);
    }
}
