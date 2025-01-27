using BulletHellGame.Data.DataTransferObjects;
using BulletHellGame.Entities;
using BulletHellGame.Managers;
using BulletHellGame.Systems;
using Microsoft.Xna.Framework.Content;
using System.Linq;

namespace BulletHellGame.Scenes
{
    public class TestScene : IScene
    {
        private EntityManager _entityManager;
        private SystemManager _systemManager;
        private ContentManager _contentManager;
        private GraphicsDevice _graphicsDevice;

        private SpriteData background;
        private SpriteData bush1Sprite;
        private SpriteData bush2Sprite;
        private Texture2D whitePixel;

        private float scrollOffset = 0f;
        private const float scrollSpeed = 100f; // Pixels per second
        private float stageTime = 0f; // Timer to track stage duration
        private Rectangle playableArea;
        private Rectangle uiArea;

        public TestScene(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            playableArea = new Rectangle(0, 0, Globals.WindowSize.X * 2 / 3, Globals.WindowSize.Y);
            uiArea = new Rectangle(playableArea.Width, 0, Globals.WindowSize.X / 3, Globals.WindowSize.Y);

            this._contentManager = contentManager;
            this._graphicsDevice = graphicsDevice;
            this._entityManager = new EntityManager(this.playableArea);

            // Set up system manager and systems:
            this._systemManager = new SystemManager();
            _systemManager.AddSystem(new CollisionSystem());
            _systemManager.AddSystem(new HealthSystem());
            _systemManager.AddSystem(new MovementSystem());
            _systemManager.AddSystem(new PlayerInputSystem());
            _systemManager.AddSystem(new ShootingSystem());
            _systemManager.AddSystem(new DrawingSystem());
            _systemManager.AddSystem(new HomingSystem());
            _systemManager.AddSystem(new MovementPatternSystem());

            // Set the player:
            PlayerData pd = new PlayerData();
            pd.Name = "Reimu";
            pd.SpriteName = "Reimu";
            pd.MovementSpeed = 7f;
            pd.FocusedSpeed = 3f;
            pd.Health = 100;
            this._entityManager.SetPlayerCharacter(pd);
        }

        public void Load()
        {
            // Use content manager to load assets
            FontManager.Instance.LoadFont(_contentManager, "DFPPOPCorn-W12");
            background = TextureManager.Instance.GetSpriteData("Level1Background");
            bush1Sprite = TextureManager.Instance.GetSpriteData("Bush1");
            bush2Sprite = TextureManager.Instance.GetSpriteData("Bush2");
            // Create a 1x1 white pixel texture for the box
            whitePixel = new Texture2D(_graphicsDevice, 1, 1);
            whitePixel.SetData(new Color[] { Color.White });
        }

        public void Update(GameTime gameTime)
        {
            // Update all entities
            _systemManager.Update(_entityManager, gameTime);

            // Update scroll offset based on time elapsed
            scrollOffset = (scrollOffset + (float)(scrollSpeed * gameTime.ElapsedGameTime.TotalSeconds)) % bush1Sprite.Animations.First().Value.First().Height;

            // Update stage timer
            stageTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Spawn a batch of enemies on F key press:
            if (InputManager.Instance.KeyPressed(Keys.F))
            {
                int gridSize = 5;
                float offset = 50; // Distance between enemies
                float baseSpawnX = playableArea.Center.X; // Start from the center of the playable area
                float baseSpawnY = playableArea.Top + offset; // Start at the top of the playable area

                for (int row = 0; row < gridSize; row++)
                {
                    for (int col = 0; col < gridSize; col++)
                    {
                        // Adjust spawn positions based on the grid, making sure they are inside the playable area
                        float spawnX = baseSpawnX + (col - (gridSize / 2)) * offset;
                        float spawnY = baseSpawnY + row * offset;

                        // Ensure the enemy's position stays within the bounds of the playable area
                        spawnX = MathHelper.Clamp(spawnX, playableArea.Left, playableArea.Right);
                        spawnY = MathHelper.Clamp(spawnY, playableArea.Top, playableArea.Bottom);

                        EnemyData enemyData = new EnemyData();
                        enemyData.SpawnPosition = new Vector2(this._entityManager.Bounds.Left, this._entityManager.Bounds.Top);
                        enemyData.StartPosition = new Vector2(this._entityManager.Bounds.Width / 2, this._entityManager.Bounds.Height / 2);
                        enemyData.ExitPosition = new Vector2(this._entityManager.Bounds.Left, this._entityManager.Bounds.Top);
                        enemyData.MovementPattern = MovementPattern.Zigzag;
                        BulletPatternData bpd = new BulletPatternData();
                        bpd.FireRate = 0.5f;
                        bpd.Speed = 50f;
                        enemyData.BulletPattern = bpd;
                        enemyData.Health = 100;
                        enemyData.Type = EnemyType.FairyBlue;

                        _entityManager.SpawnEnemy(enemyData, new Vector2(spawnX, spawnY));

                    }
                }
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            // Get the background texture dimensions
            var backgroundTexture = background.Texture;
            int backgroundHeight = background.Animations.First().Value.First().Height;

            // Calculate how many times the background texture needs to be repeated
            int numTiles = (int)Math.Ceiling((float)playableArea.Height / backgroundHeight) + 1;

            // Draw the scrolling background within the playable area
            for (int i = 0; i < numTiles; i++)
            {
                // Calculate the Y position for each repeated background tile
                float tileOffset = (scrollOffset + (i * backgroundHeight)) % backgroundHeight;

                // Draw the background tiles at calculated positions
                spriteBatch.Draw(
                    backgroundTexture,
                    new Rectangle(playableArea.Left, (int)(-tileOffset + (i * backgroundHeight)), (int)(playableArea.Width), backgroundHeight),
                    background.Animations.First().Value.First(),
                    Color.White
                );
            }

            // Draw the non-stretched bushes on the left side of the playable area
            for (float y = -scrollOffset; y < playableArea.Height; y += bush1Sprite.Animations.First().Value.First().Height)
            {
                spriteBatch.Draw(
                    bush1Sprite.Texture,
                    new Vector2(playableArea.Left, y),
                    bush1Sprite.Animations.First().Value.First(),
                    Color.White
                );
            }

            // Draw the non-stretched bushes on the right side of the playable area
            for (float y = -scrollOffset; y < playableArea.Height; y += bush2Sprite.Animations.First().Value.First().Height)
            {
                spriteBatch.Draw(
                    bush2Sprite.Texture,
                    new Vector2(playableArea.Right - bush2Sprite.Animations.First().Value.First().Width, y),
                    bush2Sprite.Animations.First().Value.First(),
                    Color.White
                );
            }

            // Draw all entities within the playable area (left 2/3 of the screen)
            _systemManager.Draw(_entityManager, spriteBatch);

            // Draw UI elements (this area is reserved for the right part of the screen)
            DrawUI(spriteBatch);
        }


        private void DrawUI(SpriteBatch spriteBatch)
        {
            // Draw the blue background for the UI area
            spriteBatch.Draw(
                whitePixel,
                uiArea,
                Color.Black
            );

            SpriteFont font = FontManager.Instance.GetFont("DFPPOPCorn-W12");
            Vector2 position = new Vector2(uiArea.Left + 10, uiArea.Top + 10);

            int bulletCount = _entityManager.GetBulletCount();
            int enemyCount = _entityManager.GetEnemyCount();
            int collectibleCount = _entityManager.GetCollectibleCount();
            int characterCount = _entityManager.GetPlayerCount();
            spriteBatch.DrawString(font, $"Bullets: {bulletCount}", position, Color.White);
            position.Y += 20;
            spriteBatch.DrawString(font, $"Enemies: {enemyCount}", position, Color.White);
            position.Y += 20;
            spriteBatch.DrawString(font, $"Collectibles: {collectibleCount}", position, Color.White);
            position.Y += 20;
            spriteBatch.DrawString(font, $"Players: {characterCount}", position, Color.White);
            position.Y += 20;
            spriteBatch.DrawString(font, $"Stage Time: {stageTime:F1}", position, Color.White);
        }
    }
}
