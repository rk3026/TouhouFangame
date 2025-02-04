using BulletHellGame.Entities;

namespace BulletHellGame.Components
{
    public class OwnerComponent : IComponent
    {
        public OwnerComponent(Entity owner = null, Vector2 offset = default)
        {
            Owner = owner;
            Offset = offset;
        }

        public Entity Owner { get; set; }
        public Vector2 Offset { get; set; } = Vector2.Zero;
    }
}
