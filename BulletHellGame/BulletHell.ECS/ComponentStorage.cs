namespace BulletHell.ECS
{
    /// <summary>
    /// Uses a dense array structure to store the components of a specific type for entities that have it.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class ComponentStorage<T> where T : struct
    {
        private List<T> _components = new();  // Store components in a list (vector)
        private Dictionary<int, int> _entityToIndex = new(); // Map entityId to index in list

        public void Add(int entityId, T component)
        {
            // Add component to list and map the entityId to the index in the list
            _components.Add(component);
            _entityToIndex[entityId] = _components.Count - 1;  // New index is the last position
        }

        public void Remove(int entityId)
        {
            if (_entityToIndex.ContainsKey(entityId))
            {
                // Get the index of the component to remove
                int index = _entityToIndex[entityId];

                // Swap the element with the last element to maintain a contiguous list
                _components[index] = _components[_components.Count - 1];
                _components.RemoveAt(_components.Count - 1);

                // If the list is not empty, update the dictionary for the moved element
                if (_components.Count > 0)
                {
                    // Get the entityId for the last moved component
                    var lastEntityId = _entityToIndex.First(kvp => kvp.Value == _components.Count).Key;

                    // Update the dictionary with the new index for the last moved component
                    _entityToIndex[lastEntityId] = index;
                }

                // Remove the entityId entry from the dictionary
                _entityToIndex.Remove(entityId);
            }
        }

        public T Get(int entityId)
        {
            if (!_entityToIndex.ContainsKey(entityId))
                throw new InvalidOperationException($"Entity {entityId} does not have a {typeof(T).Name}.");

            // Get the index of the component in the _components array
            int index = _entityToIndex[entityId];

            // Return the component at that index
            return _components[index];
        }

        public IEnumerable<(int entityId, T component)> GetAll()
        {
            foreach (var kvp in _entityToIndex)
            {
                yield return (kvp.Key, _components[kvp.Value]);
            }
        }
    }
}
