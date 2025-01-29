using BulletHellGame.Components;
using BulletHellGame.Entities;
using BulletHellGame.Managers;
using System.Linq;

namespace BulletHellGame.Systems.LogicSystems
{
    public class HomingSystem : ILogicSystem
    {
        public void Update(EntityManager entityManager, GameTime gameTime)
        {
            List<Entity> entities = entityManager.GetActiveEntities().ToList();
            foreach (Entity entity in entities)
            {
                if (entity.TryGetComponent<HomingComponent>(out var hc) &&
                    entity.TryGetComponent<PositionComponent>(out var pc) &&
                    entity.TryGetComponent<VelocityComponent>(out var vc))
                {
                    Entity currentTarget = hc.CurrentTarget;

                    // Check if current target is valid or find a new one
                    if (currentTarget == null || !currentTarget.IsActive || !IsWithinRange(pc.Position, currentTarget.GetComponent<PositionComponent>().Position, hc.HomingRange))
                    {
                        currentTarget = FindNewTarget(entityManager, pc.Position, hc.HomingRange);
                        hc.CurrentTarget = currentTarget;
                    }

                    // Adjust the bullet's direction if a valid target is found
                    if (currentTarget != null && currentTarget.IsActive)
                    {
                        AdjustDirectionTowardsTarget(entity, currentTarget, hc, pc, vc, gameTime);
                    }
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
                    float distanceSquared = Vector2.DistanceSquared(target.GetComponent<PositionComponent>().Position, bulletPosition);
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

        private void AdjustDirectionTowardsTarget(Entity bullet, Entity target, HomingComponent homingComponent, PositionComponent positionComponent, VelocityComponent velocityComponent, GameTime gameTime)
        {
            Vector2 directionToTarget = target.GetComponent<PositionComponent>().Position - bullet.GetComponent<PositionComponent>().Position;
            if (directionToTarget.LengthSquared() > 0)
            {
                directionToTarget.Normalize();
            }

            Vector2 desiredVelocity = directionToTarget * homingComponent.MaxHomingSpeed;

            // Smoothly interpolate toward the desired velocity
            velocityComponent.Velocity = Vector2.Lerp(
                velocityComponent.Velocity,
                desiredVelocity,
                homingComponent.HomingStrength * (float)gameTime.ElapsedGameTime.TotalSeconds
            );

            // Clamp to maximum speed
            if (velocityComponent.Velocity.LengthSquared() > homingComponent.MaxHomingSpeed * homingComponent.MaxHomingSpeed)
            {
                velocityComponent.Velocity = Vector2.Normalize(velocityComponent.Velocity) * homingComponent.MaxHomingSpeed;
            }
        }
    }
}
