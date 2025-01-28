using BulletHellGame.Components;
using BulletHellGame.Entities;
using BulletHellGame.Managers;
using System.Linq;

namespace BulletHellGame.Systems
{
    public class CollisionSystem : ISystem
    {
        private const int CELL_WIDTH = 100;
        private Dictionary<(int, int), List<HitboxComponent>> hitboxGrid;

        public CollisionSystem()
        {
            hitboxGrid = new Dictionary<(int, int), List<HitboxComponent>>();
        }

        private void ApplyCollision(EntityManager entityManager, Entity owner, Entity other)
        {
            if (owner.TryGetComponent<HealthComponent>(out var health) &&
                other.TryGetComponent<DamageComponent>(out var damage))
            {

                // Apply damage if no ownership conflict exists
                health.TakeDamage(damage.CalculateDamage());

                if (owner.TryGetComponent<SpriteComponent>(out var sprite))
                {
                    sprite.FlashRed(0.1f);
                }

                entityManager.QueueEntityForRemoval(other);
            }
        }

        public void Update(EntityManager entityManager, GameTime gameTime)
        {
            hitboxGrid.Clear();

            // Precompute the hitboxes for entities with the relevant components
            foreach (var entity in entityManager.GetActiveEntities().Where(e => e.HasComponent<HitboxComponent>()))
            {
                if (entity.TryGetComponent<HitboxComponent>(out var hitboxComponent) &&
                    entity.TryGetComponent<MovementComponent>(out var movementComponent) &&
                    entity.TryGetComponent<SpriteComponent>(out var spriteComponent))
                {
                    // Update hitbox position based on movement
                    hitboxComponent.Hitbox = new Rectangle(
                        (int)movementComponent.Position.X,
                        (int)movementComponent.Position.Y,
                        spriteComponent.CurrentFrame.Width,
                        spriteComponent.CurrentFrame.Height
                    );

                    // Insert the hitbox into the appropriate grid cells
                    InsertIntoGrid(hitboxComponent);
                }
            }

            DoCollisionLogic(entityManager);
        }

        private void InsertIntoGrid(HitboxComponent hitboxComponent)
        {
            int minX = hitboxComponent.Hitbox.Left;
            int minY = hitboxComponent.Hitbox.Top;
            int maxX = hitboxComponent.Hitbox.Right;
            int maxY = hitboxComponent.Hitbox.Bottom;

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
                    hitboxGrid[gridKey].Add(hitboxComponent);
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
                        if (i==j) continue; // Don't check the same hitbox
                        var hitboxA = hitboxes[i];
                        var hitboxB = hitboxes[j];
                        if (hitboxA.Layer == hitboxB.Layer) continue; // Don't collide if on same layer

                        if (hitboxA.Hitbox.Intersects(hitboxB.Hitbox))
                        {
                            ApplyCollision(entityManager, hitboxA.Owner, hitboxB.Owner);
                        }
                    }
                }
            }
        }
    }
}
