using BulletHellGame.Components;
using BulletHellGame.Data;

namespace BulletHellGame.Entities.Bullets
{
    public class Bullet : Entity
    {
        public BulletType BulletType { get; private set; }
        public Bullet(BulletType type, SpriteData spriteData, Vector2 position) : base(spriteData, position)
        {
            this.BulletType = type;
            // Add components for damage and rotation
            AddComponent(new RotationComponent(this));
            AddComponent(new DamageComponent(25));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
