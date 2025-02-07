﻿using BulletHellGame.Components;
using BulletHellGame.Data.DataTransferObjects;
using BulletHellGame.Entities;
using BulletHellGame.Managers;
using Microsoft.Xna.Framework.Content;
using System.Linq;

namespace BulletHellGame.Scenes
{
    public class TestScene : IScene
    {
        private Random _random = new Random();
        // Managers and Graphics:
        private EntityManager _entityManager;
        private SystemManager _systemManager;
        private ContentManager _contentManager;
        private GraphicsDevice _graphicsDevice;

        // Scene Assets:
        private SpriteData background;
        private SpriteData bush1Sprite;
        private SpriteData bush2Sprite;
        private Texture2D whitePixel;

        // Scene Data:
        private float scrollOffset = 0f;
        private const float scrollSpeed = 100f; // Pixels per second
        private float stageTime = 0f; // Timer to track stage duration
        private Rectangle playableArea;
        private Rectangle uiArea;

        // Retry Menu:
        private int retrySelectedIndex = 0;
        private string[] retryOptions = { "Yes", "No" };

        public TestScene(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            playableArea = new Rectangle(0, 0, Globals.WindowSize.X * 2 / 3, Globals.WindowSize.Y);
            uiArea = new Rectangle(playableArea.Width, 0, Globals.WindowSize.X / 3, Globals.WindowSize.Y);

            this._contentManager = contentManager;
            this._graphicsDevice = graphicsDevice;
            this._entityManager = new EntityManager(this.playableArea);

            // Set up system manager:
            this._systemManager = new SystemManager(this._graphicsDevice);

            // Set the player:
            PlayerData pd = new PlayerData();
            pd.SpriteName = "Reimu";
            pd.MovementSpeed = 7f;
            pd.FocusedSpeed = 3f;
            pd.Health = 100;
            pd.InitialLives = 3;
            pd.InitialBombs = 5;
            pd.MaxPower = 4f;

            // Create new bullet data for the weapons of the player
            BulletData bd = new BulletData();
            bd.Damage = 100;
            bd.BulletType = BulletType.Homing;
            bd.SpriteName = "Reimu.WhiteBullet";

            // Create the weapon datas for the player
            WeaponData leftW = new WeaponData();
            leftW.SpriteName = "Reimu.YinYangOrb";
            leftW.FireRate = 1f;
            leftW.FireDirections = new List<Vector2> { new Vector2(-2, -5) };
            leftW.bulletData = bd;
            WeaponData rightW = new WeaponData();
            rightW.SpriteName = "Reimu.YinYangOrb";
            rightW.FireRate = 1f;
            rightW.FireDirections = new List<Vector2> { new Vector2(2, -5) };
            rightW.bulletData = bd;

            // Add two weapons (left and right of the player)
            pd.WeaponsAndOffsets.Add(new Vector2(-20,0), leftW);
            pd.WeaponsAndOffsets.Add(new Vector2(20, 0), rightW);
            this._entityManager.SpawnPlayer(pd);
        }

        public void Load()
        {
            // Use content manager to load assets
            FontManager.Instance.LoadFont(_contentManager, "DFPPOPCorn-W12");
            background = TextureManager.Instance.GetSpriteData("Level1.Background");
            bush1Sprite = TextureManager.Instance.GetSpriteData("Level1.Bush1");
            bush2Sprite = TextureManager.Instance.GetSpriteData("Level1.Bush2");
            // Create a 1x1 white pixel texture for the box
            whitePixel = new Texture2D(_graphicsDevice, 1, 1);
            whitePixel.SetData(new Color[] { Color.White });
        }

        public void Update(GameTime gameTime)
        {
            // Update all entities
            _systemManager.Update(_entityManager, gameTime);

            if (InputManager.Instance.ActionPressed(GameAction.Pause))
            {
                SceneManager.Instance.AddScene(new PausedScene(this._contentManager, this._graphicsDevice));
                return;
            }

            // Update scroll offset based on time elapsed
            scrollOffset = (scrollOffset + (float)(scrollSpeed * gameTime.ElapsedGameTime.TotalSeconds));

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
                        enemyData.SpriteName = "Fairy.Blue";
                        enemyData.SpawnPosition = new Vector2(this._entityManager.Bounds.Left, this._entityManager.Bounds.Top);
                        enemyData.StartPosition = new Vector2(this._entityManager.Bounds.Width / 2, this._entityManager.Bounds.Height / 2);
                        enemyData.ExitPosition = new Vector2(this._entityManager.Bounds.Left, this._entityManager.Bounds.Top);
                        enemyData.MovementPattern = "zigzag";
                        enemyData.Health = 100;
                        enemyData.Type = EnemyType.FairyBlue;

                        CollectibleData cd = new CollectibleData();
                        cd.SpriteName = "PowerUpSmall";
                        cd.Effects.Add(CollectibleType.PowerUp, 10);
                        enemyData.Loot.Add(cd);

                        // Setting up and adding a weapon
                        BulletData bd = new BulletData();
                        bd.SpriteName = "DoubleCircle.White";
                        bd.Damage = _random.Next(20, 30);  // Randomized damage between 20-30
                        bd.BulletType = BulletType.Standard;
                        enemyData.BulletData = bd;

                        _entityManager.SpawnEnemy(enemyData, new Vector2(spawnX, spawnY));

                    }
                }
            }

            if (InputManager.Instance.KeyPressed(Keys.G))
            {
                EnemyData phase1 = new EnemyData();
                phase1.SpriteName = "Cirno";
                phase1.SpawnPosition = new Vector2(this._entityManager.Bounds.Left, this._entityManager.Bounds.Top);
                phase1.StartPosition = new Vector2(this._entityManager.Bounds.Width / 2, this._entityManager.Bounds.Height / 2);
                phase1.ExitPosition = new Vector2(this._entityManager.Bounds.Left, this._entityManager.Bounds.Top);
                phase1.MovementPattern = "circular";
                phase1.Health = 5000;
                phase1.Type = EnemyType.FairyBlue;
                BulletData bd = new BulletData();
                bd.SpriteName = "DoubleCircle.Red";
                bd.Damage = _random.Next(30, 50); // Randomized damage between 30-50
                bd.BulletType = BulletType.Standard;
                phase1.BulletData = bd;

                EnemyData phase2 = new EnemyData();
                phase2.SpriteName = "Cirno";
                phase2.SpawnPosition = new Vector2(this._entityManager.Bounds.Left, this._entityManager.Bounds.Top);
                phase2.StartPosition = new Vector2(this._entityManager.Bounds.Width / 2, this._entityManager.Bounds.Height / 2);
                phase2.ExitPosition = new Vector2(this._entityManager.Bounds.Left, this._entityManager.Bounds.Top);
                phase2.MovementPattern = "zigzag";
                phase2.Health = 5000;
                phase2.Type = EnemyType.FairyBlue;
                BulletData bd2 = new BulletData();
                bd2.SpriteName = "DoubleCircle.DarkRed";
                bd2.Damage = _random.Next(30, 50); // Randomized damage between 30-50
                bd2.BulletType = BulletType.Standard;
                phase2.BulletData = bd2;

                BossData bossData = new BossData();
                bossData.Phases = new List<EnemyData>() {
                    phase1,
                    phase2,
                };

                Vector2 spawnPos = new Vector2(this._entityManager.Bounds.Width / 2, this._entityManager.Bounds.Height / 10);

                _entityManager.SpawnBoss(bossData, spawnPos);
            }

            // Retry Menu:
            if (_entityManager.GetEntityCount(EntityType.Player) == 0)
            {
                if (InputManager.Instance.ActionPressed(GameAction.MenuUp) && retrySelectedIndex > 0)
                    retrySelectedIndex--;
                if (InputManager.Instance.ActionPressed(GameAction.MenuDown) && retrySelectedIndex < retryOptions.Length - 1)
                    retrySelectedIndex++;

                if (InputManager.Instance.ActionPressed(GameAction.Select))
                {
                    if (retrySelectedIndex == 0)
                    {
                        // Logic to restart the test scene
                        SceneManager.Instance.RemoveScene();
                        SceneManager.Instance.AddScene(new TestScene(_contentManager, _graphicsDevice));
                    }
                    else
                    {
                        // Logic to return to main menu
                        SceneManager.Instance.RemoveScene();
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
            for (int i = 0; i < numTiles; ++i)
            {
                // Calculate the Y position for each repeated background tile
                float tileOffset = ((int)(scrollOffset) % backgroundHeight) - backgroundHeight + (i * backgroundHeight) ;

                // Draw the background tiles at calculated positions
                spriteBatch.Draw(
                    backgroundTexture,
                    new Rectangle(playableArea.Left, (int)tileOffset, (int)(playableArea.Width), backgroundHeight),
                    background.Animations.First().Value.First(),
                    Color.White
                );
            }

            // Draw the non-stretched bushes on the left side of the playable area
            for (float y = scrollOffset % bush1Sprite.Animations.First().Value.First().Height - bush1Sprite.Animations.First().Value.First().Height; y < playableArea.Height; y += bush1Sprite.Animations.First().Value.First().Height)
            {
                spriteBatch.Draw(
                    bush1Sprite.Texture,
                    new Vector2(playableArea.Left, y),
                    bush1Sprite.Animations.First().Value.First(),
                    Color.White
                );
            }

            // Draw the non-stretched bushes on the right side of the playable area
            for (float y = scrollOffset % bush1Sprite.Animations.First().Value.First().Height - bush1Sprite.Animations.First().Value.First().Height; y  < playableArea.Height; y += bush2Sprite.Animations.First().Value.First().Height)
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

            // Draw retry menu if player is null
            if (_entityManager.GetEntityCount(EntityType.Player) == 0)
            {
                DrawRetryMenu(spriteBatch);
            }
        }


        private void DrawUI(SpriteBatch spriteBatch)
        {
            // Draw the black background for the UI area
            spriteBatch.Draw(
                whitePixel,
                uiArea,
                Color.Black
            );

            SpriteFont font = FontManager.Instance.GetFont("DFPPOPCorn-W12");
            Vector2 position = new Vector2(uiArea.Left + 10, uiArea.Top + 10);

            int bulletCount = _entityManager.GetEntityCount(EntityType.Bullet);
            int enemyCount = _entityManager.GetEntityCount(EntityType.Enemy);
            int collectibleCount = _entityManager.GetEntityCount(EntityType.Collectible);
            int characterCount = _entityManager.GetEntityCount(EntityType.Player);

            spriteBatch.DrawString(font, $"Bullets: {bulletCount}", position, Color.White);
            position.Y += 20;
            spriteBatch.DrawString(font, $"Enemies: {enemyCount}", position, Color.White);
            position.Y += 20;
            spriteBatch.DrawString(font, $"Collectibles: {collectibleCount}", position, Color.White);
            position.Y += 20;
            spriteBatch.DrawString(font, $"Players: {characterCount}", position, Color.White);
            position.Y += 20;
            spriteBatch.DrawString(font, $"Stage Time: {stageTime:F1}", position, Color.White);
            position.Y += 40; // Extra spacing before player stats

            // Get the player entity
            foreach (Entity player in _entityManager.GetEntitiesWithComponents(typeof(PlayerStatsComponent)))
            {
                var playerStats = player.GetComponent<PlayerStatsComponent>();
                if (playerStats != null)
                {
                    spriteBatch.DrawString(font, $"Score: {playerStats.Score:F1}", position, Color.White);
                    position.Y += 20;
                    spriteBatch.DrawString(font, $"Lives: {playerStats.Lives}", position, Color.White);
                    position.Y += 20;
                    spriteBatch.DrawString(font, $"Bombs: {playerStats.Bombs}", position, Color.White);
                    position.Y += 20;
                    spriteBatch.DrawString(font, $"Power: {playerStats.Power:F1}", position, Color.White);
                    position.Y += 20;
                    spriteBatch.DrawString(font, $"Cherry Points: {playerStats.CherryPoints:F1}", position, Color.White);
                }
            }
        }


        private void DrawRetryMenu(SpriteBatch spriteBatch)
        {
            SpriteFont font = FontManager.Instance.GetFont("DFPPOPCorn-W12");
            string message = "Retry?";

            Vector2 centerScreen = new Vector2(Globals.WindowSize.X / 2, Globals.WindowSize.Y / 2);
            Vector2 messageSize = font.MeasureString(message);
            Vector2 messagePos = centerScreen - messageSize / 2;

            // Determine the full size of the menu box
            float maxWidth = messageSize.X;
            float totalHeight = messageSize.Y;

            Vector2[] optionPositions = new Vector2[retryOptions.Length];

            for (int i = 0; i < retryOptions.Length; i++)
            {
                Vector2 optionSize = font.MeasureString(retryOptions[i]);
                if (optionSize.X > maxWidth)
                    maxWidth = optionSize.X; // Keep track of the widest text

                optionPositions[i] = centerScreen + new Vector2(0, (i + 1) * 40) - optionSize / 2;
                totalHeight += optionSize.Y + 20; // Adding spacing
            }

            // Rectangle Padding
            Vector2 padding = new Vector2(40, 30);
            Rectangle backgroundRect = new Rectangle(
                (int)(centerScreen.X - (maxWidth / 2) - padding.X / 2),
                (int)(messagePos.Y - padding.Y / 2),
                (int)(maxWidth + padding.X),
                (int)(totalHeight + padding.Y)
            );

            // Draw background rectangle (semi-transparent black)
            spriteBatch.Draw(whitePixel, backgroundRect, Color.Black * 0.7f);

            // Draw text
            spriteBatch.DrawString(font, message, messagePos, Color.White);

            for (int i = 0; i < retryOptions.Length; i++)
            {
                Color color = (i == retrySelectedIndex) ? Color.Yellow : Color.White;
                spriteBatch.DrawString(font, retryOptions[i], optionPositions[i], color);
            }
        }


    }
}
