using BulletHellGame.DataAccess.DataTransferObjects;

namespace BulletHellGame.Logic.Components
{
    public class LootComponent : IComponent
    {
        public List<CollectibleData> Loot {  get; set; } = new List<CollectibleData>();
        public LootComponent() { }
    }
}
