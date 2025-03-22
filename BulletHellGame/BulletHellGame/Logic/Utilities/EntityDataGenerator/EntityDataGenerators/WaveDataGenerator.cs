using BulletHellGame.DataAccess.DataTransferObjects;

namespace BulletHellGame.Logic.Utilities.EntityDataGenerator.EntityDataGenerators
{
    public static class WaveDataGenerator
    {
        public static WaveData CreateWaveData(Rectangle playableArea)
        {
            WaveData wave1 = new WaveData { StartTime = 0f, Duration = 20f }; // Wave starts at 5 seconds
            wave1.Enemies.Add(new EnemySpawnData
            {
                EnemyData = EnemyDataGenerator.CreateEnemyData(),
                SpawnTime = 0f,  // Spawns immediately at wave start
                ExitTime = 10f,   // Leaves after 10 seconds
                SpawnPosition = new Vector2(playableArea.Width / 2 + 50, playableArea.Top + 50)
            });
            wave1.Enemies.Add(new EnemySpawnData
            {
                EnemyData = EnemyDataGenerator.CreateEnemyData(),
                SpawnTime = 0f,  // Spawns 2 seconds after wave start
                ExitTime = 10f,
                SpawnPosition = new Vector2(playableArea.Width / 2 - 50, playableArea.Top + 50)
            });
            return wave1;
        }
    }
}
