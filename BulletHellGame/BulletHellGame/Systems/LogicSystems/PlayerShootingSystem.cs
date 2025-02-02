using BulletHellGame.Components;
using BulletHellGame.Managers;

namespace BulletHellGame.Systems.LogicSystems
{
    public class PlayerShootingSystem : ILogicSystem
    {
        public void Update(EntityManager entityManager, GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (var entity in entityManager.GetEntitiesWithComponent<PlayerInputComponent>())
            {
                if (entity.TryGetComponent<WeaponComponent>(out var wc) &&
                    entity.TryGetComponent<PositionComponent>(out var pc) &&
                    entity.TryGetComponent<PlayerInputComponent>(out var pic))
                {
                        // Update shot cooldown
                        wc.TimeSinceLastShot += deltaTime;

                        if (pic.IsShooting && wc.CanShoot())
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
