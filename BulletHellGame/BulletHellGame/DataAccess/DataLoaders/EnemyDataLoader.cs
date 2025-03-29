using BulletHellGame.DataAccess.DataTransferObjects;
using BulletHellGame.Logic.Entities;
using BulletHellGame.Logic.Utilities.EntityDataGenerator;
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
