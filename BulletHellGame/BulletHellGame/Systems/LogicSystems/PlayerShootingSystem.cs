using BulletHellGame.Components;
using BulletHellGame.Managers;

namespace BulletHellGame.Systems.LogicSystems
{
    public class PlayerShootingSystem : ILogicSystem
    {
        public void Update(EntityManager entityManager, GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (var entity in entityManager.GetActiveEntities())
            {
                if (entity.TryGetComponent<WeaponComponent>(out var wc) &&
                    entity.TryGetComponent<PositionComponent>(out var pc))
                {
                    // Update shot cooldown
                    wc.TimeSinceLastShot += deltaTime;

                    // Check if the entity can shoot and is attempting to shoot
                    if (entity.HasComponent<PlayerInputComponent>())
                    {
                        var playerInputComponent = entity.GetComponent<PlayerInputComponent>();

                        if (playerInputComponent.IsShooting && wc.CanShoot())
                        {
                            foreach (Vector2 firingDirection in wc.FireDirections)
                            {
                                entityManager.SpawnBullet(wc.bulletData, pc.Position, 2, firingDirection, entity);
                            }

                            // Reset cooldown after shooting
                            wc.TimeSinceLastShot = 0f;
                        }
                    }
                }
            }
        }
    }
}
