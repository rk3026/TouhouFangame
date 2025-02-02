using BulletHellGame.Entities;

namespace BulletHellGame.Components
{
    public class OwnerComponent : IComponent
    {
        public OwnerComponent(Entity owner = null) {
            Owner = owner;
        }

        public Entity Owner {  get; set; }
    }
}
