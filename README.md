# XeCute

**A card-summoning game of luck, risk, and bonus hunting.** Built for the Lucky Number game jam.

A lucky number appears. Five cards wait to be drawn. Each press of DRAW reveals one card — its face value adds to your running sum, and its score value adds to your round total. Match the lucky number exactly and the round is yours. Miss it after five cards and the run ends.

But here's the twist: **even losing rounds score points.** And the specific combinations of cards you draw unlock stacking bonus multipliers. Chase the big payouts. Push your run. Beat your high score.

---

## How to play

1. A **lucky number** (5–20) appears each round
2. Press **DRAW** (or Spacebar) to draw one card (values 1–5)
3. Each card adds its **face value** to your sum and its **score value** to your round total
4. **Match the lucky number** → win the round, continue your run
5. **Draw 5 cards without matching** → game over (no second chances)
6. **R** resets your run at any time

### Cards & scoring

| Card | Score value |
|------|-------------|
| 1    | 10          |
| 2    | 40          |
| 3    | 90          |
| 4    | 160         |
| 5    | 250         |

### Bonus system (stacks multiplicatively!)

| Bonus        | Trigger                  | Effect                |
|--------------|--------------------------|-----------------------|
| Dual Hand    | 2+ cards of 2            | ×1.25                 |
| Triplets     | 3+ cards of 3            | ×1.75                 |
| Quadrants    | 2+ cards of 4            | ×1.5 – ×2.0 (random)  |
| Packs of 5   | 2+ cards of 5            | ×2.5                  |
| Unique Ones  | All 5 cards unique (1–5) | +5000 flat            |

Multipliers stack multiplicatively. Flat bonus added after.

**Round score** = (sum of card scores) × total multiplier + flat bonus
**Total score** = sum of all round scores (added regardless of win or lose)

---

## Tech stack

- **Engine:** Unity 6.6 (2D)
- **UI:** TextMeshPro (TMP)
- **Language:** C#
- **Architecture:** Event-driven (static event bus decouples UI from game logic)
- **Persistence:** PlayerPrefs (high score saved locally)
- **Audio:** CC0 sound effects from [freesound.org](https://freesound.org)
- **Target platform:** WebGL (plays in browser)

---

## Project structure

```
XeCute/
├── Assets/
│   ├── Scenes/
│   │   └── DrawBoard.unity                  # The only scene
│   ├── Scripts/
│   │   ├── Core/                       # Game logic
│   │   │   ├── RoundManager.cs             # State machine heart of the game
│   │   │   ├── CardDeck.cs                 # Single-card draw with replacement
│   │   │   ├── LuckyNumberGenerator.cs     # Rolls 5-20 each round
│   │   │   ├── ScoreManager.cs             # Total score, high score tracking
│   │   │   ├── BonusCalculator.cs          # Bonus multipliers + flat bonus
│   │   │   ├── GameConfig.cs               # ScriptableObject with tunable constants
│   │   │   └── RoundState.cs               # Enums for state machine + outcomes
│   │   ├── Events/
│   │   │   └── GameEvents.cs               # Static event bus
│   │   ├── Persistence/
│   │   │   └── HighScorePrefs.cs           # PlayerPrefs wrapper
│   │   ├── UI/                         # UI components (TextMeshPro)
│   │   │   ├── TopBar.cs                   # Score + digit wheel + multiplier
│   │   │   ├── ScoreDigitWheel.cs          # 6-digit odometer-style score display
│   │   │   ├── LuckyNumberDisplay.cs       # Big lucky number in play area
│   │   │   ├── CardSlot.cs                 # Per-slot card reveal + flip animation
│   │   │   ├── SumDisplay.cs               # Face sum + card count
│   │   │   ├── RoundScoreDisplay.cs        # Round score + bonus breakdown
│   │   │   ├── BonusPairPanel.cs           # Live bonus detection panel
│   │   │   ├── ControlsBar.cs              # DRAW/Reset button wiring
│   │   │   ├── TitleScreen.cs              # Title overlay
│   │   │   ├── GameOverScreen.cs           # Game-over overlay
│   │   │   └── AudioManager.cs             # SFX + music playback
│   │   └── Utils/
│   │       ├── InputManager.cs             # Keyboard input (Space, R)
│   │       ├── SceneBootstrapper.cs        # Scene sanity checks
│   │       └── XeCutePalette.cs            # Color constants
│   ├── Audio/                          # CC0 sound files from freesound.org
│   │   ├── card_flip.wav
│   │   ├── win_chime.wav
│   │   ├── lose_thud.wav
│   │   ├── bonus_sparkle.wav
│   │   └── ambient_loop.wav
```

---

## Getting started (for developers)

### Prerequisites

- **Unity 6.6** (install via Unity Hub)
- A code editor (Visual Studio, VS Code, or JetBrains Rider)

### Setup

1. **Clone the repo**
   ```bash
   git clone https://github.com/stellar009/XeCute.git
   cd XeCute
   ```

2. **Open in Unity Hub**
   - Open Unity Hub → Add project → select the cloned folder
   - Open with Unity 6.6

3. **Import TMP Essentials** (one-time)
   - Window → TextMeshPro → Import TMP Essentials

4. **Open the main scene**
   - `Assets/Scenes/Main.unity`

5. **Press Play**

See [`SETUP_README.md`](./SETUP_README.md) for the full scene setup walkthrough if you're starting from an empty project.

### Building for WebGL

1. File → Build Settings
2. Add `Main` scene
3. Switch platform to WebGL
4. Player Settings → Resolution: 1920 × 1080, Run In Background ✓
5. Build → choose output folder
6. Test `index.html` in a fresh incognito browser

---

## Controls

| Input              | Action              |
|--------------------|---------------------|
| Spacebar / DRAW    | Draw one card       |
| R / Reset button   | Reset current run   |

---

## Game flow

```
Title Screen
    │
    ▼
[Play button]
    │
    ▼
Round Start → Lucky number generated (5-20)
    │
    ▼
[Player presses DRAW] ────► Card revealed
    │                            │
    │                            ▼
    │                       Sum += card value
    │                       Round score += card score
    │                       Live bonus recalculated
    │                            │
    │                            ▼
    └──────────── ◄ ───── Sum == lucky?
                            │
                  ┌─────────┴─────────┐
                  │ Yes               │ No
                  ▼                   ▼
              WIN!              5 cards drawn?
                  │                   │
                  │              ┌────┴────┐
                  │              │ Yes     │ No
                  │              ▼         │
                  │            LOSE        │
                  │              │         │
                  │              ▼         ▼
                  │         Game Over  [DRAW again]
                  │
                  ▼
            Next round
```

---

## Features

- **Endless run format** — play until you lose
- **Card-by-card draw** — suspenseful one-card-at-a-time reveal
- **Live bonus detection** — see which bonuses are active as you draw
- **Stacking multipliers** — combine Dual Hand + Triplets + Packs for huge payouts
- **Persistent high score** — saved locally via PlayerPrefs
- **6-digit score wheel** — odometer-style display
- **Clean minimal UI** — TextMeshPro for crisp text at all sizes
- **Color-coded feedback** — green win, red lose, gold bonus
- **Full audio pass** — card flips, win chimes, lose thuds, bonus sparkles, ambient music

---

## Audio credits

All sound effects and music are sourced from [freesound.org](https://freesound.org) under the Creative Commons Zero (CC0) license — free for commercial use, no attribution required (but appreciated).

| Sound          | Source                                        | License |
|----------------|-----------------------------------------------|---------|
| Card flip      | [https://freesound.org](https://freesound.org/people/Splashdust/sounds/84322/)        | CC0     |
| Win chime      | [https://freesound.org](https://freesound.org/people/JustInvoke/sounds/446111/)        | CC0     |
| Lose thud      | [https://freesound.org](https://freesound.org/people/Reitanna/sounds/332668/)        | CC0     |
| Bonus sparkle  | [https://freesound.org](https://freesound.org/people/MLaudio/sounds/511485/)        | CC0     |
| Ambient music  | [https://freesound.org](https://freesound.org/people/MuzaProduction/sounds/650278/)        | CC0     |

> **Note to maintainer:** Replace the table above with the specific freesound.org sound URLs and author names for full attribution. You can find this info on each sound's page under "Attribution".

---

## What's NOT included

- **Card sprites** — uses Unity UI primitives (Image + TMP_Text)
- **Particle effects** — light juice only (card flip animation, color feedback)

---

## AI Disclosure

Generative AI was used in the development of XeCute in the following ways:

- **Code:** AI assistants wrote or helped write the majority of the C# scripts, including the round state machine, scoring system, bonus calculation, UI managers, and event bus. Code was reviewed, tested, and iterated on by the developer.
- **Game design:** AI helped brainstorm the concept, refine the ruleset (card values, lucky number range, bonus multipliers), and design the scoring formula.


**Not used for:**
- Art assets (none used — built with Unity UI primitives)
- Audio assets (sourced from freesound.org, CC0 licensed)

All final design decisions, balance choices, Unity scene setup, audio integration, playtesting, and submission were done by the developer.

---

## Credits

- **Developer:** [stellar009](https://github.com/stellar009)
- **Engine:** Unity 6.6
- **UI:** TextMeshPro
- **Audio:** [freesound.org](https://freesound.org) contributors (CC0)
- **Fonts:** [Space Grotesk](https://fonts.google.com/specimen/Space+Grotesk), [Inter](https://fonts.google.com/specimen/Inter) (Google Fonts, SIL Open Font License)

---

## License

This project's source code is released under the **MIT License**. See [LICENSE](./LICENSE) for details.

Game design, rules, and the XeCute name are © stellar009.

Audio files in `Assets/Audio/` are licensed under CC0 by their respective freesound.org creators — see the Audio credits section above for specifics.

---

## Links

- **Play in browser:** [itch.io page](https://stellar009.itch.io/xecute)
- **Report issues:** [GitHub Issues](https://github.com/stellar009/XeCute/issues)
- **Jam:** Lucky Number Game Jam

---

*Made for the Lucky Number game jam.*
