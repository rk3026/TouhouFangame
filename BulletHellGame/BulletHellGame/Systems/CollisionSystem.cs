using BulletHellGame.Components;
using BulletHellGame.Entities;
using BulletHellGame.Managers;
using System.Linq;

namespace BulletHellGame.Systems
{
    public class CollisionSystem : ISystem
    {
        public CollisionSystem()
        {
            _checkQueue = new Queue<HitboxComponent>();
            _insertQueue = new Queue<HitboxComponent>();
        }

        private const int CELL_WIDTH = 100;

        private bool _changed;
        private Queue<HitboxComponent> _checkQueue;
        private Queue<HitboxComponent> _insertQueue;

        private void ApplyCollision(EntityManager entityManager, Entity owner, Entity other)
        {
            if (owner.HasComponent<HealthComponent>() && other.HasComponent<DamageComponent>())
            {
                if (other.HasComponent<OwnerComponent>())
                {
                    OwnerComponent otherOwner = other.GetComponent<OwnerComponent>();

                    // (Owned things don't apply damage to their owner)
                    if (otherOwner.Owner == owner)
                    {
                        return;
                    }
                }

                // Apply damage if no ownership conflict exists
                HealthComponent health = owner.GetComponent<HealthComponent>();
                DamageComponent damage = other.GetComponent<DamageComponent>();
                SpriteComponent sprite = owner.GetComponent<SpriteComponent>();
                health.TakeDamage(damage.CalculateDamage());
                sprite.FlashRed(0.1f);

                entityManager.QueueEntityForRemoval(other);
            }
        }

        public void Update(EntityManager entityManager, GameTime gameTime)
        {
            foreach (Entity entity in entityManager.GetActiveEntities().Where(e => e.HasComponent<HitboxComponent>()))
            {
                var hitboxComponent = entity.GetComponent<HitboxComponent>();
                var movementComponent = entity.GetComponent<MovementComponent>();
                var spriteComponent = entity.GetComponent<SpriteComponent>();

                // Update hitbox position based on movement

                hitboxComponent.Hitbox = new Rectangle(
                    (int)movementComponent.Position.X,
                    (int)movementComponent.Position.Y,
                    spriteComponent.CurrentFrame.Width,
                    spriteComponent.CurrentFrame.Height
                );

                _insertQueue.Enqueue(entity.GetComponent<HitboxComponent>());
                _checkQueue.Enqueue(entity.GetComponent<HitboxComponent>());
            }

            DoCollisionLogic(entityManager);
        }

        private void DoCollisionLogic(EntityManager entityManager)
        {
            if (_insertQueue.Count() == 0)
                return;

            int minX, minY, maxX, maxY;
            minX = minY = int.MaxValue;
            maxX = maxY = int.MinValue;

            // determine boundaries and density of grid
            foreach (var hitbox in _insertQueue)
            {
                if (hitbox.Hitbox.Left < minX)
                    minX = hitbox.Hitbox.Left;
                if (hitbox.Hitbox.Top < minY)
                    minY = hitbox.Hitbox.Top;
                if (hitbox.Hitbox.Right > maxX)
                    maxX = hitbox.Hitbox.Right;
                if (hitbox.Hitbox.Bottom > maxY)
                    maxY = hitbox.Hitbox.Bottom;
            }

            // dimensions of grid
            int cols = (maxX - minX) / CELL_WIDTH + 1;
            int rows = (maxY - minY) / CELL_WIDTH + 1;

            // grid cells containing linked lists of hitboxes
            LinkedList<HitboxComponent>[,] hitboxGrid = new LinkedList<HitboxComponent>[cols, rows];

            // insert all queued hitboxes into grid
            HitboxComponent cHitbox;
            while (_insertQueue.TryDequeue(out cHitbox))
            {
                int lowCol = (cHitbox.Hitbox.Left - minX) / CELL_WIDTH;
                int highCol = (cHitbox.Hitbox.Right - minX) / CELL_WIDTH;
                int lowRow = (cHitbox.Hitbox.Top - minY) / CELL_WIDTH;
                int highRow = (cHitbox.Hitbox.Bottom - minY) / CELL_WIDTH;

                // insert hitbox into all grid cells that it occupies
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
            while (_checkQueue.TryDequeue(out cHitbox))
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