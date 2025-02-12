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
                    HandleEntityHealth(entityManager, entity, healthComponent, entitiesToRemove);
                }
            }

            // After iterating, remove entities that should be destroyed (stops the list was modified error)
            foreach (var entity in entitiesToRemove)
            {
                entityManager.QueueEntityForRemoval(entity);
            }
        }

        private void HandleEntityHealth(EntityManager entityManager, Entity entity, HealthComponent healthComponent, List<Entity> entitiesToRemove)
        {
            if (healthComponent.CurrentHealth <= 0)
            {
                if (entity.TryGetComponent<BossPhaseComponent>(out var bossPhase))
                {
                    HandleBossPhase(entity, healthComponent, bossPhase, entitiesToRemove);
                }
                else
                {
                    HandleLootDrop(entityManager, entity);
                    entitiesToRemove.Add(entity);
                }
            }
        }

        private const float LOOT_SPREAD_RADIUS = 8f; // Maximum distance from enemy center
        private Vector2 lootVelocity = new Vector2(0,1);
        private void HandleLootDrop(EntityManager entityManager, Entity entity)
        {
            if (entity.TryGetComponent<LootComponent>(out var lc) &&
                entity.TryGetComponent<PositionComponent>(out var pc))
            {
                Random rng = new Random();

                foreach (var lootData in lc.Loot)
                {
                    // Random offset for loot position
                    float offsetX = (float)(rng.NextDouble() * 2 * LOOT_SPREAD_RADIUS - LOOT_SPREAD_RADIUS);
                    float offsetY = (float)(rng.NextDouble() * 2 * LOOT_SPREAD_RADIUS - LOOT_SPREAD_RADIUS);
                    Vector2 lootPosition = pc.Position + new Vector2(offsetX, offsetY);

                    entityManager.SpawnCollectible(lootData, lootPosition, lootVelocity);
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
            var shootingComponent = entity.GetComponent<ShootingComponent>();
            shootingComponent.SetWeapons(newPhaseData.Weapons);
        }
    }
}
