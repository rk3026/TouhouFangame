namespace BulletHellGame.Data.DataTransferObjects
{
    public class ShotData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Dictionary<int, WeaponData> PowerLevels { get; set; }
        public ShotData() { }
    }
}
