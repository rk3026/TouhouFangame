using BulletHellGame.Data.DataTransferObjects;

namespace BulletHellGame.Logic.Components
{
    public class PowerLevelComponent : IComponent
    {
        public Dictionary<int, PowerLevelData> FocusedPowerLevels {  get; set; } = new Dictionary<int, PowerLevelData>();
        public Dictionary<int, PowerLevelData> UnfocusedPowerLevels { get;set; } = new Dictionary<int, PowerLevelData>();
        public PowerLevelComponent() { }
    }
}
