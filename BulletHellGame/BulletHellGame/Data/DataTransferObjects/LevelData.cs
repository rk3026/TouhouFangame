namespace BulletHellGame.Data.DataTransferObjects
{
    public class LevelData
    {
        public int LevelId { get; set; }
        public string LevelName { get; set; }
        public string Background { get; set; }
        public string Music { get; set; }
        public List<EnemyData> Enemies { get; set; }
        public BossData SubBoss { get; set; }
        public BossData Boss { get; set; }
    }
}
