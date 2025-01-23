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
            return _activeBullets
                .Where(b => b.IsActive)
                .Concat<Entity>(_activeEnemies.Where(e => e.IsActive))
                .Concat<Entity>(_activeCollectibles.Where(c => c.IsActive))
                .Concat<Entity>(new[] { _playerCharacter }.Where(pc => pc?.IsActive == true)); // Filter active player
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
            entity.Deactivate();
            RemoveFromActiveEntities(entity);
            AddToPool(entity);
        }

        public void Update(GameTime gameTime)
        {
            HitboxManager.Instance.Update(gameTime);

            // Update active bullets
            foreach (var bullet in _activeBullets.Where(b => b.IsActive).ToList())
            {
                bullet.Update(gameTime);
                if (bullet.Position.Y < 0 || bullet.Position.Y > Globals.WindowSize.Y ||
                    bullet.Position.X < 0 || bullet.Position.X > Globals.WindowSize.X)
                {
                    QueueEntityForRemoval(bullet);
                }
            }

            // Update active enemies
            foreach (var enemy in _activeEnemies.Where(e => e.IsActive).ToList())
            {
                enemy.Update(gameTime);

                // Handle movement and screen boundary behavior
                var movementComponent = enemy.GetComponent<MovementComponent>();
                if (enemy.Position.Y < 0 || enemy.Position.Y > Globals.WindowSize.Y)
                {
                    movementComponent.Velocity = new Vector2(movementComponent.Velocity.X, -movementComponent.Velocity.Y);
                }
                if (enemy.Position.X < 0 || enemy.Position.X > Globals.WindowSize.X)
                {
                    movementComponent.Velocity = new Vector2(-movementComponent.Velocity.X, movementComponent.Velocity.Y);
                }

                // Remove if health is <= 0
                if (enemy.GetComponent<HealthComponent>().CurrentHealth <= 0)
                {
                    QueueEntityForRemoval(enemy);
                }
            }

            // Update active collectibles
            foreach (var collectible in _activeCollectibles.Where(c => c.IsActive).ToList())
            {
                collectible.Update(gameTime);
            }

            // Update the player character
            if (_playerCharacter?.IsActive == true)
            {
                _playerCharacter.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw active bullets
            foreach (var bullet in _activeBullets.Where(b => b.IsActive))
            {
                bullet.Draw(spriteBatch);
            }

            // Draw active enemies
            foreach (var enemy in _activeEnemies.Where(e => e.IsActive))
            {
                enemy.Draw(spriteBatch);
            }

            // Draw active collectibles
            foreach (var collectible in _activeCollectibles.Where(c => c.IsActive))
            {
                collectible.Draw(spriteBatch);
            }

            // Draw the player character if active
            if (_playerCharacter?.IsActive == true)
            {
                _playerCharacter.Draw(spriteBatch);
            }
        }

        public Bullet SpawnBullet(BulletType type, Vector2 position, Vector2 velocity)
        {
            Bullet bullet = null;

            // Retrieve from the pool if available
            if (_bulletPool.Count > 0)
            {
                bullet = _bulletPool.Dequeue();
            }
            else
            {
                bullet = _bulletFactory.CreateBullet(type);
            }

            bullet.Activate(position, velocity);
            AddToActiveEntities(bullet);
            return bullet;
        }

        public Enemy SpawnEnemy(EnemyType type, Vector2 position)
        {
            Enemy enemy;
            if (_enemyPool.Count > 0)
            {
                enemy = _enemyPool.Dequeue();
            }
            else
            {
                enemy = _enemyFactory.CreateEnemy(type);
            }

            enemy.Activate(position, new Vector2(0,0)); // Spawn the enemy with 0 velocity at start
            AddToActiveEntities(enemy);
            return enemy;
        }

        public Collectible SpawnCollectible(CollectibleType type, Vector2 position, Vector2 velocity)
        {
            Collectible collectible;
            if (_collectiblePool.Count > 0)
            {
                collectible = _collectiblePool.Dequeue();
            }
            else
            {
                collectible = _collectibleFactory.CreateCollectible(type);
            }

            collectible.Activate(position, velocity);
            AddToActiveEntities(collectible);
            return collectible;
        }

        public void SetPlayerCharacter(PlayableCharacter character)
        {
            _playerCharacter = character;
            if (_playerCharacter != null)
            {
                _playerCharacter.Activate(new Vector2(Globals.WindowSize.X / 2, Globals.WindowSize.Y / 2), new Vector2(0, 0));
            }
        }
    }
}
