using BulletHellGame.DataAccess.DataTransferObjects;
using BulletHellGame.Logic.Builders;
using BulletHellGame.Logic.Components;
using BulletHellGame.Logic.Entities;
using System.Linq;

namespace BulletHellGame.Logic.Managers
{
    public class EntityManager
    {
        public Rectangle Bounds { get; set; }

        // For inactive entities:
        private readonly PoolManager _poolManager = new();

        // For storing active entities
        private readonly Dictionary<EntityType, List<Entity>> _activeEntities = new();
        private readonly Dictionary<Type, ISet<Entity>> _componentRegistry = new();

        // For entity creation:
        private readonly EntityDirector _entityDirector = new();
        private BossBuilder _bossBuilder = new();
        private BulletBuilder _bulletBuilder = new();
        private CollectibleBuilder _collectibleBuilder = new();
        private EnemyBuilder _enemyBuilder = new();
        private PlayerBuilder _playerBuilder = new();
        private SpawnerBuilder _spawnerBuilder = new();

        public EntityManager(Rectangle bounds)
        {
            Bounds = bounds;

            foreach (EntityType type in Enum.GetValues(typeof(EntityType)))
            {
                _activeEntities[type] = new List<Entity>();
            }
        }

        public delegate void Operation(Entity entity);
        public void OperateOnEntities(Operation op, params Type[] componentTypes)
        {
            var entities = GetEntitiesWithComponents(componentTypes);

            foreach (var entity in entities)
            {
                op(entity);
            }
        }

        // For getting entities with a combination of 1 or more components
        public List<Entity> GetEntitiesWithComponents(params Type[] componentTypes)
        {
            if (!_componentRegistry.TryGetValue(componentTypes.First(), out var entitiesWithAllComponents))
                return new List<Entity>();

            // Create a new set to avoid modifying the original
            var resultSet = new HashSet<Entity>(entitiesWithAllComponents);

            foreach (var componentType in componentTypes.Skip(1))
            {
                if (!_componentRegistry.TryGetValue(componentType, out var componentSet))
                    return new List<Entity>();

                resultSet.IntersectWith(componentSet);
            }

            return resultSet.ToList();
        }

        public int GetEntityCount(EntityType type)
        {
            if (type == EntityType.Enemy)
            {
                return _activeEntities[EntityType.Enemy].Count + _activeEntities[EntityType.Boss].Count; // Boss are considered as enemies too.
            }

            return _activeEntities[type].Count;
        }

        public int TotalEntityCount => _activeEntities.Values.Sum(list => list.Count);

        public void QueueEntityForRemoval(Entity entity)
        {
            if (entity == null) return;

            entity.Deactivate();

            foreach (var entityType in _activeEntities)
            {
                if (entityType.Value.Remove(entity))
                {
                    _poolManager.ReturnEntity(entityType.Key, entity);
                    break;  // Exit early once entity is found and removed
                }
            }

            // Also remove from component registry
            foreach (var component in entity.GetComponents())
            {
                UnregisterComponent(component.GetType(), entity);
            }
        }

        public void SpawnBullet(BulletData bulletData, Vector2 position, int layer, Vector2 velocity = default, Entity owner = null)
        {
            Entity bullet;

            if (_poolManager.TryGetPooledEntity(EntityType.Bullet, out bullet))
            {
                _bulletBuilder.ApplyBulletData(bullet, bulletData);
                bullet.GetComponent<HitboxComponent>().Layer = layer;
                bullet.GetComponent<OwnerComponent>().Owner = owner;
                bullet.GetComponent<SpriteComponent>().CurrentRotation = (float)Math.Atan2(velocity.Y, velocity.X) + (float)Math.PI / 2;

            }
            else
            {
                _bulletBuilder.SetEntityData(bulletData);
                _entityDirector.ConstructEntity(_bulletBuilder);
                bullet = _bulletBuilder.GetResult();
                bullet.AddComponent(new OwnerComponent(owner));
                bullet.GetComponent<HitboxComponent>().Layer = layer;
                bullet.GetComponent<SpriteComponent>().CurrentRotation = (float)Math.Atan2(velocity.Y, velocity.X)+(float)Math.PI/2;
            }

            SpawnEntity(EntityType.Bullet, bullet, position, velocity);
        }

        public Entity SpawnEnemy(GruntData enemyData, Vector2 position, Vector2 velocity = default)
        {
            // Create the enemy entity
            _enemyBuilder.SetEntityData(enemyData);
            _entityDirector.ConstructEntity(_enemyBuilder);
            Entity enemy = _enemyBuilder.GetResult();
            SpawnEntity(EntityType.Enemy, enemy, position, velocity);

            // Create a spawner for the enemy's weapons
            if (enemyData.Weapons != null && enemyData.Weapons.Count > 0)
            {
                SpawnerData enemySpawnerData = new SpawnerData(enemyData.Weapons, enemy, string.Empty);
                _spawnerBuilder.SetEntityData(enemySpawnerData);
                _entityDirector.ConstructEntity(_spawnerBuilder);
                Entity enemySpawner = _spawnerBuilder.GetResult();
                SpawnEntity(EntityType.Spawner, enemySpawner, position, velocity);
            }

            return enemy;
        }

        public void SpawnPlayer(CharacterData playerData)
        {
            // Create the player entity
            Vector2 playerStartPosition = new Vector2(Bounds.Width / 2, Bounds.Height - Bounds.Height / 10);
            _playerBuilder.SetEntityData(playerData);
            _entityDirector.ConstructEntity(_playerBuilder);
            Entity player = _playerBuilder.GetResult();
            SpawnEntity(EntityType.Player, player, playerStartPosition, Vector2.Zero);

            // Create the main spawner for the player
            SpawnerData playerSpawnerData = new SpawnerData(playerData.ShotTypes.First().UnfocusedShot.PowerLevels[0].MainWeapons, player, string.Empty);
            _spawnerBuilder.SetEntityData(playerSpawnerData);
            _entityDirector.ConstructEntity(_spawnerBuilder);
            Entity playerSpawner = _spawnerBuilder.GetResult();
            SpawnEntity(EntityType.Spawner, playerSpawner, playerStartPosition, Vector2.Zero);

            // Create additional spawners that were previously "Options"
            var powerLevelData = playerData.ShotTypes.First().UnfocusedShot.PowerLevels.FirstOrDefault().Value;
            for (int i = 0; i < powerLevelData.Options.Count; i++) // At first, set them to power level 0
            {
                // Create a spawner that replaces the old "Option"
                SpawnerData optionSpawnerData = powerLevelData.Options[i];
                optionSpawnerData.Owner = player;
                _spawnerBuilder.SetEntityData(optionSpawnerData);
                _entityDirector.ConstructEntity(_spawnerBuilder);
                Entity optionSpawner = _spawnerBuilder.GetResult();
                SpawnEntity(EntityType.Spawner, optionSpawner, optionSpawnerData.Offset, Vector2.Zero);
            }
        }

        public Entity SpawnBoss(BossData bossData, Vector2 position)
        {
            // Create the boss entity
            _bossBuilder.SetEntityData(bossData);
            _entityDirector.ConstructEntity(_bossBuilder);
            Entity boss = _bossBuilder.GetResult();
            if (boss != null)
            {
                _activeEntities[EntityType.Boss].Add(boss);
                boss.Activate(position, Vector2.Zero);

                // Register components for the boss
                foreach (var component in boss.GetComponents())
                {
                    RegisterComponent(component.GetType(), boss);
                }

                // Create a spawner for the boss's weapons
                if (bossData.Weapons != null && bossData.Weapons.Count > 0)
                {
                    SpawnerData bossSpawnerData = new SpawnerData(bossData.Weapons, boss, string.Empty);
                    _spawnerBuilder.SetEntityData(bossSpawnerData);
                    _entityDirector.ConstructEntity(_spawnerBuilder);
                    Entity bossSpawner = _spawnerBuilder.GetResult();
                    SpawnEntity(EntityType.Spawner, bossSpawner, position, Vector2.Zero);
                }
            }
            return boss;
        }


        public void SpawnCollectible(CollectibleData collectibleData, Vector2 position, Vector2 velocity = default)
        {
            _collectibleBuilder.SetEntityData(collectibleData);
            _entityDirector.ConstructEntity(_collectibleBuilder);
            Entity collectible = _collectibleBuilder.GetResult();

            SpawnEntity(EntityType.Collectible, collectible, position, velocity);
        }

        private void SpawnEntity(EntityType type, Entity entity, Vector2 position, Vector2 velocity = default)
        {
            if (entity == null) return;

            _activeEntities[type].Add(entity);
            entity.Activate(position, velocity);

            foreach (var component in entity.GetComponents())
            {
                RegisterComponent(component.GetType(), entity);
            }

            entity.OnComponentAdded += (component) => RegisterComponent(component.GetType(), entity);
            entity.OnComponentRemoved += (component) => UnregisterComponent(component.GetType(), entity);
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
