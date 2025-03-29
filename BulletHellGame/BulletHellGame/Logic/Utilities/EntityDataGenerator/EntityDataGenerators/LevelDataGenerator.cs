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
                bool circular = i % 2 == 0;
                WaveData wave = WaveDataGenerator.CreateWaveData(playableArea, circular);
                levelData.Waves.Add(wave);
            }

            // Add 1 boss
            BossData subboss = BossDataGenerator.CreateSubBossData();
            BossData boss = BossDataGenerator.CreateBossData();
            levelData.SubBoss = subboss;
            levelData.Boss = boss;

            return levelData;
        }
    }
}
