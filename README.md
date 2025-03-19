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
   - 
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

## 🏗️ Design Patterns and Architecture
We aimed to follow good architectural/design principles throughout the project. Below are lists of the patterns/design choices implemented or planned to be implemented.

### ✅ Implemented
- **Entity-Component-System (ECS):** Bullets, Enemies, Players, Collectibles, etc. are entities. We attach components to these entities holding data. Then systems operate on entities with specific components.
- **Builder Pattern:** We use the builder pattern to construct the Entities by attaching all the components they need.
- **Singleton Pattern:** For managers like InputManager and TextureManager.
- **Observer Pattern:** For updating UI elements like the game Window.
- **JSON Loading:** We load all the data we need in our game from JSON files (Character stats, Level descriptions, etc).

### 🛠️ To be Implemented  
- **Command Pattern:** For scheduling events with Bullets and handling player input.

---

## 🎨 Credits & Attribution
- **Assets & Inspiration:** ZUN, creator of the Touhou Project.  
- **Engine:** Built using **MonoGame** in **C#**.  
