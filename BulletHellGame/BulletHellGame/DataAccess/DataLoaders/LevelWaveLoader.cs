using System.IO;
using BulletHellGame.DataAccess.DataTransferObjects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BulletHellGame.DataAccess.DataLoaders
{
    /**
     * Implementing level1.json with 5 waves. Reference DataTransferObjects (LevelData, WaveData)
     * Wave 1, 2: Standard Grunt fights
     * Wave 3: Mid-level boss
     * Wave 4: Grunt fights with additional complexity
     * Wave 5: Final boss which can spawn grunts in its path
     */
    public class LevelWaveLoader
    {
        private static LevelWaveLoader _instance;
        public static LevelWaveLoader Instance => _instance ??= new LevelWaveLoader();

        private Dictionary<string, LevelData> _levels;

        private LevelWaveLoader()
        {
            _levels = new Dictionary<string, LevelData>();
            LoadLevels();
        }

        private void LoadLevels()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/Levels/level1.json");
            string json = File.ReadAllText(filePath);

            // Deserialize the levels from the JSON file
            var levelJson = JObject.Parse(json);

            LevelData levelData = new LevelData
            {
                Waves = new List<WaveData>()
            };

            foreach (var wave in levelJson["waves"])
            {
                WaveData waveData = new WaveData
                {
                    StartTime = wave["spawn_time"].Value<float>(),
                    Formation = wave["formation"].Value<string>(),
                    Enemies = new List<EnemySpawnData>()
                };

                foreach (var enemy in wave["enemies"])
                {
                    waveData.Enemies.Add(new EnemySpawnData
                    {
                        Type = enemy["id"].Value<string>(), // TODO: Switch to EnemyDataLoader.cs which will reference EnemyData.json in Data Directory
                        SpawnPosition = new Vector2(enemy["spawn_position"]["x"].Value<float>(), enemy["spawn_position"]["y"].Value<float>()),
                        Health = enemy["health"].Value<int>()
                    });
                }

                levelData.Waves.Add(waveData);
            }

            // Cache the level data for reuse
            _levels[levelJson["level_name"].Value<string>()] = levelData;
        }

        // Retrieve a level by name
        public LevelData GetLevel(string levelName)
        {
            if (_levels.ContainsKey(levelName))
            {
                return _levels[levelName];
            }
            else
            {
                Console.WriteLine($"Level '{levelName}' not found");
                return null;
            }
        }
    }
}