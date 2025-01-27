using BulletHellGame.Entities;

namespace BulletHellGame.Components
{
    public class HitboxComponent : IComponent
    {
        public int Layer { get; set; } // 1 = bullets, 2 = enemies, 3 = collectibles, 4 = player
        public Rectangle Hitbox { get; set; } = Rectangle.Empty;
        public Entity Owner { get; private set; }

        public HitboxComponent(Entity owner, int layer)
        {
            Owner = owner;
            Layer = layer;

        }

    }
}
