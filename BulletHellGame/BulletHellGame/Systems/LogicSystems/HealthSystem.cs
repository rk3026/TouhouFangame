using BulletHellGame.Components;
using BulletHellGame.Entities;
using BulletHellGame.Managers;

namespace BulletHellGame.Systems.LogicSystems
{
    public class HealthSystem : ILogicSystem
    {
        public void Update(EntityManager entityManager, GameTime gameTime)
        {
            var entitiesToRemove = new List<Entity>();

            foreach (var entity in entityManager.GetEntitiesWithComponents(typeof(HealthComponent)))
            {
                if (entity.TryGetComponent<HealthComponent>(out var healthComponent))
                {
                    HandleEntityHealth(entity, healthComponent, entitiesToRemove);
                }
            }

            // After iterating, remove entities that should be destroyed (stops the list was modified error)
            foreach (var entity in entitiesToRemove)
            {
                entityManager.QueueEntityForRemoval(entity);
            }
        }

        private void HandleEntityHealth(Entity entity, HealthComponent healthComponent, List<Entity> entitiesToRemove)
        {
            if (healthComponent.CurrentHealth <= 0)
            {
                if (entity.TryGetComponent<BossPhaseComponent>(out var bossPhase))
                {
                    HandleBossPhase(entity, healthComponent, bossPhase, entitiesToRemove);
                }
                else
                {
                    // Regular entity removal
                    entitiesToRemove.Add(entity);
                }
            }
        }

        private void HandleBossPhase(Entity entity, HealthComponent healthComponent, BossPhaseComponent bossPhase, List<Entity> entitiesToRemove)
        {
            if (bossPhase.AdvancePhase())
            {
                // Reset stats for the new phase
                ResetBossStats(entity, healthComponent, bossPhase);
            }
            else
            {
                // No more phases left, remove the boss
                entitiesToRemove.Add(entity);
            }
        }

        private void ResetBossStats(Entity entity, HealthComponent healthComponent, BossPhaseComponent bossPhase)
        {
            var newPhaseData = bossPhase.GetCurrentPhaseData();

            // Reset health
            healthComponent.Heal(newPhaseData.Health);

            // Reset sprite
            var spriteComponent = entity.GetComponent<SpriteComponent>();
            spriteComponent.SpriteData = TextureManager.Instance.GetSpriteData(newPhaseData.SpriteName);

            // Reset movement pattern
            var movementPatternComponent = entity.GetComponent<MovementPatternComponent>();
            movementPatternComponent.PatternData = MovementPatternManager.Instance.GetPattern(newPhaseData.MovementPattern);

            // Reset weapon stats
            var weaponComponent = entity.GetComponent<ShootingComponent>();
            weaponComponent.bulletData = newPhaseData.BulletData;
        }
    }
}
