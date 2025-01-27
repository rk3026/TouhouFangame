using BulletHellGame.Components;
using BulletHellGame.Entities;
using BulletHellGame.Managers;
using System.Linq;

namespace BulletHellGame.Systems
{
    public class HomingSystem : ISystem
    {
        public void Update(EntityManager entityManager, GameTime gameTime)
        {
            List<Entity> entities = entityManager.GetActiveEntities().ToList();
            foreach (Entity entity in entities)
            {
                if (!entity.HasComponent<HomingComponent>()) continue;

                HomingComponent homingComponent = entity.GetComponent<HomingComponent>();
                MovementComponent movementComponent = entity.GetComponent<MovementComponent>();

                if (movementComponent == null) continue;

                Entity currentTarget = homingComponent.CurrentTarget;

                // Check if current target is valid or find a new one
                if (currentTarget == null || !currentTarget.IsActive || !IsWithinRange(entity.GetComponent<MovementComponent>().Position, currentTarget.GetComponent<MovementComponent>().Position, homingComponent.HomingRange))
                {
                    currentTarget = FindNewTarget(entityManager, entity.GetComponent<MovementComponent>().Position, homingComponent.HomingRange);
                    homingComponent.CurrentTarget = currentTarget;
                }

                // Adjust the bullet's direction if a valid target is found
                if (currentTarget != null && currentTarget.IsActive)
                {
                    AdjustDirectionTowardsTarget(entity, currentTarget, homingComponent, movementComponent, gameTime);
                }

            }
        }

        private Entity FindNewTarget(EntityManager entityManager, Vector2 bulletPosition, float homingRange)
        {
            List<Entity> potentialTargets = entityManager.GetActiveEntities()
                .Where(entity => entity.HasComponent<HealthComponent>() && !entity.HasComponent<PlayerInputComponent>())
                .ToList();

            float closestDistanceSquared = homingRange * homingRange;
            Entity closestTarget = null;

            foreach (var target in potentialTargets)
            {
                if (target.IsActive)
                {
                    float distanceSquared = Vector2.DistanceSquared(target.GetComponent<MovementComponent>().Position, bulletPosition);
                    if (distanceSquared < closestDistanceSquared)
                    {
                        closestDistanceSquared = distanceSquared;
                        closestTarget = target;
                    }
                }
            }

            return closestTarget;
        }

        private bool IsWithinRange(Vector2 position1, Vector2 position2, float range)
        {
            return Vector2.DistanceSquared(position1, position2) <= range * range;
        }

        private void AdjustDirectionTowardsTarget(Entity bullet, Entity target, HomingComponent homingComponent, MovementComponent movementComponent, GameTime gameTime)
        {
            Vector2 directionToTarget = target.GetComponent<MovementComponent>().Position - bullet.GetComponent<MovementComponent>().Position;
            if (directionToTarget.LengthSquared() > 0)
            {
                directionToTarget.Normalize();
            }

            Vector2 desiredVelocity = directionToTarget * homingComponent.MaxHomingSpeed;

            // Smoothly interpolate toward the desired velocity
            movementComponent.Velocity = Vector2.Lerp(
                movementComponent.Velocity,
                desiredVelocity,
                homingComponent.HomingStrength * (float)gameTime.ElapsedGameTime.TotalSeconds
            );

            // Clamp to maximum speed
            if (movementComponent.Velocity.LengthSquared() > homingComponent.MaxHomingSpeed * homingComponent.MaxHomingSpeed)
            {
                movementComponent.Velocity = Vector2.Normalize(movementComponent.Velocity) * homingComponent.MaxHomingSpeed;
            }
        }
    }
}
