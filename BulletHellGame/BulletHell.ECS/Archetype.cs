namespace BulletHell.ECS
{
    internal class Archetype
    {
        // Unique identifier for the archetype (a bitmask of the components it contains)
        public int Bitmask { get; private set; }

        // Set of entity IDs that belong to this archetype
        public HashSet<int> EntityIDs { get; private set; }

        public Archetype(int bitmask)
        {
            Bitmask = bitmask;
            EntityIDs = new HashSet<int>();
        }

        public void AddEntity(int entityId)
        {
            EntityIDs.Add(entityId);
        }

        public void RemoveEntity(int entityId)
        {
            EntityIDs.Remove(entityId);
        }
    }
}