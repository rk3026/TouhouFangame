namespace BulletHellGame.DataAccess.DataTransferObjects
{
    public class WaveData
    {
        public float StartTime { get; set; } // When this wave starts in seconds
        public float Duration { get; set; } // How long this wave lasts in seconds
        public List<EnemySpawnData> Enemies { get; set; } = new List<EnemySpawnData>();
        public float SpawnTime { get; set; }
        public string Formation { get; set; }
    }
}
