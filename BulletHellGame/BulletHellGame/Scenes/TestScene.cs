using BulletHellGame.Components;
using BulletHellGame.Data.DataTransferObjects;
using BulletHellGame.Entities.Bullets;
using BulletHellGame.Entities.Characters.Enemies;
using BulletHellGame.Entities.Collectibles;
using BulletHellGame.Managers;
using Microsoft.Xna.Framework.Content;
using System.Linq;

namespace BulletHellGame.Scenes
{
    public class TestScene : IScene
    {
        private ContentManager _contentManager;
        private SpriteData background;
        private SpriteData bush1Sprite;
        private SpriteData bush2Sprite;

        private float scrollOffset = 0f;
        private const float scrollSpeed = 100f; // Pixels per second
        private float stageTime = 0f; // Timer to track stage duration

        public TestScene(ContentManager contentManager)
        {
            this._contentManager = contentManager;
        }

        public void Load()
        {
            // Use content manager to load assets
            FontManager.Instance.LoadFont(_contentManager, "DFPPOPCorn-W12");
            background = TextureManager.Instance.GetSpriteData("Level1Background");
            bush1Sprite = TextureManager.Instance.GetSpriteData("Bush1");
            bush2Sprite = TextureManager.Instance.GetSpriteData("Bush2");
        }

        public void Update(GameTime gameTime)
        {
            // Update all entities
            EntityManager.Instance.Update(gameTime);

            // Update scroll offset based on time elapsed
            scrollOffset = (scrollOffset + (float)(scrollSpeed * gameTime.ElapsedGameTime.TotalSeconds)) % bush1Sprite.Animations.First().Value.First().Height;

            // Update stage timer
            stageTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Spawn a batch of enemies on F key press:
            if (InputManager.Instance.KeyPressed(Keys.F))
            {
                int enemyCount = EntityManager.Instance.GetActiveEntities().OfType<Enemy>().Count();

                if (true) //if (enemyCount == 0)
                {
                    int gridSize = 5;
                    float offset = 50; // Distance between enemies
                    float baseSpawnX = Globals.WindowSize.X / 2; // Start from the middle
                    float spawnY = 0;

                    for (int row = 0; row < gridSize; row++)
                    {
                        for (int col = 0; col < gridSize; col++)
                        {
                            float spawnX = baseSpawnX + (col - 2) * offset;
                            float spawnOffsetY = (row + 2) * offset;

                            Enemy enemy = EntityManager.Instance.SpawnEnemy(EnemyType.FairyBlue, new Vector2(spawnX, spawnY + spawnOffsetY));
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
            // Draw scrolling background
            float textureHeight = background.Animations.First().Value.First().Height;
            float textureWidth = background.Animations.First().Value.First().Width;

            for (float y = -scrollOffset; y < Globals.WindowSize.Y; y += textureHeight)
            {
                for (float x = 0; x < Globals.WindowSize.X; x += textureWidth)
                {
                    spriteBatch.Draw(
                        background.Texture,
                        new Vector2(x, y),
                        background.Animations.First().Value.First(),
                        Color.White
                    );
                }
            }

            // Draw scrolling bushes
            float bushHeight = bush1Sprite.Animations.First().Value.First().Height;
            float bushWidth = bush1Sprite.Animations.First().Value.First().Width;

            // Left side bushes
            for (float y = -scrollOffset; y < Globals.WindowSize.Y; y += bushHeight)
            {
                spriteBatch.Draw(
                    bush1Sprite.Texture,
                    new Vector2(0, y),
                    bush1Sprite.Animations.First().Value.First(),
                    Color.White
                );
            }

            // Right side bushes
            for (float y = -scrollOffset; y < Globals.WindowSize.Y; y += bushHeight)
            {
                spriteBatch.Draw(
                    bush2Sprite.Texture,
                    new Vector2(Globals.WindowSize.X - bushWidth, y),
                    bush2Sprite.Animations.First().Value.First(),
                    Color.White
                );
            }

            // Draw all entities
            EntityManager.Instance.Draw(spriteBatch);

            // Display debug info
            DisplayDebugInfo(spriteBatch);
        }

        private void DisplayDebugInfo(SpriteBatch spriteBatch)
        {
            int bulletCount = EntityManager.Instance.GetActiveEntities().OfType<Bullet>().Count();
            int enemyCount = EntityManager.Instance.GetActiveEntities().OfType<Enemy>().Count();
            int collectibleCount = EntityManager.Instance.GetActiveEntities().OfType<Collectible>().Count();
            int characterCount = EntityManager.Instance.GetActiveEntities().OfType<PlayableCharacter>().Count();

            SpriteFont font = FontManager.Instance.GetFont("DFPPOPCorn-W12");

            Vector2 position = new Vector2(10, 10);
            spriteBatch.DrawString(font, $"Bullets: {bulletCount}", position, Color.White);
            position.Y += 20;
            spriteBatch.DrawString(font, $"Enemies: {enemyCount}", position, Color.White);
            position.Y += 20;
            spriteBatch.DrawString(font, $"Collectibles: {collectibleCount}", position, Color.White);
            position.Y += 20;
            spriteBatch.DrawString(font, $"Playable Characters: {characterCount}", position, Color.White);
            position.Y += 20;

            // Display stage time
            spriteBatch.DrawString(font, $"Stage Time: {stageTime:F2} seconds", position, Color.White);
        }
    }
}
