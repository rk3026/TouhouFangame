using BulletHellGame.DataAccess.DataTransferObjects;
using BulletHellGame.Logic.Entities;

namespace BulletHellGame.Logic.Utilities.EntityDataGenerator.EntityDataGenerators
{
    public static class GruntDataGenerator
    {
        private readonly static List<string> _bulletSprites = new()
        {
            "DoubleCircle.Gray", "DoubleCircle.DarkRed", "DoubleCircle.Red", "DoubleCircle.Purple",
            "DoubleCircle.Pink", "DoubleCircle.DarkBlue", "DoubleCircle.Blue", "DoubleCircle.Teal",
            "DoubleCircle.LightBlue", "DoubleCircle.Green", "DoubleCircle.LightGreen", "DoubleCircle.Chartreuse",
            "DoubleCircle.DarkYellow", "DoubleCircle.Yellow", "DoubleCircle.Orange", "DoubleCircle.White"
        };

        private readonly static List<string> _fairySprites = new()
        {
            "Fairy.Blue", "Fairy.Pink", "Fairy.White"
        };

        private static Random _random = new Random();

        public static GruntData CreateGruntData()
        {
            // Assign a random sprite from the fairy sprites list
            string sprite = _fairySprites[_random.Next(_fairySprites.Count)];

            // Using the common method to create a grunt with specific parameters
            return CreateGrunt(sprite, "zigzag", 100);
        }

        public static GruntData CreateCircularMovementGruntData()
        {
            // Assign a random sprite from the fairy sprites list
            string sprite = _fairySprites[_random.Next(_fairySprites.Count)];

            // Using the common method to create a grunt with specific parameters
            return CreateGrunt(sprite, "circular", 150);
        }

        // Extracted common logic for creating grunt data
        private static GruntData CreateGrunt(string spriteName, string movementPattern, int health)
        {
            // Generate bullets with randomized angles
            var fireDirections = GenerateBulletDirections();

            var enemy = new GruntData
            {
                SpriteName = spriteName,
                MovementPattern = movementPattern,
                Health = health,
                Weapons = new List<WeaponData>
                {
                    new WeaponData()
                    {
                        BulletData = new BulletData()
                        {
                            SpriteName = _bulletSprites[_random.Next(_bulletSprites.Count)],
                            Damage = _random.Next(20, 30),
                            BulletType = BulletType.Standard,
                        },
                        FireRate = 3f + (float)_random.NextDouble() * 3f, // Fire rate between 3.0s and 6.0s
                        FireDirections = fireDirections
                    }
                },
                Loot = CollectibleDataGenerator.GenerateRandomLoot()
            };
            return enemy;
        }

        // Extracted common logic for bullet direction generation
        private static List<Vector2> GenerateBulletDirections()
        {
            int numBullets = _random.Next(1, 3); // Fire 1 to 2 bullets per shot
            List<Vector2> fireDirections = new List<Vector2>();
            for (int i = 0; i < numBullets; i++)
            {
                // Randomize an angle between -15 and +15 degrees (centered around downward)
                float randomAngle = -15f + (float)_random.NextDouble() * 30f; // -15 to +15 degrees
                float angleRad = (randomAngle + 90f) * (MathF.PI / 180f); // Convert to radians (90° is downward)

                // Compute direction vector
                fireDirections.Add(new Vector2(MathF.Cos(angleRad), MathF.Sin(angleRad) + 1f));
            }
            return fireDirections;
        }
    }
}
