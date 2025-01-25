using BulletHellGame.Entities.Characters.Enemies;

namespace BulletHellGame.Data.DataTransferObjects
{
    public class EnemyData
    {
        public EnemyType Type { get; set; }
        public float SpawnTime { get; set; }
        public Vector2 SpawnPosition { get; set; }
        public Vector2 StartPosition { get; set; }
        public float ExitTime { get; set; }
        public Vector2 ExitPosition { get; set; }
        public string MovementPattern { get; set; }
        public int Health { get; set; }
        public BulletPatternData BulletPattern { get; set; }
    }
}
