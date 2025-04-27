using System.IO;
using BulletHellGame.DataAccess.DataTransferObjects;
using BulletHellGame.Logic.Utilities.EntityDataGenerator.EntityDataGenerators;
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
    public static class LevelWaveLoader
    {
        const string WAVE_JSON_PATH = "./Data/Levels/waves.json";

        private static IEnemyData GetEnemyData(string type)
        {
            switch(type)
            {
                case "grunt":
                    return GruntDataGenerator.CreateGruntData();
                case "subBoss":
                    return BossDataGenerator.CreateSubBossData();
            }

            return null;
        }

        public static WaveData GetWave(string waveId, Rectangle playableArea)
        {
            var waveJson = JsonUtility.LoadJson(WAVE_JSON_PATH)[waveId];

            WaveData wave = new WaveData();

            wave.StartTime = waveJson["startTime"].Value<float>();
            wave.Duration = waveJson["duration"].Value<float>();
            wave.WaveType = Enum.Parse<WaveType>(waveJson["waveType"].Value<string>());

            List<EnemySpawnData> enemies = new List<EnemySpawnData>();
            foreach(var enemyJson in waveJson["enemySpawnData"])
            {
                enemies.Add(new EnemySpawnData
                {
                    EnemyData = GetEnemyData(enemyJson["type"].Value<string>()),
                    SpawnTime = enemyJson["spawnTime"].Value<float>(),
                    ExitTime = enemyJson["exitTime"].Value<float>(),
                    SpawnPosition = new Vector2(
                        enemyJson["x"].Value<float>() * playableArea.Width,
                        enemyJson["y"].Value<float>() * playableArea.Height
                    )
                });
            }

            wave.Enemies = enemies;

            //Enum.Parse<WaveType>("Normal");

            return wave;
        }



    }
}