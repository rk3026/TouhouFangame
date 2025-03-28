//using System.IO;
//using System.Linq;
//using System.Text.Json;
//using BulletHellGame.DataAccess.DataTransferObjects;

//namespace BulletHellGame.DataAccess.DataLoaders
//{
//    public static class LevelDataLoader
//    {
//        private static readonly string LevelDirectory = "Data/Levels";

//        public static LevelData LoadLevel(string levelName)
//        {
//            var filePath = Path.Combine(LevelDirectory, $"{levelName}.json");

//            if (!File.Exists(filePath))
//            {
//                throw new FileNotFoundException($"Level file not found: {filePath}");
//            }

//            string json = File.ReadAllText(filePath);

//            var dto = JsonSerializer.Deserialize<LevelDataDto>(json);
//            if (dto == null)
//            {
//                throw new Exception("Failed to deserialize level data.");
//            }

//            return ConvertDtoToLevelData(dto);
//        }

//        private static LevelData ConvertDtoToLevelData(LevelDataDto dto)
//        {
//            return new LevelData
//            {
//                LevelId = dto.level_id,
//                LevelName = dto.level_name,
//                Background = dto.background,
//                Music = dto.music,
//                Waves = dto.waves.Select(w => new WaveData
//                {
//                    StartTime = w.spawn_time,
//                    Formation = w.formation,
//                    Enemies = w.enemies.Select(e => EnemyDataLoader.LoadEnemyData(e.id)).ToList(),
//                }).ToList(),
//            };
//        }
//    }

//    public class LevelDataDto
//    {
//        public int level_id { get; set; }
//        public string level_name { get; set; }
//        public string background { get; set; }
//        public string music { get; set; }
//        public List<WaveDto> waves { get; set; }
//    }

//    public class WaveDto
//    {
//        public float spawn_time { get; set; }
//        public string formation { get; set; }
//        public List<EnemyDto> enemies { get; set; }
//    }

//    public class EnemyDto
//    {
//        public string id { get; set; }
//        public PositionDto spawn_position { get; set; }
//        public int health { get; set; }
//    }

//    public class PositionDto
//    {
//        public float x { get; set; }
//        public float y { get; set; }
//    }
//}
