namespace BulletHell.ECS
{
    /// <summary>
    /// Specifies the components required for a query.
    /// </summary>
    public class QueryDescription
    {
        private List<Type> requiredTypes = new List<Type>();

        public QueryDescription WithAll<T>() => WithAll(typeof(T));

        public QueryDescription WithAll(params Type[] componentTypes)
        {
            requiredTypes.AddRange(componentTypes);
            return this;
        }

        public bool Matches(Type componentType) => requiredTypes.Contains(componentType);

        public IReadOnlyList<Type> RequiredTypes => requiredTypes.AsReadOnly();
    }
}
