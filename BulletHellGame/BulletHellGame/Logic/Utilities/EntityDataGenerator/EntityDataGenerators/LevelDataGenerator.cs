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

            // Add 3 waves
            for (int i = 0; i < 5; i++)
            {
                WaveData wave = WaveDataGenerator.CreateWaveData(playableArea);
                levelData.Waves.Add(wave);
            }

            // Add 1 boss
            BossData boss = BossDataGenerator.CreateBossData();
            levelData.Bosses.Add(boss);

            return levelData;
        }
    }
}
