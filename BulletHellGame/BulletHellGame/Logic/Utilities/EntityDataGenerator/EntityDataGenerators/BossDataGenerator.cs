using BulletHellGame.DataAccess.DataTransferObjects;
using BulletHellGame.Logic.Entities;
using System;

namespace BulletHellGame.Logic.Utilities.EntityDataGenerator.EntityDataGenerators
{
    public class BossDataGenerator
    {
        private static Random _random = new Random();

        public static BossData CreateBossData()
        {
            EnemyData phase1 = CreateBossPhase("Diamond.Blue");
            EnemyData phase2 = CreateBossPhase("SingleCircle.Blue", "zigzag", 0.5f);
            return new BossData { Phases = new List<EnemyData> { phase1, phase2 } };
        }

        private static float spiralAngle = 0f; // Tracks the spiral's rotation

        public static EnemyData CreateBossPhase(string bulletSprite, string movementPattern = "none", float bulletRotationSpeed = 0f)
        {
            // Spiral bullet pattern
            int numBullets = _random.Next(8, 13); // Boss fires between 8-12 bullets per shot
            float angleStep = 360f / numBullets; // Evenly distribute bullets

            List<Vector2> fireDirections = new List<Vector2>();
            for (int i = 0; i < numBullets; i++)
            {
                float angle = (spiralAngle + i * angleStep) * (MathF.PI / 180f); // Apply spiral offset and convert degrees to radians
                fireDirections.Add(new Vector2(MathF.Cos(angle), MathF.Sin(angle)));
            }

            // Increment the spiral angle to create the spinning effect
            spiralAngle += 10f; // Adjust this value to control spiral tightness
            if (spiralAngle >= 360f) spiralAngle -= 360f; // Wrap around

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
                            BulletType = BulletType.Standard,
                            RotationSpeed = bulletRotationSpeed,
                        },
                        FireDirections = fireDirections, // Assign generated directions
                        FireRate = 0.3f + (float)_random.NextDouble() * (1.0f - 0.3f) // Random fire rate between 0.3s and 1.0s
                    }
                }
            };
        }

    }
}
