using UnityEngine;
using XeCute.Events;
using XeCute.Core;

namespace XeCute.UI
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSource cardFlipSource;
        [SerializeField] private AudioSource winSource;
        [SerializeField] private AudioSource loseSource;
        [SerializeField] private AudioSource bonusSource;
        [SerializeField] private AudioSource musicSource;

        private bool audioUnlocked = false;

        private void OnEnable()
        {
            GameEvents.OnCardDrawn += (_, _, _) => Play(cardFlipSource);
            GameEvents.OnOutcomeResolved += HandleOutcome;
            GameEvents.OnLiveBonusCalculated += HandleBonus;
        }

        private void OnDisable()
        {
            GameEvents.OnCardDrawn -= (_, _, _) => Play(cardFlipSource);
            GameEvents.OnOutcomeResolved -= HandleOutcome;
            GameEvents.OnLiveBonusCalculated -= HandleBonus;
        }

        // Call this from your TitleScreen OnPlayClicked to unlock audio
        public void UnlockAudio()
        {
            audioUnlocked = true;
            if (musicSource != null && !musicSource.isPlaying)
                musicSource.Play();
        }

        private void Play(AudioSource src)
        {
            if (audioUnlocked && src != null) src.Play();
        }

        private void HandleOutcome(RoundOutcome outcome)
        {
            if (outcome == RoundOutcome.Win) Play(winSource);
            else Play(loseSource);
        }

        private void HandleBonus(BonusCalculator.BonusResult bonus)
        {
            if (bonus.triggeredBonuses.Count > 0) Play(bonusSource);
        }
    }
}