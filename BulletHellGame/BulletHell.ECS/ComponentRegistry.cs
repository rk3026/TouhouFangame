namespace BulletHell.ECS
{
    /// <summary>
    /// A class that keeps track of newly created component types and assigns them an index (used to make the bitmasks for an archetype).
    /// </summary>
    internal static class ComponentRegistry
    {
        private static Dictionary<Type, int> _componentIndices = new();
        private static int _nextIndex = 0;

        public static int GetComponentIndex(Type componentType)
        {
            if (!_componentIndices.ContainsKey(componentType))
            {
                _componentIndices[componentType] = _nextIndex++;
            }
            return _componentIndices[componentType];
        }
    }

}
