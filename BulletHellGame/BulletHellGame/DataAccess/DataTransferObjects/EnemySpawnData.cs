namespace BulletHellGame.DataAccess.DataTransferObjects
{
    public class EnemySpawnData
    {
        public IEnemyData EnemyData { get; set; }
        public Vector2 SpawnPosition { get; set; }
        public float SpawnTime { get; set; }
        public float ExitTime { get; set; }
        public string Type { get; set; }
        public int Health { get; set; }
    }
}
