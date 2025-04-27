using BulletHellGame.DataAccess.DataTransferObjects;

namespace BulletHellGame.Logic.Utilities.EntityDataGenerator.EntityDataGenerators
{
    public static class LevelDataGenerator
    {
        public static LevelData GenerateLevelData(Rectangle playableArea)
        {
            var levelData = new LevelData();

            levelData.LevelName = "Level 1";
            levelData.LevelId = 1;

            AddGruntWaves(playableArea, levelData);
            levelData.Waves.Add(WaveDataGenerator.CreateSubBossWaveData(playableArea));
            AddGruntWaves(playableArea, levelData);
            levelData.Waves.Add(WaveDataGenerator.CreateBossWaveData(playableArea));

            return levelData;
        }

        private static void AddGruntWaves(Rectangle playableArea, LevelData levelData)
        {
            for (int i = 0; i < 5; i++)
            {
                bool circular = i % 2 == 0;
                WaveData wave = WaveDataGenerator.CreateWaveData(playableArea, circular);
                levelData.Waves.Add(wave);
            }
        }
    }
}
