using BulletHellGame.Data;
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
        private WaveManager _waveManager;
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
        private const int _playableAreaOffset = 15; // Offset for the playable area on all sides

        public TestScene(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            _playableArea = new Rectangle(
                _playableAreaOffset,
                _playableAreaOffset,
                Globals.WindowSize.X * 2 / 3 - 2 * _playableAreaOffset,
                Globals.WindowSize.Y - 2 * _playableAreaOffset
            );
            _uiArea = new Rectangle(_playableArea.Right, 0, Globals.WindowSize.X - _playableArea.Width, Globals.WindowSize.Y);

            this._contentManager = contentManager;
            this._graphicsDevice = graphicsDevice;
            this._entityManager = new EntityManager(this._playableArea);
            this._waveManager = new WaveManager(_entityManager);

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
            CreateWaves();
        }

        private void CreateWaves()
        {
            WaveData wave1 = new WaveData { StartTime = 5f }; // Wave starts at 5 seconds
            wave1.Enemies.Add(new EnemySpawnData
            {
                EnemyData = CreateEnemyData(),
                SpawnTime = 0f,  // Spawns immediately at wave start
                ExitTime = 10f,   // Leaves after 10 seconds
                SpawnPosition = new Vector2(_playableArea.Width / 2, _playableArea.Top)
            });
            wave1.Enemies.Add(new EnemySpawnData
            {
                EnemyData = CreateEnemyData(),
                SpawnTime = 2f,  // Spawns 2 seconds after wave start
                ExitTime = 12f,
                SpawnPosition = new Vector2(_playableArea.Width / 2, _playableArea.Top)
            });

            _waveManager.AddWave(wave1);
        }



        public void Update(GameTime gameTime)
        {
            // Update all entities
            _systemManager.Update(_entityManager, gameTime);
            _waveManager.Update(gameTime);

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

            // Draw a border around the playable area
            DrawBorder(spriteBatch, _playableArea, 3, Color.Red);
        }

        private void DrawBorder(SpriteBatch spriteBatch, Rectangle area, int thickness, Color color)
        {
            // Top border
            spriteBatch.Draw(whitePixel, new Rectangle(area.Left, area.Top, area.Width, thickness), color);
            // Bottom border
            spriteBatch.Draw(whitePixel, new Rectangle(area.Left, area.Bottom - thickness, area.Width, thickness), color);
            // Left border
            spriteBatch.Draw(whitePixel, new Rectangle(area.Left, area.Top, thickness, area.Height), color);
            // Right border
            spriteBatch.Draw(whitePixel, new Rectangle(area.Right - thickness, area.Top, thickness, area.Height), color);
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
                UnfocusedPowerLevels = new Dictionary<int, PowerLevelData>(),
                FocusedPowerLevels = new Dictionary<int, PowerLevelData>()
            };

            for (int i = 0; i <= 8; i++)
            {
                BulletData orangeBullet = new BulletData
                {
                    Damage = 25 + (i * 20), // Increase damage per level
                    BulletType = BulletType.Standard,
                    SpriteName = "Reimu.OrangeBullet"
                };
                BulletData giantCard = new BulletData
                {
                    Damage = 100 + (i * 20),
                    BulletType = BulletType.Homing,
                    SpriteName = "Reimu.GiantCard"
                };
                BulletData homingBullet = new BulletData
                {
                    Damage = 100 + (i * 20), // Increase damage per level
                    BulletType = BulletType.Homing,
                    SpriteName = "Reimu.WhiteBullet"
                };

                // Orange Card Option:
                WeaponData orangeCardWeapon = new WeaponData();
                orangeCardWeapon.BulletData = orangeBullet;
                orangeCardWeapon.FireRate = Math.Max(0.05f, .2f - (i * 0.02f));
                orangeCardWeapon.FireDirections = new List<Vector2>();
                orangeCardWeapon.TimeSinceLastShot = 0;

                // Giant Card Option:
                WeaponData giantCardWeapon = new WeaponData();
                giantCardWeapon.BulletData = giantCard;
                giantCardWeapon.FireRate = Math.Max(0.2f, 0.6f - (i * 0.05f)); // Start 0.6, decrease by .05 per level
                giantCardWeapon.FireDirections = new List<Vector2>();
                giantCardWeapon.TimeSinceLastShot = 0;

                OptionData leftOption = CreateOption(homingBullet, fireRate: 1f - (i * 0.02f));
                leftOption.Offset = new Vector2(-20, 0);
                OptionData rightOption = CreateOption(homingBullet, fireRate: 1f - (i * 0.02f));
                rightOption.Offset = new Vector2(20, 0);

                if (i >= 0)
                {
                    orangeCardWeapon.FireDirections.Add(new Vector2(0, -5f));
                    giantCardWeapon.FireDirections.Add(new Vector2(0, -4f));
                    leftOption.Weapons.First().FireDirections.Add(new Vector2(-0.2f, -3f)); // Slight angle left
                    rightOption.Weapons.First().FireDirections.Add(new Vector2(0.2f, -3f)); // Slight angle right
                }

                if (i >= 3)
                {
                    orangeCardWeapon.FireDirections.Add(new Vector2(-0.2f, -5f));
                    orangeCardWeapon.FireDirections.Add(new Vector2(0.2f, -5f));
                }
                if (i >= 6)
                {
                    orangeCardWeapon.FireDirections.Add(new Vector2(-0.4f, -5f));
                    orangeCardWeapon.FireDirections.Add(new Vector2(0.4f, -5f));
                }

                PowerLevelData focusedPld = new();
                focusedPld.MainWeapons.Add(orangeCardWeapon);
                focusedPld.MainWeapons.Add(giantCardWeapon);
                pd.FocusedPowerLevels[i] = focusedPld;

                PowerLevelData unfocusedPld = new();
                unfocusedPld.MainWeapons.Add(orangeCardWeapon);
                unfocusedPld.Options.Add(leftOption);
                unfocusedPld.Options.Add(rightOption);
                pd.UnfocusedPowerLevels[i] = unfocusedPld;
            }

            _entityManager.SpawnPlayer(pd);
        }


        private OptionData CreateOption(BulletData bulletData, float fireRate)
        {
            return new OptionData
            {
                SpriteName = "Reimu.YinYangOrb",
                Weapons = new List<WeaponData>
                {
                    new WeaponData {
                        BulletData = bulletData,
                        FireRate = Math.Max(0.05f, fireRate), // Prevents fire rate from getting too low
                        FireDirections = new List<Vector2> { }, // Default straight shot
                        TimeSinceLastShot = 0
                    }
                }
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
            // Fire 1 to 2 bullets per shot
            int numBullets = _random.Next(1, 3);

            List<Vector2> fireDirections = new List<Vector2>();
            for (int i = 0; i < numBullets; i++)
            {
                // Randomize an angle between -15 and +15 degrees (centered around downward)
                float randomAngle = -15f + (float)_random.NextDouble() * 30f; // -15 to +15 degrees
                float angleRad = (randomAngle + 90f) * (MathF.PI / 180f); // Convert to radians (90° is downward)

                // Compute direction vector
                fireDirections.Add(new Vector2(MathF.Cos(angleRad), MathF.Sin(angleRad) + 1f));
            }

            var enemy = new EnemyData
            {
                SpriteName = "Fairy.Blue",
                MovementPattern = "zigzag",
                Health = 100,
                Weapons = new List<WeaponData>
                {
                    new WeaponData() {
                        BulletData = new BulletData()
                        {
                            SpriteName = "DoubleCircle.White",
                            Damage = _random.Next(20, 30),
                            BulletType = BulletType.Standard,
                        },
                        FireRate = 1f + (float)_random.NextDouble() * 2f, // Fire rate between 1.0s and 3.0s
                        FireDirections = fireDirections
                    }
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
            // Circular bullet pattern
            int numBullets = _random.Next(8, 13); // Boss fires between 8-12 bullets per shot
            float angleStep = 360f / numBullets; // Evenly distribute bullets

            List<Vector2> fireDirections = new List<Vector2>();
            for (int i = 0; i < numBullets; i++)
            {
                float angle = (i * angleStep) * (MathF.PI / 180f); // Convert degrees to radians
                fireDirections.Add(new Vector2(MathF.Cos(angle), MathF.Sin(angle))); // Circular pattern
            }

            return new EnemyData
            {
                SpriteName = "Cirno",
                MovementPattern = movementPattern,
                Health = 5000,

                Weapons = new List<WeaponData>
                {
                    new WeaponData
                    {
                        BulletData = new BulletData
                        {
                            SpriteName = bulletSprite,
                            Damage = _random.Next(30, 50),
                            BulletType = BulletType.Standard
                        },
                        FireDirections = fireDirections, // Assign generated directions
                        FireRate = 0.3f + (float)_random.NextDouble() * (1.0f - 0.3f) // Random fire rate between 0.3s and 1.0s
                    }
                }
            };
        }
    }
}
