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

                if (ic != null)
                {
                    ic.RemainingTime = 2f;
                }

                if (other.TryGetComponent<SpriteComponent>(out var sprite))
                {
                    sprite.FlashRed();
                }

                if (other.TryGetComponent<PlayerStatsComponent>(out var psc))
                {
                    if (health.CurrentHealth <= 0) // Check if the player is out of health
                    {
                        if (psc.Lives > 0) // Only decrement if they have extra lives
                        {
                            psc.Lives -= 1;
                            health.Heal(health.MaxHealth);
                            sprite.FlashRed();
                            return; // Prevent player removal
                        }
                    }
                }
                entityManager.QueueEntityForRemoval(collidingEntity);
            }
        }
    }
}
