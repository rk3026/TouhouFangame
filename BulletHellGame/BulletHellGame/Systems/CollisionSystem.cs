using BulletHellGame.Components;
using BulletHellGame.Entities;
using BulletHellGame.Managers;
using System.Linq;

namespace BulletHellGame.Systems
{
    public class CollisionSystem : ISystem
    {
        private const int CELL_WIDTH = 100;

        private bool _changed;
        private List<HitboxComponent> _checkList;
        private List<HitboxComponent> _insertList;

        public CollisionSystem()
        {
            _checkList = new List<HitboxComponent>();
            _insertList = new List<HitboxComponent>();
        }

        private void ApplyCollision(EntityManager entityManager, Entity owner, Entity other)
        {
            if (owner.TryGetComponent<HealthComponent>(out var health) &&
                other.TryGetComponent<DamageComponent>(out var damage))
            {
                if (other.TryGetComponent<OwnerComponent>(out var otherOwner) &&
                    otherOwner.Owner == owner)
                {
                    return; // Owned things don't apply damage to their owner
                }

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
            _insertList.Clear();
            _checkList.Clear();

            foreach (Entity entity in entityManager.GetActiveEntities().Where(e => e.HasComponent<HitboxComponent>()))
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

                    _insertList.Add(hitboxComponent);
                    _checkList.Add(hitboxComponent);
                }
            }

            DoCollisionLogic(entityManager);
        }

        private void DoCollisionLogic(EntityManager entityManager)
        {
            if (_insertList.Count == 0) return;

            int minX = int.MaxValue, minY = int.MaxValue;
            int maxX = int.MinValue, maxY = int.MinValue;

            // determine boundaries and density of grid
            foreach (var hitbox in _insertList)
            {
                minX = Math.Min(minX, hitbox.Hitbox.Left);
                minY = Math.Min(minY, hitbox.Hitbox.Top);
                maxX = Math.Max(maxX, hitbox.Hitbox.Right);
                maxY = Math.Max(maxY, hitbox.Hitbox.Bottom);
            }

            // dimensions of grid
            int cols = (maxX - minX) / CELL_WIDTH + 1;
            int rows = (maxY - minY) / CELL_WIDTH + 1;

            // grid cells containing linked lists of hitboxes
            LinkedList<HitboxComponent>[,] hitboxGrid = new LinkedList<HitboxComponent>[cols, rows];

            // insert all queued hitboxes into grid
            foreach (var cHitbox in _insertList)
            {
                int lowCol = (cHitbox.Hitbox.Left - minX) / CELL_WIDTH;
                int highCol = (cHitbox.Hitbox.Right - minX) / CELL_WIDTH;
                int lowRow = (cHitbox.Hitbox.Top - minY) / CELL_WIDTH;
                int highRow = (cHitbox.Hitbox.Bottom - minY) / CELL_WIDTH;

                // insert hitbox into all grid cells it occupies
                for (int c = lowCol; c <= highCol; ++c)
                {
                    for (int r = lowRow; r <= highRow; ++r)
                    {
                        if (hitboxGrid[c, r] == null)
                        {
                            hitboxGrid[c, r] = new LinkedList<HitboxComponent>();
                        }

                        hitboxGrid[c, r].AddLast(cHitbox);
                    }
                }
            }

            // check all queued collision checks
            foreach (var cHitbox in _checkList)
            {
                int lowCol = (cHitbox.Hitbox.Left - minX) / CELL_WIDTH;
                int highCol = (cHitbox.Hitbox.Right - minX) / CELL_WIDTH;
                int lowRow = (cHitbox.Hitbox.Top - minY) / CELL_WIDTH;
                int highRow = (cHitbox.Hitbox.Bottom - minY) / CELL_WIDTH;

                // check all grid cells hitbox occupies
                for (int c = lowCol; c <= highCol; ++c)
                {
                    for (int r = lowRow; r <= highRow; ++r)
                    {
                        if (hitboxGrid[c, r] != null)
                        {
                            foreach (var hitbox in hitboxGrid[c, r])
                            {
                                if (cHitbox == hitbox) continue;
                                if (cHitbox.Hitbox.Intersects(hitbox.Hitbox))
                                {
                                    ApplyCollision(entityManager, hitbox.Owner, cHitbox.Owner);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
