﻿using BulletHellGame.Logic.Components;
using BulletHellGame.Logic.Entities;
using BulletHellGame.Logic.Managers;

namespace BulletHellGame.Logic.Strategies.CollisionStrategies
{
    public class EnemyCollisionStrategy : ICollisionStrategy
    {
        public void ApplyCollision(EntityManager entityManager, Entity collidingEntity, Entity other)
        {
            if (other.TryGetComponent<InvincibilityComponent>(out var ic) && ic.RemainingTime > 0) return; // Ignore damage while invincible

            // Handle damage logic if the collidingEntity has health and the other entity has a damage component
            if (other.TryGetComponent<HealthComponent>(out var health) &&
                collidingEntity.TryGetComponent<DamageComponent>(out var damage) &&
                other.TryGetComponent<SpriteComponent>(out var sprite))
            {
                // Apply damage if no ownership conflict exists
                health.TakeDamage(damage.CalculateDamage());
                if (ic != null)
                {
                    ic.RemainingTime = 2f;
                }
                SFXManager.Instance.PlaySound("se_damage00");

                if (other.TryGetComponent<PlayerStatsComponent>(out var psc))
                {
                    if (health.CurrentHealth <= 0) // Check if the player is out of health
                    {
                        if (psc.Lives > 0) // Only decrement if they have extra lives
                        {
                            psc.Lives -= 1;
                            health.Heal(health.MaxHealth);
                        }
                    }
                }
            }
        }
    }
}
