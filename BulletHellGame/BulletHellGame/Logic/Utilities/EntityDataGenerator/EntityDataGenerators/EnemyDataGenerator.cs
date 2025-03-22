using BulletHellGame.DataAccess.DataTransferObjects;
using BulletHellGame.Logic.Entities;

namespace BulletHellGame.Logic.Utilities.EntityDataGenerator.EntityDataGenerators
{
    public static class EnemyDataGenerator
    {
        private readonly static List<string> _bulletSprites = new() {
            "DoubleCircle.Gray", "DoubleCircle.DarkRed", "DoubleCircle.Red", "DoubleCircle.Purple",
            "DoubleCircle.Pink", "DoubleCircle.DarkBlue", "DoubleCircle.Blue", "DoubleCircle.Teal",
            "DoubleCircle.LightBlue", "DoubleCircle.Green", "DoubleCircle.LightGreen", "DoubleCircle.Chartreuse",
            "DoubleCircle.DarkYellow", "DoubleCircle.Yellow", "DoubleCircle.Orange", "DoubleCircle.White"
        };
        private static Random _random = new Random();

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
                Loot = CollectibleDataGenerator.GenerateRandomLoot()
            };
            return enemy;
        }
    }
}
