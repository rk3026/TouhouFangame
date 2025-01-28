using BulletHellGame.Entities;

namespace BulletHellGame.Components
{
    public class HitboxComponent : IComponent
    {
        public int Layer { get; set; } // 1 = enemies and enemy bullets, 2 = player and player bullets, 3 = collectibles,
        public Rectangle Hitbox { get; set; } = Rectangle.Empty;
        public Entity Owner { get; private set; }

        public HitboxComponent(Entity owner, int layer)
        {
            Owner = owner;
            Layer = layer;
        }

    }
}
