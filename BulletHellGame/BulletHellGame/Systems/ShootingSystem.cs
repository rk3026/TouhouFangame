using BulletHellGame.Components;
using BulletHellGame.Managers;

namespace BulletHellGame.Systems
{
    public class ShootingSystem : ISystem
    {
        public void Update(EntityManager entityManager, GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (var entity in entityManager.GetActiveEntities())
            {
                if (!entity.HasComponent<WeaponComponent>())
                    continue;

                WeaponComponent weaponComponent = entity.GetComponent<WeaponComponent>();
                MovementComponent movementComponent = entity.GetComponent<MovementComponent>();

                // Update shot cooldown
                weaponComponent.TimeSinceLastShot += deltaTime;

                // Check if the entity can shoot and is attempting to shoot
                if (entity.HasComponent<PlayerInputComponent>())
                {
                    var playerInputComponent = entity.GetComponent<PlayerInputComponent>();

                    if (playerInputComponent.IsShooting && weaponComponent.CanShoot())
                    {
                        foreach (Vector2 firingDirection in weaponComponent.FireDirections)
                        {
                            entityManager.SpawnBullet(weaponComponent.bulletData, movementComponent.Position, firingDirection, entity);
                        }

                        // Reset cooldown after shooting
                        weaponComponent.TimeSinceLastShot = 0f;
                    }
                }
            }
        }
    }
}
