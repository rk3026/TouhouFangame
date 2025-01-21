using BulletHellGame.Components;

namespace BulletHellGame.Entities.Bullets
{
    public class Bullet : Entity
    {
        public BulletType BulletType { get; private set; }
        public Bullet(BulletType type, Texture2D texture, Vector2 position, Vector2 velocity) : base(texture, position, velocity)
        {
            this.BulletType = type;
            // Add components for damage and rotation
            AddComponent(new RotationComponent(this));
            AddComponent(new DamageComponent(25));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Components will handle their own logic (e.g., rotation based on velocity)
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Use the rotation from the RotationComponent
            var rotation = GetComponent<RotationComponent>()?.Rotation ?? 0f;

            // Draw the bullet with the specified rotation
            spriteBatch.Draw(Texture, Position, null, Color.White, rotation,
                new Vector2(Texture.Width / 2, Texture.Height / 2), 1f, SpriteEffects.None, 0f);
        }
    }
}
