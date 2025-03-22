using System.IO;
using BulletHellGame.DataAccess.DataTransferObjects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BulletHellGame.DataAccess.DataLoaders;

/**
 * Implementing leve1.json with 5 waves. Reference DataTransferObjects (LevelData, WaveData)
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
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/Levels/Levels.json");
        string json = File.ReadAllText(filePath);

        // Deserialize the levels from the JSON file
        var levels = JsonConvert.DeserializeObject<Dictionary<string, List<JObject>>>(json);

        // Manually create LevelData and cache it
        foreach (var level in levels)
        {
            LevelData levelData = new LevelData
            {
                Waves = new List<WaveData>()
            };

            foreach (var wave in level.Value)
            {
                WaveData waveData = new WaveData
                {
                    Enemies = new List<EnemySpawnData>()
                };

                foreach (var enemySpawn in wave["enemies"])
                {
                    float spawnTime = enemySpawn["spawnTime"].Value<float>();
                    var enemyData = enemySpawn["enemyData"] as JObject;
                    var spawnPosition = enemySpawn["spawnPosition"] as JObject;

                    waveData.Enemies.Add(new EnemySpawnData
                    {
                        SpawnTime = spawnTime,
                        EnemyData = new EnemyData
                        {
                            Name = enemyData["name"].Value<string>(),
                            Health = enemyData["health"].Value<int>(),
                            MovementPattern = enemyData["movementPattern"].Value<string>()
                        },
                        SpawnPosition = new Vector2(spawnPosition["x"].Value<float>(), spawnPosition["y"].Value<float>())
                    });
                }

                levelData.Waves.Add(waveData);
            }

            // Cache the level data for reuse
            _levels[level.Key] = levelData;
        }
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
            return null;
        }
    }
}