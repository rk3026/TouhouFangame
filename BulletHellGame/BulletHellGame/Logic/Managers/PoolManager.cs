using BulletHellGame.Logic.Entities;

namespace BulletHellGame.Logic.Managers
{
    public class PoolManager
    {
        private readonly Dictionary<EntityType, Queue<Entity>> _entityPools = new();
        private readonly Dictionary<EntityType, int> _maxPoolSizes = new();

        public PoolManager()
        {
            // Define maximum sizes for entity types
            _maxPoolSizes[EntityType.Boss] = 0; //3
            _maxPoolSizes[EntityType.Bullet] = 1000;
            _maxPoolSizes[EntityType.Collectible] = 0; //100
            _maxPoolSizes[EntityType.Enemy] = 0; //100
            _maxPoolSizes[EntityType.Option] = 0; //10
            _maxPoolSizes[EntityType.Player] = 0; //5
        }

        // Retrieves an entity from the pool
        public bool TryGetPooledEntity(EntityType type, out Entity entity)
        {
            if (_entityPools.TryGetValue(type, out Queue<Entity> pool) && pool.Count > 0)
            {
                entity = pool.Dequeue();
                return true;
            }

            entity = null;
            return false;
        }


        // Returns an entity to the pool
        public void ReturnEntity(EntityType type, Entity entity)
        {
            if (!_entityPools.ContainsKey(type))
                _entityPools[type] = new Queue<Entity>();

            if (_entityPools[type].Count < _maxPoolSizes[type])
            {
                _entityPools[type].Enqueue(entity);
            }
        }
    }
}
