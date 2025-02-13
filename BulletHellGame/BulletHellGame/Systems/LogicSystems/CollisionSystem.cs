using BulletHellGame.Components;
using BulletHellGame.Entities;
using BulletHellGame.Managers;

namespace BulletHellGame.Systems.LogicSystems
{
    public class CollisionSystem : ILogicSystem
    {
        private const int CELL_WIDTH = 100;
        private Dictionary<(int, int), List<HitboxComponent>> hitboxGrid;

        public CollisionSystem()
        {
            hitboxGrid = new Dictionary<(int, int), List<HitboxComponent>>();
        }

        private void ApplyCollision(EntityManager entityManager, Entity owner, Entity other)
        {
            if (owner.TryGetComponent<InvincibilityComponent>(out var ic) && ic.RemainingTime > 0) return; // Ignore damage while invincible

            // Handle damage logic if the owner has health and the other entity has a damage component
            if (owner.TryGetComponent<HealthComponent>(out var health) &&
                other.TryGetComponent<DamageComponent>(out var damage))
            {
                // Apply damage if no ownership conflict exists
                health.TakeDamage(damage.CalculateDamage());

                if (owner.TryGetComponent<SpriteComponent>(out var sprite))
                {
                    sprite.FlashRed(0.1f);
                }

                if (owner.TryGetComponent<PlayerStatsComponent>(out var psc))
                {
                    if (health.CurrentHealth <= 0) // Check if the player is out of health
                    {
                        if (psc.Lives > 0) // Only decrement if they have extra lives
                        {
                            psc.Lives -= 1;
                            health.Heal(health.MaxHealth);
                            ic.RemainingTime = 3f; // Add 3 seconds of invincibility
                            sprite.FlashRed();
                            return; // Prevent player removal
                        }
                    }
                }
                entityManager.QueueEntityForRemoval(other);
            }

            // Handle collectible pickup logic
            if (owner.TryGetComponent<CollectorComponent>(out var cc) &&
                owner.TryGetComponent<PlayerStatsComponent>(out var stats) &&
                other.TryGetComponent<PickUpEffectComponent>(out var pec))
            {
                foreach (var effect in pec.Effects)
                {
                    switch (effect.Key)
                    {
                        case CollectibleType.OneUp:
                            stats.Lives += effect.Value;
                            break;

                        case CollectibleType.Bomb:
                            stats.Bombs += effect.Value;
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
                }

                entityManager.QueueEntityForRemoval(other); // Remove collectible after pickup
            }
        }


        public void Update(EntityManager entityManager, GameTime gameTime)
        {
            hitboxGrid.Clear();


            entityManager.OperateOnEntities((entity) => {

                entity.TryGetComponent<HitboxComponent>(out var hc);
                entity.TryGetComponent<PositionComponent>(out var pc);
                // Insert the hitbox into the appropriate grid cells
                InsertIntoGrid(hc, pc);

            }, typeof(HitboxComponent), typeof(PositionComponent));

            DoCollisionLogic(entityManager);
        }

        private void InsertIntoGrid(HitboxComponent hc, PositionComponent pc)
        {
            int minX = (int)(pc.Position.X - hc.Hitbox.X / 2);
            int minY = (int)(pc.Position.Y - hc.Hitbox.Y / 2);
            int maxX = (int)(pc.Position.X + hc.Hitbox.X / 2);
            int maxY = (int)(pc.Position.Y + hc.Hitbox.Y / 2);

            int lowCol = minX / CELL_WIDTH;
            int highCol = maxX / CELL_WIDTH;
            int lowRow = minY / CELL_WIDTH;
            int highRow = maxY / CELL_WIDTH;

            for (int c = lowCol; c <= highCol; ++c)
            {
                for (int r = lowRow; r <= highRow; ++r)
                {
                    var gridKey = (c, r);
                    if (!hitboxGrid.ContainsKey(gridKey))
                    {
                        hitboxGrid[gridKey] = new List<HitboxComponent>();
                    }
                    hitboxGrid[gridKey].Add(hc);
                }
            }
        }

        private void DoCollisionLogic(EntityManager entityManager)
        {
            // Check collisions for each grid cell
            foreach (var cell in hitboxGrid)
            {
                var hitboxes = cell.Value;
                for (int i = 0; i < hitboxes.Count; i++)
                {
                    for (int j = 0; j < hitboxes.Count; j++)
                    {
                        if (i == j) continue; // Don't check the same hitbox
                        var hitboxA = hitboxes[i];
                        var hitboxB = hitboxes[j];
                        if (hitboxA.Layer == hitboxB.Layer) continue; // Don't collide if on same layer

                        Vector2 hitAPos = hitboxA.Owner.GetComponent<PositionComponent>().Position;
                        Vector2 hitBPos = hitboxB.Owner.GetComponent<PositionComponent>().Position;
                        //Rectangle rectA = new Rectangle((int)hitAPos.X, (int)hitAPos.Y, (int)hitboxA.Hitbox.X, (int)hitboxA.Hitbox.Y);
                        //Rectangle rectB = new Rectangle((int)hitBPos.X, (int)hitBPos.Y, (int)hitboxB.Hitbox.X, (int)hitboxB.Hitbox.Y);
                        Rectangle rectA = new Rectangle((int)(hitAPos.X - hitboxA.Hitbox.X / 2), (int)(hitAPos.Y - hitboxA.Hitbox.Y / 2), (int)hitboxA.Hitbox.X, (int)hitboxA.Hitbox.Y);
                        Rectangle rectB = new Rectangle((int)(hitBPos.X - hitboxB.Hitbox.X / 2), (int)(hitBPos.Y - hitboxB.Hitbox.Y / 2), (int)hitboxB.Hitbox.X, (int)hitboxB.Hitbox.Y);

                        if (rectA.Intersects(rectB))
                        {
                            ApplyCollision(entityManager, hitboxA.Owner, hitboxB.Owner);
                        }
                    }
                }
            }
        }
    }
}
