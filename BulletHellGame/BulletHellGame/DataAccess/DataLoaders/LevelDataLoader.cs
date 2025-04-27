using System.IO;
using System.Linq;
using System.Text.Json;
using BulletHellGame.DataAccess.DataTransferObjects;
using BulletHellGame.Logic.Utilities.EntityDataGenerator;
using BulletHellGame.Logic.Utilities.EntityDataGenerator.EntityDataGenerators;
using Newtonsoft.Json.Linq;

namespace BulletHellGame.DataAccess.DataLoaders
{
    public static class LevelDataLoader
    {
        const string LEVEL_JSON_PATH = "./Data/Levels/levels.json";

        public static Dictionary<int, LevelData> LoadLevelData(Rectangle playableArea)
        {
            var levelsJson = JsonUtility.LoadJson(LEVEL_JSON_PATH);
            var levels = new Dictionary<int, LevelData>();

            foreach (var levelData in levelsJson)
            {
                levels.Add(int.Parse(levelData.Key), GetLevel(levelData.Key, playableArea));
            }
            //JsonUtility.LoadJson(LEVEL_JSON_PATH);

            return levels;
        }

        public static List<int> GetLevelIds()
        {
            var levelsJson = JsonUtility.LoadJson(LEVEL_JSON_PATH);
            
            List<int> levelsIds = new List<int>();
            foreach (var level in levelsJson)
            {
                levelsIds.Add(int.Parse(level.Key));
            }

            return levelsIds;
        }

        private static WaveData GetWave(string waveId, Rectangle playableArea)
        {
            return LevelWaveLoader.GetWave(waveId, playableArea);
        }

        public static LevelData GetLevel(string levelId, Rectangle playableArea)
        {
            var levelJson = JsonUtility.LoadJson(LEVEL_JSON_PATH)[levelId];

            

            List<WaveData> waves = new List<WaveData>();
            foreach (string waveId in levelJson["waveIds"])
            {
                waves.Add(GetWave(waveId, playableArea));
            }

            LevelData level = new LevelData();
            level.Background = levelJson["background"].Value<string>();
            level.Music = levelJson["music"].Value<string>();
            level.LevelName = levelJson["levelName"].Value<string>();
            level.LevelId = int.Parse(levelId);
            level.Waves = waves;

            return level;
        }

        public static LevelData LoadLevel(string levelName)
        {
            return null;
        }

        private static readonly string LevelDirectory = "Data/Levels";

       

        

        private static LevelData ConvertDtoToLevelData(LevelDataDto dto)
        {
            return new LevelData
            {
                LevelId = dto.level_id,
                LevelName = dto.level_name,
                Background = dto.background,
                Music = dto.music,
                Waves = dto.waves.Select(w => new WaveData
                {
                    StartTime = w.spawn_time,
                    Formation = w.formation,
                    //Enemies = w.enemies.Select(e => EnemyDataLoader.LoadEnemyData(e.id)).ToList(),
                }).ToList(),
            };
        }
    }

    public class LevelDataDto
    {
        public int level_id { get; set; }
        public string level_name { get; set; }
        public string background { get; set; }
        public string music { get; set; }
        public List<WaveDto> waves { get; set; }
    }

    public class WaveDto
    {
        public float spawn_time { get; set; }
        public string formation { get; set; }
        public List<EnemyDto> enemies { get; set; }
    }

    public class EnemyDto
    {
        public string id { get; set; }
        public PositionDto spawn_position { get; set; }
        public int health { get; set; }
    }

    public class PositionDto
    {
        public float x { get; set; }
        public float y { get; set; }
    }
}
