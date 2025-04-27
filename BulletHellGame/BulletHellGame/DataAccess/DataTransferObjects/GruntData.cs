namespace BulletHellGame.DataAccess.DataTransferObjects
{
    public class GruntData : IEnemyData
    {
        public string SpriteName { get; set; }
        public int Health { get; set; }
        public string MovementPattern { get; set; }
        public ShootingPatternData BulletPattern { get; set; }
        public List<WeaponData> Weapons { get; set; }
        public List<CollectibleData> Loot {  get; set; } = new List<CollectibleData>();
        public string Name { get; set; }
    }
}
