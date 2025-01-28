using BulletHellGame.Components;
using BulletHellGame.Managers;

namespace BulletHellGame.Systems
{
    public class EnemyShootingSystem : ISystem
    {
        public void Update(EntityManager entityManager, GameTime gameTime)
        {
            foreach (var entity in entityManager.GetActiveEntities())
            {
                // Skip the player or entities that don't have the required components
                if (entity.TryGetComponent<WeaponComponent>(out var weapon) &&
                    entity.TryGetComponent<MovementComponent>(out var movement) &&
                    !entity.TryGetComponent<PlayerInputComponent>(out var playerInput))
                {
                    // NPC shooting logic
                    if (weapon.CanShoot())
                    {
                        // Spawn bullets in all firing directions
                        foreach (Vector2 firingDirection in weapon.FireDirections)
                        {
                            entityManager.SpawnBullet(weapon.bulletData, movement.Position, 1, firingDirection, entity);
                        }

                        // Reset cooldown after shooting
                        weapon.TimeSinceLastShot = 0f;
                    }
                    else
                    {
                        // Update cooldown timer
                        weapon.TimeSinceLastShot += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                }
            }
        }
    }
}
