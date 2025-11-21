# Touhou-Inspired Bullet Hell Fangame

**Developers:**  
Ross Kugler — [GitHub](https://github.com/rk3026)  
Huy (Harry) Ky — [GitHub](https://github.com/Harry908)  
Dylan Gyori — [GitHub](https://github.com/JustDylan)  
Linh (Jason) Nguyen — [GitHub](https://github.com/linhnt-98)  
Toufic Majdalani — [GitHub](https://github.com/majdaltouzach)  
Ben Bordon — [GitHub](https://github.com/wizkid0101)  

---

![image](https://github.com/user-attachments/assets/474ad122-4b9c-4dab-928d-204cf3081e2e)

## Overview
This project is an arcade-like vertical-scrolling bullet hell video game. Our goal was to replicate the core gameplay of *Touhou 7: Perfect Cherry Blossom* while focusing on game architecture and design patterns. The game was built in C# utilizing the Monogame engine.

## Screenshots

<img width="642" height="512" alt="Screenshot 2025-11-20 170056" src="https://github.com/user-attachments/assets/c251909c-4759-462e-a16a-5023b6a5a273" />
<img width="642" height="512" alt="Screenshot 2025-11-20 170204" src="https://github.com/user-attachments/assets/728e0409-43a7-42b6-9b11-8f65cfd746d4" />
<img width="642" height="512" alt="Screenshot 2025-11-20 170344" src="https://github.com/user-attachments/assets/e00cd1b8-0c1c-42ff-9b9f-7f264505ed90" />
<img width="642" height="512" alt="Screenshot 2025-11-20 170735" src="https://github.com/user-attachments/assets/dd25c2c2-65ea-49f1-8752-165d69be2a5c" />
<img width="642" height="512" alt="Screenshot 2025-11-20 170742" src="https://github.com/user-attachments/assets/bb7235c0-4dc0-4353-981f-332b3c4ce6f5" />
<img width="642" height="512" alt="Screenshot 2025-11-20 170810" src="https://github.com/user-attachments/assets/1c643cb8-7a81-4e0a-9d71-fd35bc324a6b" />

![ezgif com-speed](https://github.com/user-attachments/assets/56822863-69cd-42da-8b45-8e81dfdc30f4)

---

## Game Features
- **Player movement, shooting, and dodging bullets**
- **Unique playable characters** with different attack styles.  
- **The Touhou "grazing" mechanic** - narrowly avoiding bullets increases score.
- **Collectibles and Power-Ups** to improve the player's shooting ability and raise their score.
- **Unique levels** that contain different enemy and boss layouts as well as various parallax backgrounds.
- **Pausing and Setting Management** for key binding, volume management, etc.
- **Time-Based Level Progression** so players can do 'Pacifist Runs', playing levels without shooting.

---

## Gameplay Flow

0. **Main Menu:**
   - At the main menu, a player selects a difficulty, a character to play as, an attack type for their character, and a level.

1. **Level Start:**
   - The level begins, and after a few seconds, some grunt enemies will spawn and fly around the level.
   - The early enemies have simpler bullet patterns, giving the player time to warm up.
   - Enemies have a chance to drop collectible loot that either increases score or gives the player extra shooting power, lives, or bombs.

3. **Escalation:**  
   - As the level progresses, the grunt enemies get stronger.
   - Larger enemy groups.

4. **Mini-Boss Encounter:**  
   - Midway through the level, a **Mini-Boss** appears with multiple attack phases and better stats (faster movement, more damage, etc.)
   - Its health bar shows at top of screen.
   - The Mini-Boss must either be defeated by reducing its HP to **0** or outlasted until it retreats.  

5. **Second Enemy Section:**  
   - After the Mini-Boss, another section of grunt enemies spawning occurs.  
   - New enemy types are introduced.  

6. **Final Boss Battle:**  
   - At the end of the level, the **Final Boss** appears.
   - The Final Boss fight consists of multiple phases.
   - Will be the most difficult to defeat.

7. **Victory:**  
   - The level ends when the player either defeats the Final Boss (by lowering its HP to 0) or survives long enough.  
   - The player’s performance is evaluated based on score, lives remaining, and grazing bonuses.  

---
## Architecture

### Main Architecture: Multi-layered and ECS

**Description:**  
Our software architecture utilizes the multi-layered architecture and Entity-Component System design. We separate the system into four layers: Presentation, Logic, Data-Access, and Data. Within the Logic layer, we implement ECS. Bullets, Enemies, Collectibles, etc., are Entities composed of Components holding data, and Systems operate on those entities based on their components.

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

## Project Evaluation

After wrapping up the project (at least for now), we wanted to reflect on what went well in our game design and what could have been improved.
What went well:
- Scenes as the state pattern made it very easy and intuitive to construct a scene.
- BGM and SFX managers were perfect for what we needed in this kind of game.
- Textures on spritesheets helped with performance.
- Splitting UI components of scenes into their own reusable class (We only got to doing this near the end of our project, but what little we did was great for shortening code).
- The Builder classes worked well to construct entities. Builders could still probably be implemented even if we changed our ECS to be more like a traditional with Entities being just an index.
- Strategy pattern for the Collision types was cool, though I don't know how well it would work with a more traditional ECS system (since components as structs wouldn't allow for containing a class as a field).

What did not go well:
- Our ECS design could have been better. Because this was our first time utilizing ECS in a game, we had to learn along the way. If we redid this project, we would definitely change our ECS design. Namely, we would go with a more performant and traditional design. Our current design had an Entity class. For improvement, we would remove the Entity class entirely, and just make Entities an integer (id). Additionally, components would be structs instead of classes. By using this design, we could get the actual performance benefits of ECS that a bullet-hell game would benefit from. Another poor design within the ECS was the EntityManager class, which had too much responsibility. In traditional ECS systems, our EntityManager is similar to the 'world', and we could have made it more single-responsibility (and fewer lines of code).
- Our animation system was hard to work with. It was tedious to have to define things like currently animating, loop animation, etc. Would like to redo that design and make it more clean.
- Collision detection was okay for what we needed, but we would've liked to research and try to improve it more so we could have more bullets on screen in game.

---

## Credits & Attribution
- **Assets & Inspiration:** ZUN, creator of the Touhou Project.  
- **Engine:** Built using **MonoGame** in **C#**.  
