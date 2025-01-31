using BulletHellGame.Components;
using BulletHellGame.Managers;

namespace BulletHellGame.Systems.LogicSystems
{
    public class EnemyShootingSystem : ILogicSystem
    {
        public void Update(EntityManager entityManager, GameTime gameTime)
        {
            foreach (var entity in entityManager.GetEntitiesWithComponent<WeaponComponent>())
            {
                // Skip the player or entities that don't have the required components
                if (entity.TryGetComponent<WeaponComponent>(out var wc) &&
                    entity.TryGetComponent<PositionComponent>(out var pc) &&
                    !entity.TryGetComponent<PlayerInputComponent>(out var pic))
                {
                    // NPC shooting logic
                    if (wc.CanShoot())
                    {
                        // Spawn bullets in all firing directions
                        foreach (Vector2 firingDirection in wc.FireDirections)
                        {
                            entityManager.SpawnBullet(wc.bulletData, pc.Position, 1, firingDirection, entity);
                        }

                        // Reset cooldown after shooting
                        wc.TimeSinceLastShot = 0f;
                    }
                    else
                    {
                        // Update cooldown timer
                        wc.TimeSinceLastShot += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                }
            }
        }
    }
}
