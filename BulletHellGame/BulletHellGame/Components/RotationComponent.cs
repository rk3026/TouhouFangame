using BulletHellGame.Entities;

namespace BulletHellGame.Components
{
    public class RotationComponent : IComponent
    {
        private readonly Entity _entity;
        public float Rotation { get; private set; } // Rotation angle in radians

        public RotationComponent(Entity entity)
        {
            _entity = entity;
            Rotation = 0f;
        }

        public void SetRotation(float rotation)
        {
            Rotation = rotation;
        }

        public void Update(GameTime gameTime)
        {
            var movement = _entity.GetComponent<MovementComponent>();
            if (movement != null)
            {
                // Rotate based on velocity direction
                Vector2 velocity = movement.Velocity;
                if (velocity.LengthSquared() > 0)
                {
                    Rotation = (float)Math.Atan2(velocity.Y, velocity.X);
                }
            }
        }
    }
}

