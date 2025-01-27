using BulletHellGame.Entities;

namespace BulletHellGame.Components
{
    public class HitboxComponent : IComponent
    {
        public Rectangle Hitbox { get; set; } = Rectangle.Empty;
        public Entity Owner { get; private set; }

        public HitboxComponent(Entity owner) {
            Owner = owner;
        }

    }
}
