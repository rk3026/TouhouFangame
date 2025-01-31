using BulletHellGame.Components;
using BulletHellGame.Data.DataTransferObjects;
using BulletHellGame.Entities;
using BulletHellGame.Factories;
using System.Linq;

namespace BulletHellGame.Managers
{
    public class EntityManager
    {
        public Rectangle Bounds { get; set; }

        private readonly Dictionary<EntityType, List<Entity>> _entities = new();
        private readonly Dictionary<Type, HashSet<Entity>> _componentRegistry = new(); // Stores components and the entities that have it together.

        // Factories for entity creation
        private readonly EnemyFactory _enemyFactory;
        private readonly BulletFactory _bulletFactory;
        private readonly CollectibleFactory _collectibleFactory;
        private readonly PlayerFactory _playerFactory;
        private readonly BossFactory _bossFactory;

        public EntityManager(Rectangle bounds)
        {
            Bounds = bounds;
            _enemyFactory = new EnemyFactory();
            _bulletFactory = new BulletFactory();
            _collectibleFactory = new CollectibleFactory();
            _playerFactory = new PlayerFactory();
            _bossFactory = new BossFactory();

            // Initialize entity lists
            foreach (EntityType type in Enum.GetValues(typeof(EntityType)))
            {
                _entities[type] = new List<Entity>();
            }
        }

        // Used to quickly get all entities that exist with a certain component.
        public HashSet<Entity> GetEntitiesWithComponent<T>() where T : IComponent
        {
            if (_componentRegistry.ContainsKey(typeof(T)))
            {
                return _componentRegistry[typeof(T)];
            }

            return new HashSet<Entity>();
        }

        // Getting entities with 1 component type
        public HashSet<Entity> GetEntitiesWithComponent(Type componentType)
        {
            if (_componentRegistry.ContainsKey(componentType))
            {
                return _componentRegistry[componentType];
            }
            return new HashSet<Entity>();
        }


        // For getting entities with a combination of 2 or more components
        public HashSet<Entity> GetEntitiesWithComponents(params Type[] componentTypes)
        {
            // Start by getting the entities that have the first component type
            var entitiesWithAllComponents = GetEntitiesWithComponent(componentTypes[0]);

            // Iterate over the remaining component types and intersect the entities that have them
            foreach (var componentType in componentTypes.Skip(1))
            {
                entitiesWithAllComponents = entitiesWithAllComponents.Intersect(GetEntitiesWithComponent(componentType)).ToHashSet();
            }

            return entitiesWithAllComponents;
        }


        public int GetEntityCount(EntityType type)
        {
            if (type == EntityType.Enemy)
            {
                return _entities[EntityType.Enemy].Count + _entities[EntityType.Boss].Count; // Bosses are considered as enemies too.
            }

            return _entities[type].Count;
        }

        public int TotalEntityCount => _entities.Values.Sum(list => list.Count);

        public List<Entity> GetActiveEntities() => _entities.Values.SelectMany(e => e).ToList();

        public void QueueEntityForRemoval(Entity entity)
        {
            if (entity == null) return;

            entity.Deactivate();

            foreach (var entry in _entities)
            {
                if (entry.Value.Remove(entity))
                    break;  // Exit early once entity is found and removed
            }

            // Also remove from component registry
            foreach (var component in entity.GetComponents())
            {
                UnregisterComponent(component.GetType(), entity);
            }
        }

        private void SpawnEntity(EntityType type, Entity entity, Vector2 position, Vector2 velocity = default)
        {
            if (entity == null) return;

            _entities[type].Add(entity);
            entity.Activate(position, velocity);

            // Register components for this entity
            foreach (var component in entity.GetComponents())
            {
                RegisterComponent(component.GetType(), entity);
            }
        }

        public void SpawnBullet(BulletData bulletData, Vector2 position, int layer, Vector2 velocity = default, Entity owner = null)
        {
            Entity bullet = _bulletFactory.CreateBullet(bulletData);
            if (bullet != null)
            {
                bullet.GetComponent<HitboxComponent>().Layer = layer;
                SpawnEntity(EntityType.Bullet, bullet, position, velocity);
            }
        }

        public void SpawnEnemy(EnemyData enemyData, Vector2 position, Vector2 velocity = default)
        {
            SpawnEntity(EntityType.Enemy, _enemyFactory.CreateEnemy(enemyData), position, velocity);
        }

        public void SpawnCollectible(CollectibleData collectibleData, Vector2 position, Vector2 velocity = default)
        {
            SpawnEntity(EntityType.Collectible, _collectibleFactory.CreateCollectible(collectibleData), position, velocity);
        }

        public void SpawnPlayer(PlayerData playerData)
        {
            Vector2 playerStartPosition = new Vector2(Bounds.Width / 2, Bounds.Height - (Bounds.Height / 10));
            SpawnEntity(EntityType.Player, _playerFactory.CreatePlayer(playerData), playerStartPosition, Vector2.Zero);
        }

        public void SpawnBoss(BossData bossData, Vector2 position)
        {
            Entity boss = _bossFactory.CreateBoss(bossData);
            if (boss != null)
            {
                _entities[EntityType.Boss].Add(boss);
                boss.Activate(position, Vector2.Zero);

                // Register components for the boss
                foreach (var component in boss.GetComponents())
                {
                    RegisterComponent(component.GetType(), boss);
                }
            }
        }

        private void RegisterComponent(Type componentType, Entity entity)
        {
            if (!_componentRegistry.ContainsKey(componentType))
            {
                _componentRegistry[componentType] = new HashSet<Entity>();
            }

            _componentRegistry[componentType].Add(entity);
        }

        private void UnregisterComponent(Type componentType, Entity entity)
        {
            if (_componentRegistry.ContainsKey(componentType))
            {
                _componentRegistry[componentType].Remove(entity);
            }
        }
    }
}
