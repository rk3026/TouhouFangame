using BulletHellGame.Data.DataTransferObjects;
using BulletHellGame.Entities;
using BulletHellGame.Managers;
using BulletHellGame.UI;
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

        // Sprite/Texture Assets:
        private SpriteData _background;
        private SpriteData _bush1Sprite;
        private SpriteData _bush2Sprite;
        private Texture2D whitePixel;
        private SpriteFont _font;

        // UI Elements:
        private ParallaxBackground _parallaxBackground;
        private GameUI _gameUI;

        // Scene Data:
        private Rectangle _playableArea;
        private Rectangle _uiArea;

        public TestScene(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            _playableArea = new Rectangle(0, 0, Globals.WindowSize.X * 2 / 3, Globals.WindowSize.Y);
            _uiArea = new Rectangle(_playableArea.Width, 0, Globals.WindowSize.X / 3, Globals.WindowSize.Y);

            this._contentManager = contentManager;
            this._graphicsDevice = graphicsDevice;
            this._entityManager = new EntityManager(this._playableArea);

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
            // Load font and textures
            FontManager.Instance.LoadFont(_contentManager, "DFPPOPCorn-W12");
            _font = FontManager.Instance.GetFont("DFPPOPCorn-W12");
            _background = TextureManager.Instance.GetSpriteData("Level1.Background");
            _bush1Sprite = TextureManager.Instance.GetSpriteData("Level1.Bush1");
            _bush2Sprite = TextureManager.Instance.GetSpriteData("Level1.Bush2");

            // Create a 1x1 white pixel texture
            whitePixel = new Texture2D(_graphicsDevice, 1, 1);
            whitePixel.SetData(new Color[] { Color.White });

            _parallaxBackground = new ParallaxBackground();

            // Add background layer (stretched to fill the playable area)
            _parallaxBackground.AddLayer(
                _background.Texture,
                _background.Animations.First().Value.First(),
                _playableArea,
                speed: 100f
            );

            // Determine bush width from sprite data
            int bushWidth = _bush1Sprite.Animations.First().Value.First().Width;

            // Define bush areas on the left and right sides
            Rectangle leftBushArea = new Rectangle(_playableArea.Left, _playableArea.Top, bushWidth, _playableArea.Height);
            Rectangle rightBushArea = new Rectangle(_playableArea.Right - bushWidth, _playableArea.Top, bushWidth, _playableArea.Height);

            // Add left and right bushes as parallax layers
            _parallaxBackground.AddLayer(_bush1Sprite.Texture, _bush1Sprite.Animations.First().Value.First(), leftBushArea, 100f);
            _parallaxBackground.AddLayer(_bush2Sprite.Texture, _bush2Sprite.Animations.First().Value.First(), rightBushArea, 100f);

            _gameUI = new GameUI(_font, whitePixel, _uiArea, _entityManager);
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

            if (_entityManager.GetEntityCount(EntityType.Player) == 0) // if (playerLives == 0) show retry menu
            {
                SceneManager.Instance.AddScene(new RetryMenuScene(_font, whitePixel, new Vector2(_playableArea.Width / 2, _playableArea.Height / 2), _contentManager, _graphicsDevice));
            }

            _parallaxBackground.Update(gameTime);
            _gameUI.Update(gameTime);

            // Spawn a batch of enemies on F key press:
            if (InputManager.Instance.KeyPressed(Keys.F))
            {
                int gridSize = 5;
                float offset = 50; // Distance between enemies
                float baseSpawnX = _playableArea.Center.X; // Start from the center of the playable area
                float baseSpawnY = _playableArea.Top + offset; // Start at the top of the playable area

                for (int row = 0; row < gridSize; row++)
                {
                    for (int col = 0; col < gridSize; col++)
                    {
                        // Adjust spawn positions based on the grid, making sure they are inside the playable area
                        float spawnX = baseSpawnX + (col - (gridSize / 2)) * offset;
                        float spawnY = baseSpawnY + row * offset;

                        // Ensure the enemy's position stays within the bounds of the playable area
                        spawnX = MathHelper.Clamp(spawnX, _playableArea.Left, _playableArea.Right);
                        spawnY = MathHelper.Clamp(spawnY, _playableArea.Top, _playableArea.Bottom);

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
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            _parallaxBackground.Draw(spriteBatch);

            // Draw all entities within the playable area (left 2/3 of the screen)
            _systemManager.Draw(_entityManager, spriteBatch);

            _gameUI.Draw(spriteBatch);
        }
    }
}
