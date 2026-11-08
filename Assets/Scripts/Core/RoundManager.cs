using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XeCute.Events;
using XeCute.Persistence;

namespace XeCute.Core
{
    /// <summary>
    /// The heart of XeCute (v2 — card-by-card draw + bonus scoring).
    ///
    /// Round flow:
    ///   1. Round starts: lucky number (5-20) generated, 5 empty slots
    ///   2. Player presses DRAW → one card drawn (1-5), revealed in next slot
    ///   3. After each draw:
    ///      - Card score added to round score
    ///      - Card face added to face sum
    ///      - If face sum == lucky → WIN (instant)
    ///      - If 5 cards drawn and no match → LOSE
    ///      - Otherwise → back to Idle for next draw
    ///   4. At round end: bonus calculated from cards drawn, applied to round score
    ///   5. Round score added to total
    ///   6. Win → next round, Lose → GameOver (no strikes)
    /// </summary>
    public class RoundManager : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private GameConfig config;

        [Header("Scene References")]
        [SerializeField] private UnityEngine.UI.Button drawButton;

        private LuckyNumberGenerator luckyGen;
        private CardDeck deck;
        private ScoreManager score;
        private HighScorePrefs prefs;

        // Per-round state
        private RoundState state = RoundState.Idle;
        private List<int> cardsDrawnThisRound = new List<int>();
        private int faceSum = 0;
        private int roundScore = 0;
        private int currentLuckyNumber = 0;
        private bool canDrawNext = true;

        public RoundState State => state;
        public bool CanDraw => state == RoundState.Idle && canDrawNext && cardsDrawnThisRound.Count < config.maxCardsPerRound;
        public int CardsDrawn => cardsDrawnThisRound.Count;
        public int CardsRemaining => config.maxCardsPerRound - cardsDrawnThisRound.Count;
        public int CurrentLuckyNumber => currentLuckyNumber;
        public int CurrentFaceSum => faceSum;
        public int CurrentRoundScore => roundScore;

        // Public accessors for UI
        public int CurrentTotalScore => score.TotalScore;
        public int CurrentHighScore => score.HighScore;

        private void Awake()
        {
            prefs = new HighScorePrefs();
            luckyGen = new LuckyNumberGenerator(config);
            deck = new CardDeck(config);
            score = new ScoreManager(config, prefs);

            Application.targetFrameRate = 30;
        }

        private void OnEnable()
        {
            GameEvents.OnGameOver += HandleGameOver;
        }

        private void OnDisable()
        {
            GameEvents.OnGameOver -= HandleGameOver;
        }

        public void StartNewRun()
        {
            score.Reset();
            SetState(RoundState.Idle);
            StartNewRound();
        }

        public void StartNewRound()
        {
            cardsDrawnThisRound.Clear();
            faceSum = 0;
            roundScore = 0;
            canDrawNext = true;
            currentLuckyNumber = luckyGen.RollNew();
            GameEvents.RaiseCardsReset();
            GameEvents.RaiseFaceSumUpdated(0, 0);
            GameEvents.RaiseRoundScoreUpdated(0);
            GameEvents.RaiseCardsRemainingChanged(config.maxCardsPerRound);
            SetState(RoundState.Idle);
        }

        /// <summary>
        /// Draw one card. Triggered by DRAW button or Spacebar.
        /// </summary>
        public void DrawCard()
        {
            if (!CanDraw) return;
            StartCoroutine(DrawCardCoroutine());
        }

        public void ResetRun()
        {
            StopAllCoroutines();
            StartNewRun();
        }

        // ── Core draw flow ──

        private IEnumerator DrawCardCoroutine()
        {
            canDrawNext = false;
            SetState(RoundState.Drawing);

            int faceValue = deck.DrawOne();
            int scoreValue = config.GetCardScore(faceValue);
            int slotIndex = cardsDrawnThisRound.Count;

            cardsDrawnThisRound.Add(faceValue);
            faceSum += faceValue;
            roundScore += scoreValue;

            // Reveal card
            GameEvents.RaiseCardDrawn(slotIndex, faceValue, scoreValue);
            GameEvents.RaiseFaceSumUpdated(faceSum, cardsDrawnThisRound.Count);
            GameEvents.RaiseRoundScoreUpdated(roundScore);
            GameEvents.RaiseCardsRemainingChanged(CardsRemaining);

            // Live bonus update — show player what bonuses are active so far
            var liveBonus = BonusCalculator.Calculate(cardsDrawnThisRound, config);
            GameEvents.RaiseLiveBonusCalculated(liveBonus);

            // Wait for flip animation + small delay
            yield return new WaitForSeconds(config.cardFlipDuration + config.afterCardDelay);

            // Check win: face sum == lucky
            if (faceSum == currentLuckyNumber)
            {
                yield return StartCoroutine(EndRound(RoundOutcome.Win));
                yield break;
            }

            // Check lose: all cards drawn, no match
            if (cardsDrawnThisRound.Count >= config.maxCardsPerRound)
            {
                yield return StartCoroutine(EndRound(RoundOutcome.Lose));
                yield break;
            }

            // Continue: back to idle for next draw
            SetState(RoundState.Idle);
            canDrawNext = true;
        }

        private IEnumerator EndRound(RoundOutcome outcome)
        {
            SetState(RoundState.Resolving);
            GameEvents.RaiseOutcomeResolved(outcome);

            // Calculate bonus
            var bonus = BonusCalculator.Calculate(cardsDrawnThisRound, config);
            GameEvents.RaiseBonusCalculated(bonus);

            int finalRoundScore = BonusCalculator.ApplyBonus(roundScore, bonus);
            GameEvents.RaiseFinalRoundScoreAwarded(finalRoundScore);

            // Apply to total score. AddRoundScore returns true if game over (i.e., was a loss).
            bool gameOver = score.AddRoundScore(finalRoundScore, outcome == RoundOutcome.Win);

            yield return new WaitForSeconds(config.resolveDelay + config.nextRoundDelay);

            // Check game over
            if (gameOver)
            {
                GameEvents.RaiseGameOver();
                SetState(RoundState.GameOver);
                yield break;
            }

            // Next round
            StartNewRound();
        }

        private void SetState(RoundState newState)
        {
            state = newState;
            GameEvents.RaiseStateChanged(state);
        }

        private void HandleGameOver()
        {
            if (drawButton != null) drawButton.interactable = false;
        }
    }
}
