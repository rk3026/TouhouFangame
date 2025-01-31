using BulletHellGame.Entities;

namespace BulletHellGame.Managers
{
    public class PoolManager
    {
        private readonly Dictionary<string, Queue<Entity>> _entityPools = new();
        private readonly Dictionary<string, int> _maxPoolSizes = new();

        public PoolManager()
        {
            // Define maximum sizes for entity types
            _maxPoolSizes["Bullet"] = 300;
            _maxPoolSizes["Enemy"] = 100;
            _maxPoolSizes["Collectible"] = 100;
            _maxPoolSizes["Player"] = 5;
        }

        // Retrieves an entity from the pool or creates a new one if none exist
        public Entity GetEntity(string type, System.Func<Entity> createEntity)
        {
            if (_entityPools.TryGetValue(type, out Queue<Entity> pool) && pool.Count > 0)
            {
                Entity entity = pool.Dequeue();
                entity.Activate(Vector2.Zero, Vector2.Zero); // Reset entity
                return entity;
            }

            return createEntity();
        }

        // Returns an entity to the pool
        public void ReturnEntity(string type, Entity entity)
        {
            if (!_entityPools.ContainsKey(type))
                _entityPools[type] = new Queue<Entity>();

            if (_entityPools[type].Count < _maxPoolSizes[type])
            {
                entity.Deactivate();
                _entityPools[type].Enqueue(entity);
            }
        }
    }
}
