namespace BulletHellGame.DataAccess.DataTransferObjects
{
    public interface IEnemyData
    {
        public string SpriteName { get; set; }
        public int Health { get; set; }
        public string MovementPattern { get; set; }
        public ShootingPatternData BulletPattern { get; set; }
        public List<WeaponData> Weapons { get; set; }
        public List<CollectibleData> Loot { get; set; }
        public string Name { get; set; }
    }
}
