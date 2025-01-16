using BulletHellGame.Entities;
using BulletHellGame.Factories;
using System.Linq;
using BulletHellGame.Entities.Bullets;
using BulletHellGame.Entities.Enemies;
using BulletHellGame.Components;

namespace BulletHellGame.Managers
{
    // TODO: make specific manager classes for each type.
    public class EntityManager
    {
        private static EntityManager _instance;
        public static EntityManager Instance => _instance ??= new EntityManager();
        private readonly List<Entity> _generalEntities;
        private readonly List<Bullet> _bullets = new();
        private readonly List<Enemy> _enemies = new();
        private readonly List<Collectible> _collectibles = new();

        private readonly BulletFactory _bulletFactory;
        private readonly EnemyFactory _enemyFactory;

        private EntityManager()
        {
            _bulletFactory = new BulletFactory();
            _enemyFactory = new EnemyFactory();
        }

        // Add entity to the appropriate list
        public void AddEntity(Entity entity)
        {
            switch (entity)
            {
                case Bullet bullet:
                    _bullets.Add(bullet);
                    break;
                case Enemy enemy:
                    _enemies.Add(enemy);
                    break;
                case Collectible collectible:
                    _collectibles.Add(collectible);
                    break;
                default:
                    _generalEntities.Add(entity);
                    break;
            }
        }

        // Remove entity from the appropriate list
        public void RemoveEntity(Entity entity)
        {
            switch (entity)
            {
                case Bullet bullet:
                    _bullets.Remove(bullet);
                    break;
                case Enemy enemy:
                    _enemies.Remove(enemy);
                    break;
                case Collectible collectible:
                    _collectibles.Remove(collectible);
                    break;
                default:
                    _generalEntities.Remove(entity);
                    break;
            }
        }

        public void Update(GameTime gameTime)
        {
            // Update bullets
            foreach (var bullet in _bullets.ToList()) // Use ToList() to avoid modification during iteration
            {
                bullet.Update(gameTime);
                // Remove the bullet if it goes off-screen
                if (bullet.Position.Y < 0 || bullet.Position.Y > Globals.WindowSize.Y ||
                bullet.Position.X < 0 || bullet.Position.X > Globals.WindowSize.X)
                {
                    _bullets.Remove(bullet);
                }
            }

            // Update enemies
            foreach (var enemy in _enemies.ToList())
            {
                enemy.Update(gameTime);
                if (enemy.GetComponent<HealthComponent>().CurrentHealth <= 0) {
                    _enemies.Remove(enemy);
                }
            }

            // Update power-ups
            foreach (var collectible in _collectibles.ToList())
            {
                collectible.Update(gameTime);
            }

            // Update general entities
            foreach (var entity in _generalEntities.ToList())
            {
                entity.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw bullets
            foreach (var bullet in _bullets)
            {
                bullet.Draw(spriteBatch);
            }

            // Draw enemies
            foreach (var enemy in _enemies)
            {
                enemy.Draw(spriteBatch);
            }

            // Draw power-ups
            foreach (var collectible in _collectibles)
            {
                collectible.Draw(spriteBatch);
            }

            // Draw general entities
            foreach (var entity in _generalEntities)
            {
                entity.Draw(spriteBatch);
            }
        }

        // Factory integration
        public Bullet CreateBullet(BulletType type, Vector2 position, Vector2 velocity)
        {
            var bullet = _bulletFactory.CreateBullet(type, position, velocity);
            AddEntity(bullet);
            return bullet;
        }

        public Enemy CreateEnemy(EnemyType type, Vector2 position)
        {
            var enemy = _enemyFactory.CreateEnemy(type, position);
            AddEntity(enemy);
            return enemy;
        }
    }
}
