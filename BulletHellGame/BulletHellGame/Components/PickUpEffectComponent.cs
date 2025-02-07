using BulletHellGame.Entities;

namespace BulletHellGame.Components
{
    public class PickUpEffectComponent : IComponent
    {
        public Dictionary<CollectibleType, int> Effects { get; set; } = new();
        public PickUpEffectComponent() {
        }
    }
}
