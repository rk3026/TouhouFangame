namespace BulletHell.ECS
{
    /// <summary>
    /// Represents an entity in the game world, which is just an id.
    /// The id is used to index into the component storages for specific components.
    /// </summary>
    internal struct Entity
    {
        public int Id;
        public Entity(int id) => Id = id;
    }

}
