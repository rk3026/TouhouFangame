namespace BulletHellGame.DataAccess.DataTransferObjects
{
    public class EnemySpawnData
    {
        public EnemyData EnemyData { get; set; }
        public Vector2 SpawnPosition { get; set; }
        public float SpawnTime { get; set; }
        public float ExitTime { get; set; }
    }
}
