using BulletHellGame.DataAccess.DataTransferObjects;
using BulletHellGame.Logic.Entities;

namespace BulletHellGame.Logic.Utilities.EntityDataGenerator.EntityDataGenerators
{
    public class BossDataGenerator
    {
        private static Random _random = new Random();
        private static float spiralAngle = 0f;

        public static BossData CreateBossData()
        {
            GruntData phase1 = CreateLettyRadialPhase();
            GruntData phase2 = CreateLettyLaserWallPhase();
            GruntData phase3 = CreateLettyChaosRainPhase();

            return new BossData
            {
                Phases = new List<GruntData> { phase1, phase2, phase3 }
            };
        }

        public static BossData CreateSubBossData()
        {
            GruntData phase1 = CreateCirnoSpiralPhase();
            //GruntData phase1 = CreateCirnoLaserPhase();
            GruntData phase2 = CreateCirnoAimedBurstPhase();

            return new BossData
            {
                Phases = new List<GruntData> { phase1, phase2 }
            };
        }

        private static GruntData CreateLettyRadialPhase()
        {
            string bulletSprite = "DoubleCircle.Green";
            int numBullets = 24;
            float angleStep = 360f / numBullets;
            var fireDirections = new List<Vector2>();

            for (int i = 0; i < numBullets; i++)
            {
                float angle = i * angleStep * (MathF.PI / 180f);
                fireDirections.Add(new Vector2(MathF.Cos(angle), MathF.Sin(angle)));
            }

            return new GruntData
            {
                Name = "Letty Whiterock - Phase 1",
                SpriteName = "LettyWhiterock",
                MovementPattern = "none",
                Health = 10000,

                Weapons = new List<WeaponData>
                {
                    new WeaponData
                    {
                        BulletData = new BulletData
                        {
                            SpriteName = bulletSprite,
                            Damage = 20,
                            BulletType = BulletType.Standard
                        },
                        FireDirections = fireDirections,
                        FireRate = 0.8f
                    }
                }
            };
        }

        private static GruntData CreateLettyLaserWallPhase()
        {
            string bulletSprite = "Diamond.Teal";
            var fireDirections = new List<Vector2>
            {
                new Vector2(-1, 0),
                new Vector2(1, 0)
            };

            return new GruntData
            {
                Name = "Letty Whiterock - Phase 2",
                SpriteName = "LettyWhiterock",
                MovementPattern = "circular",
                Health = 15000,

                Weapons = new List<WeaponData>
                {
                    new WeaponData
                    {
                        BulletData = new BulletData
                        {
                            SpriteName = bulletSprite,
                            Damage = 35,
                            BulletType = BulletType.Homing,
                            RotationSpeed = 0
                        },
                        FireDirections = fireDirections,
                        FireRate = 1.2f
                    }
                }
            };
        }

        private static GruntData CreateLettyChaosRainPhase()
        {
            string bulletSprite = "DoubleCircle.Pink";
            var fireDirections = new List<Vector2>();
            for (int i = 0; i < 25; i++)
            {
                float angle = _random.Next(0, 360) * (MathF.PI / 180f);
                fireDirections.Add(new Vector2(MathF.Cos(angle), MathF.Sin(angle)));
            }

            return new GruntData
            {
                Name = "Letty Whiterock - Phase 3",
                SpriteName = "LettyWhiterock",
                MovementPattern = "zigzag",
                Health = 30000,

                Weapons = new List<WeaponData>
                {
                    new WeaponData
                    {
                        BulletData = new BulletData
                        {
                            SpriteName = bulletSprite,
                            Damage = 15,
                            BulletType = BulletType.Standard,
                            RotationSpeed = 0f
                        },
                        FireDirections = fireDirections,
                        FireRate = 0.5f
                    }
                }
            };
        }

        private static GruntData CreateCirnoLaserPhase()
        {
            string bulletSprite = "Lazer.Red";
            int numBullets = 16;
            float angleStep = 360f / numBullets;
            var fireDirections = new List<Vector2>();

            for (int i = 0; i < numBullets; i++)
            {
                float angle = (spiralAngle + i * angleStep) * (MathF.PI / 180f);
                fireDirections.Add(new Vector2(MathF.Cos(angle), MathF.Sin(angle)));
            }

            spiralAngle += 20f;
            if (spiralAngle >= 360f) spiralAngle -= 360f;

            return new GruntData
            {
                Name = "Cirno - Phase 1",
                SpriteName = "Cirno",
                MovementPattern = "circle",
                Health = 7000,

                Weapons = new List<WeaponData>
                {
                    new WeaponData
                    {
                        BulletData = new BulletData
                        {
                            SpriteName = bulletSprite,
                            Damage = 25,
                            BulletType = BulletType.Laser,
                            RotationSpeed = 0f
                        },
                        FireDirections = fireDirections,
                        FireRate = 0.5f
                    }
                }
            };
        }

        private static GruntData CreateCirnoSpiralPhase()
        {
            string bulletSprite = "Diamond.Blue";
            int numBullets = 16;
            float angleStep = 360f / numBullets;
            var fireDirections = new List<Vector2>();

            for (int i = 0; i < numBullets; i++)
            {
                float angle = (spiralAngle + i * angleStep) * (MathF.PI / 180f);
                fireDirections.Add(new Vector2(MathF.Cos(angle), MathF.Sin(angle)));
            }

            spiralAngle += 20f;
            if (spiralAngle >= 360f) spiralAngle -= 360f;

            return new GruntData
            {
                Name = "Cirno - Phase 1",
                SpriteName = "Cirno",
                MovementPattern = "circle",
                Health = 7000,

                Weapons = new List<WeaponData>
                {
                    new WeaponData
                    {
                        BulletData = new BulletData
                        {
                            SpriteName = bulletSprite,
                            Damage = 25,
                            BulletType = BulletType.Standard,
                            RotationSpeed = 1.0f
                        },
                        FireDirections = fireDirections,
                        FireRate = 0.5f
                    }
                }
            };
        }

        private static GruntData CreateCirnoAimedBurstPhase()
        {
            string bulletSprite = "SingleCircle.Blue";
            var fireDirections = new List<Vector2>
            {
                new Vector2(0, 1),
                new Vector2(-0.3f, 1),
                new Vector2(0.3f, 1)
            };

            return new GruntData
            {
                Name = "Cirno - Phase 2",
                SpriteName = "Cirno",
                MovementPattern = "zigzag",
                Health = 30000,

                Weapons = new List<WeaponData>
                {
                    new WeaponData
                    {
                        BulletData = new BulletData
                        {
                            SpriteName = bulletSprite,
                            Damage = 30,
                            BulletType = BulletType.Standard
                        },
                        FireDirections = fireDirections,
                        FireRate = 0.3f
                    }
                }
            };
        }
    }
}
