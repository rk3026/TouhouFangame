using BulletHellGame.Entities;

namespace BulletHellGame.Components
{
    public class MovementComponent : IComponent
    {
        private Sprite _owner;
        public Vector2 Velocity { get; set; }

        public MovementComponent(Sprite owner)
        {
            _owner = owner;
            Velocity = Vector2.Zero;
        }

        public void Update(GameTime gameTime)
        {
            // Update the position based on the velocity and elapsed time
            _owner.Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
