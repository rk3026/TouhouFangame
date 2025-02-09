using BulletHellGame.Data.DataTransferObjects;

namespace BulletHellGame.Components
{
    public class ShootingLevelComponent : IComponent
    {
        public Dictionary<int, WeaponData> PowerLevels { get;set; } = new Dictionary<int, WeaponData>();
        public ShootingLevelComponent() { }
    }
}
