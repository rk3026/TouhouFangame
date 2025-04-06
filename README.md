# 🎇 Touhou-Inspired Bullet Hell Fangame

**Developers:**  
Ross Kugler — [GitHub](https://github.com/rk3026)  
Huy (Harry) Ky — [GitHub](https://github.com/Harry908)  
Dylan Gyori — [GitHub](https://github.com/JustDylan)  
Linh (Jason) Nguyen — [GitHub](https://github.com/linhnt-98)  
Toufic Majdalani — [GitHub](https://github.com/majdaltouzach)  
Ben Bordon — [GitHub](https://github.com/wizkid0101)  

---

## 🌟 Overview
This game is an arcade-like vertical-scrolling bullet hell where players dodge bullet patterns, defeat enemies and bosses, and collect items to increase their score for a level. Our goal was to replicate the core gameplay of *Touhou 7: Perfect Cherry Blossom* while focusing on robust game architecture and well-structured design patterns.

![image](https://github.com/user-attachments/assets/474ad122-4b9c-4dab-928d-204cf3081e2e)

---

## 🎮 Game Features
- **Bullet dodging** with precise hitboxes.  
- **Unique playable characters** with distinct attack styles.  
- **The Touhou "grazing" mechanic**, rewarding players for narrowly avoiding bullets.
- **Score tracking**, like an arcade game.
- **Collectibles and Power-Ups** to improve the player's strength and raise their score.
- **Unique levels** that contain different enemy and boss layouts.
- **Pausing and Setting Management** to allow the user customization.
- **Time-Based Level Progression** so players can do 'Pacifist Runs', playing levels without shooting.

---

## ⚙️ Gameplay Flow

0. **Main Menu:**
   - At the main menu, a player selects a character to play as, an attack type for their character, and a level.

1. **Level Start:**
   - The level begins, and after a few seconds, some grunt enemies will spawn and fly around the level.
   - These early enemies have simpler bullet patterns, giving the player time to warm up.
   - Enemies may drop collectible loot that either increases score or gives the player extra shooting power, lives, or bombs.

3. **Escalation:**  
   - As the level progresses, enemies grow stronger, attack in larger numbers, and unleash more intricate bullet patterns.  
   - Players must skillfully weave through increasingly dense bullet formations while maximizing their score through grazing and enemy takedowns.  

4. **Mid-Boss Encounter:**  
   - Midway through the level, a **Mid-Boss** appears with multiple attack phases and faster movement.  
   - The Mid-Boss must either be defeated by reducing its HP to **0** or outlasted until it retreats.  

5. **Second Enemy Section:**  
   - After the Mid-Boss, another section of enemies spawning occurs, this time with greater intensity.  
   - New enemy types are introduced, each with unique bullet patterns.  

6. **Final Boss Battle:**  
   - At the end of the level, the **Final Boss** appears, boasting the most complex attack patterns and highest HP of any enemy.  
   - The Final Boss fight consists of multiple phases, each ramping up in difficulty.  

7. **Victory:**  
   - The level ends when the player either defeats the Final Boss or survives long enough.  
   - The player’s performance is evaluated based on score, lives remaining, and grazing bonuses.  

---
## 🧱 Architecture

### Main Architecture: Multi-layered  
### Sub-Architecture: Entity-Component System (ECS)  

**Description:**  
Our software architecture combines a multi-layered structure with an Entity-Component System. We separate the system into four layers: Presentation, Logic, Data-Access, and Data. Within the Logic layer, we implement ECS—where Bullets, Enemies, Collectibles, etc., are Entities composed of Components holding data, and Systems operate on those entities based on their components.

**Rationale:**  
We chose a multi-layered architecture for its clean separation of concerns and compatibility with ECS. It allows us to decouple and modularize subsystems such as JSON loading, game logic, and rendering. We opted for an *open* layered architecture—allowing higher layers to directly access lower ones when needed—because performance is a key priority in games. For example, scenes in the Presentation layer can directly access the Data-Access layer to quickly load needed resources, bypassing intermediate logic layers when appropriate.

**Breakdown of Layers:**

1. **Presentation Layer**  
   - Includes all Scenes and UI classes.  
   - Each Scene has a `Draw()` function to render to the screen.  
   - UI classes are reusable and integrated into various scenes.  

2. **Logic Layer**  
   - Core of the game's behavior and mechanics.  
   - Contains the ECS architecture, where Systems manipulate Entities via their Components.  

3. **Data-Access Layer**  
   - Handles reading from and writing to external data sources (e.g., JSON).  
   - Contains DataLoader classes for converting JSON data into usable game objects.  

4. **Data Layer**  
   - Raw JSON files that define game assets, entity stats, patterns, and configurations.

![Architecture Diagram](https://github.com/user-attachments/assets/e816a722-531f-4407-980e-a284517c1d27)

---

## Design Patterns

We used various design patterns to promote clean code, reuse, and flexibility:
- **Builder:** Used for constructing Entities by attaching the required Components step-by-step.
- **Singleton:** Used for manager classes like `InputManager` and `TextureManager` to ensure only one instance exists globally.
- **Observer:** Enables UI elements (like the main window) to update in response to internal changes/events.
- **State:** Scenes represent "states" in the game, and a `SceneManager` acts as the context managing scene transitions.
- **Facade:** The `EntityDataGenerator` class acts as a simplified interface to multiple generator classes, used for generating test game data.
- **Flyweight:** `TextureManager` stores and reuses textures (intrinsic state), while `SpriteComponent` carries extrinsic state like position and scale.
- **Strategy:** Entities use interchangeable collision strategies via `ICollisionStrategy`, allowing different responses based on type (e.g., bullets, enemies, player).

---

## 🎨 Credits & Attribution
- **Assets & Inspiration:** ZUN, creator of the Touhou Project.  
- **Engine:** Built using **MonoGame** in **C#**.  
