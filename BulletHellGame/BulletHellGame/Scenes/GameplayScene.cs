using BulletHellGame.Components;
using BulletHellGame.Entities.Bullets;
using BulletHellGame.Entities.Characters.Enemies;
using BulletHellGame.Entities.Collectibles;
using BulletHellGame.Managers;
using Microsoft.Xna.Framework.Content;
using System.Linq;

namespace BulletHellGame.Scenes
{
    public class GameplayScene : IScene
    {
        private ContentManager _contentManager;

        public GameplayScene(ContentManager contentManager) {
            this._contentManager = contentManager;
        }

        public void Load()
        {
            // Use content manager to load assets
        }

        public void Update(GameTime gameTime)
        {
            // Update all entities
            EntityManager.Instance.Update(gameTime);

            // Spawn a batch of enemies on F key press:
            if (InputManager.Instance.KeyPressed(Keys.F))
            {
                // Check how many enemies are currently in the game
                int enemyCount = EntityManager.Instance.GetActiveEntities().OfType<Enemy>().Count();

                // Only spawn a new batch if there are no enemies currently in the game
                if (true)//if (enemyCount == 0)
                {
                    // Define the grid size (5x5)
                    int gridSize = 5;

                    // Offset from the center to the left or right to create a 5x5 grid
                    float offset = 50;  // Adjust this to control the distance between enemies

                    // Calculate the base spawn position at the top middle of the screen
                    float baseSpawnX = Globals.WindowSize.X / 2;  // Start from the middle of the screen
                    float spawnY = 0;  // At the top of the screen

                    // Spawn the enemies in a 5x5 grid, centered on the screen
                    for (int row = 0; row < gridSize; row++)
                    {
                        for (int col = 0; col < gridSize; col++)
                        {
                            // Calculate the spawn position for each enemy
                            float spawnX = baseSpawnX + (col - 2) * offset;  // Offset left/right based on the column
                            float spawnOffsetY = (row + 2) * offset;  // Offset up/down based on the row

                            // Create the enemy at the calculated position
                            Enemy enemy = EntityManager.Instance.SpawnEnemy(EnemyType.Generic1, new Vector2(spawnX, spawnY + spawnOffsetY));
                            enemyCount = EntityManager.Instance.GetActiveEntities().OfType<Enemy>().Count();
                            if (enemyCount % 2 == 0)
                            {
                                enemy.AddComponent(new MovementPatternComponent(enemy, "zigzag"));
                            }
                            else
                            {
                                enemy.AddComponent(new MovementPatternComponent(enemy, "circular"));
                            }
                        }
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw all entities
            EntityManager.Instance.Draw(spriteBatch);

            // Get counts for bullets, enemies, collectibles, and playable characters
            int bulletCount = EntityManager.Instance.GetActiveEntities().OfType<Bullet>().Count();
            int enemyCount = EntityManager.Instance.GetActiveEntities().OfType<Enemy>().Count();
            int collectibleCount = EntityManager.Instance.GetActiveEntities().OfType<Collectible>().Count();
            int characterCount = EntityManager.Instance.GetActiveEntities().OfType<PlayableCharacter>().Count();

            SpriteFont font = FontManager.Instance.GetFont("Arial");  // Make sure to load an appropriate font in LoadContent

            // Define a position to draw the counts (top-right corner)
            Vector2 position = new Vector2(10, 10);  // Start drawing a bit away from the corner to avoid clipping

            // Draw the text showing the counts
            spriteBatch.DrawString(font, $"Bullets: {bulletCount}", position, Color.White);
            position.Y += 20;  // Move down for the next line
            spriteBatch.DrawString(font, $"Enemies: {enemyCount}", position, Color.White);
            position.Y += 20;  // Move down for the next line
            spriteBatch.DrawString(font, $"Collectibles: {collectibleCount}", position, Color.White);
            position.Y += 20;  // Move down for the next line
            spriteBatch.DrawString(font, $"Playable Characters: {characterCount}", position, Color.White);
        }
    }
}
