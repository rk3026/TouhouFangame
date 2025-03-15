using BulletHellGame.Components;
using BulletHellGame.Entities;
using BulletHellGame.Managers;

namespace BulletHellGame.Systems.LogicSystems
{
    public class ShootingSystem : ILogicSystem
    {
        public void Update(EntityManager entityManager, GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (var entity in entityManager.GetEntitiesWithComponents(typeof(ShootingComponent), typeof(PositionComponent)))
            {
                var shooting = entity.GetComponent<ShootingComponent>();
                var position = entity.GetComponent<PositionComponent>();

                // Update cooldowns for each weapon
                foreach (var weapon in shooting.Weapons)
                {
                    if (shooting.WeaponCooldowns.ContainsKey(weapon))
                    {
                        shooting.WeaponCooldowns[weapon] += deltaTime;
                    }
                }

                // Determine bullet layer (player or enemy)
                int bulletLayer = GetBulletLayer(entity);
                if (!shooting.IsShooting)
                    continue;

                // Fire bullets for all available weapons that are off cooldown
                FireBullets(entityManager, entity, shooting, position.Position, bulletLayer);
            }
        }

        private int GetBulletLayer(Entity entity)
        {
            if (entity.TryGetComponent<HitboxComponent>(out var hc))
            {
                return hc.Layer;
            }
            else return 2; // Default to player layer
        }

        private void FireBullets(EntityManager entityManager, Entity owner, ShootingComponent shooting, Vector2 spawnPosition, int bulletLayer)
        {
            foreach (var weapon in shooting.Weapons)
            {
                if (shooting.WeaponCooldowns.TryGetValue(weapon, out float cooldown) && cooldown >= weapon.FireRate)
                {
                    // Fire bullets in all designated directions
                    foreach (Vector2 firingDirection in weapon.FireDirections)
                    {
                        entityManager.SpawnBullet(
                            weapon.BulletData,
                            spawnPosition,
                            bulletLayer,
                            firingDirection,
                            owner
                        );
                    }

                    // Reset cooldown for this weapon
                    shooting.WeaponCooldowns[weapon] = 0f;
                }
            }
        }
    }
}
