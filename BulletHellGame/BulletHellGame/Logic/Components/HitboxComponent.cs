using BulletHellGame.Logic.Entities;

namespace BulletHellGame.Logic.Components
{
    public class HitboxComponent : IComponent
    {
        public int Layer { get; set; } // 1 = enemies and enemy bullets, 2 = player and player bullets, 3 = collectibles,
        public Vector2 Hitbox {  get; set; }
        public Vector2 Offset { get; set; }
        public Entity Owner { get; private set; }

        public HitboxComponent(Entity owner, int layer)
        {
            Hitbox = Vector2.Zero;
            Offset = Vector2.Zero;
            Owner = owner;
            Layer = layer;
        }

    }
}
