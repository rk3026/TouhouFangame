using BulletHellGame.Data.DataTransferObjects;

namespace BulletHellGame.Components
{
    public class PowerLevelComponent : IComponent
    {
        public Dictionary<int, PowerLevelData> PowerLevels { get;set; } = new Dictionary<int, PowerLevelData>();
        public PowerLevelComponent() { }
    }
}
