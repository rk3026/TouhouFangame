using BulletHellGame.Entities;

namespace BulletHellGame.Data.DataTransferObjects
{
    public class EnemyData
    {
        public EnemyType Type { get; set; }
        public SpriteData SpriteData { get; set; }
        public Vector2 SpawnPosition { get; set; }
        public Vector2 StartPosition { get; set; }
        public Vector2 ExitPosition { get; set; }
        public int Health { get; set; }
        public MovementPattern MovementPattern { get; set; }
        public BulletPatternData BulletPattern { get; set; }
    }
}
