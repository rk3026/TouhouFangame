using BulletHellGame.Logic.Entities;

namespace BulletHellGame.Logic.Components
{
    public enum HitboxLayer
    {
        EnemiesAndEnemyBullets = 1,
        PlayerAndPlayerBullets = 2,
        Collectibles = 3,
    }
    public class HitboxComponent : IComponent
    {
        public HitboxLayer Layer { get; set; }
        public Vector2 Hitbox {  get; set; }
        public Vector2 Offset { get; set; }
        public Entity Owner { get; private set; }

        public HitboxComponent(Entity owner, HitboxLayer layer)
        {
            Hitbox = Vector2.Zero;
            Offset = Vector2.Zero;
            Owner = owner;
            Layer = layer;
        }

    }
}
