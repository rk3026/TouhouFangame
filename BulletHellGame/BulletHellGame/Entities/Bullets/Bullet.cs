using BulletHellGame.Components;

namespace BulletHellGame.Entities.Bullets
{
    public class Bullet : Entity
    {
        public Bullet(Texture2D texture, Vector2 position, Vector2 velocity) : base(texture, position)
        {
            AddComponent(new MovementComponent(this, velocity));

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
