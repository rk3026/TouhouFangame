using BulletHellGame.Logic.Components;
using BulletHellGame.Logic.Entities;
using BulletHellGame.Logic.Managers;

namespace BulletHellGame.Logic.Strategies.CollisionStrategies
{
    public class BulletCollisionStrategy : ICollisionStrategy
    {
        public void ApplyCollision(EntityManager entityManager, Entity collidingEntity, Entity other)
        {
            if (other.TryGetComponent<InvincibilityComponent>(out var ic) && ic.RemainingTime > 0) return; // Ignore damage while invincible

            if (other.TryGetComponent<HealthComponent>(out var health) &&
                collidingEntity.TryGetComponent<DamageComponent>(out var dc))
            {
                int damageVal = dc.CalculateDamage();
                health.TakeDamage(damageVal);
                ParticleEffectManager.Instance.SpawnDamageNumber(other.GetComponent<PositionComponent>().Position, damageVal);

                if (other.TryGetComponent<SpriteComponent>(out var sprite))
                {
                    sprite.FlashRed();
                }

                if (health.CurrentHealth <= 0)
                {
                    if (ic != null)
                    {
                        ic.RemainingTime = 2f; // Apply 2 seconds of invincibility
                    }
                    if (other.TryGetComponent<PlayerStatsComponent>(out var psc))
                    {
                        if (psc.Lives > 0) // Only decrement if they have extra lives
                        {
                            psc.Lives -= 1;
                            health.Heal(health.MaxHealth);
                        }
                    }
                }
                entityManager.QueueEntityForRemoval(collidingEntity);
            }

            // Handle pushing interactions
            if (collidingEntity.HasComponent<PusherComponent>() && other.HasComponent<PushableComponent>())
            {
                var pusherVelocity = collidingEntity.GetComponent<VelocityComponent>().Velocity;
                var pushablePosition = other.GetComponent<PositionComponent>();

                // Move the pushable entity by the pusher's velocity
                pushablePosition.Position += pusherVelocity;
            }


        }
    }
}
