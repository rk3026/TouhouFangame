using BulletHellGame.DataAccess.DataTransferObjects;
using BulletHellGame.Logic.Entities;
using BulletHellGame.Logic.Utilities.EntityDataGenerator;
using BulletHellGame.Logic.Utilities.EntityDataGenerator.EntityDataGenerators;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace BulletHellGame.DataAccess.DataLoaders
{
    /// <summary>
    /// This class will read data from GruntData.json.
    /// It will load all enemy types and return the specific one requested.
    /// </summary>
    public static class EnemyDataLoader
    {
        const string ENEMY_JSON_PATH = "./Data/Levels/enemies.json";

        public static GruntData GetGrunt(string enemyId)
        {
            var enemyJson = JsonUtility.LoadJson(ENEMY_JSON_PATH)[enemyId];

            GruntData gruntData = new GruntData();
            gruntData.Health = enemyJson["health"].Value<int>();
            gruntData.SpriteName = enemyJson["sprite"].Value<string>();
            gruntData.Name = enemyId;
            gruntData.MovementPattern = enemyJson["movementPattern"].Value<string>();

            List<WeaponData> weapons = new List<WeaponData>();
            foreach(var weaponId in enemyJson["weapons"])
            {
                weapons.Add(GetWeapon(weaponId.Value<string>()));
            }
            
            gruntData.Weapons = weapons;
            gruntData.Loot = LootTableLoader.GetLoot(enemyJson["lootTable"].Value<string>());

            return gruntData;
        }

        public static BossData GetBoss(string enemyId)
        {
            var enemyJson = JsonUtility.LoadJson(ENEMY_JSON_PATH)[enemyId];

            BossData bossData = new BossData();

            foreach (var gruntId in enemyJson["phases"])
            {
                bossData.Phases.Add(GetGrunt(gruntId.Value<string>()));
            }

            return bossData;
        }

        public static IEnemyData GetEnemy(string enemyId)
        {
            var enemyJson = JsonUtility.LoadJson(ENEMY_JSON_PATH)[enemyId];

            switch(enemyJson["type"].Value<string>())
            {
                case "grunt":
                    return GetGrunt(enemyId);
                case "boss":
                    return GetBoss(enemyId);
            }

            return null;
        }

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

        private static WeaponData GetWeapon(string weaponId)
        {
            return WeaponDataLoader.GetWeapon(weaponId);
            /*
            switch (weaponId)
            {
                case "grunt":
                    return new WeaponData()
                    {
                        BulletData = new BulletData()
                        {
                            SpriteName = _bulletSprites[_random.Next(_bulletSprites.Count)],
                            Damage = _random.Next(20, 30),
                            BulletType = BulletType.Standard,
                        },
                        FireRate = 3f + (float)_random.NextDouble() * 3f, // Fire rate between 3.0s and 6.0s
                        FireDirections = GenerateBulletDirections()
                    };
                case "boss":
                    string bulletSprite = "Diamond.Blue";
                    int numBullets = 16;
                    float angleStep = 360f / numBullets;
                    var fireDirections = new List<Vector2>();

                    for (int i = 0; i < numBullets; i++)
                    {
                        float angle = (i * angleStep) * (MathF.PI / 180f);
                        fireDirections.Add(new Vector2(MathF.Cos(angle), MathF.Sin(angle)));
                    }


                    return new WeaponData
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
                    };

            }

            return null;*/
        }

        












        private static readonly string EnemiesDirectory = Path.Combine("Data", "Enemies");

        public static GruntData LoadEnemyData(string enemyId)
        {
            var filePath = Path.Combine(EnemiesDirectory, "Enemies.json");
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Enemy data file not found: {filePath}");
            }

            string json = File.ReadAllText(filePath);
            var dto = JsonSerializer.Deserialize<EnemiesDataDto>(json);
            if (dto == null)
            {
                throw new Exception("Failed to deserialize enemy data.");
            }

            var enemyDto = dto.Enemies.FirstOrDefault(e => e.Id == enemyId);
            if (enemyDto == null)
            {
                throw new Exception($"Enemy id '{enemyId}' not found.");
            }

            return ConvertDtoToEnemyData(enemyDto);
        }

        private static GruntData ConvertDtoToEnemyData(EnemyDto dto)
        {
            return new GruntData
            {
                SpriteName = dto.SpriteName,
                Health = dto.Health,
                MovementPattern = dto.MovementPattern,
                Loot = EntityDataGenerator.GenerateRandomLoot(),
            };
        }

        // DTO for all enemies in the JSON file
        public class EnemiesDataDto
        {
            public List<EnemyDto> Enemies { get; set; } = new List<EnemyDto>();
        }

        // DTO for an individual enemy
        public class EnemyDto
        {
            public string Id { get; set; }
            public string SpriteName { get; set; }
            public int Health { get; set; }
            public string MovementPattern { get; set; }
            public List<LootDropDto> LootTable { get; set; } = new List<LootDropDto>();
        }

        // DTO for loot table
        public class LootDropDto
        {
            public CollectibleType CollectibleType { get; set; }
            public float DropRate { get; set; }
        }
    }
}
