using BulletHellGame.Logic.Components;
using BulletHellGame.Logic.Entities;
using BulletHellGame.Logic.Managers;

namespace BulletHellGame.Logic.Systems.LogicSystems
{
    public class HealthSystem : ILogicSystem
    {
        private const float LootSpreadRadius = 8f;
        private readonly Vector2 lootVelocity = new Vector2(0, 1);

        public void Update(EntityManager entityManager, GameTime gameTime)
        {
            var entitiesToRemove = new List<Entity>();

            foreach (var entity in entityManager.GetEntitiesWithComponents(typeof(HealthComponent)))
            {
                if (entity.TryGetComponent<HealthComponent>(out var health))
                {
                    ProcessHealth(entityManager, entity, health, entitiesToRemove);
                }
            }

            foreach (var entity in entitiesToRemove)
            {
                entityManager.QueueEntityForRemoval(entity);
            }
        }

        private void ProcessHealth(EntityManager entityManager, Entity entity, HealthComponent health, List<Entity> entitiesToRemove)
        {
            if (health.CurrentHealth > 0)
                return;

            if (entity.TryGetComponent<BossPhaseComponent>(out var bossPhase))
            {
                HandleBossPhase(entityManager, entity, health, bossPhase, entitiesToRemove);
            }
            else
            {
                DropLoot(entityManager, entity);
                entitiesToRemove.Add(entity);
            }
        }

        private void HandleBossPhase(EntityManager entityManager, Entity entity, HealthComponent health, BossPhaseComponent phase, List<Entity> entitiesToRemove)
        {
            if (phase.AdvancePhase())
            {
                ResetBossForNewPhase(entityManager, entity, health, phase);
            }
            else
            {
                entitiesToRemove.Add(entity);
            }
        }

        private void ResetBossForNewPhase(EntityManager entityManager, Entity boss, HealthComponent health, BossPhaseComponent phase)
        {
            var data = phase.GetCurrentPhaseData();

            health.MaxHealth = data.Health;

            // Reset health
            health.Heal(data.Health);

            // Reset visual and movement
            boss.GetComponent<SpriteComponent>().SpriteData = TextureManager.Instance.GetSpriteData(data.SpriteName);
            boss.GetComponent<MovementPatternComponent>().PatternData = MovementPatternManager.Instance.GetPattern(data.MovementPattern);

            // Update weapons on owned spawner entities
            foreach (var spawner in entityManager.GetEntitiesWithComponents(typeof(ShootingComponent), typeof(OwnerComponent)))
            {
                var ownerComponent = spawner.GetComponent<OwnerComponent>();
                if (ownerComponent.Owner == boss)
                {
                    spawner.GetComponent<ShootingComponent>().SetWeapons(data.Weapons);
                }
            }
        }

        private void DropLoot(EntityManager entityManager, Entity entity)
        {
            if (!entity.TryGetComponent<LootComponent>(out var loot) ||
                !entity.TryGetComponent<PositionComponent>(out var position))
                return;

            var rng = new Random();
            foreach (var item in loot.Loot)
            {
                var offset = new Vector2(
                    (float)(rng.NextDouble() * 2 * LootSpreadRadius - LootSpreadRadius),
                    (float)(rng.NextDouble() * 2 * LootSpreadRadius - LootSpreadRadius)
                );

                entityManager.SpawnCollectible(item, position.Position + offset, lootVelocity);
            }
        }
    }
}
