using BulletHellGame.DataAccess.DataTransferObjects;
using BulletHellGame.Logic.Entities;
using System.Linq;

namespace BulletHellGame.Logic.Utilities.EntityDataGenerator
{
    public class EntityDataGenerator
    {
        private readonly static Random _random = new();
        private readonly static List<string> _bulletSprites = new() {
            "DoubleCircle.Gray", "DoubleCircle.DarkRed", "DoubleCircle.Red", "DoubleCircle.Purple",
            "DoubleCircle.Pink", "DoubleCircle.DarkBlue", "DoubleCircle.Blue", "DoubleCircle.Teal",
            "DoubleCircle.LightBlue", "DoubleCircle.Green", "DoubleCircle.LightGreen", "DoubleCircle.Chartreuse",
            "DoubleCircle.DarkYellow", "DoubleCircle.Yellow", "DoubleCircle.Orange", "DoubleCircle.White"
        };

        public static LevelData GenerateLevelData(Rectangle playableArea)
        {
            var levelData = new LevelData();

            levelData.LevelName = "Level 1";
            levelData.LevelId = 1;

            // Add 3 waves
            for (int i = 0; i < 5; i++)
            {
                WaveData wave = CreateWaveData(playableArea);
                levelData.Waves.Add(wave);
            }

            // Add 1 boss
            BossData boss = CreateBossData();
            levelData.Bosses.Add(boss);

            return levelData;
        }

        public static WaveData CreateWaveData(Rectangle playableArea)
        {
            WaveData wave1 = new WaveData { StartTime = 0f , Duration=20f }; // Wave starts at 5 seconds
            wave1.Enemies.Add(new EnemySpawnData
            {
                EnemyData = CreateEnemyData(),
                SpawnTime = 0f,  // Spawns immediately at wave start
                ExitTime = 10f,   // Leaves after 10 seconds
                SpawnPosition = new Vector2(playableArea.Width / 2 +50, playableArea.Top+50)
            });
            wave1.Enemies.Add(new EnemySpawnData
            {
                EnemyData = CreateEnemyData(),
                SpawnTime = 0f,  // Spawns 2 seconds after wave start
                ExitTime = 10f,
                SpawnPosition = new Vector2(playableArea.Width / 2 -50, playableArea.Top+50)
            });
            return wave1;
        }

        public static CharacterData CreateReimuData()
        {
            CharacterData pd = new CharacterData
            {
                Name = "Reimu",
                SpriteName = "Reimu",
                MovementSpeed = 7f,
                FocusedSpeed = 3f,
                Health = 1,
                InitialLives = 3,
                InitialBombs = 5,
                ShotTypes = new List<ShotTypeData>() {
                    new ShotTypeData
                    {
                        Name = "Test",
                        UnfocusedShot = new ShotData
                        {
                            PowerLevels = new Dictionary<int, PowerLevelData>()
                        },
                        FocusedShot = new ShotData
                        {
                            PowerLevels = new Dictionary<int, PowerLevelData>()
                        }
                    }
                }
            };

            for (int i = 0; i <= 8; i++)
            {
                BulletData orangeBullet = new BulletData
                {
                    Damage = 25 + i * 20, // Increase damage per level
                    BulletType = BulletType.Standard,
                    SpriteName = "Reimu.OrangeBullet"
                };
                BulletData giantCard = new BulletData
                {
                    Damage = 100 + i * 20,
                    BulletType = BulletType.Homing,
                    SpriteName = "Reimu.GiantCard"
                };
                BulletData homingBullet = new BulletData
                {
                    Damage = 100 + i * 20, // Increase damage per level
                    BulletType = BulletType.Homing,
                    SpriteName = "Reimu.WhiteBullet"
                };

                // Orange Card Weapon:
                WeaponData orangeCardWeapon = new WeaponData();
                orangeCardWeapon.BulletData = orangeBullet;
                orangeCardWeapon.FireRate = Math.Max(0.05f, .2f - i * 0.02f);
                orangeCardWeapon.FireDirections = new List<Vector2>();
                orangeCardWeapon.TimeSinceLastShot = 0;

                // Giant Card Weapon:
                WeaponData giantCardWeapon = new WeaponData();
                giantCardWeapon.BulletData = giantCard;
                giantCardWeapon.FireRate = Math.Max(0.2f, 0.6f - i * 0.05f); // Start 0.6, decrease by .05 per level
                giantCardWeapon.FireDirections = new List<Vector2>();
                giantCardWeapon.TimeSinceLastShot = 0;

                OptionData leftOption = CreateOption("Reimu.YinYangOrb", homingBullet, fireRate: 1f - i * 0.02f);
                leftOption.Offset = new Vector2(-20, 0);
                OptionData rightOption = CreateOption("Reimu.YinYangOrb", homingBullet, fireRate: 1f - i * 0.02f);
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
                pd.ShotTypes.First().FocusedShot.PowerLevels[i] = focusedPld;

                PowerLevelData unfocusedPld = new();
                unfocusedPld.MainWeapons.Add(orangeCardWeapon);
                unfocusedPld.Options.Add(leftOption);
                unfocusedPld.Options.Add(rightOption);
                pd.ShotTypes.First().UnfocusedShot.PowerLevels[i] = unfocusedPld;
            }
            return pd;
        }

        public static CharacterData CreateMarisaData()
        {
            CharacterData pd = new CharacterData
            {
                Name = "Marisa Kirisame",
                SpriteName = "Marisa",
                MovementSpeed = 8f,
                FocusedSpeed = 4f,
                Health = 1,
                InitialLives = 3,
                InitialBombs = 5,
                ShotTypes = new List<ShotTypeData>() {
                    new ShotTypeData
                    {
                        Name = "Test",
                        UnfocusedShot = new ShotData { PowerLevels = new Dictionary<int, PowerLevelData>() },
                        FocusedShot = new ShotData { PowerLevels = new Dictionary<int, PowerLevelData>() }
                    }
                }
            };

            for (int i = 0; i <= 8; i++)
            {
                BulletData laser = new BulletData { Damage = 50 + i * 30, BulletType = BulletType.Standard, SpriteName = "Marisa.LightBeam", RotationSpeed = 0f };
                BulletData rocket = new BulletData { Damage = 80 + i * 25, BulletType = BulletType.Homing, SpriteName = "Marisa.Rocket" , RotationSpeed = 0f };

                WeaponData laserWeapon = new WeaponData { BulletData = laser, FireRate = Math.Max(0.1f, 0.5f - i * 0.04f), FireDirections = new List<Vector2> { new Vector2(0, -5f) }, TimeSinceLastShot = 0 };
                WeaponData rocketWeapon = new WeaponData { BulletData = rocket, FireRate = Math.Max(0.2f, 0.7f - i * 0.05f), FireDirections = new List<Vector2>(), TimeSinceLastShot = 0 };

                OptionData leftOption = CreateOption("Marisa.StarOption", rocket, fireRate: 1f - i * 0.02f);
                leftOption.Offset = new Vector2(-20, 0);
                OptionData rightOption = CreateOption("Marisa.StarOption", rocket, fireRate: 1f - i * 0.02f);
                rightOption.Offset = new Vector2(20, 0);

                if (i >= 0)
                {
                    laserWeapon.FireDirections.Add(new Vector2(0, -5f));
                    rocketWeapon.FireDirections.Add(new Vector2(0, -4f));
                    leftOption.Weapons.First().FireDirections.Add(new Vector2(-0.2f, -3f)); // Slight angle left
                    rightOption.Weapons.First().FireDirections.Add(new Vector2(0.2f, -3f)); // Slight angle right
                }

                if (i >= 3)
                {
                    laserWeapon.FireDirections.Add(new Vector2(-0.2f, -5f));
                    rocketWeapon.FireDirections.Add(new Vector2(0.2f, -5f));
                }
                if (i >= 6)
                {
                    laserWeapon.FireDirections.Add(new Vector2(-0.4f, -5f));
                    rocketWeapon.FireDirections.Add(new Vector2(0.4f, -5f));
                }

                PowerLevelData focusedPld = new();
                focusedPld.MainWeapons.Add(rocketWeapon);
                pd.ShotTypes.First().FocusedShot.PowerLevels[i] = focusedPld;

                PowerLevelData unfocusedPld = new();
                unfocusedPld.MainWeapons.Add(laserWeapon);
                unfocusedPld.Options.Add(leftOption);
                unfocusedPld.Options.Add(rightOption);
                pd.ShotTypes.First().UnfocusedShot.PowerLevels[i] = unfocusedPld;
            }
            return pd;
        }

        public static CharacterData CreateSakuyaData()
        {
            CharacterData pd = new CharacterData
            {
                Name = "Sakuya Izayoi",
                SpriteName = "Sakuya",
                MovementSpeed = 6f,
                FocusedSpeed = 2.5f,
                Health = 1,
                InitialLives = 3,
                InitialBombs = 5,
                ShotTypes = new List<ShotTypeData>() {
                    new ShotTypeData
                    {
                        Name = "Test",
                        UnfocusedShot = new ShotData { PowerLevels = new Dictionary<int, PowerLevelData>() },
                        FocusedShot = new ShotData { PowerLevels = new Dictionary<int, PowerLevelData>() }
                    }
                }
            };

            for (int i = 0; i <= 8; i++)
            {
                BulletData blueKnife = new BulletData { Damage = 40 + i * 20, BulletType = BulletType.Standard, SpriteName = "Sakuya.KnifeBlue", RotationSpeed = 0f };
                BulletData pinkKnife = new BulletData { Damage = 120 + i * 30, BulletType = BulletType.Standard, SpriteName = "Sakuya.KnifePink", RotationSpeed = 0f };

                WeaponData knifeSpread = new WeaponData { BulletData = blueKnife, FireRate = Math.Max(0.15f, 0.4f - i * 0.03f), FireDirections = new List<Vector2>(), TimeSinceLastShot = 0 };
                for (int j = -1; j <= 1; j++)
                {
                    knifeSpread.FireDirections.Add(new Vector2(j * 0.3f, -5f));
                }

                WeaponData mainWeapon = new WeaponData { BulletData = pinkKnife, FireRate = Math.Max(0.5f, 1f - i * 0.1f), FireDirections = new List<Vector2> { new Vector2(0, -4f) }, TimeSinceLastShot = 0 };

                OptionData leftOption = CreateOption("Sakuya.Option", blueKnife, fireRate: 1f - i * 0.02f);
                leftOption.Offset = new Vector2(-20, 0);
                OptionData rightOption = CreateOption("Sakuya.Option", pinkKnife, fireRate: 1f - i * 0.02f);
                rightOption.Offset = new Vector2(20, 0);

                if (i >= 0)
                {
                    knifeSpread.FireDirections.Add(new Vector2(0, -5f));
                    mainWeapon.FireDirections.Add(new Vector2(0, -4f));
                    leftOption.Weapons.First().FireDirections.Add(new Vector2(-0.2f, -3f)); // Slight angle left
                    rightOption.Weapons.First().FireDirections.Add(new Vector2(0.2f, -3f)); // Slight angle right
                }

                if (i >= 3)
                {
                    knifeSpread.FireDirections.Add(new Vector2(-0.2f, -5f));
                    mainWeapon.FireDirections.Add(new Vector2(0.2f, -5f));
                }
                if (i >= 6)
                {
                    knifeSpread.FireDirections.Add(new Vector2(-0.4f, -5f));
                    mainWeapon.FireDirections.Add(new Vector2(0.4f, -5f));
                }

                PowerLevelData focusedPld = new();
                focusedPld.MainWeapons.Add(knifeSpread);
                pd.ShotTypes.First().FocusedShot.PowerLevels[i] = focusedPld;

                PowerLevelData unfocusedPld = new();
                unfocusedPld.MainWeapons.Add(mainWeapon);
                unfocusedPld.Options.Add(leftOption);
                unfocusedPld.Options.Add(rightOption);
                pd.ShotTypes.First().UnfocusedShot.PowerLevels[i] = unfocusedPld;
            }
            return pd;
        }

        public static OptionData CreateOption(string spriteName, BulletData bulletData, float fireRate)
        {
            return new OptionData
            {
                SpriteName = spriteName,
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

        public static EnemyData CreateEnemyData()
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
                            SpriteName = _bulletSprites[_random.Next(_bulletSprites.Count)],
                            Damage = _random.Next(20, 30),
                            BulletType = BulletType.Standard,
                        },
                        FireRate = 5f + (float)_random.NextDouble() * 5f, // Fire rate between 5.0s and 10.0s
                        FireDirections = fireDirections
                    }
                },
                Loot = GenerateRandomLoot()
            };
            return enemy;
        }

        public static List<CollectibleData> GenerateRandomLoot()
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

        public static BossData CreateBossData()
        {
            EnemyData phase1 = CreateBossPhase("DoubleCircle.Red", "circular");
            EnemyData phase2 = CreateBossPhase("DoubleCircle.DarkRed", "zigzag");
            return new BossData { Phases = new List<EnemyData> { phase1, phase2 } };
        }

        public static EnemyData CreateBossPhase(string bulletSprite, string movementPattern)
        {
            // Circular bullet pattern
            int numBullets = _random.Next(8, 13); // Boss fires between 8-12 bullets per shot
            float angleStep = 360f / numBullets; // Evenly distribute bullets

            List<Vector2> fireDirections = new List<Vector2>();
            for (int i = 0; i < numBullets; i++)
            {
                float angle = i * angleStep * (MathF.PI / 180f); // Convert degrees to radians
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
