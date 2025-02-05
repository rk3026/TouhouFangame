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

            foreach (var entity in entityManager.GetEntitiesWithComponents(
                typeof(ShootingComponent), typeof(PositionComponent)))
            {
                var shooting = entity.GetComponent<ShootingComponent>();
                var position = entity.GetComponent<PositionComponent>();

                // Update cooldown
                shooting.TimeSinceLastShot += deltaTime;

                // Check if owner can shoot and if cooldown is over
                if (!ShouldEntityShoot(entity, shooting, out int bulletLayer))
                    continue;

                // Fire bullets and reset cooldown
                FireBullets(entityManager, entity, shooting, position.Position, bulletLayer);
                shooting.TimeSinceLastShot = 0f;
            }
        }

        private bool ShouldEntityShoot(Entity entity, ShootingComponent shooting, out int bulletLayer)
        {
            bulletLayer = 1; // Default to enemy bullet layer

            if (shooting.TimeSinceLastShot < shooting.FireRate)
                return false; // Cooldown not over

            if (entity.TryGetComponent<PlayerInputComponent>(out var playerInput))
            {
                if (!playerInput.IsShooting) return false;
                bulletLayer = 2;
                return true;
            }

            if (entity.TryGetComponent<OwnerComponent>(out var ownerComponent) &&
                ownerComponent.Owner.TryGetComponent<PlayerInputComponent>(out var ownerInput))
            {
                if (!ownerInput.IsShooting) return false;
                bulletLayer = 2;
                return true;
            }

            return shooting.CanShoot();
        }

        private void FireBullets(EntityManager entityManager, Entity owner, ShootingComponent shooting, Vector2 spawnPosition, int bulletLayer)
        {
            foreach (Vector2 firingDirection in shooting.FireDirections)
            {
                entityManager.SpawnBullet(
                    shooting.bulletData,
                    spawnPosition,
                    bulletLayer,
                    firingDirection,
                    owner
                );
            }
        }
    }
}
