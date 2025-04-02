using BulletHellGame.Logic.Components;
using BulletHellGame.Logic.Entities;
using BulletHellGame.Logic.Managers;

namespace BulletHellGame.Logic.Strategies.CollisionStrategies
{
    public class CollectibleCollisionStrategy : ICollisionStrategy
    {
        public void ApplyCollision(EntityManager entityManager, Entity collidingEntity, Entity other)
        {
            // Handle collectible pickup logic
            if (other.TryGetComponent<CollectorComponent>(out var cc) &&
                other.TryGetComponent<PlayerStatsComponent>(out var stats) &&
                other.TryGetComponent<BombingComponent>(out var bc) &&
                collidingEntity.TryGetComponent<PickUpEffectComponent>(out var pec))
            {
                foreach (var effect in pec.Effects)
                {
                    switch (effect.Key)
                    {
                        case CollectibleType.OneUp:
                            stats.Lives += effect.Value;
                            break;

                        case CollectibleType.Bomb:
                            bc.BombCount += effect.Value;
                            break;

                        case CollectibleType.PowerUp:
                            stats.Score += effect.Value;
                            stats.Power += effect.Value;
                            break;

                        case CollectibleType.PointItem:
                            stats.Score += effect.Value;
                            break;

                        case CollectibleType.StarItem:
                            stats.Score += effect.Value / 2; // Small score reward
                            stats.CherryPoints += effect.Value; // Add cherry points
                            break;

                        case CollectibleType.CherryItem:
                            stats.CherryPoints += effect.Value; // Increase cherry points
                            stats.CherryPlus += effect.Value / 2; // Some amount goes into Cherry+
                            break;
                    }
                    entityManager.QueueEntityForRemoval(collidingEntity);
                }
            }
        }
    }
}
