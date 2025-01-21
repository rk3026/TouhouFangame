using BulletHellGame.Components;
using BulletHellGame.Entities.Bullets;
using BulletHellGame.Entities.Characters.Enemies;
using BulletHellGame.Entities.Collectibles;
using BulletHellGame.Entities;
using BulletHellGame.Factories;
using System.Linq;

namespace BulletHellGame.Managers
{
    public class EntityManager
    {
        private static EntityManager _instance;
        public static EntityManager Instance => _instance ??= new EntityManager();

        // Maximum pool sizes
        private const int MAX_BULLET_POOL_SIZE = 100;
        private const int MAX_ENEMY_POOL_SIZE = 50;
        private const int MAX_COLLECTIBLE_POOL_SIZE = 30;

        // Factories
        private readonly BulletFactory _bulletFactory;
        private readonly EnemyFactory _enemyFactory;
        private readonly CollectibleFactory _collectibleFactory;

        // Pools for reusable entities
        private readonly Queue<Bullet> _bulletPool = new();
        private readonly Queue<Enemy> _enemyPool = new();
        private readonly Queue<Collectible> _collectiblePool = new();

        // Active entities in the game world
        private readonly List<Bullet> _activeBullets = new();
        private readonly List<Enemy> _activeEnemies = new();
        private readonly List<Collectible> _activeCollectibles = new();
        private PlayableCharacter _playerCharacter;

        private EntityManager()
        {
            _bulletFactory = new BulletFactory();
            _enemyFactory = new EnemyFactory();
            _collectibleFactory = new CollectibleFactory();
        }

        public IEnumerable<Entity> GetActiveEntities()
        {
            return _activeBullets.ToList()
                .Concat<Entity>(_activeEnemies.ToList())
                .Concat<Entity>(_activeCollectibles.ToList());
        }

        private void AddToActiveEntities(Entity entity)
        {
            switch (entity)
            {
                case Bullet bullet:
                    _activeBullets.Add(bullet);
                    break;
                case Enemy enemy:
                    _activeEnemies.Add(enemy);
                    break;
                case Collectible collectible:
                    _activeCollectibles.Add(collectible);
                    break;
            }
        }

        private void AddToPool(Entity entity)
        {
            switch (entity)
            {
                case Bullet bullet:
                    if (_bulletPool.Count >= MAX_BULLET_POOL_SIZE)
                    {
                        _bulletPool.Dequeue(); // Remove the oldest bullet
                    }
                    _bulletPool.Enqueue(bullet);
                    break;

                case Enemy enemy:
                    if (_enemyPool.Count >= MAX_ENEMY_POOL_SIZE)
                    {
                        _enemyPool.Dequeue(); // Remove the oldest enemy
                    }
                    _enemyPool.Enqueue(enemy);
                    break;

                case Collectible collectible:
                    if (_collectiblePool.Count >= MAX_COLLECTIBLE_POOL_SIZE)
                    {
                        _collectiblePool.Dequeue(); // Remove the oldest collectible
                    }
                    _collectiblePool.Enqueue(collectible);
                    break;
            }
        }

        private void RemoveFromActiveEntities(Entity entity)
        {
            switch (entity)
            {
                case Bullet bullet:
                    _activeBullets.Remove(bullet);
                    break;
                case Enemy enemy:
                    _activeEnemies.Remove(enemy);
                    break;
                case Collectible collectible:
                    _activeCollectibles.Remove(collectible);
                    break;
            }
        }

        public void QueueEntityForRemoval(Entity entity)
        {
            // Remove the entity from active entities and add it to the pool
            RemoveFromActiveEntities(entity);
            AddToPool(entity);
        }

        public void Update(GameTime gameTime)
        {
            // Update active bullets
            foreach (var bullet in _activeBullets.ToList())
            {
                bullet.Update(gameTime);
                if (bullet.Position.Y < 0 || bullet.Position.Y > Globals.WindowSize.Y ||
                    bullet.Position.X < 0 || bullet.Position.X > Globals.WindowSize.X)
                {
                    QueueEntityForRemoval(bullet);
                }
            }

            // Update active enemies
            foreach (var enemy in _activeEnemies.ToList())
            {
                enemy.Update(gameTime);
                if (enemy.GetComponent<HealthComponent>().CurrentHealth <= 0)
                {
                    QueueEntityForRemoval(enemy);
                }
            }

            // Update active collectibles
            foreach (var collectible in _activeCollectibles.ToList())
            {
                collectible.Update(gameTime);
            }

            // Update the player character
            _playerCharacter?.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw active entities
            foreach (var bullet in _activeBullets)
            {
                bullet.Draw(spriteBatch);
            }
            foreach (var enemy in _activeEnemies)
            {
                enemy.Draw(spriteBatch);
            }
            foreach (var collectible in _activeCollectibles)
            {
                collectible.Draw(spriteBatch);
            }

            _playerCharacter?.Draw(spriteBatch);
        }

        public Bullet CreateBullet(BulletType type, Vector2 position, Vector2 velocity)
        {
            Bullet bullet = null;

            // Check the pool for a bullet of the correct type
            if (_bulletPool.Count > 0)
            {
                var poolCount = _bulletPool.Count;
                for (int i = 0; i < poolCount; i++)
                {
                    var pooledBullet = _bulletPool.Dequeue();
                    // Check if the bullet type matches
                    if (pooledBullet.BulletType == type)
                    {
                        bullet = pooledBullet;
                        break;
                    }
                    else
                    {
                        // Put the bullet back in the pool if it doesn't match
                        _bulletPool.Enqueue(pooledBullet);
                    }
                }
            }

            // If no matching bullet was found, create a new one
            if (bullet == null)
            {
                bullet = _bulletFactory.CreateBullet(type, position, velocity);
            }

            // Reset the bullet and add it to the active list
            bullet.Reset(position, velocity);
            AddToActiveEntities(bullet);
            return bullet;
        }

        public Enemy CreateEnemy(EnemyType type, Vector2 position)
        {
            Enemy enemy;
            if (_enemyPool.Count > 0)
            {
                enemy = _enemyPool.Dequeue();
                enemy.Reset(position, new Vector2(0,0));
            }
            else
            {
                enemy = _enemyFactory.CreateEnemy(type, position, new Vector2(0, 0));
            }
            AddToActiveEntities(enemy);
            return enemy;
        }

        public Collectible CreateCollectible(CollectibleType type, Vector2 position, Vector2 velocity)
        {
            Collectible collectible;
            if (_collectiblePool.Count > 0)
            {
                collectible = _collectiblePool.Dequeue();
                collectible.Reset(position, new Vector2(0,0));
            }
            else
            {
                collectible = _collectibleFactory.CreateCollectible(type, position, velocity);
            }
            AddToActiveEntities(collectible);
            return collectible;
        }

        public void SetPlayerCharacter(PlayableCharacter character)
        {
            _playerCharacter = character;
        }
    }
}
