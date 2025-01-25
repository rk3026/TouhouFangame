using BulletHellGame.Components;
using BulletHellGame.Data.DataTransferObjects;

namespace BulletHellGame.Entities.Bullets
{
    public class Bullet : Entity
    {
        public Bullet(SpriteData spriteData) : base(spriteData)
        {
            // Add components for damage and rotation
            AddComponent(new DamageComponent(25));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
