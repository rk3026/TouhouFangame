using BulletHellGame.DataAccess.DataTransferObjects;

namespace BulletHellGame.Logic.Utilities.EntityDataGenerator.EntityDataGenerators
{
    public static class WaveDataGenerator
    {
        public static WaveData CreateWaveData(Rectangle playableArea, bool circular = false)
        {
            WaveData wave = new WaveData { StartTime = 0f, Duration = 20f }; // Wave starts immediately and lasts for 20 seconds

            int enemyCount = 15;
            int rows = 3;
            int enemiesPerRow = enemyCount / rows;
            float rowSpacing = playableArea.Height / (rows + 1) /2;
            float colSpacing = playableArea.Width / (enemiesPerRow + 1);

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < enemiesPerRow; col++)
                {
                    wave.Enemies.Add(new EnemySpawnData
                    {
                        EnemyData = circular ? EnemyDataGenerator.CreateCircularMovementEnemyData() : EnemyDataGenerator.CreateEnemyData(),
                        SpawnTime = 0f,  // Spawns immediately at wave start
                        ExitTime = 10f,  // Leaves after 10 seconds
                        SpawnPosition = new Vector2(colSpacing * (col + 1), rowSpacing * (row + 1))
                    });
                }
            }

            return wave;
        }
    }
}
