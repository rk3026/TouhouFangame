using BulletHellGame.Data.DataTransferObjects;

namespace BulletHellGame.Components
{
    public class LootComponent : IComponent
    {
        public List<CollectibleData> Loot {  get; set; } = new List<CollectibleData>();
        public LootComponent() { }
    }
}
