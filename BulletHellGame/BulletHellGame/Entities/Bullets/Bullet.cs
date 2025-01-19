using BulletHellGame.Components;
using Microsoft.Xna.Framework;

namespace BulletHellGame.Entities.Bullets
{
    public class Bullet : Entity
    {
        private float _rotation; // Rotation angle in radians

        public Bullet(Texture2D texture, Vector2 position, Vector2 velocity) : base(texture, position)
        {
            AddComponent(new MovementComponent(this));
            this.GetComponent<MovementComponent>().Velocity = velocity;
            _rotation = 0f; // Initialize rotation
        }

        public void SetRotation(float rotation)
        {
            _rotation = rotation;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Optionally, you could modify the rotation based on the velocity direction or other factors
            // For example, to rotate the bullet based on its velocity:
            _rotation = (float)Math.Atan2(GetComponent<MovementComponent>().Velocity.Y, GetComponent<MovementComponent>().Velocity.X);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw the bullet with the specified rotation
            spriteBatch.Draw(Texture, Position, null, Color.White, _rotation, new Vector2(Texture.Width / 2, Texture.Height / 2), 1f, SpriteEffects.None, 0f);
        }

        public enum BulletType
        {
            Standard,
            Homing,
        }
    }
}
