using BulletHellGame.DataAccess.DataTransferObjects;

namespace BulletHellGame.Logic.Utilities.EntityDataGenerator.EntityDataGenerators
{
    public static class WaveDataGenerator
    {
        public static WaveData CreateWaveData(Rectangle playableArea, bool circular = false)
        {
            WaveData waveData = new WaveData { StartTime = 0f, Duration = 20f, WaveType = WaveType.Normal };

            int enemyCount = 15;
            int rows = 3;
            int enemiesPerRow = enemyCount / rows;
            float rowSpacing = playableArea.Height / (rows + 1) /2;
            float colSpacing = playableArea.Width / (enemiesPerRow + 1);

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < enemiesPerRow; col++)
                {
                    waveData.Enemies.Add(new EnemySpawnData
                    {
                        EnemyData = circular ? GruntDataGenerator.CreateCircularMovementGruntData() : GruntDataGenerator.CreateGruntData(),
                        SpawnTime = 2f,
                        ExitTime = 12f,
                        SpawnPosition = new Vector2(colSpacing * (col + 1), rowSpacing * (row + 1))
                    });
                }
            }

            return waveData;
        }

        public static WaveData CreateSubBossWaveData(Rectangle playableArea)
        {
            WaveData waveData = new WaveData { StartTime = 0f, Duration = 30f, WaveType = WaveType.SubBoss };
            waveData.Enemies.Add(new EnemySpawnData
            {
                EnemyData = BossDataGenerator.CreateSubBossData(),
                SpawnTime = 5f,
                ExitTime = 90f,
                SpawnPosition = new Vector2(playableArea.X + playableArea.Width / 2, playableArea.Y + 30)
            });
            return waveData;
        }

        public static WaveData CreateBossWaveData(Rectangle playableArea)
        {
            WaveData waveData = new WaveData { StartTime = 0f, Duration = 120f, WaveType = WaveType.Boss };
            waveData.Enemies.Add(new EnemySpawnData
            {
                EnemyData = BossDataGenerator.CreateBossData(),
                SpawnTime = 5f,
                ExitTime = 120f,
                SpawnPosition = new Vector2(playableArea.X + playableArea.Width / 2, playableArea.Y + 30)
            });
            return waveData;
        }
    }
}
