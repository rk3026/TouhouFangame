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
    }
}