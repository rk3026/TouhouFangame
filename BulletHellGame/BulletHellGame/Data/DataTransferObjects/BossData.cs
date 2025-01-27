namespace BulletHellGame.Data.DataTransferObjects
{
    public class BossData
    {
        public string Name { get; set; }
        public float SpawnTime { get; set; }
        public Vector2 SpawnPosition { get; set; }
        public Vector2 StartPosition { get; set; }
        public int Health { get; set; }
        public List<BossPhaseData> Phases { get; set; }
    }
}
