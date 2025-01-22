using BulletHellGame.Entities;

namespace BulletHellGame.Components
{
    public class MovementComponent : IComponent
    {
        private Entity _owner;
        public Vector2 Velocity { get; set; }

        public MovementComponent(Entity owner)
        {
            _owner = owner;
            Velocity = Vector2.Zero;
        }

        public void Update(GameTime gameTime)
        {
            // Update the position based on the velocity and elapsed time
            _owner.Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Velocity.X < 0)
            {
                _owner.GetComponent<SpriteComponent>().SpriteEffect = SpriteEffects.FlipHorizontally;
            }
            else
            {
                _owner.GetComponent<SpriteComponent>().SpriteEffect = SpriteEffects.None;
            }
        }
    }
}
