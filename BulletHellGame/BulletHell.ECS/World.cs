namespace BulletHell.ECS
{
    /// <summary>
    /// Represents the game world, which contains entities and their components.
    /// </summary>
    public class World
    {
        private Dictionary<int, Archetype> _archetypes = new(); // Key: Bitmask, Value: Archetype
        private Dictionary<Type, object> componentStorages = new(); // Key: ComponentType, Value: ComponentStorage object
        private Dictionary<int, int> _entityComponentMasks = new();  // Key: entityId, Value: Bitmask
        private int nextEntityId = 0; // Next available entity ID to assign to a newly created entity
        private Queue<int> availableEntityIds = new();  // Queue for reused entity IDs

        public static World Create() => new World();

        /// <summary>
        /// Creates an entity with the specified components and returns its ID in the world.
        /// </summary>
        /// <param name="components"></param>
        /// <returns></returns>
        public int CreateEntity(params object[] components)
        {
            int entityId;
            if (availableEntityIds.Count > 0)
            {
                // Reuse an available ID from the deleted entities
                entityId = availableEntityIds.Dequeue();
            }
            else
            {
                // Otherwise, use the next available ID
                entityId = nextEntityId++;
            }

            foreach (var component in components)
            {
                AddComponent(entityId, component);
            }

            // Update archetype based on the entity's component mask
            UpdateEntityArchetype(entityId);
            return entityId;
        }

        public void DestroyEntity(int entityId)
        {
            if (_entityComponentMasks.ContainsKey(entityId))
            {
                foreach (var kvp in componentStorages)
                {
                    var storage = kvp.Value;
                    var removeMethod = storage.GetType().GetMethod("Remove");
                    removeMethod.Invoke(storage, new object[] { entityId });
                }
                _entityComponentMasks.Remove(entityId);

                foreach (var archetype in _archetypes.Values)
                {
                    archetype.RemoveEntity(entityId);
                }

                // Reuse the entity ID
                availableEntityIds.Enqueue(entityId);
            }
        }

        /// <summary>
        /// Adds a component to an entity.
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="component"></param>
        public void AddComponent(int entityId, object component)
        {
            var componentType = component.GetType();

            // Ensure the component storage exists
            if (!componentStorages.ContainsKey(componentType))
            {
                var storageType = typeof(ComponentStorage<>).MakeGenericType(componentType);
                componentStorages[componentType] = Activator.CreateInstance(storageType);
            }

            // Add component to storage
            var storage = componentStorages[componentType];
            var addMethod = storage.GetType().GetMethod("Add");
            addMethod.Invoke(storage, new object[] { entityId, component });

            // Update the entity's component mask directly
            var componentIndex = ComponentRegistry.GetComponentIndex(componentType);
            if (_entityComponentMasks.ContainsKey(entityId))
            {
                _entityComponentMasks[entityId] |= (1 << componentIndex);  // Set the appropriate bit for the added component
            }
            else
            {
                _entityComponentMasks[entityId] = (1 << componentIndex);  // Initialize with the component's bit
            }

            // Recompute the entity's archetype based on the updated component mask
            UpdateEntityArchetype(entityId);
        }

        /// <summary>
        /// Removes a component from an entity.
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="componentType"></param>
        public void RemoveComponent(int entityId, Type componentType)
        {
            if (!componentStorages.ContainsKey(componentType)) return;

            var storage = componentStorages[componentType];
            var removeMethod = storage.GetType().GetMethod("Remove");
            removeMethod.Invoke(storage, new object[] { entityId });

            // Update the entity's component mask directly
            var componentIndex = ComponentRegistry.GetComponentIndex(componentType);
            if (_entityComponentMasks.ContainsKey(entityId))
            {
                _entityComponentMasks[entityId] &= ~(1 << componentIndex);  // Clear the bit for the removed component
            }

            // Recompute the entity's archetype based on the updated component mask
            UpdateEntityArchetype(entityId);
        }

        /// <summary>
        /// Queries the world for entities that match the specified query (what components they have).
        /// </summary>
        /// <param name="query"></param>
        /// <param name="queryAction"></param>
        public void Query(QueryDescription query, Action<int, object[]> queryAction)
        {
            foreach (var archetype in _archetypes.Values)
            {
                // Check if the archetype matches the query
                if (MatchesQuery(archetype.Bitmask, query))
                {
                    foreach (var entityId in archetype.EntityIDs)
                    {
                        var components = GetComponentsForEntity(entityId, query.RequiredTypes).ToArray();
                        queryAction(entityId, components);
                    }
                }
            }
        }

        public IEnumerable<(int entityId, object[] components)> Query(QueryDescription query)
        {
            var results = new List<(int, object[])>();

            foreach (var archetype in _archetypes.Values)
            {
                // Check if the archetype matches the query
                if (MatchesQuery(archetype.Bitmask, query))
                {
                    foreach (var entityId in archetype.EntityIDs)
                    {
                        // Get components for this entity based on the query
                        var components = GetComponentsForEntity(entityId, query.RequiredTypes).ToArray();
                        results.Add((entityId, components));
                    }
                }
            }

            return results;
        }

        private void UpdateEntityArchetype(int entityId)
        {
            int componentMask = GetEntityComponentMask(entityId);

            // Check if archetype already exists
            if (!_archetypes.ContainsKey(componentMask))
            {
                _archetypes[componentMask] = new Archetype(componentMask);
            }

            var archetype = _archetypes[componentMask];

            // Remove the entity from any existing archetype
            foreach (var existingArchetype in _archetypes.Values)
            {
                existingArchetype.RemoveEntity(entityId);
            }

            // Add the entity to the new archetype
            archetype.AddEntity(entityId);
        }

        private int GetEntityComponentMask(int entityId)
        {
            // Directly return the component mask for the entity from the dictionary
            if (_entityComponentMasks.ContainsKey(entityId))
            {
                return _entityComponentMasks[entityId];
            }
            else
            {
                return 0;  // Return 0 if no components are assigned
            }
        }

        private bool MatchesQuery(int archetypeMask, QueryDescription query)
        {
            foreach (var requiredType in query.RequiredTypes)
            {
                int componentIndex = ComponentRegistry.GetComponentIndex(requiredType);
                if ((archetypeMask & (1 << componentIndex)) == 0)
                    return false;
            }
            return true;
        }

        private IEnumerable<object> GetComponentsForEntity(int entityId, IReadOnlyList<Type> componentTypes)
        {
            foreach (var componentType in componentTypes)
            {
                yield return GetComponent(entityId, componentType);
            }
        }

        private object GetComponent(int entityId, Type componentType)
        {
            var storage = componentStorages[componentType];
            var getMethod = storage.GetType().GetMethod("Get");
            return getMethod.Invoke(storage, new object[] { entityId });
        }
    }
}
