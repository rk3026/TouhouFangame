namespace BulletHellGame.DataAccess.DataTransferObjects
{
    public class PowerLevelData
    {
        public List<WeaponData> MainWeapons { get; set; } = new();
        public List<SpawnerData> Options { get; set; } = new();
    }
}