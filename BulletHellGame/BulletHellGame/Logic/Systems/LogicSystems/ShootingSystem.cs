using BulletHellGame.Logic.Components;
using BulletHellGame.Logic.Entities;
using BulletHellGame.Logic.Managers;

namespace BulletHellGame.Logic.Systems.LogicSystems
{
    public class ShootingSystem : ILogicSystem
    {
        public void Update(EntityManager entityManager, GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (var entity in entityManager.GetEntitiesWithComponents(typeof(ShootingComponent), typeof(PositionComponent)))
            {
                if (entity.TryGetComponent<ShootingComponent>(out var sc) &&
                    entity.TryGetComponent<PositionComponent>(out var pc) &&
                    entity.TryGetComponent<ControllerComponent>(out var cc))
                {

                    // Update cooldowns for each weapon
                    foreach (var weapon in sc.Weapons)
                    {
                        if (sc.WeaponCooldowns.ContainsKey(weapon))
                        {
                            sc.WeaponCooldowns[weapon] += deltaTime;
                        }
                    }

                    // Determine bullet layer (player or enemy)
                    HitboxLayer bulletLayer = GetBulletLayer(entity);
                    if (!cc.Controller.IsShooting)
                        continue;

                    // Fire bullets for all available weapons that are off cooldown
                    FireBullets(entityManager, entity, sc, pc.Position, bulletLayer);
                }
            }
        }

        private HitboxLayer GetBulletLayer(Entity entity)
        {
            if (entity.TryGetComponent<HitboxComponent>(out var hc))
            {
                return hc.Layer;
            }
            else if (entity.TryGetComponent<OwnerComponent>(out var oc) && oc.Owner.TryGetComponent<HitboxComponent>(out hc))
            {
                return hc.Layer;
            }
            else return HitboxLayer.PlayerAndPlayerBullets; // Default to player layer
        }

        private void FireBullets(EntityManager entityManager, Entity owner, ShootingComponent shooting, Vector2 spawnPosition, HitboxLayer bulletLayer)
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
                        SFXManager.Instance.PlaySound("shoot");
                    }

                    // Reset cooldown for this weapon
                    shooting.WeaponCooldowns[weapon] = 0f;
                }
            }
        }
    }
}
