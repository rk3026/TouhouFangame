using BulletHellGame.Logic.Entities;

namespace BulletHellGame.Logic.Components
{
    public class PickUpEffectComponent : IComponent
    {
        public Dictionary<CollectibleType, int> Effects { get; set; } = new();
        public PickUpEffectComponent() {
        }
    }
}
