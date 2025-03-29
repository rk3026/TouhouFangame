using BulletHellGame.Logic.Components;
using BulletHellGame.Logic.Entities;
using BulletHellGame.Logic.Managers;

namespace BulletHellGame.Logic.Systems.LogicSystems
{
    public class BombingSystem : ILogicSystem
    {
        public void Update(EntityManager entityManager, GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (Entity entity in entityManager.GetEntitiesWithComponents(typeof(BombingComponent)))
            {
                if (!entity.TryGetComponent(out BombingComponent bombing) ||
                    !entity.TryGetComponent(out ControllerComponent controller))
                {
                    continue;
                }

                // Update bomb cooldown timer
                bombing.TimeSinceLastBomb += deltaTime;

                if (!controller.Controller.IsBombing)
                    continue;

                if (bombing.BombCount <= 0 || bombing.TimeSinceLastBomb < bombing.BombCooldown)
                    continue;

                // Perform Bomb: Clear all damageable entities
                foreach (Entity target in entityManager.GetEntitiesWithComponents(typeof(DamageComponent)))
                {
                    if (target.TryGetComponent<HealthComponent>(out var hc))
                        hc.TakeDamage(bombing.BombDamage);
                    else
                        entityManager.QueueEntityForRemoval(target);
                }
                SFXManager.Instance.PlaySound("se_gun00");

                bombing.BombCount--;
                bombing.TimeSinceLastBomb = 0f;
            }
        }
    }
}
