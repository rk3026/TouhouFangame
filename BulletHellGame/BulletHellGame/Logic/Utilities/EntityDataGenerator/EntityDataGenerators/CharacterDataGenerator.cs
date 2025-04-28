using BulletHellGame.DataAccess.DataTransferObjects;
using BulletHellGame.Logic.Entities;
using System.Linq;

namespace BulletHellGame.Logic.Utilities.EntityDataGenerator.EntityDataGenerators
{
    public static class CharacterDataGenerator
    {
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

                SpawnerData leftOption = CreateOption("Reimu.YinYangOrb", homingBullet, fireRate: 1f - i * 0.02f);
                leftOption.Offset = new Vector2(-20, 0);
                SpawnerData rightOption = CreateOption("Reimu.YinYangOrb", homingBullet, fireRate: 1f - i * 0.02f);
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

            float minFireRate = 0.20f;
            float maxFireRate = 0.05f;
            float stepFireRate = 0.02f;
            for (int i = 0; i <= 8; i++)
            {
                BulletData laser = new BulletData { Damage = 50 + i * 30, BulletType = BulletType.Standard, SpriteName = "Marisa.LightBeam", RotationSpeed = 0f };
                BulletData rocket = new BulletData { Damage = 80 + i * 25, BulletType = BulletType.Standard, SpriteName = "Marisa.Rocket", RotationSpeed = 0f, Acceleration = new Vector2(0, -6) };

                WeaponData laserWeapon = new WeaponData { BulletData = laser, FireRate = Math.Max(minFireRate, maxFireRate - i * stepFireRate), FireDirections = new List<Vector2> { new Vector2(0, -5f) }, TimeSinceLastShot = 0 };
                WeaponData rocketWeapon = new WeaponData { BulletData = rocket, FireRate = Math.Max(minFireRate, maxFireRate - i * stepFireRate), FireDirections = new List<Vector2>(), TimeSinceLastShot = 0 };

                SpawnerData leftOption = CreateOption("Marisa.StarOption", rocket, fireRate: 1f - i * 0.02f);
                leftOption.Offset = new Vector2(-20, 0);
                SpawnerData rightOption = CreateOption("Marisa.StarOption", rocket, fireRate: 1f - i * 0.02f);
                rightOption.Offset = new Vector2(20, 0);

                if (i >= 0)
                {
                    laserWeapon.FireDirections.Add(new Vector2(0, -5f));
                    rocketWeapon.FireDirections.Add(new Vector2(0, -0.001f));
                    leftOption.Weapons.First().FireDirections.Add(new Vector2(-0.2f, -3f)); // Slight angle left
                    rightOption.Weapons.First().FireDirections.Add(new Vector2(0.2f, -3f)); // Slight angle right
                }

                if (i >= 3)
                {
                    laserWeapon.FireDirections.Add(new Vector2(-0.2f, -5f));
                    rocketWeapon.FireDirections.Add(new Vector2(0.2f, -0.001f));
                }
                if (i >= 6)
                {
                    laserWeapon.FireDirections.Add(new Vector2(-0.4f, -5f));
                    rocketWeapon.FireDirections.Add(new Vector2(0.4f, -0.001f));
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

            float minFireRate = 0.20f;
            float maxFireRate = 0.05f;
            float stepFireRate = 0.02f;
            for (int i = 0; i <= 8; i++)
            {
                BulletData blueKnife = new BulletData { Damage = 40 + i * 20, BulletType = BulletType.Standard, SpriteName = "Sakuya.KnifeBlue", RotationSpeed = 0f };
                BulletData pinkKnife = new BulletData { Damage = 120 + i * 30, BulletType = BulletType.Standard, SpriteName = "Sakuya.KnifePink", RotationSpeed = 0f };

                WeaponData knifeSpread = new WeaponData { BulletData = blueKnife, FireRate = Math.Max(minFireRate, maxFireRate - i * stepFireRate), FireDirections = new List<Vector2>(), TimeSinceLastShot = 0 };
                for (int j = -1; j <= 1; j++)
                {
                    knifeSpread.FireDirections.Add(new Vector2(j * 0.3f, -5f));
                }

                WeaponData mainWeapon = new WeaponData { BulletData = pinkKnife, FireRate = Math.Max(minFireRate, maxFireRate - i * stepFireRate), FireDirections = new List<Vector2> { new Vector2(0, -4f) }, TimeSinceLastShot = 0 };

                SpawnerData leftOption = CreateOption("Sakuya.Option", blueKnife, fireRate: 1f - i * 0.02f);
                leftOption.Offset = new Vector2(-20, 0);
                SpawnerData rightOption = CreateOption("Sakuya.Option", pinkKnife, fireRate: 1f - i * 0.02f);
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

        public static CharacterData CreateEpicTestData()
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
                ShotTypes = new List<ShotTypeData>()
                {
                    new ShotTypeData
                    {
                        Name = "Test",
                        UnfocusedShot = new ShotData { PowerLevels = new Dictionary<int, PowerLevelData>() },
                        FocusedShot = new ShotData { PowerLevels = new Dictionary<int, PowerLevelData>() }
                    }
                }
            };

            // Bullet data for large bullets
            BulletData largeBullet = new BulletData
            {
                Damage = 100, // Set the damage as desired
                BulletType = BulletType.Standard,
                SpriteName = "LargeBullet.Red", // You can change to Blue, Green, or Yellow as needed
                RotationSpeed = 0f
            };

            // Weapon data for straight firing
            WeaponData mainWeapon = new WeaponData
            {
                BulletData = largeBullet,
                FireRate = 0.2f, // Set the fire rate as desired
                FireDirections = new List<Vector2> { new Vector2(0, -5f) }, // Straight ahead
                TimeSinceLastShot = 0
            };

            // Iterate over all possible power levels (0 to 8) and set the same weapon for each level
            for (int i = 0; i <= 8; i++)
            {
                PowerLevelData focusedPld = new PowerLevelData();
                focusedPld.MainWeapons.Add(mainWeapon);
                pd.ShotTypes.First().FocusedShot.PowerLevels[i] = focusedPld;

                PowerLevelData unfocusedPld = new PowerLevelData();
                unfocusedPld.MainWeapons.Add(mainWeapon);
                pd.ShotTypes.First().UnfocusedShot.PowerLevels[i] = unfocusedPld;
            }

            return pd;
        }



        public static SpawnerData CreateOption(string spriteName, BulletData bulletData, float fireRate)
        {
            return new SpawnerData
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
    }
}
