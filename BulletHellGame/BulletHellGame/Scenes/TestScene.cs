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
            
            InitializePlayer();
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

            if (InputManager.Instance.KeyPressed(Keys.F))
                SpawnEnemyGrid(5, 50);

            if (InputManager.Instance.KeyPressed(Keys.G))
                SpawnBoss();
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            _parallaxBackground.Draw(spriteBatch);

            // Draw all entities within the playable area (left 2/3 of the screen)
            _systemManager.Draw(_entityManager, spriteBatch);

            _gameUI.Draw(spriteBatch);
        }
        private void InitializePlayer()
        {
            CharacterData pd = new CharacterData
            {
                SpriteName = "Reimu",
                MovementSpeed = 7f,
                FocusedSpeed = 3f,
                Health = 100,
                InitialLives = 3,
                InitialBombs = 5,
                PowerLevels = new Dictionary<int, PlayerShootingData>()
            };

            Vector2 leftWeaponOffset = new Vector2(-20, 0);
            Vector2 rightWeaponOffset = new Vector2(20, 0);

            for (int i = 0; i <= 8; i++)
            {
                BulletData frontWeaponBullet = new BulletData
                {
                    Damage = 25 + (i * 20), // Increase damage per level
                    BulletType = BulletType.Standard,
                    SpriteName = "Reimu.OrangeBullet"
                };
                BulletData sideWeaponBullet = new BulletData
                {
                    Damage = 100 + (i * 20), // Increase damage per level
                    //BulletType = (i >= 4) ? BulletType.Laser : BulletType.Homing, // Switch to lasers at level 4+
                    BulletType = BulletType.Homing,
                    SpriteName = "Reimu.WhiteBullet"
                };
                WeaponData frontWeapon = new WeaponData();
                frontWeapon.BulletData = frontWeaponBullet;
                frontWeapon.FireRate = Math.Max(0.05f, .2f - (i * 0.02f));
                frontWeapon.FireDirections = new List<Vector2>();
                frontWeapon.TimeSinceLastShot = 0;
                WeaponData leftWeapon = CreateWeapon(sideWeaponBullet, fireRate: 1f - (i * 0.02f), powerLevel: i);
                WeaponData rightWeapon = CreateWeapon(sideWeaponBullet, fireRate: 1f - (i * 0.02f), powerLevel: i);
                if (i >= 0)
                {
                    frontWeapon.FireDirections.Add(new Vector2(0, -5f));
                    leftWeapon.FireDirections.Add(new Vector2(-0.2f, -3f)); // Slight angle left
                    rightWeapon.FireDirections.Add(new Vector2(0.2f, -3f)); // Slight angle right
                }
                // Spread bullets at higher levels
                if (i >= 3)
                {
                    frontWeapon.FireDirections.Add(new Vector2(-0.2f, -5f));
                    frontWeapon.FireDirections.Add(new Vector2(0.2f, -5f));
                    leftWeapon.FireDirections.Add(new Vector2(-0.2f, -1f)); // Slight angle left
                    rightWeapon.FireDirections.Add(new Vector2(0.2f, -1f)); // Slight angle right
                }
                if (i >= 6)
                {
                    frontWeapon.FireDirections.Add(new Vector2(-0.4f, -5f));
                    frontWeapon.FireDirections.Add(new Vector2(0.4f, -5f));
                    leftWeapon.FireDirections.Add(new Vector2(-0.4f, -1f)); // Wider angle left
                    rightWeapon.FireDirections.Add(new Vector2(0.4f, -1f)); // Wider angle right
                }

                PlayerShootingData psd = new();
                psd.FrontWeapon = frontWeapon;
                psd.SideWeapons.Add(leftWeaponOffset, leftWeapon);
                psd.SideWeapons.Add(rightWeaponOffset, rightWeapon);
                pd.PowerLevels[i] = psd;
            }

            _entityManager.SpawnPlayer(pd);
        }

        private WeaponData CreateWeapon(BulletData bulletData, float fireRate, int powerLevel)
        {
            return new WeaponData
            {
                SpriteName = "Reimu.YinYangOrb",
                BulletData = bulletData,
                FireRate = Math.Max(0.05f, fireRate), // Prevents fire rate from getting too low
                FireDirections = new List<Vector2> {}, // Default straight shot
                TimeSinceLastShot = 0
            };
        }


        private void SpawnEnemyGrid(int gridSize, float offset)
        {
            float baseSpawnX = _playableArea.Center.X;
            float baseSpawnY = _playableArea.Top + offset;

            for (int row = 0; row < gridSize; row++)
            {
                for (int col = 0; col < gridSize; col++)
                {
                    float spawnX = baseSpawnX + (col - (gridSize / 2)) * offset;
                    float spawnY = baseSpawnY + row * offset;

                    spawnX = MathHelper.Clamp(spawnX, _playableArea.Left, _playableArea.Right);
                    spawnY = MathHelper.Clamp(spawnY, _playableArea.Top, _playableArea.Bottom);

                    _entityManager.SpawnEnemy(CreateEnemyData(), new Vector2(spawnX, spawnY));
                }
            }
        }

        private EnemyData CreateEnemyData()
        {
            var enemy = new EnemyData
            {
                SpriteName = "Fairy.Blue",
                SpawnPosition = new Vector2(_entityManager.Bounds.Left, _entityManager.Bounds.Top),
                StartPosition = new Vector2(_entityManager.Bounds.Width / 2, _entityManager.Bounds.Height / 2),
                ExitPosition = new Vector2(_entityManager.Bounds.Left, _entityManager.Bounds.Top),
                MovementPattern = "zigzag",
                Health = 100,
                BulletData = new BulletData
                {
                    SpriteName = "DoubleCircle.White",
                    Damage = _random.Next(20, 30),
                    BulletType = BulletType.Standard
                },
                Loot = GenerateRandomLoot()
            };

            return enemy;
        }

        private List<CollectibleData> GenerateRandomLoot()
        {
            var lootTable = new List<(CollectibleData item, float weight)>
            {
                (new CollectibleData { SpriteName = "PowerUpSmall", Effects = { { CollectibleType.PowerUp, 1 } } }, 0.15f),
                (new CollectibleData { SpriteName = "PowerUpLarge", Effects = { { CollectibleType.PowerUp, 8 } } }, 0.01f),
                (new CollectibleData { SpriteName = "PointItem", Effects = { { CollectibleType.PointItem, 10 } } }, 0.01f),
                (new CollectibleData { SpriteName = "BombItem", Effects = { { CollectibleType.Bomb, 1 } } }, 0.10f),
                (new CollectibleData { SpriteName = "FullPowerItem", Effects = { { CollectibleType.PowerUp, 128 } } }, 0.005f),
                (new CollectibleData { SpriteName = "OneUp", Effects = { { CollectibleType.OneUp, 1 } } }, 0.02f),
                (new CollectibleData { SpriteName = "StarItem", Effects = { { CollectibleType.StarItem, 10 } } }, 0.10f),
                (new CollectibleData { SpriteName = "CherryItem", Effects = { { CollectibleType.CherryItem, 10 } } }, 0.15f)
            };

            var selectedLoot = new List<CollectibleData>();

            foreach (var (item, weight) in lootTable)
            {
                if (_random.NextDouble() < weight) // Compare against weight (probability)
                {
                    selectedLoot.Add(item);
                }
            }

            return selectedLoot;
        }


        private void SpawnBoss()
        {
            EnemyData phase1 = CreateBossPhase("DoubleCircle.Red", "circular");
            EnemyData phase2 = CreateBossPhase("DoubleCircle.DarkRed", "zigzag");

            BossData bossData = new BossData { Phases = new List<EnemyData> { phase1, phase2 } };
            Vector2 spawnPos = new Vector2(_entityManager.Bounds.Width / 2, _entityManager.Bounds.Height / 10);
            _entityManager.SpawnBoss(bossData, spawnPos);
        }

        private EnemyData CreateBossPhase(string bulletSprite, string movementPattern)
        {
            return new EnemyData
            {
                SpriteName = "Cirno",
                SpawnPosition = new Vector2(_entityManager.Bounds.Left, _entityManager.Bounds.Top),
                StartPosition = new Vector2(_entityManager.Bounds.Width / 2, _entityManager.Bounds.Height / 2),
                ExitPosition = new Vector2(_entityManager.Bounds.Left, _entityManager.Bounds.Top),
                MovementPattern = movementPattern,
                Health = 5000,
                BulletData = new BulletData
                {
                    SpriteName = bulletSprite,
                    Damage = _random.Next(30, 50),
                    BulletType = BulletType.Standard
                }
            };
        }
    }
}
