using BulletHellGame.Components;
using BulletHellGame.Data.DataTransferObjects;
using BulletHellGame.Entities;
using BulletHellGame.Factories;
using System.Collections.Immutable;
using System.Linq;

namespace BulletHellGame.Managers
{
    public class EntityManager
    {
        public Rectangle Bounds { get; set; }

        // Factories for entity creation
        private readonly EnemyFactory _enemyFactory;
        private readonly BulletFactory _bulletFactory;
        private readonly CollectibleFactory _collectibleFactory;
        private readonly PlayerFactory _playerFactory;

        // Maximum pool sizes per entity type
        private const int MAX_BULLET_POOL_SIZE = 300;
        private const int MAX_ENEMY_POOL_SIZE = 100;
        private const int MAX_COLLECTIBLE_POOL_SIZE = 100;

        // Pools for reusable entities
        private readonly Queue<Entity> _enemyPool = new();
        private readonly Queue<Entity> _bulletPool = new();
        private readonly Queue<Entity> _collectiblePool = new();

        // Lists of active entities
        private readonly List<Entity> _activeEnemies = new();
        private readonly List<Entity> _activeBullets = new();
        private readonly List<Entity> _activeCollectibles = new();
        private Entity _playerCharacter;

        public EntityManager(Rectangle bounds)
        {
            this.Bounds = bounds;
            // Initialize factories
            _enemyFactory = new EnemyFactory();
            _bulletFactory = new BulletFactory();
            _collectibleFactory = new CollectibleFactory();
            _playerFactory = new PlayerFactory();
        }

        public int GetEnemyCount() => _activeEnemies.Count;

        public int GetBulletCount() => _activeBullets.Count;

        public int GetCollectibleCount() => _activeCollectibles.Count;

        public int GetPlayerCount() => 1;

        public List<Entity> GetActiveEntities()
        {
            return _activeEnemies.Concat<Entity>(_activeBullets)
                                 .Concat<Entity>(_activeCollectibles)
                                 .Concat(new[] { _playerCharacter })
                                 .ToList();
        }

        public void QueueEntityForRemoval(Entity entity)
        {
            if (entity == null) return;  // Ensure the entity isn't null before processing

            entity.Deactivate();

            if (_activeBullets.Contains(entity))
            {
                _activeBullets.Remove(entity);
                ReturnEntityToPool(_bulletPool, entity, MAX_BULLET_POOL_SIZE);
            }
            else if (_activeCollectibles.Contains(entity))
            {
                _activeCollectibles.Remove(entity);
                ReturnEntityToPool(_collectiblePool, entity, MAX_COLLECTIBLE_POOL_SIZE);
            }
            else if (_activeEnemies.Contains(entity))
            {
                _activeEnemies.Remove(entity);
                ReturnEntityToPool(_enemyPool, entity, MAX_ENEMY_POOL_SIZE);
            }
        }

        private void ReturnEntityToPool(Queue<Entity> pool, Entity entity, int maxPoolSize)
        {
            // Only return to pool if pool size is less than max
            if (pool.Count < maxPoolSize)
            {
                pool.Enqueue(entity);
            }
            else
            {
                pool.Dequeue();  // Remove an old entity if pool is full
                pool.Enqueue(entity);
            }
        }

        public void SpawnBullet(BulletData bulletData, Vector2 position, int layer, Vector2 velocity = default, Entity owner = null)
        {
            Entity entity = null;

            // Reuse or create a new bullet entity
            if (_bulletPool.Count > 0)
            {
                entity = _bulletPool.Dequeue();
            }
            else
            {
                entity = _bulletFactory.CreateBullet(bulletData);
            }

            if (entity != null)
            {
                entity.GetComponent<HitboxComponent>().Layer = layer;
                _activeBullets.Add(entity);
                entity.Activate(position, velocity);
            }
        }

        public void SpawnEnemy(EnemyData enemyData, Vector2 position, Vector2 velocity = default)
        {
            Entity entity = null;

            // Reuse or create a new enemy entity
            if (_enemyPool.Count > 0)
            {
                entity = _enemyPool.Dequeue();
            }
            else
            {
                entity = _enemyFactory.CreateEnemy(enemyData);
            }

            if (entity != null)
            {
                _activeEnemies.Add(entity);
                entity.Activate(position, velocity);
            }
        }

        public void SpawnCollectible(CollectibleData collectibleData, Vector2 position, Vector2 velocity = default)
        {
            Entity entity = null;

            // Reuse or create a new collectible entity
            if (_collectiblePool.Count > 0)
            {
                entity = _collectiblePool.Dequeue();
            }
            else
            {
                entity = _collectibleFactory.CreateCollectible(collectibleData);
            }

            if (entity != null)
            {
                _activeCollectibles.Add(entity);
                entity.Activate(position, velocity);
            }
        }

        public void SetPlayerCharacter(PlayerData playerData)
        {
            this._playerCharacter = _playerFactory.CreatePlayer(playerData);
            _playerCharacter.Activate(new Vector2(Bounds.Width / 2, Bounds.Height - (Bounds.Height / 10)), Vector2.Zero);
        }

    }
}
