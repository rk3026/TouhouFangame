namespace BulletHellGame.Data
{
    public class WaveData
    {
        public float StartTime { get; set; } // When this wave starts in seconds
        public List<EnemySpawnData> Enemies { get; set; } = new List<EnemySpawnData>();
    }
}
