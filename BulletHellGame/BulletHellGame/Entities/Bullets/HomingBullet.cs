using BulletHellGame.Components;
using BulletHellGame.Data;
using BulletHellGame.Entities.Characters.Enemies;
using BulletHellGame.Managers;
using System.Linq;

namespace BulletHellGame.Entities.Bullets
{
    public class HomingBullet : Bullet
    {
        private const float HomingRange = 200f; // Detection range for homing
        private const float HomingStrength = 5f; // How fast the bullet adjusts its direction
        private const float MaxHomingSpeed = 300f; // Maximum speed of the bullet

        private Entity _currentTarget;

        public HomingBullet(SpriteData spriteData) : base(spriteData)
        {
            AddComponent(new MovementComponent(this));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (_currentTarget == null || !_currentTarget.IsActive || !IsWithinRange(_currentTarget))
            {
                FindNewTarget();
            }

            if (_currentTarget != null && _currentTarget.IsActive)
            {
                AdjustDirectionTowardsTarget(gameTime);
            }
        }

        private void FindNewTarget()
        {
            List<Enemy> potentialTargets = EntityManager.Instance.GetActiveEntities().OfType<Enemy>().ToList();
            float closestDistanceSquared = HomingRange * HomingRange;
            Entity closestTarget = null;

            foreach (var target in potentialTargets)
            {
                if (target.IsActive)
                {
                    float distanceSquared = Vector2.DistanceSquared(target.Position, Position);
                    if (distanceSquared < closestDistanceSquared)
                    {
                        closestDistanceSquared = distanceSquared;
                        closestTarget = target;
                    }
                }
            }

            _currentTarget = closestTarget;
        }

        private bool IsWithinRange(Entity target)
        {
            return Vector2.DistanceSquared(Position, target.Position) <= HomingRange * HomingRange;
        }

        private void AdjustDirectionTowardsTarget(GameTime gameTime)
        {
            Vector2 directionToTarget = _currentTarget.Position - Position;
            if (directionToTarget.LengthSquared() > 0)
            {
                directionToTarget.Normalize();
            }

            Vector2 desiredVelocity = directionToTarget * MaxHomingSpeed;
            var movementComponent = GetComponent<MovementComponent>();
            if (movementComponent != null)
            {
                // Smoothly interpolate toward the desired velocity
                movementComponent.Velocity = Vector2.Lerp(
                    movementComponent.Velocity,
                    desiredVelocity,
                    HomingStrength * (float)gameTime.ElapsedGameTime.TotalSeconds
                );

                // Clamp to maximum speed
                if (movementComponent.Velocity.LengthSquared() > MaxHomingSpeed * MaxHomingSpeed)
                {
                    movementComponent.Velocity = Vector2.Normalize(movementComponent.Velocity) * MaxHomingSpeed;
                }
            }
        }
    }
}
