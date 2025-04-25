using BulletHellGame.DataAccess.DataTransferObjects;

namespace BulletHellGame.Logic.Components
{
    public class BulletContainerComponent : IComponent
    {
        // When a bullet is about to despawn, we can use this to spawn numerous different types of bullets
        public Dictionary<int, BulletData> BulletsToSpawn { get; set; } = new();
    }
}
