# Game Vision Document - TouHou Bullet Hell Fangame

**Developers:** Harry Ky, Dylan Gyori, Ross Kugler, Linh Nguyen, Toufic Majdalani  
**Engine:** C# MonoGame  

## 1. Game Overview
This game is a **Bullet Hell fangame** inspired by the **TouHou Project**, featuring fast-paced gameplay with increasingly complex enemy patterns. The game will last **approximately 2-3 minutes**, structured into **four distinct phases**:

1. **Grunt Enemy Waves** – Basic enemy introduction with simple bullet patterns.
2. **Mid-Boss Battle** – A stronger enemy with elaborate attack patterns.
3. **More Grunt Waves** – Increased difficulty leading up to the final challenge.
4. **Final Boss Battle** – A two-stage fight mimicking TouHou spell cards.

## 2. Gameplay Flow (Chronological Sequence)

### Phase 1: Grunt Enemies (0:00 - 0:30)
- The game begins with **small enemy waves** appearing from different screen positions.
- Each wave lasts **10 seconds**, progressively increasing in difficulty.
- Enemies use basic **spread shots and straight-line projectiles**.
- Player moves using **WASD**, fires using a designated **shoot button**, and can **activate a slow-move mode** for precise dodging.

#### Enemy Waves:
- **Wave 1 (0:00 - 0:10):** Simple enemies shooting single bullets.
- **Wave 2 (0:10 - 0:20):** Increased enemy count, introducing diagonal bullet spreads.
- **Wave 3 (0:20 - 0:30):** Faster enemies that fire in alternating burst patterns.

### Phase 2: Mid-Boss Battle (0:30 - 1:00)
- A **mid-boss appears at the top of the screen**, challenging the player with more complex bullet patterns.
- **Mid-Boss HP:** 800 HP
- The mid-boss has two distinct **spell card attacks**:
  1. **"Lunar Spiral - Celestial Barrage"** – Fires a spiral pattern of bullets that gradually expands outward.
  2. **"Blazing Whirlwind - Chaos Burst"** – Alternates between slow-moving large bullets and fast-moving micro-projectiles.
- Player must balance attacking while dodging dense bullet formations.

### Phase 3: More Grunts (1:00 - 1:30)
- A second wave of **more aggressive enemies** spawns.
- New enemy types introduced:
  - **Homing enemies** – Fire tracking bullets that follow the player.
  - **Shielded enemies** – Require multiple hits to defeat.
- **Final wave at 1:20** introduces dense, randomized bullet spreads.

### Phase 4: Final Boss Battle (1:30 - End)
- The **final boss appears dramatically**, signaling the climax of the game.
- **Boss HP:** 1500 HP

#### Spell Card Stage 1 (1:30 - 2:22) – *Mimicking First Boss Stage (01:36 – 02:22 in the video)*
**"Scarlet Veil - Moonlight Blood Dance"**  
- Fires a **combination of slow-moving dense bullet waves** and **fast-moving projectiles**.
- Radial bullet spreads appear at **fixed intervals**, forcing the player to navigate precise gaps.

#### Spell Card Stage 2 (2:22 - 3:07) – *Mimicking Third Boss Stage (03:07 – 03:52 in the video)*
**"Phantom Eclipse - Spirit Bloom Barrage"**  
- Uses **complex attack patterns**, including:
  - **Spiral Barrages** – Bullets expand in an elegant, timed swirl pattern.
  - **Alternating Homing & Non-Homing Shots** – Forces rapid directional changes.
  - **Delayed Exploding Bullets** – Projectiles that pause before bursting into smaller fragments.
- As the boss’s HP decreases, **attack density and speed escalate**.

- Upon defeating the boss, the game ends with a **victory screen**.

## 3. Player Information
- **Lives:** 3
- **Hitbox Size:** Small, positioned at the center of the sprite.
- **Controls:**
  - **WASD:** Movement
  - **Shoot (Space or customizable):** Continuous fire
  - **Slow Mode (Shift):** Reduces movement speed for precise dodging

## 4. Additional Features for Authenticity
- **Grazing Bonus:** Players earn points for narrowly dodging bullets.
- **Combo Kill Bonus:** Chain kills increase score multipliers.
- **Unique Bullet Patterns:** Attacks form **circles, waves, and floral spirals**, mimicking TouHou’s signature aesthetic.
- **Soundtrack & Visual Effects:** Inspired by the fast-paced, high-energy themes of classic TouHou games.
---
