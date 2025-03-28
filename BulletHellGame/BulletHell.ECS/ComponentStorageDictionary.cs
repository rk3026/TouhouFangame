using System.Runtime.InteropServices;

namespace BulletHell.ECS
{
    /// <summary>
    /// Uses a dictionary (unordered map) structure as storage for the components of a specific type for entities that have it.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class ComponentStorageDictionary<T> where T : struct
    {
        private Dictionary<int, T> _components = new();

        public void Add(int entityId, T component)
        {
            _components[entityId] = component;
        }

        public void Remove(int entityId)
        {
            _components.Remove(entityId);
        }

        public bool Has(int entityId)
        {
            return _components.ContainsKey(entityId);
        }

        public ref T Get(int entityId)
        {
            if (!_components.ContainsKey(entityId))
                throw new InvalidOperationException($"Entity {entityId} does not have a {typeof(T).Name}.");

            return ref _components.GetValueRef(entityId);
        }

        public IEnumerable<(int entityId, T component)> GetAll()
        {
            foreach (var kvp in _components)
            {
                yield return (kvp.Key, kvp.Value);
            }
        }

        public IEnumerable<(int entityId, T component)> GetAllSorted()
        {
            foreach (var kvp in _components.OrderBy(k => k.Key)) // Sort by entity ID (key)
            {
                yield return (kvp.Key, kvp.Value);
            }
        }

    }

    /// <summary>
    /// Extension to allow ref access to Dictionary values.
    /// </summary>
    internal static class DictionaryExtensions
    {
        public static ref T GetValueRef<TKey, T>(this Dictionary<TKey, T> dictionary, TKey key)
        {
            return ref CollectionsMarshal.GetValueRefOrAddDefault(dictionary, key, out _);
        }
    }
}
