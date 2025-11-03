# LexiRun — Feature & Logic Requirements

## 1) Core Objective
- A **single level** competitive word-collection race between **one player** and **N bots**.
- Each actor must **complete 3 words**.  
- **First** actor to complete 3 words **wins**.
- The **player** can also **lose** if:
  - Their **time reaches 0**, or
  - Their **HP reaches 0**, or
  - **Any bot** completes 3 words first.

## 2) Arena & Nodes
- The level contains exactly **26 letter nodes**, one for each **A–Z**.
- Each node is permanently assigned a **single letter** for the entire level (no rerolling letters at any time).
- A node can be “touched” by any actor any number of times; it is **non-consumable** and remains available to all.
- A node visually indicates the **last actor who touched it** (purely cosmetic; does not lock ownership).

## 3) Words & Progress
- At match start, the **player** and each **bot** receive **one word** (each actor’s word can differ).
- On **completing a word**, that actor is immediately assigned a **new word**; nodes **do not** change.
- **Duplicate-letter rule:** Touching the node for a letter that exists **multiple times** in the actor’s current word **fills all occurrences** of that letter simultaneously (e.g., touching `P` fills both `P`s in “APPLE”).
- Progress display must show, for each actor, which letters are filled vs. missing for their current word.

## 4) Interaction with Nodes (Touch Logic)
When an actor touches a node:
- Determine whether the node’s letter is **needed** for the actor’s current word (i.e., there exists at least one **unfilled instance** of that letter).
- **If needed (Correct):**
  - Fill all unfilled instances of that letter in the word.
  - Trigger correct feedback (visual/audio).
  - If this action completes the entire word, trigger **Word Complete** (see §5).
- **If not needed (Wrong):**
  - Apply **penalty** (see §6) according to the actor type and current progress state.

**Touch Debounce Requirement:**  
A single continuous contact should not cause repeated processing spikes. Each (actor, node) pair should apply logic at most once per brief window, then require a re-exit/re-enter or cooldown before re-applying.

## 5) Word Completion
- On completing a word:
  - Register the actor’s **word count += 1**.
  - Trigger clear completion feedback.
  - After a brief completion moment (during which further input for that actor does not trigger new progression), assign the **next word**.
- The match **ends immediately** if any actor reaches **3 completed words**; see §11 for tie handling.

## 6) Penalties & Fail States
- **Player Penalties:**
  - **Wrong touch and current word progress > 0:**  
    - **HP –1** and **remove the last correctly filled letter** from this word’s progress.  
  - **Wrong touch and current word progress = 0:**  
    - **Deduct time** (default design intent: **–5 seconds**, configurable).
  - The player **loses immediately** if:
    - **HP ≤ 0**, or
    - **Time ≤ 0**.
- **Bot Penalties:**
  - Bots do **not** have a timer.
  - Bots **accumulate mistakes**; on **3 wrong touches total**, that bot is **eliminated** (no longer competes).
- Penalty actions must produce clear feedback (so players can read what happened).

## 7) Timer & Health
- **Player-only timer:**  
  - Starts at a configured duration.  
  - Decreases in real time.  
  - If it reaches **0**, the **player loses** (regardless of bot state).
- **Player HP:**  
  - Starts at a configured value (e.g., 3).  
  - **Persists** across words (does **not** reset on new word).  
  - Reaches **0** → player loses.
- **Bots:**  
  - No timer, no HP; only the **3-wrong rule** applies to eliminate a bot.

## 8) Collisions & Movement Rules
- Actors (player and bots) **cannot pass through** each other; collisions are **blocking**.
- If physical blockage prevents node contact, that is an intended competitive element (no ghosting).
- Movement parameters (speed, acceleration, etc.) are **configurable** per actor type/difficulty.

## 9) Bot Behavior (Decision Logic)
- Each bot is always pursuing **one of the letters it still needs** for its current word.
- **Target Selection:** Choose a needed letter and a reachable node carrying that letter.  
  - The choice should favor **progression** (e.g., next earliest missing letter) and reasonable proximity.  
  - Bots must **avoid intentionally targeting wrong letters** (no deliberate grief).
- **Replanning:** Bots periodically reconsider their target (e.g., when blocked, target becomes filled by other actions, or a shorter path appears).
- **Mistakes:** If a bot touches a non-needed letter, that counts as a **wrong touch** (see §6, bot elimination rule).

## 10) UI/Feedback (Logic Expectations)
- **Player HUD must include:**  
  - Current **word** with filled/missing letters clearly indicated.  
  - **HP** (e.g., hearts or numeric).  
  - **Timer** (countdown).
- **Rival visibility:**  
  - Show each bot’s **current word progress** and their **mistake count** (0–3), in a compact manner readable during play.
- **Node feedback:**  
  - Indicate the **last actor** who touched a node (e.g., color tag). This is cosmetic and does **not** restrict others.
- **Events feedback:**  
  - Correct touch, wrong touch, word completion, player defeat, bot elimination, and match end must each have distinct, readable feedback.

## 11) Tie & Simultaneity Rules
- **Node touch simultaneity:** If multiple actors touch the **same node at the same time**, all eligible actors may receive progression from that touch (since nodes are non-consumable).
- **Word completion simultaneity:**  
  - When multiple completions occur in the **same frame/update cycle**, the winner is the actor with the **earliest valid completion timestamp** recognized in that cycle.  
  - If the player and a bot both complete their **third** word within the same cycle, the actor with the earlier completion event **wins**; the other **does not**.
- The system must avoid ambiguous double-wins.

## 12) Start/End Flow
- **Start:**  
  - Spawn all 26 nodes (A–Z).  
  - Assign one starting word to each actor.  
  - Initialize player HP and timer, bot mistake counters to 0.  
  - Begin active play.
- **End (any of):**  
  - Any actor reaches 3 completed words → **immediate** match end (announce winner).  
  - Player timer reaches 0 → player **loses**.  
  - Player HP reaches 0 → player **loses**.  
  - If all bots are eliminated before the player finishes, the player still must **reach 3 words** to win; the match does not auto-complete.

## 13) Configurable Parameters
- **Player:** starting HP, starting time, move speed.  
- **Bots:** count, move speed per difficulty tier, target-replan cadence, mistake limit (default 3).  
- **Penalties:** time deduction at zero progress (default 5s), HP loss amount (default 1), letter removal policy (fixed: remove **last** filled letter at wrong touch with progress).  
- **Completion cadence:** brief completion freeze duration before the next word is assigned.  
- **Words to win:** fixed at **3** for this design.  
- **Word selection:** usable word lists by length; whether lengths are fixed or progress (e.g., 3→5→7).  
- **UI thresholds:** final seconds warning behavior (e.g., at 5s).

## 14) Data Integrity & Fairness Constraints
- **Letter availability:** Because the map always contains **all 26 letters**, any word is theoretically completable without rerolls.  
- **No mid-level letter changes:** Nodes never change letters during the level.  
- **Duplicate letters:** Always fill **all occurrences** of the touched letter in the actor’s current word.  
- **Deterministic ordering:** Completion and penalty outcomes must be deterministic within an update cycle.

## 15) Test Scenarios (Acceptance)
1. **Duplicate letter fill:** Actor word “APPLE”; touching `P` fills both `P` slots at once.  
2. **Wrong @ no progress:** Player with 0 filled letters touches a wrong node → time decreases by configured amount; HP remains unchanged; no letters are removed.  
3. **Wrong @ some progress:** Player with at least 1 filled letter touches a wrong node → HP decreases by 1 and **last filled letter** becomes unfilled.  
4. **Bot elimination:** A bot makes 3 wrong touches across the match → bot eliminated and cannot win thereafter.  
5. **Timer loss:** Player timer reaches 0 while incomplete → immediate defeat regardless of bot state.  
6. **HP loss:** Player HP reaches 0 → immediate defeat.  
7. **Simultaneous touch, same node:** Player and bot touch the same needed letter; both receive progression.  
8. **Simultaneous third-word completion:** Two actors complete their third word in the same cycle; earlier completion event is declared the winner; the other loses.  
9. **No letter reroll:** After completing a word, the next word appears; node letters remain unchanged.  
10. **Collision validity:** Actors cannot pass through one another; collisions can block access to nodes, affecting race outcomes.
